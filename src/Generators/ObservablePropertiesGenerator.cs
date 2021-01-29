using MaSch.Generators.Common;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using static MaSch.Generators.Common.CodeGenerationHelpers;

namespace MaSch.Generators
{
    /// <summary>
    /// A C# 9 Source Generator that generates properties for observable objects.
    /// </summary>
    /// <seealso cref="ISourceGenerator" />
    [Generator]
    public class ObservablePropertiesGenerator : ISourceGenerator
    {
        /// <inheritdoc />
        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        /// <inheritdoc />
        public void Execute(GeneratorExecutionContext context)
        {
            var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.DebugGeneratorAttribute");
            var definitionAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.ObservablePropertyDefinitionAttribute");
            var observableObjectSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Observable.IObservableObject");
            var observableObjectAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.ObservableObjectAttribute");

            if (definitionAttributeSymbol == null || (observableObjectSymbol == null && observableObjectAttributeSymbol == null))
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                        where (observableObjectSymbol != null && typeSymbol.AllInterfaces.Contains(observableObjectSymbol)) ||
                              (observableObjectAttributeSymbol != null && typeSymbol.GetAllAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, observableObjectAttributeSymbol)) != null)
                        from @interface in typeSymbol.Interfaces
                        where @interface.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, definitionAttributeSymbol)) != null
                        from propertySource in @interface.AllInterfaces.Prepend(@interface)
                        from property in propertySource.GetMembers().OfType<IPropertySymbol>()
                        group property by typeSymbol into g
                        select g;

            foreach (var type in query)
            {
                if (type.Key.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol)))
                    LaunchDebuggerOnBuild();

                var builder = new SourceBuilder();

                builder.AppendLine("using System.Diagnostics.CodeAnalysis;")
                       .AppendLine();

                using (builder.AddBlock($"namespace {type.Key.ContainingNamespace}"))
                using (builder.AddBlock($"partial class {type.Key.Name}"))
                {
                    bool isFirst = true;
                    foreach (var propInfo in type)
                    {
                        var fieldName = $"_{propInfo.Name[0].ToString().ToLowerInvariant()}{propInfo.Name[1..]}";
                        var propertyName = propInfo.Name;

                        if (!isFirst)
                            builder.AppendLine();
                        isFirst = false;

                        builder.AppendLine($"private {propInfo.Type} {fieldName};");

                        var xmlDoc = propInfo.GetFormattedDocumentationCommentXml();
                        if (xmlDoc != null)
                            builder.AppendLine(xmlDoc);

                        foreach (var attribute in propInfo.GetAllAttributes())
                        {
                            builder.AppendLine($"[{Regex.Replace(attribute.ToString(), @"[\{\}]", string.Empty)}]");
                        }

                        using (builder.AddBlock($"public {propInfo.Type} {propertyName}"))
                        {
                            builder.AppendLine($"get => {fieldName};");
                            using (builder.AddBlock("set"))
                            {
                                builder.AppendLine($"var previous = {fieldName};")
                                       .AppendLine($"SetProperty(ref {fieldName}, value);")
                                       .AppendLine($"On{propertyName}Changed(previous, value);");
                            }
                        }

                        builder.AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                               .AppendLine($"partial void On{propertyName}Changed({propInfo.Type} previous, {propInfo.Type} value);");
                    }
                }

                context.AddSource(type.Key, builder, nameof(ObservablePropertiesGenerator));
            }
        }
    }
}
