using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IEnumBuilder : ISourceBuilder
{
    IEnumBuilder AppendValue<TParams>(string name, TParams @params, Action<IEnumValueConfiguration, TParams> enumValueConfiguration);
}

public partial class SourceBuilder : IEnumBuilder
{
    /// <inheritdoc/>
    IEnumBuilder IEnumBuilder.AppendValue<TParams>(string name, TParams @params, Action<IEnumValueConfiguration, TParams> enumValueConfiguration)
        => AppendValue(name, @params, enumValueConfiguration);
}

public static class EnumBuilderExtensions
{
    public static IEnumBuilder AppendValue<TParams>(this IEnumBuilder builder, string name, string value, TParams @params, Action<IEnumValueConfiguration, TParams> enumValueConfiguration)
    {
        return builder.AppendValue(
            name,
            (value, @params, enumValueConfiguration),
            static (config, @params) =>
            {
                config.WithValue(@params.value);
                @params.enumValueConfiguration?.Invoke(config, @params.@params);
            });
    }

    public static IEnumBuilder AppendValue(this IEnumBuilder builder, string name, Action<IEnumValueConfiguration> enumValueConfiguration)
        => builder.AppendValue(name, enumValueConfiguration, static (builder, config) => config?.Invoke(builder));

    public static IEnumBuilder AppendValue(this IEnumBuilder builder, string name, string value, Action<IEnumValueConfiguration> enumValueConfiguration)
    {
        return builder.AppendValue(
            name,
            (value, enumValueConfiguration),
            static (config, @params) =>
            {
                config.WithValue(@params.value);
                @params.enumValueConfiguration?.Invoke(config);
            });
    }

    public static IEnumBuilder AppendValue(this IEnumBuilder builder, string name)
        => builder.AppendValue<object?>(name, null, (_, _) => { });

    public static IEnumBuilder AppendValue(this IEnumBuilder builder, string name, string value)
        => builder.AppendValue(name, value, static (config, value) => config.WithValue(value));
}