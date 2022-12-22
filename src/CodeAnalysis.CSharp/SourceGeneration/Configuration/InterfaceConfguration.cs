namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of an interface code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IInterfaceConfguration : ITypeConfiguration<IInterfaceConfguration>
{
}

internal sealed class InterfaceConfguration : TypeConfiguration<IInterfaceConfguration>, IInterfaceConfguration
{
    public InterfaceConfguration(string interfaceName)
        : base(interfaceName)
    {
    }

    protected override IInterfaceConfguration This => this;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCommentsTo(sourceBuilder);
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("interface ");
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
