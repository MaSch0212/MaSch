using MaSch.Core;
using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MaSch.Generators.Generators.ObservableObject.Common;

[SyntaxValidator(typeof(ClassDeclarationSyntax))]
internal readonly partial struct ClassDeclarationValidator
{
    private static readonly string GenerateNotifyPropertyChangedAttributeName = typeof(GenerateNotifyPropertyChangedAttribute).FullName;
    private static readonly string GenerateObservableObjectAttributeName = typeof(GenerateObservableObjectAttribute).FullName;

    public InterfaceType GetGenerateAttributeType()
    {
        var currentType = InterfaceType.None;
        foreach (var attributeListSyntax in _syntaxNode.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (_semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                    continue;

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (fullName == GenerateObservableObjectAttributeName)
                    return InterfaceType.ObservableObject;
                if (fullName == GenerateNotifyPropertyChangedAttributeName)
                    currentType = InterfaceType.NotifyPropertyChanged;
            }
        }

        return currentType;
    }
}
