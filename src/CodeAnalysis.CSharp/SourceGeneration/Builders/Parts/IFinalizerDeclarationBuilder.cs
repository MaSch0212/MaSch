using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IFinalizerDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IFinalizerDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IFinalizerConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
}