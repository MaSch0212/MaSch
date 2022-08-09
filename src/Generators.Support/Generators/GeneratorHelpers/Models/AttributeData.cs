using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;

#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Generators.GeneratorHelpers.Models;

internal abstract class AttributeDataBase
{
    protected AttributeDataBase(INamedTypeSymbol typeSymbol)
    {
        TypeSymbol = typeSymbol;
    }

    public INamedTypeSymbol TypeSymbol { get; }
}

internal class FileGeneratorAttributeData : AttributeDataBase
{
    public FileGeneratorAttributeData(INamedTypeSymbol typeSymbol, INamedTypeSymbol contextTypeSymbol)
        : base(typeSymbol)
    {
        ContextTypeSymbol = contextTypeSymbol;
    }

    public INamedTypeSymbol ContextTypeSymbol { get; }

    public static FileGeneratorAttributeData Create(INamedTypeSymbol typeSymbol, AttributeData attributeData)
    {
        if (attributeData.ConstructorArguments.Length < 1)
            return null;

        var contextType = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
        if (contextType == null)
            return null;
        return new FileGeneratorAttributeData(typeSymbol, contextType);
    }
}

internal class MemberGeneratorAttributeData : AttributeDataBase
{
    public MemberGeneratorAttributeData(INamedTypeSymbol typeSymbol, INamedTypeSymbol contextTypeSymbol)
        : base(typeSymbol)
    {
        ContextTypeSymbol = contextTypeSymbol;
    }

    public INamedTypeSymbol ContextTypeSymbol { get; }

    public static MemberGeneratorAttributeData Create(INamedTypeSymbol typeSymbol, AttributeData attributeData)
    {
        if (attributeData.ConstructorArguments.Length < 1)
            return null;

        var contextType = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
        if (contextType == null)
            return null;
        return new MemberGeneratorAttributeData(typeSymbol, contextType);
    }
}

internal class SyntaxValidatorAttributeData : AttributeDataBase
{
    public SyntaxValidatorAttributeData(INamedTypeSymbol typeSymbol, INamedTypeSymbol syntaxTypeSymbol, bool needsSemanticModel)
        : base(typeSymbol)
    {
        SyntaxTypeSymbol = syntaxTypeSymbol;
        NeedsSemanticModel = needsSemanticModel;
    }

    public INamedTypeSymbol SyntaxTypeSymbol { get; }
    public bool NeedsSemanticModel { get; }

    public static SyntaxValidatorAttributeData Create(INamedTypeSymbol typeSymbol, AttributeData attributeData)
    {
        if (attributeData.ConstructorArguments.Length < 1)
            return null;

        var syntaxType = attributeData.ConstructorArguments[0].Value as INamedTypeSymbol;
        if (syntaxType == null)
            return null;

        bool needsSemanticModel = true;
        foreach (var arg in attributeData.NamedArguments)
        {
            if (arg.Key == nameof(SyntaxValidatorAttribute.NeedsSemanticModel) && arg.Value.Value as bool? == false)
                needsSemanticModel = false;
        }

        return new SyntaxValidatorAttributeData(typeSymbol, syntaxType, needsSemanticModel);
    }
}

internal class IncrementalValueProviderFactoryAttributeData : AttributeDataBase
{
    public IncrementalValueProviderFactoryAttributeData(INamedTypeSymbol typeSymbol)
        : base(typeSymbol)
    {
    }

    public static IncrementalValueProviderFactoryAttributeData Create(INamedTypeSymbol typeSymbol, AttributeData attributeData)
    {
        return new(typeSymbol);
    }
}
