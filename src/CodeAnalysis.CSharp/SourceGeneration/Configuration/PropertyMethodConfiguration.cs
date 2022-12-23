namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a constructor code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IPropertyMethodConfiguration : ISupportsCodeAttributeConfiguration<IPropertyMethodConfiguration>, ISupportsAccessModifierConfiguration<IPropertyMethodConfiguration>
{
    /// <summary>
    /// Gets the method keyword (<c>get</c>/<c>set</c>) of the property method represented by this <see cref="IPropertyMethodConfiguration"/>.
    /// </summary>
    string MethodKeyword { get; }

    /// <summary>
    /// Gets the body type of the property method represented by this <see cref="IPropertyMethodConfiguration"/>.
    /// </summary>
    MethodBodyType BodyType { get; }

    /// <summary>
    /// Sets the body type of the property method represented by this <see cref="IPropertyMethodConfiguration"/> to <see cref="MethodBodyType.Expression"/>.
    /// </summary>
    /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
    /// </remarks>
    IPropertyMethodConfiguration AsExpression(bool placeInNewLine = false);
}

internal sealed class PropertyMethodConfiguration : CodeConfigurationBase, IPropertyMethodConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public PropertyMethodConfiguration(string methodKeyword)
    {
        MethodKeyword = methodKeyword;
    }

    public string MethodKeyword { get; internal set; }
    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;
    public AccessModifier AccessModifier { get; private set; } = AccessModifier.Default;
    public IReadOnlyList<ICodeAttributeConfiguration> Attributes => new ReadOnlyCollection<ICodeAttributeConfiguration>(_codeAttributes);

    protected override int StartCapacity => 16;

    public IPropertyMethodConfiguration AsExpression(bool placeInNewLine = false)
    {
        BodyType = placeInNewLine ? MethodBodyType.ExpressionNewLine : MethodBodyType.Expression;
        return this;
    }

    public IPropertyMethodConfiguration WithAccessModifier(AccessModifier accessModifier)
    {
        AccessModifier = accessModifier;
        return this;
    }

    public IPropertyMethodConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    public IPropertyMethodConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Append(AccessModifier.ToMemberPrefix());
        sourceBuilder.Append(MethodKeyword);
    }

    ISupportsAccessModifierConfiguration ISupportsAccessModifierConfiguration.WithAccessModifier(AccessModifier accessModifier)
        => WithAccessModifier(accessModifier);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);
}
