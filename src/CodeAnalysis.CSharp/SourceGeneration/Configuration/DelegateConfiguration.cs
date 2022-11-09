namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IDelegateConfiguration : IMethodConfiguration<IDelegateConfiguration>
{
}

internal sealed class DelegateConfiguration : MethodConfiguration<IDelegateConfiguration>, IDelegateConfiguration
{
    public DelegateConfiguration(string delegateName)
        : base(delegateName)
    {
    }

    /// <inheritdoc/>
    protected override int StartCapacity => 128;

    /// <inheritdoc/>
    protected override IDelegateConfiguration This => this;

    /// <inheritdoc/>
    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteCodeAttributesTo(sourceBuilder);
        WriteKeywordsTo(sourceBuilder);
        sourceBuilder.Append("delegate ");
        WriteReturnTypeTo(sourceBuilder);
        WriteNameTo(sourceBuilder);
        WriteParametersTo(sourceBuilder);
        WriteGenericConstraintsTo(sourceBuilder);
    }
}
