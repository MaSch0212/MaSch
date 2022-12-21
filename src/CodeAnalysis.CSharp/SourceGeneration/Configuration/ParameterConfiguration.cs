namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IParameterConfiguration : ISupportsCodeAttributeConfiguration<IParameterConfiguration>
{
    IParameterConfiguration WithDefaultValue(string defaultValue);

    void WriteTo(ISourceBuilder sourceBuilder, bool lineBreakAfterCodeAttribute);
}

internal sealed class ParameterConfiguration : CodeConfigurationBase, IParameterConfiguration
{
    private readonly string _type;
    private readonly string _name;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private string? _defaultValue;

    public ParameterConfiguration(string type, string name)
    {
        _type = type;
        _name = name;
    }

    public bool HasAttributes => _codeAttributes.Count > 0;

    protected override int StartCapacity => 16;

    public static ParameterConfiguration AddParameter(IList<IParameterConfiguration> parameters, string type, string name, Action<IParameterConfiguration>? parameterConfiguration)
    {
        var config = new ParameterConfiguration(type, name);
        parameterConfiguration?.Invoke(config);
        parameters.Add(config);
        return config;
    }

    public static void WriteParametersTo(IList<IParameterConfiguration> parameters, ISourceBuilder sourceBuilder, bool multiline)
    {
        for (int i = 0; i < parameters.Count; i++)
        {
            if (multiline)
                sourceBuilder.AppendLine();
            else if (i > 0)
                sourceBuilder.Append(' ');

            parameters[i].WriteTo(sourceBuilder, multiline);
            if (i < parameters.Count - 1)
                sourceBuilder.Append(',');
        }
    }

    public IParameterConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IParameterConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    public IParameterConfiguration WithDefaultValue(string defaultValue)
    {
        _defaultValue = defaultValue;
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
        => WriteTo(sourceBuilder, false);

    public void WriteTo(ISourceBuilder sourceBuilder, bool lineBreakAfterCodeAttribute)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            if (lineBreakAfterCodeAttribute)
                sourceBuilder.AppendLine();
            else
                sourceBuilder.Append(' ');
        }

        sourceBuilder.Append(_type).Append(' ').Append(_name);

        if (_defaultValue != null)
            sourceBuilder.Append(" = ").Append(_defaultValue);
    }
}
