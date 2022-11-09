namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IEnumConfiguration : IMemberConfiguration<IEnumConfiguration>, ISupportsInheritanceConfiguration<IEnumConfiguration>
{
}

internal sealed class EnumConfiguration : MemberConfiguration<IEnumConfiguration>, IEnumConfiguration
{
    private string _baseType;

    public EnumConfiguration(string enumName)
        : base(enumName)
    {
    }

    /// <inheritdoc/>
    protected override IEnumConfiguration This => this;

    /// <inheritdoc/>
    protected override int StartCapacity => 128;

    /// <inheritdoc/>
    public IEnumConfiguration DerivesFrom(string typeName)
    {
        _baseType = typeName;
        return This;
    }

    /// <inheritdoc/>
    ISupportsInheritanceConfiguration ISupportsInheritanceConfiguration.DerivesFrom(string typeName)
        => DerivesFrom(typeName);

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("enum ");
        WriteNameTo(sourceBuilder);
        if (_baseType is not null)
            sourceBuilder.Append($" : {_baseType}");
    }
}
