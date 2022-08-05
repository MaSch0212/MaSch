﻿using MaSch.Core;
using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace MaSch.Generators;

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

        var definitionAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(ObservablePropertyDefinitionAttribute).FullName);
        var observableObjectSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Observable.IObservableObject");
        var observableObjectAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(GenerateObservableObjectAttribute).FullName);
        var notifyPropChangeAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(GenerateNotifyPropertyChangedAttribute).FullName);
        var accessModifierAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(ObservablePropertyAccessModifierAttribute).FullName);

        if (definitionAttributeSymbol == null || (observableObjectSymbol == null && observableObjectAttributeSymbol == null))
            return;

        var query = from typeSymbol in context.Compilation.SourceModule.GlobalNamespace.EnumerateNamespaceTypes()
                    where (observableObjectSymbol != null && typeSymbol.AllInterfaces.Contains(observableObjectSymbol)) ||
                          (observableObjectAttributeSymbol != null && typeSymbol.EnumerateAllAttributes().FirstOrDefault(x =>
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
            var builder = new SourceBuilder();

            _ = builder.AppendLine($"#nullable {nullableProperty}")
                   .AppendLine()
                   .AppendLine("using System.Diagnostics.CodeAnalysis;")
                   .AppendLine();

            using (builder.AddBlock($"namespace {type.Key.ContainingNamespace}"))
            using (builder.AddBlock($"partial class {type.Key.Name}"))
            {
                bool isFirst = true;
                foreach (var propInfo in type)
                {
                    var fieldName = $"_{propInfo.Name[0].ToString().ToLowerInvariant()}{propInfo.Name.Substring(1)}";
                    var propertyName = propInfo.Name;

                    if (!isFirst)
                        _ = builder.AppendLine();
                    isFirst = false;

                    _ = builder.AppendLine($"private {propInfo.Type.ToUsageString()} {fieldName};");

                    var xmlDoc = propInfo.GetFormattedDocumentationCommentXml();
                    if (xmlDoc != null)
                        _ = builder.AppendLine(xmlDoc);

                    foreach (var attribute in propInfo.EnumerateAllAttributes())
                    {
                        _ = builder.AppendLine($"[{Regex.Replace(attribute.ToString(), @"[\{\}]", string.Empty)}]");
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

                    using (builder.AddBlock($"{accessModifier} {propInfo.Type.ToUsageString()} {propertyName}"))
                    {
                        using (builder.AddBlock($"{getterModifier}get"))
                        {
                            _ = builder.AppendLine($"var result = {fieldName};")
                                   .AppendLine($"OnGet{propertyName}(ref result);")
                                   .AppendLine("return result;");
                        }

                        using (builder.AddBlock($"{setterModifier}set"))
                        {
                            _ = builder.AppendLine($"var previous = {fieldName};")
                                   .AppendLine($"On{propertyName}Changing(previous, ref value);")
                                   .AppendLine($"SetProperty(ref {fieldName}, value);")
                                   .AppendLine($"On{propertyName}Changed(previous, value);");
                        }
                    }

                    _ = builder.AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                           .AppendLine($"partial void OnGet{propertyName}(ref {propInfo.Type.ToUsageString()} value);")
                           .AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                           .AppendLine($"partial void On{propertyName}Changing({propInfo.Type.ToUsageString()} previous, ref {propInfo.Type.ToUsageString()} value);")
                           .AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                           .AppendLine($"partial void On{propertyName}Changed({propInfo.Type.ToUsageString()} previous, {propInfo.Type.ToUsageString()} value);");
                }
            }

            context.AddSource(builder.ToSourceText(), type.Key);
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
