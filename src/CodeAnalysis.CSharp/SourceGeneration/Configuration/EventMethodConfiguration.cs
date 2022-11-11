namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IEventMethodConfiguration : ISupportsCodeAttributeConfiguration<IEventMethodConfiguration>
{
    MethodBodyType BodyType { get; }

    IEventMethodConfiguration AsExpression();
}

internal sealed class EventMethodConfiguration : CodeConfigurationBase, IEventMethodConfiguration
{
    private readonly string _methodKeyword;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public EventMethodConfiguration(string methodKeyword)
    {
        _methodKeyword = methodKeyword;
    }

    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;

    protected override int StartCapacity => 16;

    public IEventMethodConfiguration AsExpression()
    {
        BodyType = MethodBodyType.Expression;
        return this;
    }

    public IEventMethodConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IEventMethodConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
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
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Append(_methodKeyword);
    }
}
