namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class CodeConfigurationBase : ICodeConfiguration
{
    protected abstract int StartCapacity { get; }

    public abstract void WriteTo(ISourceBuilder sourceBuilder);

    public override string ToString()
    {
        var options = new SourceBuilderOptions
        {
            Capacity = StartCapacity,
            IncludeFileHeader = false,
        };
        var builder = SourceBuilder.Create(options);
        WriteTo(builder);
        return builder.ToString();
    }
}
