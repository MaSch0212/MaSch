namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface IRegionConfiguration : ICodeConfiguration
{
    string Name { get; }

    void WriteStartTo(ISourceBuilder sourceBuilder);
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
