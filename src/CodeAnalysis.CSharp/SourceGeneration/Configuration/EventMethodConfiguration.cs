namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a event method code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IEventMethodConfiguration : ISupportsCodeAttributeConfiguration<IEventMethodConfiguration>
{
    /// <summary>
    /// Gets the body type of the event method represented by this <see cref="IEventMethodConfiguration"/>.
    /// </summary>
    MethodBodyType BodyType { get; }

    /// <summary>
    /// Sets the body type of the event method represented by this <see cref="IEventMethodConfiguration"/> to <see cref="MethodBodyType.Expression"/>.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
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
    public IReadOnlyList<ICodeAttributeConfiguration> Attributes => new ReadOnlyCollection<ICodeAttributeConfiguration>(_codeAttributes);

    protected override int StartCapacity => 16;

    public IEventMethodConfiguration AsExpression()
    {
        BodyType = MethodBodyType.Expression;
        return this;
    }

    public IEventMethodConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IEventMethodConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

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
