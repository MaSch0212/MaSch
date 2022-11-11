using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IEnumDeclarationBuilder<TBuilder>
    where TBuilder : IEnumDeclarationBuilder<TBuilder>
{
    TBuilder Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc);
}