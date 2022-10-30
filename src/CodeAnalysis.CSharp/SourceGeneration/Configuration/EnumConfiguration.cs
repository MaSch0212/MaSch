namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IEnumConfiguration : ITypeConfiguration<IEnumConfiguration>
{
}

public sealed class EnumConfiguration : TypeConfiguration<IEnumConfiguration>, IEnumConfiguration
{
    public EnumConfiguration(string enumName)
        : base(enumName)
    {
    }

    protected override IEnumConfiguration This => this;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("enum ");
        WriteNameTo(sourceBuilder);
        WriteBaseTypesTo(sourceBuilder);
    }
}
