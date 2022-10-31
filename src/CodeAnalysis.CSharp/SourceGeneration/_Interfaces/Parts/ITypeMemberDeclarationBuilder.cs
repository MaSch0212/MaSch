using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ITypeMemberDeclarationBuilder : ISourceBuilder
{
    string CurrentTypeName { get; set; }
    string CurrentTypeNameWithoutGenericParameters { get; set; }

    ITypeMemberDeclarationBuilder AppendField<TParams>(string fieldTypeName, string fieldName, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration);
    ITypeMemberDeclarationBuilder AppendMethod<TParams>(string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration);
    SourceBuilderCodeBlock AppendMethod<TParams>(string methodName, out ISourceBuilder methodBuilder, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration);
}

public interface ITypeMemberDeclarationBuilder<T> : ITypeMemberDeclarationBuilder
    where T : ITypeMemberDeclarationBuilder<T>
{
    new T AppendField<TParams>(string fieldTypeName, string fieldName, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration);
    new T AppendMethod<TParams>(string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration);
}

public partial class SourceBuilder : ITypeMemberDeclarationBuilder
{
    /// <inheritdoc/>
    string ITypeMemberDeclarationBuilder.CurrentTypeName
    {
        get => CurrentTypeName;
        set => CurrentTypeName = value;
    }

    /// <inheritdoc/>
    string ITypeMemberDeclarationBuilder.CurrentTypeNameWithoutGenericParameters
    {
        get => CurrentTypeNameWithoutGenericParameters;
        set => CurrentTypeNameWithoutGenericParameters = value;
    }

