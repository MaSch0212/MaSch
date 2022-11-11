using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IFinalizerDeclarationBuilder : ISourceBuilder
    {
        IFinalizerDeclarationBuilder Append(Func<IFinalizerConfigurationFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
    }

    public interface IFinalizerDeclarationBuilder<TBuilder, TConfigFactory> : IFinalizerDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IFinalizerDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IFinalizerConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IFinalizerDeclarationBuilder
    {
        /// <inheritdoc/>
        IFinalizerDeclarationBuilder IFinalizerDeclarationBuilder.Append(Func<IFinalizerConfigurationFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);
    }
}