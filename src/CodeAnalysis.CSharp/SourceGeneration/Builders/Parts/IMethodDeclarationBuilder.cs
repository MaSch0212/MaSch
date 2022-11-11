using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IMethodDeclarationBuilder<TBuilder>
    where TBuilder : IMethodDeclarationBuilder<TBuilder>
{
    TBuilder Append(IMethodConfiguration methodConfiguration);
    TBuilder Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc);
}