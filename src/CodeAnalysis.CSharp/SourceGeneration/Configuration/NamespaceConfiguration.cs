namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a namespace code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface INamespaceConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets the name of the namespace represented by this <see cref="INamespaceConfiguration"/>.
    /// </summary>
    string Name { get; }
}

internal sealed class NamespaceConfiguration : CodeConfigurationBase, INamespaceConfiguration
{
    public NamespaceConfiguration(string name)
    {
        Name = name;
    }

    public string Name { get; }

    protected override int StartCapacity => 32;

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        sourceBuilder.Append("namespace ");
        sourceBuilder.Append(Name);
    }
}
