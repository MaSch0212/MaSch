using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IConstructorDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IConstructorDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IConstructorConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
    TBuilder Append(Func<TConfigFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
}