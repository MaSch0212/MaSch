namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IClassConfiguration : ITypeConfiguration<IClassConfiguration>
{
}

public sealed class ClassConfiguration : TypeConfiguration<IClassConfiguration>, IClassConfiguration
{
    public ClassConfiguration(string className)
        : base(className)
    {
    }

    protected override IClassConfiguration This => this;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("class ");
        WriteNameTo(sourceBuilder);
        WriteBaseTypesTo(sourceBuilder);
        WriteGenericConstraintsTo(sourceBuilder);
    }
}
