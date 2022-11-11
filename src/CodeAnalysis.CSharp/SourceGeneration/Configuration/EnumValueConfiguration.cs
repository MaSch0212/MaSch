namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IEnumValueConfiguration : ISupportsCodeAttributeConfiguration<IEnumValueConfiguration>
{
}

internal sealed class EnumValueConfiguration : CodeConfiguration, IEnumValueConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private readonly string _name;
    private readonly string? _value;

    public EnumValueConfiguration(string name)
        : this(name, null)
    {
    }

    public EnumValueConfiguration(string name, string? value)
    {
        _name = name;
        _value = value;
    }

    protected override int StartCapacity => 16;

    public IEnumValueConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IEnumValueConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

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
    }
}
