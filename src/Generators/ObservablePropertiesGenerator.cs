using MaSch.CodeAnalysis.CSharp.Extensions;
using MaSch.Core;
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
        context.RegisterForPostInitialization(static ctx =>
        {
            new StaticSourceCreator(ctx.AddSource)
                .AddSource(StaticFiles.AccessModifier)
                .AddSource(StaticFiles.ObservablePropertyDefinitionAttribute)
                .AddSource(StaticFiles.ObservablePropertyAccessModifierAttribute);
        });
    }

    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        var definitionAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(ObservablePropertyDefinitionAttribute).FullName);
        var observableObjectSymbol = context.Compilation.GetTypeByMetadataName("MaSch.Core.Observable.IObservableObject");
        var observableObjectAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(GenerateObservableObjectAttribute).FullName);
        var notifyPropChangeAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(GenerateNotifyPropertyChangedAttribute).FullName);
        var accessModifierAttributeSymbol = context.Compilation.GetTypeByMetadataName(typeof(ObservablePropertyAccessModifierAttribute).FullName);

        if (definitionAttributeSymbol == null || (observableObjectSymbol == null && observableObjectAttributeSymbol == null))
            return;

#pragma warning disable RS1024 // Symbols should be compared for equality
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
#pragma warning restore RS1024 // Symbols should be compared for equality

        foreach (var type in query)
        {
            var builder = SourceBuilder.Create();

            _ = builder.AppendLine($"#nullable enable")
                   .AppendLine()
                   .AppendLine("using System.Diagnostics.CodeAnalysis;")
                   .AppendLine();

            builder.Append(Block($"namespace {type.Key.ContainingNamespace}"), builder =>
            {
                builder.Append(Block($"partial class {type.Key.Name}"), builder =>
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

                        builder.Append(Block($"{accessModifier} {propInfo.Type.ToUsageString()} {propertyName}"), builder =>
                        {
                            builder.Append(Block($"{getterModifier}get"), builder =>
                            {
                                _ = builder.AppendLine($"var result = {fieldName};")
                                       .AppendLine($"OnGet{propertyName}(ref result);")
                                       .AppendLine("return result;");
                            });

                            builder.Append(Block($"{setterModifier}set"), builder =>
                            {
                                _ = builder.AppendLine($"var previous = {fieldName};")
                                       .AppendLine($"On{propertyName}Changing(previous, ref value);")
                                       .AppendLine($"SetProperty(ref {fieldName}, value);")
                                       .AppendLine($"On{propertyName}Changed(previous, value);");
                            });
                        });

                        _ = builder.AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                               .AppendLine($"partial void OnGet{propertyName}(ref {propInfo.Type.ToUsageString()} value);")
                               .AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                               .AppendLine($"partial void On{propertyName}Changing({propInfo.Type.ToUsageString()} previous, ref {propInfo.Type.ToUsageString()} value);")
                               .AppendLine($"[SuppressMessage(\"Style\", \"IDE0060:Remove unused parameter\", Justification = \"Partial Method!\")]")
                               .AppendLine($"partial void On{propertyName}Changed({propInfo.Type.ToUsageString()} previous, {propInfo.Type.ToUsageString()} value);");
                    }
                });
            });

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
