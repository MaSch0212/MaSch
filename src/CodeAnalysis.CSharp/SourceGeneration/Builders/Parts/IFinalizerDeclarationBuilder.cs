using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IFinalizerDeclarationBuilder<TBuilder>
    where TBuilder : IFinalizerDeclarationBuilder<TBuilder>
{
    TBuilder Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc);
}