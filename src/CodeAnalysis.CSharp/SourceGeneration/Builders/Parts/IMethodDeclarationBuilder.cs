using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IMethodDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IMethodDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IMethodConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IMethodConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
}