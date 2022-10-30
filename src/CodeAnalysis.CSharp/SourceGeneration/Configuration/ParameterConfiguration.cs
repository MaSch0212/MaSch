namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

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

    public bool LinebreakAfterCodeAttribute { get; set; } = false;

    protected override int StartCapacity => 16;

    public IParameterConfiguration WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
    {
        var codeAttribute = new CodeAttributeConfiguration(attributeTypeName);
        attributeConfiguration?.Invoke(codeAttribute, @params);
        _codeAttributes.Add(codeAttribute);
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            if (LinebreakAfterCodeAttribute)
                sourceBuilder.AppendLine();
            else
                sourceBuilder.Append(' ');
        }

        sourceBuilder.Append(_type).Append(' ').Append(_name);
    }
}
