namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a namespace import code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface INamespaceImportConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets the name of the namespace to import with the namespace import represented by this <see cref="INamespaceImportConfiguration"/>.
    /// </summary>
    string NamespaceName { get; }

    /// <summary>
    /// Gets a value indicating whether the namespace import represented by this <see cref="INamespaceImportConfiguration"/> is global.
    /// </summary>
    bool IsGlobal { get; }

    /// <summary>
    /// Gets a value indicating whether the namespace import represented by this <see cref="INamespaceImportConfiguration"/> is static.
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Gets the alias of the namespace import represented by this <see cref="INamespaceImportConfiguration"/>.
    /// </summary>
    string? Alias { get; }

    /// <summary>
    /// Sets the namespace import represented by this <see cref="INamespaceImportConfiguration"/> to a static namespace import.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    INamespaceImportConfiguration AsStatic();

    /// <summary>
    /// Sets the namespace import represented by this <see cref="INamespaceImportConfiguration"/> to a global namespace import.
    /// </summary>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    INamespaceImportConfiguration AsGlobal();

    /// <summary>
    /// Sets the alias of the namespace import represented by this <see cref="INamespaceImportConfiguration"/>.
    /// </summary>
    /// <param name="alias">The alias to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    INamespaceImportConfiguration WithAlias(string alias);
}

internal sealed class NamespaceImportConfiguration : CodeConfigurationBase, INamespaceImportConfiguration
{
    public NamespaceImportConfiguration(string @namespace)
    {
        NamespaceName = @namespace;
    }

    public string NamespaceName { get; }
    public bool IsGlobal { get; private set; }
    public bool IsStatic { get; private set; }
    public string? Alias { get; private set; }

    protected override int StartCapacity => 24;

    public INamespaceImportConfiguration AsGlobal()
    {
        IsGlobal = true;
        return this;
    }

    public INamespaceImportConfiguration AsStatic()
    {
        IsStatic = true;
        return this;
    }

    public INamespaceImportConfiguration WithAlias(string alias)
    {
        Alias = alias;
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        if (IsGlobal)
            sourceBuilder.Append("global ");
        sourceBuilder.Append("using ");
        if (IsStatic)
            sourceBuilder.Append("static ");
        if (Alias is not null)
            sourceBuilder.Append($"{Alias} = ");
        sourceBuilder.Append(NamespaceName);
    }
}
