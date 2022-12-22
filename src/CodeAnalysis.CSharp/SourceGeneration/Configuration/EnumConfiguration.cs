namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a enum code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IEnumConfiguration : IMemberConfiguration<IEnumConfiguration>, ISupportsInheritanceConfiguration<IEnumConfiguration>
{
}

internal sealed class EnumConfiguration : MemberConfiguration<IEnumConfiguration>, IEnumConfiguration
{
    public EnumConfiguration(string enumName)
        : base(enumName)
    {
    }

    public string? BaseType { get; private set; }

    protected override IEnumConfiguration This => this;
    protected override int StartCapacity => 128;

    public IEnumConfiguration DerivesFrom(string typeName)
    {
        BaseType = typeName;
        return This;
    }

    ISupportsInheritanceConfiguration ISupportsInheritanceConfiguration.DerivesFrom(string typeName)
        => DerivesFrom(typeName);

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("enum ");
        WriteNameTo(sourceBuilder);
        if (BaseType is not null)
            sourceBuilder.Append($" : {BaseType}");
    }
}
