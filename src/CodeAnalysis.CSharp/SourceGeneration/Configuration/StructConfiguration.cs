namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a struct code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IStructConfiguration : ITypeConfiguration<IStructConfiguration>
{
}

internal sealed class StructConfiguration : TypeConfiguration<IStructConfiguration>, IStructConfiguration
{
    public StructConfiguration(string structName)
        : base(structName)
    {
    }

    protected override IStructConfiguration This => this;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("struct ");
        WriteNameTo(sourceBuilder);

        sourceBuilder.Indent(sourceBuilder =>
        {
            if (IsDerivingOrImplementingInterface)
                sourceBuilder.Append(' ');
            WriteBaseTypesTo(sourceBuilder);
            WriteGenericConstraintsTo(sourceBuilder);
        });
    }
}
