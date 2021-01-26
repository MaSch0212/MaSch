using MaSch.Generators.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static MaSch.Generators.Common.CodeGenerationHelpers;

namespace MaSch.Generators
{
    [Generator]
    public class ObservableObjectGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.DebugGeneratorAttribute");
            var definitionAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.ObservablePropertyDefinition");
            var observableObjectSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Observable.IObservableObject");

            if (definitionAttributeSymbol == null || observableObjectSymbol == null)
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                        where typeSymbol.AllInterfaces.Contains(observableObjectSymbol)
                        from @interface in typeSymbol.Interfaces
                        where @interface.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, definitionAttributeSymbol)) != null
                        from property in @interface.GetMembers().OfType<IPropertySymbol>()
                        group property by typeSymbol into g
                        select g;
                
            foreach (var type in query)
            {
                if (type.Key.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, debugGeneratorSymbol)))
                    LaunchDebuggerOnBuild();

                var builder = new SourceBuilder();

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

                        foreach (var attribute in propInfo.GetAllAttributes())
                        {
                            builder.AppendLine($"[{Regex.Replace(attribute.ToString(), @"[\{\}]", "")}]");
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
                            builder.AppendLine($"[System.Diagnostics.CodeAnalysis.SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                                   .AppendLine($"partial void On{propertyName}Changed({propInfo.Type} previous, {propInfo.Type} value);");
                        }
                    }
                }

                context.AddSource(type.Key, builder);
            }
        }
    }
}
