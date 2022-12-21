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
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("struct ");
        WriteNameTo(sourceBuilder);

        sourceBuilder.Indent(sourceBuilder =>
        {
            if (HasBaseTypes)
                sourceBuilder.Append(' ');
            WriteBaseTypesTo(sourceBuilder);
            WriteGenericConstraintsTo(sourceBuilder);
        });
    }
}