    /// <inheritdoc/>
    ITypeMemberDeclarationBuilder ITypeMemberDeclarationBuilder.AppendField<TParams>(string fieldTypeName, string fieldName, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration)
        => AppendField<TParams>(fieldTypeName, fieldName, @params, fieldConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock ITypeMemberDeclarationBuilder.AppendMethod<TParams>(string methodName, out ISourceBuilder methodBuilder, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        => AppendMethod(methodName, out methodBuilder, @params, methodConfiguration);

    /// <inheritdoc/>
    ITypeMemberDeclarationBuilder ITypeMemberDeclarationBuilder.AppendMethod<TParams>(string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        => AppendMethod(methodName, @params, methodConfiguration);
}

public static class TypeMemberDeclarationBuilderExtensions
{
    public static TBuilder AppendField<TBuilder, TParams>(this TBuilder builder, string fieldTypeName, string fieldName, string fieldValue, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        builder.AppendField(
            fieldTypeName,
            fieldName,
            (fieldValue, @params, fieldConfiguration),
            static (config, @params) =>
            {
                config.WithValue(@params.fieldValue);
                @params.fieldConfiguration?.Invoke(config, @params.@params);
            });
        return builder;
    }

    public static TBuilder AppendField<TBuilder>(this TBuilder builder, string fieldTypeName, string fieldName, string fieldValue, Action<IFieldConfiguration> fieldConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        builder.AppendField(
            fieldTypeName,
            fieldName,
            (fieldValue, fieldConfiguration),
            static (config, @params) =>
            {
                config.WithValue(@params.fieldValue);
                @params.fieldConfiguration?.Invoke(config);
            });
        return builder;
    }

    public static TBuilder AppendField<TBuilder>(this TBuilder builder, string fieldTypeName, string fieldName, Action<IFieldConfiguration> fieldConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        builder.AppendField(fieldTypeName, fieldName, fieldConfiguration, static (config, fieldConfiguration) => fieldConfiguration?.Invoke(config));
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TParams>(this TBuilder builder, string returnTypeName, string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        builder.AppendMethod(
            methodName,
            (returnTypeName, @params, methodConfiguration),
            static (config, @params) =>
            {
                config.WithReturnType(@params.returnTypeName);
                @params.methodConfiguration?.Invoke(config, @params.@params);
            });
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder>(this TBuilder builder, string returnTypeName, string methodName, Action<IMethodConfiguration> methodConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        builder.AppendMethod(
            methodName,
            (returnTypeName, methodConfiguration),
            static (config, @params) =>
            {
                config.WithReturnType(@params.returnTypeName);
                @params.methodConfiguration?.Invoke(config);
            });
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder>(this TBuilder builder, string methodName, Action<IMethodConfiguration> methodConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        builder.AppendMethod(methodName, methodConfiguration, static (config, methodConfiguration) => methodConfiguration?.Invoke(config));
        return builder;
    }

    public static SourceBuilderCodeBlock AppendMethod<TBuilder, TParams>(this TBuilder builder, string returnTypeName, string methodName, out ISourceBuilder methodBuilder, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        return builder.AppendMethod(
            methodName,
            out methodBuilder,
            (returnTypeName, @params, methodConfiguration),
            static (config, @params) =>
            {
                config.WithReturnType(@params.returnTypeName);
                @params.methodConfiguration?.Invoke(config, @params.@params);
            });
    }

    public static SourceBuilderCodeBlock AppendMethod<TBuilder>(this TBuilder builder, string returnTypeName, string methodName, out ISourceBuilder methodBuilder, Action<IMethodConfiguration> methodConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        return builder.AppendMethod(
            methodName,
            out methodBuilder,
            (returnTypeName, methodConfiguration),
            static (config, @params) =>
            {
                config.WithReturnType(@params.returnTypeName);
                @params.methodConfiguration?.Invoke(config);
            });
    }

    public static SourceBuilderCodeBlock AppendMethod<TBuilder>(this TBuilder builder, string methodName, out ISourceBuilder methodBuilder, Action<IMethodConfiguration> methodConfiguration)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        return builder.AppendMethod(methodName, out methodBuilder, methodConfiguration, static (config, methodConfiguration) => methodConfiguration?.Invoke(config));
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string returnTypeName,
        string methodName,
        TConfigParams methodConfigurationParams,
        Action<IMethodConfiguration, TConfigParams> methodConfiguration,
        TBuilderParams methodBuilderParams,
        Action<ISourceBuilder, TBuilderParams> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(returnTypeName, methodName, out ISourceBuilder methodBuilder, methodConfigurationParams, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder, methodBuilderParams);
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string returnTypeName,
        string methodName,
        Action<IMethodConfiguration> methodConfiguration,
        TBuilderParams methodBuilderParams,
        Action<ISourceBuilder, TBuilderParams> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(returnTypeName, methodName, out ISourceBuilder methodBuilder, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder, methodBuilderParams);
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string returnTypeName,
        string methodName,
        TConfigParams methodConfigurationParams,
        Action<IMethodConfiguration, TConfigParams> methodConfiguration,
        Action<ISourceBuilder> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(returnTypeName, methodName, out ISourceBuilder methodBuilder, methodConfigurationParams, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder);
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string returnTypeName,
        string methodName,
        Action<IMethodConfiguration> methodConfiguration,
        Action<ISourceBuilder> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(returnTypeName, methodName, out ISourceBuilder methodBuilder, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder);
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string methodName,
        TConfigParams methodConfigurationParams,
        Action<IMethodConfiguration, TConfigParams> methodConfiguration,
        TBuilderParams methodBuilderParams,
        Action<ISourceBuilder, TBuilderParams> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(methodName, out ISourceBuilder methodBuilder, methodConfigurationParams, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder, methodBuilderParams);
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string methodName,
        Action<IMethodConfiguration> methodConfiguration,
        TBuilderParams methodBuilderParams,
        Action<ISourceBuilder, TBuilderParams> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(methodName, out ISourceBuilder methodBuilder, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder, methodBuilderParams);
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string methodName,
        TConfigParams methodConfigurationParams,
        Action<IMethodConfiguration, TConfigParams> methodConfiguration,
        Action<ISourceBuilder> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(methodName, out ISourceBuilder methodBuilder, methodConfigurationParams, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder);
        return builder;
    }

    public static TBuilder AppendMethod<TBuilder, TConfigParams, TBuilderParams>(
        this TBuilder builder,
        string methodName,
        Action<IMethodConfiguration> methodConfiguration,
        Action<ISourceBuilder> methodBuilderAction)
        where TBuilder : ITypeMemberDeclarationBuilder
    {
        using (builder.AppendMethod(methodName, out ISourceBuilder methodBuilder, methodConfiguration))
            methodBuilderAction?.Invoke(methodBuilder);
        return builder;
    }
}