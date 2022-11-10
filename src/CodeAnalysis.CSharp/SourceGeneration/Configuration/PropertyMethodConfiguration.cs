namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IPropertyMethodConfiguration : ISupportsCodeAttributeConfiguration<IPropertyMethodConfiguration>, ISupportsAccessModifierConfiguration<IPropertyMethodConfiguration>
{
    bool ShouldBeOnItsOwnLine { get; }
    MethodBodyType BodyType { get; }

    IPropertyMethodConfiguration AsExpression();
}

internal sealed class PropertyMethodConfiguration : CodeConfiguration, IPropertyMethodConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();
    private AccessModifier _accessModifier = AccessModifier.Default;

    public PropertyMethodConfiguration(string methodKeyword)
    {
        MethodKeyword = methodKeyword;
    }

    public bool ShouldBeOnItsOwnLine => _codeAttributes.Count > 0;
    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;
    public string MethodKeyword { get; set; }

    protected override int StartCapacity => 16;

    public IPropertyMethodConfiguration AsExpression()
    {
        BodyType = MethodBodyType.Expression;
        return this;
    }

    public IPropertyMethodConfiguration WithAccessModifier(AccessModifier accessModifier)
    {
        _accessModifier = accessModifier;
        return this;
    }

    public IPropertyMethodConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Append(_accessModifier.ToMemberPrefix());
        sourceBuilder.Append(MethodKeyword);
    }

    ISupportsAccessModifierConfiguration ISupportsAccessModifierConfiguration.WithAccessModifier(AccessModifier accessModifier)
        => WithAccessModifier(accessModifier);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);
}
