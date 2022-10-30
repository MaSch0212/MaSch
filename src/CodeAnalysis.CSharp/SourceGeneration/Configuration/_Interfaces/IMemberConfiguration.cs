namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IMemberConfiguration<T> : ISupportsCodeAttributeConfiguration<T>
    where T : IMemberConfiguration<T>
{
    T WithAccessModifier(AccessModifier accessModifier);
    T WithKeyword(MemberKeyword keyword);
    T WithGenericParameter<TParams>(string name, TParams @params, Action<IGenericParameterConfiguration, TParams> parameterConfiguration);
}

public static class MemberConfigurationExtensions
{
    public static TConfig WithGenericParameter<TConfig>(this TConfig config, string name, Action<IGenericParameterConfiguration> parameterConfiguration)
        where TConfig : IMemberConfiguration<TConfig>
        => config.WithGenericParameter(name, parameterConfiguration, (builder, config) => config(builder));

    public static TConfig WithGenericParameter<TConfig>(this TConfig config, string name)
        where TConfig : IMemberConfiguration<TConfig>
        => config.WithGenericParameter<object?>(name, null, (_, _) => { });
}