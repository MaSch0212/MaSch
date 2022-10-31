using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IConstructorDeclarationBuilder : ISourceBuilder
{
    SourceBuilderCodeBlock AppendConstructor<TParams>(out ISourceBuilder constructorBuilder, TParams @params, Action<IConstructorConfiguration, TParams> constructorConfiguration);
    SourceBuilderCodeBlock AppendStaticConstructor(out ISourceBuilder constructorBuilder);
    SourceBuilderCodeBlock AppendFinalizer(out ISourceBuilder finalizerBuilder);
}

public interface IConstructorDeclarationBuilder<T> : IConstructorDeclarationBuilder
    where T : IConstructorDeclarationBuilder<T>
{
}

public partial class SourceBuilder : IConstructorDeclarationBuilder
{
    /// <inheritdoc/>
    SourceBuilderCodeBlock IConstructorDeclarationBuilder.AppendConstructor<TParams>(out ISourceBuilder constructorBuilder, TParams @params, Action<IConstructorConfiguration, TParams> constructorConfiguration)
        => AppendConstructor(out constructorBuilder, @params, constructorConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock IConstructorDeclarationBuilder.AppendFinalizer(out ISourceBuilder finalizerBuilder)
        => AppendFinalizer(out finalizerBuilder);

    /// <inheritdoc/>
    SourceBuilderCodeBlock IConstructorDeclarationBuilder.AppendStaticConstructor(out ISourceBuilder constructorBuilder)
        => AppendStaticConstructor(out constructorBuilder);
}

public static class ConstructorDeclarationBuilderExtensions
{
    public static SourceBuilderCodeBlock AppendConstructor(this IConstructorDeclarationBuilder builder, out ISourceBuilder constructorBuilder, Action<IConstructorConfiguration> constructorConfiguration)
        => builder.AppendConstructor(out constructorBuilder, constructorConfiguration, static (builder, config) => config?.Invoke(builder));

    public static TBuilder AppendConstructor<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        TConfigParams constructorConfigurationParams,
        Action<IConstructorConfiguration, TConfigParams> constructorConfiguration,
        TBuilderParams constructorBuilderParams,
        Action<ISourceBuilder, TBuilderParams> constructorBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendConstructor(out var constructorBuilder, constructorConfigurationParams, constructorConfiguration))
            constructorBuilderAction?.Invoke(constructorBuilder, constructorBuilderParams);
        return builder;
    }

    public static TBuilder AppendConstructor<TBuilder, TBuilderParams>(
        this TBuilder builder,
        Action<IConstructorConfiguration> constructorConfiguration,
        TBuilderParams constructorBuilderParams,
        Action<ISourceBuilder, TBuilderParams> constructorBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendConstructor(out var constructorBuilder, constructorConfiguration, static (builder, config) => config?.Invoke(builder)))
            constructorBuilderAction?.Invoke(constructorBuilder, constructorBuilderParams);
        return builder;
    }

    public static TBuilder AppendConstructor<TBuilder, TConfigParams>(
        this TBuilder builder,
        TConfigParams constructorConfigurationParams,
        Action<IConstructorConfiguration, TConfigParams> constructorConfiguration,
        Action<ISourceBuilder> constructorBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendConstructor(out var constructorBuilder, constructorConfigurationParams, constructorConfiguration))
            constructorBuilderAction?.Invoke(constructorBuilder);
        return builder;
    }

    public static TBuilder AppendConstructor<TBuilder>(
        this TBuilder builder,
        Action<IConstructorConfiguration> constructorConfiguration,
        Action<ISourceBuilder> constructorBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendConstructor(out var constructorBuilder, constructorConfiguration, static (builder, config) => config?.Invoke(builder)))
            constructorBuilderAction?.Invoke(constructorBuilder);
        return builder;
    }

    public static TBuilder AppendStaticConstructor<TBuilder, TParams>(this TBuilder builder, TParams constructorBuilderParams, Action<ISourceBuilder, TParams> constructorBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendStaticConstructor(out var constructorBuilder))
            constructorBuilderAction?.Invoke(constructorBuilder, constructorBuilderParams);
        return builder;
    }

    public static TBuilder AppendStaticConstructor<TBuilder>(this TBuilder builder, Action<ISourceBuilder> constructorBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendStaticConstructor(out var constructorBuilder))
            constructorBuilderAction?.Invoke(constructorBuilder);
        return builder;
    }

    public static TBuilder AppendFinalizer<TBuilder, TParams>(this TBuilder builder, TParams finalizerBuilderParams, Action<ISourceBuilder, TParams> finalizerBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendFinalizer(out var finalizerBuilder))
            finalizerBuilderAction?.Invoke(finalizerBuilder, finalizerBuilderParams);
        return builder;
    }

    public static TBuilder AppendFinalizer<TBuilder>(this TBuilder builder, Action<ISourceBuilder> finalizerBuilderAction)
        where TBuilder : IConstructorDeclarationBuilder
    {
        using (builder.AppendFinalizer(out var finalizerBuilder))
            finalizerBuilderAction?.Invoke(finalizerBuilder);
        return builder;
    }
}