namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class CodeConfiguration : ICodeConfiguration
{
    protected abstract int StartCapacity { get; }

    public abstract void WriteTo(ISourceBuilder sourceBuilder);

    public override string ToString()
    {
        var builder = SourceBuilder.Create(capacity: StartCapacity, autoAddFileHeader: false);
        WriteTo(builder);
        return builder.ToString();
    }
}
