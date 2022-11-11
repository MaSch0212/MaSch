using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IConstructorDeclarationBuilder : ISourceBuilder
    {
        IConstructorDeclarationBuilder Append(Func<IConstructorConfigurationFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
        IConstructorDeclarationBuilder Append(Func<IConstructorConfigurationFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
    }

    public interface IConstructorDeclarationBuilder<TBuilder, TConfigFactory> : IConstructorDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IConstructorDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IConstructorConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
        TBuilder Append(Func<TConfigFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IConstructorDeclarationBuilder
    {
        /// <inheritdoc/>
        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(Func<IConstructorConfigurationFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(Func<IConstructorConfigurationFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);
    }
}