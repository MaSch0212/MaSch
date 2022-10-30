using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml;

namespace MaSch.CodeAnalysis.CSharp.Extensions;

/// <summary>
/// Provides extensions for types implementing the <see cref="ISymbol"/> interface.
/// </summary>
public static class SymbolExtensions
{
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
        SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    /// <summary>
    /// Format that can be used to get the type definition syntax of a Symbol.
    /// </summary>
    public static readonly SymbolDisplayFormat TypeDefinitionFormat = new(
        SymbolDisplayGlobalNamespaceStyle.Omitted,
        SymbolDisplayTypeQualificationStyle.NameOnly,
        SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeTypeConstraints | SymbolDisplayGenericsOptions.IncludeVariance,
        SymbolDisplayMemberOptions.IncludeType | SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeRef,
        SymbolDisplayDelegateStyle.NameAndSignature,
        SymbolDisplayExtensionMethodStyle.Default,
        SymbolDisplayParameterOptions.IncludeParamsRefOut | SymbolDisplayParameterOptions.IncludeType | SymbolDisplayParameterOptions.IncludeName | SymbolDisplayParameterOptions.IncludeDefaultValue,
        SymbolDisplayPropertyStyle.NameOnly,
        SymbolDisplayLocalOptions.None,
        SymbolDisplayKindOptions.IncludeMemberKeyword,
        SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

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
        SymbolDisplayMiscellaneousOptions.UseSpecialTypes);

    /// <summary>
    /// Converts the symbol to a string representation using the <see cref="UsageFormat"/> format.
    /// </summary>
    /// <param name="symbol">The symbol to convert.</param>
    /// <returns>A formatted string representation of the symbol.</returns>
    public static string ToUsageString(this ISymbol symbol)
        => symbol.ToDisplayString(UsageFormat);

    /// <summary>
    /// Converts the symbol to a string representation using the <see cref="TypeDefinitionFormat"/> format.
    /// </summary>
    /// <param name="symbol">The symbol to convert.</param>
    /// <returns>A formatted string representation of the symbol.</returns>
    public static string ToTypeDefinitionString(this ISymbol symbol)
        => symbol.ToDisplayString(TypeDefinitionFormat);

    /// <summary>
    /// Converts the symbol to a string representation using the <see cref="DefinitionFormat"/> format.
    /// </summary>
    /// <param name="symbol">The symbol to convert.</param>
    /// <returns>A formatted string representation of the symbol.</returns>
    public static string ToDefinitionString(this ISymbol symbol)
        => symbol.ToDisplayString(DefinitionFormat);

    /// <summary>
    /// Determines all types that are defined inside a given namespace (includes descendant namespaces).
    /// </summary>
    /// <param name="symbol">The namespace to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all types inside the given namespace <paramref name="symbol"/>.</returns>
    public static IEnumerable<INamedTypeSymbol> EnumerateNamespaceTypes(this INamespaceSymbol symbol)
    {
        foreach (var child in symbol.GetTypeMembers())
        {
            yield return child;
        }

        foreach (var ns in symbol.GetNamespaceMembers())
        {
            foreach (var child2 in ns.EnumerateNamespaceTypes())
            {
                yield return child2;
            }
        }
    }

    /// <summary>
    /// Determines all types that are defined inside a given namespace (includes descendant namespaces) and their types.
    /// </summary>
    /// <param name="symbol">The namespace to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all types inside the given namespace <paramref name="symbol"/> and their types.</returns>
    public static IEnumerable<INamedTypeSymbol> EnumerateNamespaceAndNestedTypes(this INamespaceSymbol symbol)
    {
        foreach (var child in symbol.GetTypeMembers())
        {
            yield return child;
            foreach (var child2 in child.EnumerateNestedTypes())
                yield return child2;
        }

        foreach (var ns in symbol.GetNamespaceMembers())
        {
            foreach (var child in ns.EnumerateNamespaceTypes())
            {
                yield return child;
                foreach (var child2 in child.EnumerateNestedTypes())
                    yield return child2;
            }
        }
    }

