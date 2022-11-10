using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IFinalizerDeclarationBuilder : ISourceBuilder
    {
        IFinalizerDeclarationBuilder Append(Func<IFinalizerConfigurationFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> finalizerBuilder);
    }

    public interface IFinalizerDeclarationBuilder<TBuilder, TConfigFactory> : IFinalizerDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IFinalizerDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IFinalizerConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> finalizerBuilder);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IFinalizerDeclarationBuilder
    {
        IFinalizerDeclarationBuilder IFinalizerDeclarationBuilder.Append(Func<IFinalizerConfigurationFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> finalizerBuilder)
            => Append(createFunc(_configurationFactory), finalizerBuilder);

        private SourceBuilder Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc)
            => AppendAsBlock(finalizerConfiguration, this, builderFunc);
    }
}