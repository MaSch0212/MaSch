using MaSch.Core.Attributes;
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
            if (!context.AnalyzerConfigOptions.GlobalOptions.TryGetValue("build_property.nullable", out var nullableProperty))
                nullableProperty = "disable";

            var debugGeneratorSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.DebugGeneratorAttribute");
            var definitionAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.ObservablePropertyDefinitionAttribute");
            var observableObjectSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Observable.IObservableObject");
            var observableObjectAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.GenerateObservableObjectAttribute");
            var notifyPropChangeAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.GenerateNotifyPropertyChangedAttribute");
            var accessModifierAttributeSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Attributes.ObservablePropertyAccessModifierAttribute");

            if (definitionAttributeSymbol == null || (observableObjectSymbol == null && observableObjectAttributeSymbol == null))
                return;

            var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.GetNamespaceTypes()
                        where (observableObjectSymbol != null && typeSymbol.AllInterfaces.Contains(observableObjectSymbol)) ||
                              (observableObjectAttributeSymbol != null && typeSymbol.GetAllAttributes().FirstOrDefault(x =>
                                SymbolEqualityComparer.Default.Equals(x.AttributeClass, observableObjectAttributeSymbol) ||
                                SymbolEqualityComparer.Default.Equals(x.AttributeClass, notifyPropChangeAttributeSymbol)) != null)
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

                builder.AppendLine($"#nullable {nullableProperty}")
                       .AppendLine()
                       .AppendLine("using System.Diagnostics.CodeAnalysis;")
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

                        builder.AppendLine($"private {propInfo.Type.ToDisplayString(UsageFormat)} {fieldName};");

                        var xmlDoc = propInfo.GetFormattedDocumentationCommentXml();
                        if (xmlDoc != null)
                            builder.AppendLine(xmlDoc);

                        foreach (var attribute in propInfo.GetAllAttributes())
                        {
                            builder.AppendLine($"[{Regex.Replace(attribute.ToString(), @"[\{\}]", string.Empty)}]");
                        }

                        var accessModifierAttr = accessModifierAttributeSymbol == null ? null : propInfo.GetAttributes().FirstOrDefault(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, accessModifierAttributeSymbol));
                        string accessModifier = "public";
                        string setterModifier = string.Empty;
                        string getterModifier = string.Empty;
                        if (accessModifierAttr != null)
                        {
                            var accessModifierKey = accessModifierAttr.ConstructorArguments.FirstOrDefault().Value as int? ?? (int)AccessModifier.Public;
                            accessModifier = GetAccessModifier((AccessModifier)accessModifierKey);
                            if (accessModifierAttr.NamedArguments.FirstOrDefault(x => x.Key == "IsVirtual").Value.Value as bool? ?? false)
                                accessModifier += " virtual";

                            if (accessModifierAttr.NamedArguments.FirstOrDefault(x => x.Key == "SetModifier").Value.Value is int setterModifierKey && setterModifierKey != accessModifierKey)
                                setterModifier = GetAccessModifier((AccessModifier)setterModifierKey) + " ";

                            if (accessModifierAttr.NamedArguments.FirstOrDefault(x => x.Key == "GetModifier").Value.Value is int getterModifierKey && getterModifierKey != accessModifierKey)
                                getterModifier = GetAccessModifier((AccessModifier)getterModifierKey) + " ";
                        }

                        using (builder.AddBlock($"{accessModifier} {propInfo.Type.ToDisplayString(UsageFormat)} {propertyName}"))
                        {
                            using (builder.AddBlock($"{getterModifier}get"))
                            {
                                builder.AppendLine($"var result = {fieldName};")
                                       .AppendLine($"OnGet{propertyName}(ref result);")
                                       .AppendLine("return result;");
                            }

                            using (builder.AddBlock($"{setterModifier}set"))
                            {
                                builder.AppendLine($"var previous = {fieldName};")
                                       .AppendLine($"On{propertyName}Changing(previous, ref value);")
                                       .AppendLine($"SetProperty(ref {fieldName}, value);")
                                       .AppendLine($"On{propertyName}Changed(previous, value);");
                            }
                        }

                        builder.AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                               .AppendLine($"partial void OnGet{propertyName}(ref {propInfo.Type.ToDisplayString(UsageFormat)} value);")
                               .AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                               .AppendLine($"partial void On{propertyName}Changing({propInfo.Type.ToDisplayString(UsageFormat)} previous, ref {propInfo.Type.ToDisplayString(UsageFormat)} value);")
                               .AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                               .AppendLine($"partial void On{propertyName}Changed({propInfo.Type.ToDisplayString(UsageFormat)} previous, {propInfo.Type.ToDisplayString(UsageFormat)} value);");
                    }
                }

                context.AddSource(type.Key, builder, nameof(ObservablePropertiesGenerator));
            }
        }

        private static string GetAccessModifier(AccessModifier accessModifier)
        {
            return accessModifier switch
            {
                AccessModifier.ProtectedInternal => "protected internal",
                AccessModifier.Internal => "internal",
                AccessModifier.Protected => "protected",
                AccessModifier.PrivateProtected => "private protected",
                AccessModifier.Private => "private",
                _ => "public",
            };
        }
    }
}
