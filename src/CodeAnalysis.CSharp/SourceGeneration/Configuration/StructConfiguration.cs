namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IStructConfiguration : ITypeConfiguration<IStructConfiguration>
{
}

public sealed class StructConfiguration : TypeConfiguration<IStructConfiguration>, IStructConfiguration
{
    public StructConfiguration(string structName)
        : base(structName)
    {
    }

    protected override IStructConfiguration This => this;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("struct ");
        WriteNameTo(sourceBuilder);
        WriteBaseTypesTo(sourceBuilder);
        WriteGenericConstraintsTo(sourceBuilder);
    }
}
