namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IClassConfiguration : ITypeConfiguration<IClassConfiguration>
{
}

internal sealed class ClassConfiguration : TypeConfiguration<IClassConfiguration>, IClassConfiguration
{
    public ClassConfiguration(string className)
        : base(className)
    {
    }

    /// <inheritdoc/>
    protected override IClassConfiguration This => this;

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("class ");
        WriteNameTo(sourceBuilder);

        using (sourceBuilder.Indent())
        {
            if (HasBaseTypes)
                sourceBuilder.Append(' ');
            WriteBaseTypesTo(sourceBuilder);
            WriteGenericConstraintsTo(sourceBuilder);
        }
    }
}
