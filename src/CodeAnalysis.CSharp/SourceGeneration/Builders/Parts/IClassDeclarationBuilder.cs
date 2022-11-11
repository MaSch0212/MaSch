using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IClassDeclarationBuilder<TBuilder>
    where TBuilder : IClassDeclarationBuilder<TBuilder>
{
    TBuilder Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc);
}