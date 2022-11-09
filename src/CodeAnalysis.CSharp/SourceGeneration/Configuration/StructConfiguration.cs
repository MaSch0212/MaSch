namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IStructConfiguration : ITypeConfiguration<IStructConfiguration>
{
}

internal sealed class StructConfiguration : TypeConfiguration<IStructConfiguration>, IStructConfiguration
{
    public StructConfiguration(string structName)
        : base(structName)
    {
    }

    /// <inheritdoc/>
    protected override IStructConfiguration This => this;

    /// <inheritdoc/>
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
