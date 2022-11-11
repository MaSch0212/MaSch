using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IIndexerDeclarationBuilder : ISourceBuilder
    {
        IIndexerDeclarationBuilder Append(Func<IIndexerConfigurationFactory, IIndexerConfiguration> createFunc);
        IIndexerDeclarationBuilder Append(Func<IIndexerConfigurationFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        IIndexerDeclarationBuilder Append(Func<IIndexerConfigurationFactory, IReadOnlyIndexerConfiguration> createFunc);
        IIndexerDeclarationBuilder Append(Func<IIndexerConfigurationFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
        IIndexerDeclarationBuilder Append(Func<IIndexerConfigurationFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
    }

    public interface IIndexerDeclarationBuilder<TBuilder, TConfigFactory> : IIndexerDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IIndexerDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IIndexerConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IIndexerConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        TBuilder Append(Func<TConfigFactory, IReadOnlyIndexerConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
        TBuilder Append(Func<TConfigFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IIndexerDeclarationBuilder
    {
        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(Func<IIndexerConfigurationFactory, IIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(Func<IIndexerConfigurationFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(Func<IIndexerConfigurationFactory, IReadOnlyIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(Func<IIndexerConfigurationFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(Func<IIndexerConfigurationFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);
    }
}