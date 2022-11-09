using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IConstructorDeclarationBuilder : ISourceBuilder
{
    IConstructorDeclarationBuilder Append(Func<IConstructorConfigurationFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> constructorBuilder);
    IConstructorDeclarationBuilder Append(Func<IConstructorConfigurationFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> finalizerBuilder);
    IConstructorDeclarationBuilder Append(Func<IConstructorConfigurationFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> staticConstructorBuilder);
}

public interface IConstructorDeclarationBuilder<TBuilder, TConfigFactory> : IConstructorDeclarationBuilder
    where TBuilder : IConstructorDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IConstructorConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> constructorBuilder);
    TBuilder Append(Func<TConfigFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> finalizerBuilder);
    TBuilder Append(Func<TConfigFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> staticConstructorBuilder);
}

public partial class SourceBuilder : IConstructorDeclarationBuilder
{
    IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(Func<IConstructorConfigurationFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> constructorBuilder)
        => Append(createFunc(_configurationFactory), constructorBuilder);

    IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(Func<IConstructorConfigurationFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> finalizerBuilder)
        => Append(createFunc(_configurationFactory), finalizerBuilder);

    IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(Func<IConstructorConfigurationFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> staticConstructorBuilder)
        => Append(createFunc(_configurationFactory), staticConstructorBuilder);

    private SourceBuilder Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
        => AppendAsBlock(constructorConfiguration, this, builderFunc);

    private SourceBuilder Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc)
        => AppendAsBlock(finalizerConfiguration, this, builderFunc);

    private SourceBuilder Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
        => AppendAsBlock(staticConstructorConfiguration, this, builderFunc);
}