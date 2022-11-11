using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IConstructorDeclarationBuilder<TBuilder>
    where TBuilder : IConstructorDeclarationBuilder<TBuilder>
{
    TBuilder Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc);
    TBuilder Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc);
}