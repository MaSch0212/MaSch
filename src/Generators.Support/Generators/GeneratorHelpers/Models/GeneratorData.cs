using MaSch.Generators.GeneratorHelpers.Common;
using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;
using System;

namespace MaSch.Generators.GeneratorHelpers.Models;

internal readonly struct GeneratorData
{
    private readonly AttributeType _type;
    private readonly FileGeneratorAttributeData? _fileGenerator = null;
    private readonly MemberGeneratorAttributeData? _memberGenerator = null;
    private readonly SyntaxValidatorAttributeData? _syntaxValidator = null;
    private readonly IncrementalValueProviderFactoryAttributeData? _incrementalValueProviderFactory = null;

    public GeneratorData(FileGeneratorAttributeData fileGenerator)
    {
        _type = AttributeType.FileGenerator;
        _fileGenerator = fileGenerator;
    }

    public GeneratorData(MemberGeneratorAttributeData memberGenerator)
    {
        _type = AttributeType.MemberGenerator;
        _memberGenerator = memberGenerator;
    }

    public GeneratorData(SyntaxValidatorAttributeData syntaxValidator)
    {
        _type = AttributeType.SyntaxValidator;
        _syntaxValidator = syntaxValidator;
    }

    public GeneratorData(IncrementalValueProviderFactoryAttributeData incrementalValueProviderFactory)
    {
        _type = AttributeType.IncrementalValueProviderFactory;
        _incrementalValueProviderFactory = incrementalValueProviderFactory;
    }

    public static GeneratorData FromValueProviderData(ValueProviderData data, Compilation compilation, TypeSymbols typeSymbols)
    {
        if (compilation.GetSemanticModel(data.TypeDeclaration.SyntaxTree).GetDeclaredSymbol(data.TypeDeclaration) is not INamedTypeSymbol typeSymbol)
            return default;

        return data.AttributeType switch
        {
            AttributeType.FileGenerator => From(typeSymbol, typeSymbols.FileGeneratorAttribute, FileGeneratorAttributeData.Create, x => new(x)),
            AttributeType.MemberGenerator => From(typeSymbol, typeSymbols.MemberGeneratorAttribute, MemberGeneratorAttributeData.Create, x => new(x)),
            AttributeType.SyntaxValidator => From(typeSymbol, typeSymbols.SyntaxValidatorAttribute, SyntaxValidatorAttributeData.Create, x => new(x)),
            AttributeType.IncrementalValueProviderFactory => From(typeSymbol, typeSymbols.IncrementalValueProviderFactoryAttribute, IncrementalValueProviderFactoryAttributeData.Create, x => new(x)),
            _ => default,
        };
    }

    public void Match(
        Action<FileGeneratorAttributeData> fileGeneratorHandler,
        Action<MemberGeneratorAttributeData> memberGeneratorHandler,
        Action<SyntaxValidatorAttributeData> syntaxValidatorHandler,
        Action<IncrementalValueProviderFactoryAttributeData> incrementalValueProviderFactoryHandler)
    {
        if (_type == AttributeType.FileGenerator)
            fileGeneratorHandler(_fileGenerator!);
        else if (_type == AttributeType.MemberGenerator)
            memberGeneratorHandler(_memberGenerator!);
        else if (_type == AttributeType.SyntaxValidator)
            syntaxValidatorHandler(_syntaxValidator!);
        else if (_type == AttributeType.IncrementalValueProviderFactory)
            incrementalValueProviderFactoryHandler(_incrementalValueProviderFactory!);
    }

    private static GeneratorData From<T>(INamedTypeSymbol typeSymbol, INamedTypeSymbol attributeTypeSymbol, Func<INamedTypeSymbol, AttributeData, T?> getAttributeFunc, Func<T, GeneratorData> transformFunc)
    {
        if (!typeSymbol.TryGetAttribute(attributeTypeSymbol, out var attributeData))
            return default;
        var attribute = getAttributeFunc(typeSymbol, attributeData);
        if (attribute is null)
            return default;
        return transformFunc(attribute);
    }
}
