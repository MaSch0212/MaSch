using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IIndexerDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IIndexerDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IIndexerConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IIndexerConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
    TBuilder Append(Func<TConfigFactory, IReadOnlyIndexerConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
    TBuilder Append(Func<TConfigFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
}