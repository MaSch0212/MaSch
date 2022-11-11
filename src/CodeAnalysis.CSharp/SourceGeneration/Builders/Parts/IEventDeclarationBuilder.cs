using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IEventDeclarationBuilder : ISourceBuilder
    {
        IEventDeclarationBuilder Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc);
        IEventDeclarationBuilder Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
    }

    public interface IEventDeclarationBuilder<TBuilder, TConfigFactory> : IEventDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IEventDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IEventConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IEventConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IEventDeclarationBuilder
    {
        /// <inheritdoc/>
        IEventDeclarationBuilder IEventDeclarationBuilder.Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IEventDeclarationBuilder IEventDeclarationBuilder.Append(Func<IEventConfigurationFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(createFunc(_configurationFactory), addBuilderFunc, removeBuilderFunc);
    }
}