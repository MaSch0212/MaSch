namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IParameterConfiguration : ISupportsCodeAttributeConfiguration<IParameterConfiguration>
{
    void WriteTo(ISourceBuilder sourceBuilder, bool lineBreakAfterCodeAttribute);
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
        if (parameters.Count == 0)
            return;

        using (sourceBuilder.Indent())
        {
            sourceBuilder.Append('(');
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

            sourceBuilder.Append(')');
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
    }
}
