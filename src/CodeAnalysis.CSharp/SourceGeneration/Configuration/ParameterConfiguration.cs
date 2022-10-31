using System.Reflection.Metadata;
namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IParameterConfiguration : ISupportsCodeAttributeConfiguration<IParameterConfiguration>
{
}

public class ParameterConfiguration : CodeConfiguration, IParameterConfiguration
{
    private readonly string _type;
    private readonly string _name;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public ParameterConfiguration(string type, string name)
    {
        _type = type;
        _name = name;
    }

    public bool LineBreakAfterCodeAttribute { get; set; } = false;

    /// <inheritdoc/>
    protected override int StartCapacity => 16;

    public static ParameterConfiguration AddParameter<TParams>(IList<IParameterConfiguration> parameters, string type, string name, TParams @params, Action<IParameterConfiguration, TParams> parameterConfiguration)
    {
        var config = new ParameterConfiguration(type, name) { LineBreakAfterCodeAttribute = true };
        parameterConfiguration?.Invoke(config, @params);
        parameters.Add(config);
        return config;
    }

    public static void WriteParametersTo(IList<IParameterConfiguration> parameters, ISourceBuilder sourceBuilder)
    {
        if (parameters.Count == 0)
            return;

        using (sourceBuilder.Indent())
        {
            sourceBuilder.Append('(');
            for (int i = 0; i < parameters.Count; i++)
            {
                sourceBuilder.AppendLine();
                parameters[i].WriteTo(sourceBuilder);
                if (i < parameters.Count - 1)
                    sourceBuilder.Append(',');
            }

            sourceBuilder.Append(')');
        }
    }

    /// <inheritdoc/>
    public IParameterConfiguration WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, @params, attributeConfiguration);
        return this;
    }

    /// <inheritdoc/>
    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
        => WithCodeAttribute<TParams>(attributeTypeName, @params, attributeConfiguration);

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            if (LineBreakAfterCodeAttribute)
                sourceBuilder.AppendLine();
            else
                sourceBuilder.Append(' ');
        }

        sourceBuilder.Append(_type).Append(' ').Append(_name);
    }
}
