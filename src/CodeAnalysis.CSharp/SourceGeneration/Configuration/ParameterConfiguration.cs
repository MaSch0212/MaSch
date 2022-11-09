namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IParameterConfiguration : ISupportsCodeAttributeConfiguration<IParameterConfiguration>
{
}

internal sealed class ParameterConfiguration : CodeConfiguration, IParameterConfiguration
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

    protected override int StartCapacity => 16;

    public static ParameterConfiguration AddParameter(IList<IParameterConfiguration> parameters, string type, string name, Action<IParameterConfiguration> parameterConfiguration)
    {
        var config = new ParameterConfiguration(type, name) { LineBreakAfterCodeAttribute = true };
        parameterConfiguration?.Invoke(config);
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

    public IParameterConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

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
