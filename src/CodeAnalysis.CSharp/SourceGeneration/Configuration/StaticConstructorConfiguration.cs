namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a static constructor code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IStaticConstructorConfiguration : ISupportsCodeAttributeConfiguration<IStaticConstructorConfiguration>
{
    /// <summary>
    /// Gets the containing type name for the static constructor represented by this <see cref="IStaticConstructorConfiguration"/>.
    /// </summary>
    /// <remarks>
    /// If this is <c>null</c>, the containing type name is determined using the <see cref="ISourceBuilder.CurrentTypeName"/> property of the <see cref="ISourceBuilder"/> this <see cref="IStaticConstructorConfiguration"/> is written to.
    /// </remarks>
    string? ContainingTypeName { get; }

    /// <summary>
    /// Gets the body type of the static constructor represented by this <see cref="IStaticConstructorConfiguration"/>.
    /// </summary>
    MethodBodyType BodyType { get; }

    /// <summary>
    /// Sets the body type of the static constructor represented by this <see cref="IStaticConstructorConfiguration"/> to <see cref="MethodBodyType.Expression"/>/<see cref="MethodBodyType.ExpressionNewLine"/>.
    /// </summary>
    /// <param name="placeInNewLine">Determines whether to use <see cref="MethodBodyType.Expression"/> or <see cref="MethodBodyType.ExpressionNewLine"/>.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    /// <remarks>
    /// If <paramref name="placeInNewLine"/> is set to <c>true</c> <see cref="MethodBodyType.ExpressionNewLine"/> is used; otherwise <see cref="MethodBodyType.Expression"/>.
    /// </remarks>
    IStaticConstructorConfiguration AsExpression(bool placeInNewLine = true);
}

internal sealed class StaticConstructorConfiguration : CodeConfigurationBase, IStaticConstructorConfiguration
{
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public StaticConstructorConfiguration(string? containingTypeName)
    {
        ContainingTypeName = containingTypeName;
    }

    public string? ContainingTypeName { get; }
    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;
    public IReadOnlyList<ICodeAttributeConfiguration> Attributes => new ReadOnlyCollection<ICodeAttributeConfiguration>(_codeAttributes);

    protected override int StartCapacity => 16;

    public IStaticConstructorConfiguration AsExpression(bool placeInNewLine = true)
    {
        BodyType = placeInNewLine ? MethodBodyType.ExpressionNewLine : MethodBodyType.Expression;
        return this;
    }

    public IStaticConstructorConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
    {
        CodeAttributeConfiguration.AddCodeAttribute(_codeAttributes, attributeTypeName, attributeConfiguration);
        return this;
    }

    public IStaticConstructorConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        foreach (var codeAttribute in _codeAttributes)
        {
            codeAttribute.WriteTo(sourceBuilder);
            sourceBuilder.AppendLine();
        }

        sourceBuilder.Append("static ").Append(ContainingTypeName ?? sourceBuilder.CurrentTypeName).Append("()");
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);
}