using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IConstructorDeclarationBuilder : ISourceBuilder
    {
        IConstructorDeclarationBuilder Append(Func<IConstructorConfigurationFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> constructorBuilder);
        IConstructorDeclarationBuilder Append(Func<IConstructorConfigurationFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> staticConstructorBuilder);
    }

    public interface IConstructorDeclarationBuilder<TBuilder, TConfigFactory> : IConstructorDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IConstructorDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IConstructorConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> constructorBuilder);
        TBuilder Append(Func<TConfigFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> finalizerBuilder);
        TBuilder Append(Func<TConfigFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> staticConstructorBuilder);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IConstructorDeclarationBuilder
    {
        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(Func<IConstructorConfigurationFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> constructorBuilder)
            => Append(createFunc(_configurationFactory), constructorBuilder);

        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(Func<IConstructorConfigurationFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> staticConstructorBuilder)
            => Append(createFunc(_configurationFactory), staticConstructorBuilder);

        private SourceBuilder Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
            => AppendAsBlock(constructorConfiguration, this, builderFunc);

        private SourceBuilder Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
            => AppendAsBlock(staticConstructorConfiguration, this, builderFunc);
    }
}