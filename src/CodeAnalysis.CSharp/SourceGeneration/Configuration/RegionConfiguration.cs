namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a region code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface IRegionConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets the name of the region represented by this <see cref="IRegionConfiguration"/>.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Writes the start of the region represented by this <see cref="IRegionConfiguration"/> to the target <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
    void WriteStartTo(ISourceBuilder sourceBuilder);

    /// <summary>
    /// Writes the end of the region represented by this <see cref="IRegionConfiguration"/> to the target <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
    void WriteEndTo(ISourceBuilder sourceBuilder);
}

internal class RegionConfiguration : IRegionConfiguration
{
    public RegionConfiguration(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public void WriteStartTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.EnsureCurrentLineEmpty();
        sourceBuilder.AppendLine($"#region {Name}");
    }

    public void WriteEndTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.EnsureCurrentLineEmpty();
        sourceBuilder.AppendLine("#endregion");
    }

    public void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteStartTo(sourceBuilder);
        WriteEndTo(sourceBuilder);
    }
}