    /// <summary>
    /// Determines all nested types that are defined inside a given type.
    /// </summary>
    /// <param name="symbol">The type to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all nested types inside the given type <paramref name="symbol"/>.</returns>
    public static IEnumerable<INamedTypeSymbol> EnumerateNestedTypes(this INamedTypeSymbol symbol)
    {
        foreach (var child in symbol.GetTypeMembers())
        {
            yield return child;
            foreach (var child2 in child.EnumerateNestedTypes())
                yield return child2;
        }
    }

    /// <summary>
    /// Determines all attributes of a given symbol (includes base types).
    /// </summary>
    /// <param name="symbol">The symbol to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all attributes defined for the symbol and its base types.</returns>
    public static IEnumerable<AttributeData> EnumerateAllAttributes(this ISymbol symbol)
    {
        ISymbol? currentSymbol = symbol;
        while (currentSymbol != null)
        {
            foreach (var attribute in currentSymbol.GetAttributes())
            {
                yield return attribute;
            }

            currentSymbol = (currentSymbol as INamedTypeSymbol)?.BaseType;
        }
    }

    /// <summary>
    /// Tries to get a specific attribute from the symbol.
    /// </summary>
    /// <param name="symbol">The symbol to search in.</param>
    /// <param name="attributeTypeSymbol">The type of attribute to find.</param>
    /// <param name="attributeData">The found attribute data. If no attribute was found, null is returned.</param>
    /// <returns><c>true</c> if an attribute was found; otherwise <c>false</c>.</returns>
    public static bool TryGetAttribute(this ISymbol symbol, INamedTypeSymbol attributeTypeSymbol, out AttributeData attributeData)
    {
        attributeData = symbol.GetAttributes(attributeTypeSymbol).FirstOrDefault();
        return attributeData != null;
    }

    /// <summary>
    /// Gets all attributes of a specific type from the symbol.
    /// </summary>
    /// <param name="symbol">The symbol to search in.</param>
    /// <param name="attributeTypeSymbol">The type of attributes to find.</param>
    /// <returns>An enumerable of all found attributes.</returns>
    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, INamedTypeSymbol attributeTypeSymbol)
    {
        return symbol.GetAttributes().Where(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeTypeSymbol));
    }

    /// <summary>
    /// Determines all base types of a given symbol.
    /// </summary>
    /// <param name="symbol">The symbol to search in.</param>
    /// <returns>Returns an enumerable that enumerates through all base types of the defined type symbol.</returns>
    public static IEnumerable<INamedTypeSymbol> EnumerateAllBaseTypes(this INamedTypeSymbol symbol)
    {
        var current = symbol.BaseType;
        while (current != null)
        {
            yield return current;
            current = current.BaseType;
        }
    }

    /// <summary>
    /// Determines whether an instance of the type the symbol represents can be assigned to an instance of the type a given other symbol represents.
    /// </summary>
    /// <param name="symbol">This symbol.</param>
    /// <param name="baseTypeSymbol">The other symbol.</param>
    /// <returns><c>true</c> if an instance of the type <paramref name="symbol"/> is representing can be assigned to the type <paramref name="baseTypeSymbol"/> is representing.</returns>
    public static bool IsAssignableTo(this INamedTypeSymbol symbol, INamedTypeSymbol baseTypeSymbol)
    {
        if (baseTypeSymbol.TypeKind == TypeKind.Interface)
        {
            return symbol.AllInterfaces.IndexOf(baseTypeSymbol, 0, SymbolEqualityComparer.Default) >= 0;
        }
        else if (baseTypeSymbol.TypeKind == TypeKind.Class)
        {
            var current = symbol.BaseType;
            while (current is not null)
            {
                if (SymbolEqualityComparer.Default.Equals(current, baseTypeSymbol))
                    return true;
                current = current.BaseType;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified symbol has the a given modifier.
    /// </summary>
    /// <param name="symbol">The symbol.</param>
    /// <param name="modifier">The modifier to check.</param>
    /// <returns>
    ///   <c>true</c> if the specified symbol has the <paramref name="modifier"/>; otherwise, <c>false</c>.
    /// </returns>
    public static bool HasModifier(this ISymbol symbol, SyntaxKind modifier)
    {
        return (from @ref in symbol.DeclaringSyntaxReferences
                let syntax = @ref.GetSyntax()
                where syntax is MemberDeclarationSyntax declarationSyntax
                   && declarationSyntax.Modifiers.Any(modifier)
                select syntax).Any();
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
}