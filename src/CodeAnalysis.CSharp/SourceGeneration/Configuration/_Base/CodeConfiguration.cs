namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public abstract class CodeConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets the average count in characters the configuration is in code. This is used as starting capacity when executing the <see cref="ToString"/> method.
    /// </summary>
    protected abstract int StartCapacity { get; }

    /// <inheritdoc/>
    public abstract void WriteTo(ISourceBuilder sourceBuilder);

    /// <inheritdoc/>
    public override string ToString()
    {
        var builder = SourceBuilder.Create(capacity: StartCapacity, autoAddFileHeader: false);
        WriteTo(builder);
        return builder.ToString();
    }
}
