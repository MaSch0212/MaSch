using MaSch.Generators.Support;
using Microsoft.CodeAnalysis;

namespace MaSch.Generators.GeneratorHelpers.Common;

internal class TypeSymbols
{
    private TypeSymbols(Compilation compilation)
    {
        FileGeneratorAttribute = compilation.GetTypeByMetadataName(Names.FileGeneratorAttribute)!;
        MemberGeneratorAttribute = compilation.GetTypeByMetadataName(Names.MemberGeneratorAttribute)!;
        SyntaxValidatorAttribute = compilation.GetTypeByMetadataName(Names.SyntaxValidatorAttribute)!;
        IncrementalValueProviderFactoryAttribute = compilation.GetTypeByMetadataName(Names.IncrementalValueProviderFactoryAttribute)!;
    }

    public INamedTypeSymbol FileGeneratorAttribute { get; }
    public INamedTypeSymbol MemberGeneratorAttribute { get; }
    public INamedTypeSymbol SyntaxValidatorAttribute { get; }
    public INamedTypeSymbol IncrementalValueProviderFactoryAttribute { get; }

    private bool HasAllSymbols =>
        FileGeneratorAttribute is not null &&
        MemberGeneratorAttribute is not null &&
        SyntaxValidatorAttribute is not null &&
        IncrementalValueProviderFactoryAttribute is not null;

    public static bool TryCreate(Compilation compilation, out TypeSymbols typeSymbols)
    {
        typeSymbols = new TypeSymbols(compilation);
        return typeSymbols.HasAllSymbols;
    }

    internal static class Names
    {
        internal static readonly string FileGeneratorAttribute = typeof(FileGeneratorAttribute).FullName;
        internal static readonly string MemberGeneratorAttribute = typeof(MemberGeneratorAttribute).FullName;
        internal static readonly string SyntaxValidatorAttribute = typeof(SyntaxValidatorAttribute).FullName;
        internal static readonly string IncrementalValueProviderFactoryAttribute = typeof(IncrementalValueProviderFactoryAttribute).FullName;
    }
}
