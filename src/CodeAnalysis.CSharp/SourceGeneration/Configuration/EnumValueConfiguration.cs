namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IEnumValueConfiguration : ISupportsCodeAttributeConfiguration<IEnumValueConfiguration>
{
    IEnumValueConfiguration WithValue(string value);
}

public sealed class EnumValueConfiguration : CodeConfiguration, IEnumValueConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private readonly string _name;
    private string _value;

    public EnumValueConfiguration(string name)
    {
        _name = name;
    }

    /// <inheritdoc/>
    protected override int StartCapacity => 16;

    /// <inheritdoc/>
    public IEnumValueConfiguration WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, @params, attributeConfiguration);
        return this;
    }

    /// <inheritdoc/>
    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, @params, attributeConfiguration);

    /// <inheritdoc/>
    public IEnumValueConfiguration WithValue(string value)
    {
        _value = value;
        return this;
    }

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var attribute in _codeAttributes)
        {
            attribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Append(_name);

        if (_value is not null)
            sourceBuilder.Append($" = {_value}");

        sourceBuilder.Append(',');
    }
}
