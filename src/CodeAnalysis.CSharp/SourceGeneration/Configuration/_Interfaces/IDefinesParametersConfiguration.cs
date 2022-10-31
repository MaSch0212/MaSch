namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IDefinesParametersConfiguration : ICodeConfiguration
{
    IDefinesParametersConfiguration WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration);
}

public interface IDefinesParametersConfiguration<T> : IDefinesParametersConfiguration
    where T : IDefinesParametersConfiguration<T>
{
    new T WithParameter<TParams>(string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration);
}

public static class DefinesParametersConfigurationExtensions
{
    public static TConfig WithParameter<TConfig>(this TConfig config, string type, string name, Action<IParameterConfiguration> parameterConfiguration)
        where TConfig : IDefinesParametersConfiguration
    {
        config.WithParameter(type, name, parameterConfiguration, (builder, config) => config(builder));
        return config;
    }

    public static TConfig WithParameter<TConfig>(this TConfig config, string type, string name)
        where TConfig : IDefinesParametersConfiguration
    {
        config.WithParameter<object?>(type, name, null, (_, _) => { });
        return config;
    }
}