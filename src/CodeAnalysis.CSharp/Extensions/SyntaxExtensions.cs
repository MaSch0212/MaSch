using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MaSch.CodeAnalysis.CSharp.Extensions;

/// <summary>
/// Provides extensions for <see cref="SyntaxNode"/> as derived types.
/// </summary>
public static class SyntaxExtensions
{
    /// <summary>
    /// Tries to get an attribute with a specific type name from the syntax node.
    /// </summary>
    /// <param name="memberDeclarationSyntax">The syntax node.</param>
    /// <param name="semanticModel">The semantic model.</param>
    /// <param name="attributeTypeName">The full type name of the requested attribute.</param>
    /// <param name="attribute">The attribute syntax if an attribute was found; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if an attribute was found; otherwise <c>false</c>.</returns>
    public static bool TryGetAttributeSyntax(this MemberDeclarationSyntax memberDeclarationSyntax, SemanticModel semanticModel, string attributeTypeName, out AttributeSyntax attribute)
    {
        foreach (var attributeListSyntax in memberDeclarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    continue;

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (fullName == attributeTypeName)
                {
                    attribute = attributeSyntax;
                    return true;
                }
            }
        }

        attribute = null!;
        return false;
    }

    /// <summary>
    /// Tries to get a named argument from the attribute syntax.
    /// </summary>
    /// <param name="attributeSyntax">The attribute syntax.</param>
    /// <param name="namedArgumentName">The name of the requested argument.</param>
    /// <param name="attributeArgument">The attribute argument syntax if a named argument was found; otherwise <c>null</c>.</param>
    /// <returns><c>true</c> if a named argument was found; otherwise <c>false</c>.</returns>
    public static bool GetNamedArgumentSyntax(this AttributeSyntax attributeSyntax, string namedArgumentName, out AttributeArgumentSyntax attributeArgument)
    {
        if (attributeSyntax.ArgumentList is null)
        {
            attributeArgument = null!;
            return false;
        }

        foreach (var argumentSyntax in attributeSyntax.ArgumentList.Arguments)
        {
            if (argumentSyntax.NameEquals?.Name.Identifier.ValueText == namedArgumentName)
            {
                attributeArgument = argumentSyntax;
                return true;
            }
        }

        attributeArgument = null!;
        return false;
    }
}