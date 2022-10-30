namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IDefinesParametersConfiguration<T> : ICodeConfiguration
    where T : IDefinesParametersConfiguration<T>
{
    T WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration);
}

public static class DefinesParametersConfigurationExtensions
{
    public static TConfig WithParameter<TConfig>(this TConfig config, string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        where TConfig : IDefinesParametersConfiguration<TConfig>
        => config.WithParameter(type, name, parameterConfiguration, (builder, config) => config(builder));

    public static TConfig WithParameter<TConfig>(this TConfig config, string type, string name)
        where TConfig : IDefinesParametersConfiguration<TConfig>
        => config.WithParameter<object?>(type, name, null, (_, _) => { });
}