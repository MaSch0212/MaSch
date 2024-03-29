﻿using MaSch.Core.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Xml;

namespace MaSch.Generators.Common;

/// <summary>
/// Contains helper methods and values for C# 9 Source Generators.
/// </summary>
public static class CodeGenerationHelpers
{
    /// <summary>
    /// Format that can be used to get the definition syntax of a Symbol.
    /// </summary>
    public static readonly SymbolDisplayFormat DefinitionFormat = new(
        SymbolDisplayGlobalNamespaceStyle.Included,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance,
        SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeRef,
        SymbolDisplayDelegateStyle.NameAndSignature,
        SymbolDisplayExtensionMethodStyle.Default,
        SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeDefaultValue,
        SymbolDisplayPropertyStyle.NameOnly,
        SymbolDisplayLocalOptions.None,
        SymbolDisplayKindOptions.IncludeMemberKeyword,
        SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

    /// <summary>
    /// Format that can be used to get the usage syntax of a Symbol.
    /// </summary>
    public static readonly SymbolDisplayFormat UsageFormat = new(
        SymbolDisplayGlobalNamespaceStyle.Included,
        SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        SymbolDisplayGenericsOptions.IncludeTypeParameters,
        SymbolDisplayMemberOptions.IncludeParameters,
        SymbolDisplayDelegateStyle.NameAndSignature,
        SymbolDisplayExtensionMethodStyle.Default,
        SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeName,
        SymbolDisplayPropertyStyle.NameOnly,
        SymbolDisplayLocalOptions.None,
        SymbolDisplayKindOptions.None,
        SymbolDisplayMiscellaneousOptions.UseSpecialTypes | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

    /// <summary>
    /// Determines all types that are defined inside a given namespace (includes descendant namespaces).
    /// </summary>
    /// <param name="symbol">The namespace to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all types inside the given namespace <paramref name="symbol"/>.</returns>
    public static IEnumerable<INamedTypeSymbol> GetNamespaceTypes(this INamespaceSymbol symbol)
    {
        foreach (var child in symbol.GetTypeMembers())
        {
            yield return child;
        }

        foreach (var ns in symbol.GetNamespaceMembers())
        {
            foreach (var child2 in GetNamespaceTypes(ns))
            {
                yield return child2;
            }
        }
    }

    /// <summary>
    /// Determines all attributes of a given symbol (includes base types).
    /// </summary>
    /// <param name="symbol">The symbol to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all attributes defined for the symbol and its base types.</returns>
    public static IEnumerable<AttributeData> GetAllAttributes(this ISymbol? symbol)
    {
        while (symbol != null)
        {
            foreach (var attribute in symbol.GetAttributes())
            {
                yield return attribute;
            }

            symbol = (symbol as INamedTypeSymbol)?.BaseType;
        }
    }

    /// <summary>
    /// Determines all base types of a given symbol.
    /// </summary>
    /// <param name="symbol">The symbol to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all base types of the defined type symbol.</returns>
    public static IEnumerable<INamedTypeSymbol> GetAllBaseTypes(this INamedTypeSymbol symbol)
    {
        var current = symbol.BaseType;
        while (current != null)
        {
            yield return current;
            current = current.BaseType;
        }
    }

    /// <summary>
    /// Determines whether the specified symbol has the partial modifier.
    /// </summary>
    /// <param name="symbol">The symbol.</param>
    /// <returns>
    ///   <c>true</c> if the specified symbol has the partial modifier; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasPartialModifier(this ISymbol symbol)
    {
        return (from @ref in symbol.DeclaringSyntaxReferences
                let syntax = @ref.GetSyntax()
                where syntax is MemberDeclarationSyntax declarationSyntax
                   && declarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword)
                select syntax).Any();
    }

    /// <summary>
    /// Creates a name out of a <see cref="ISymbol"/> that can be used for the generated file.
    /// </summary>
    /// <param name="symbol">The type symbol to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(this ISymbol symbol, string generatorName)
    {
        return CreateHintName(symbol, x => (x, generatorName).GetHashCode());
    }

    /// <summary>
    /// Creates a file name that can be used for a generated file.
    /// </summary>
    /// <param name="name">The name of the file to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(string name, string generatorName)
    {
        return CreateHintName(name, (name, generatorName).GetHashCode());
    }

    /// <summary>
    /// Creates a name out of a <see cref="ISymbol"/> that can be used for the generated file.
    /// </summary>
    /// <param name="symbol">The type symbol to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <param name="additionalGenerationInfo">Additional info to create a unique hash for this generation.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(this ISymbol symbol, string generatorName, string additionalGenerationInfo)
    {
        return CreateHintName(symbol, x => (x, generatorName, additionalGenerationInfo).GetHashCode());
    }

    /// <summary>
    /// Creates a file name that can be used for a generated file.
    /// </summary>
    /// <param name="name">The name of the file to use.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <param name="additionalGenerationInfo">Additional info to create a unique hash for this generation.</param>
    /// <returns>Returns a name that can be used for a generated file.</returns>
    public static string CreateHintName(string name, string generatorName, string additionalGenerationInfo)
    {
        return CreateHintName(name, (name, generatorName, additionalGenerationInfo).GetHashCode());
    }

    /// <summary>
    /// Gets the XML (as C# sytax text) for the comment associated with the symbol.
    /// </summary>
    /// <param name="symbol">The symbol.</param>
    /// <returns>The XML that would be written to a C# source file for the symbol.</returns>
    public static string? GetFormattedDocumentationCommentXml(this ISymbol symbol)
    {
        var xml = symbol.GetDocumentationCommentXml();
        if (string.IsNullOrWhiteSpace(xml))
            return null;

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        var innerXmlLines = doc.FirstChild?.InnerXml?.Replace("\r", string.Empty).Split('\n');
        if (innerXmlLines == null || innerXmlLines.Length == 0)
            return null;

        return string.Join(Environment.NewLine, innerXmlLines.Select(x => $"/// {x.Trim()}"));
    }

    /// <summary>
    /// Adds a <see cref="SourceBuilder"/> to the compilation.
    /// </summary>
    /// <param name="context">The generator execution context.</param>
    /// <param name="forType">The type for which the source was generated for.</param>
    /// <param name="builder">The builder that includes the generated source code.</param>
    /// <param name="generatorName">The name of the generator.</param>
    public static void AddSource(this GeneratorExecutionContext context, ITypeSymbol forType, SourceBuilder builder, string generatorName)
    {
        context.AddSource(forType.CreateHintName(generatorName), SourceText.From(builder.ToString(), Encoding.UTF8));
    }

    /// <summary>
    /// Adds a <see cref="SourceBuilder"/> to the compilation.
    /// </summary>
    /// <param name="context">The generator execution context.</param>
    /// <param name="forType">The type for which the source was generated for.</param>
    /// <param name="builder">The builder that includes the generated source code.</param>
    /// <param name="generatorName">The name of the generator.</param>
    /// <param name="additionalGenerationInfo">Additional info to create a unique hash for this generation.</param>
    public static void AddSource(this GeneratorExecutionContext context, ITypeSymbol forType, SourceBuilder builder, string generatorName, string additionalGenerationInfo)
    {
        context.AddSource(forType.CreateHintName(generatorName, additionalGenerationInfo), SourceText.From(builder.ToString(), Encoding.UTF8));
    }

    /// <summary>
    /// Adds a <see cref="SourceBuilder"/> to the compilation.
    /// </summary>
    /// <param name="context">The generator execution context.</param>
    /// <param name="hintName">The name of the generated file.</param>
    /// <param name="builder">The builder that includes the generated source code.</param>
    public static void AddSource(this GeneratorExecutionContext context, string hintName, SourceBuilder builder)
    {
        context.AddSource(hintName, SourceText.From(builder.ToString(), Encoding.UTF8));
    }

    /// <summary>
    /// Launches the debugger if the generator was not executed from an IDE.
    /// </summary>
    public static void LaunchDebuggerOnBuild()
    {
        var processName = Process.GetCurrentProcess().ProcessName;
        if (!Debugger.IsAttached && processName != "ServiceHub.RoslynCodeAnalysisService" && processName != "devenv")
            _ = Debugger.Launch();
    }

    private static string CreateHintName(ISymbol symbol, Func<string, int> hashFunc)
    {
        string unescapedFileName;
        string hashBaseName;

        switch (symbol)
        {
            case IAssemblySymbol assembly:
                unescapedFileName = assembly.Name;
                hashBaseName = assembly.Name;
                break;
            default:
                unescapedFileName = symbol.Name;
                hashBaseName = symbol.ToDisplayString(DefinitionFormat.WithGenericsOptions(SymbolDisplayGenericsOptions.IncludeTypeParameters));
                break;
        }

        var name = Regex.Replace(unescapedFileName, @"[\.+]", "-");
        return CreateHintName(name, hashFunc(hashBaseName));
    }

    private static string CreateHintName(string name, int hash)
    {
        return $"{name}-{BitConverter.GetBytes(hash).ToHexString()}.g";
    }
}
