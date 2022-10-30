using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MaSch.Generators.GeneratorHelpers.Models;

internal class ValueProviderData
{
    public ValueProviderData(AttributeType attributeType, TypeDeclarationSyntax typeDeclaration)
    {
        AttributeType = attributeType;
        TypeDeclaration = typeDeclaration;
    }

    public AttributeType AttributeType { get; }
    public TypeDeclarationSyntax TypeDeclaration { get; }
}
