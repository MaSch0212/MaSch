namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal abstract class CodeConfigurationBase : ICodeConfiguration
{
    protected abstract int StartCapacity { get; }

    public abstract void WriteTo(ISourceBuilder sourceBuilder);

    public override string ToString()
    {
        var builder = CreateSourceBuilder(StartCapacity);
        WriteTo(builder);
        return builder.ToString();
    }

    protected virtual ISourceBuilder CreateSourceBuilder(int capacity)
    {
        var options = new SourceBuilderOptions
        {
            Capacity = capacity,
            IncludeFileHeader = false,
        };
        var builder = SourceBuilder.Create(options);
        builder.CurrentTypeName = "[ClassName]";
        return builder;
    }
}
