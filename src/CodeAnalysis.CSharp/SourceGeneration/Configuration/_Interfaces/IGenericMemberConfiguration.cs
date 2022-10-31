namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IGenericMemberConfiguration : IMemberConfiguration
{
    string MemberNameWithoutGenericParameters { get; }

    IGenericMemberConfiguration WithGenericParameter<TParams>(string name, TParams @params, Action<IGenericParameterConfiguration, TParams> parameterConfiguration);
}

public interface IGenericMemberConfiguration<T> : IGenericMemberConfiguration, IMemberConfiguration<T>
    where T : IGenericMemberConfiguration<T>
{
    T WithGenericParameter<TParams>(string name, TParams @params, Action<IGenericParameterConfiguration, TParams> parameterConfiguration);
}

public static class GenericMemberConfigurationExtensions
{
    public static TConfig WithGenericParameter<TConfig>(this TConfig config, string name, Action<IGenericParameterConfiguration> parameterConfiguration)
        where TConfig : IGenericMemberConfiguration
    {
        config.WithGenericParameter(name, parameterConfiguration, (builder, config) => config(builder));
        return config;
    }

    public static TConfig WithGenericParameter<TConfig>(this TConfig config, string name)
        where TConfig : IGenericMemberConfiguration
    {
        config.WithGenericParameter<object?>(name, null, (_, _) => { });
        return config;
    }
}
