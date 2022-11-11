namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface INamespaceConfiguration : ICodeConfiguration
{
}

internal sealed class NamespaceConfiguration : CodeConfigurationBase, INamespaceConfiguration
{
    private readonly string _name;

    public NamespaceConfiguration(string name)
    {
        _name = name;
    }

    protected override int StartCapacity => 32;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append("namespace ");
        sourceBuilder.Append(_name);
    }
}
