using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IEventDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IEventDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IEventConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IEventConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
}