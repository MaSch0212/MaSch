namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IFinalizerConfiguration : ISupportsCodeAttributeConfiguration<IFinalizerConfiguration>
{
    MethodBodyType BodyType { get; }

    IFinalizerConfiguration AsExpression(bool placeInNewLine = true);
}

internal sealed class FinalizerConfiguration : CodeConfigurationBase, IFinalizerConfiguration
{
    private readonly string? _containingTypeName;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public FinalizerConfiguration(string? containingTypeName)
    {
        _containingTypeName = containingTypeName;
    }

    public MethodBodyType BodyType { get; private set; } = MethodBodyType.Block;

    protected override int StartCapacity => 16;

    public IFinalizerConfiguration AsExpression(bool placeInNewLine = true)
    {
        BodyType = placeInNewLine ? MethodBodyType.ExpressionNewLine : MethodBodyType.Expression;
        return this;
    }

    public IFinalizerConfiguration WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);

    public IFinalizerConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration>? attributeConfiguration)
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

        sourceBuilder.Append('~').Append(_containingTypeName ?? sourceBuilder.CurrentTypeName).Append("()");
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName)
        => WithCodeAttribute(attributeTypeName, null);
}
