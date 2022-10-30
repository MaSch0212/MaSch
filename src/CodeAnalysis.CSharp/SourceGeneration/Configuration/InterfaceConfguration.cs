namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IInterfaceConfguration : ITypeConfiguration<IInterfaceConfguration>
{
}

public sealed class InterfaceConfguration : TypeConfiguration<IInterfaceConfguration>, IInterfaceConfguration
{
    public InterfaceConfguration(string interfaceName)
        : base(interfaceName)
    {
    }

    protected override IInterfaceConfguration This => this;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("interface ");
        WriteNameTo(sourceBuilder);
        WriteBaseTypesTo(sourceBuilder);
        WriteGenericConstraintsTo(sourceBuilder);
    }
}
