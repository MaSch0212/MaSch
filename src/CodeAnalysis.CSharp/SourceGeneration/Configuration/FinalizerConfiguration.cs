namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IFinalizerConfiguration : ISupportsCodeAttributeConfiguration<IFinalizerConfiguration>
{
}

internal sealed class FinalizerConfiguration : CodeConfiguration, IFinalizerConfiguration
{
    private readonly string _containingTypeName;
    private readonly List<ICodeAttributeConfiguration> _codeAttributes = new();

    public FinalizerConfiguration(string containingTypeName)
    {
        _containingTypeName = containingTypeName;
    }

    protected override int StartCapacity => 16;

    public IFinalizerConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
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

        sourceBuilder.Append('~').Append(_containingTypeName).Append("()");
    }

    ISupportsCodeAttributeConfiguration ISupportsCodeAttributeConfiguration.WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration)
        => WithCodeAttribute(attributeTypeName, attributeConfiguration);
}
