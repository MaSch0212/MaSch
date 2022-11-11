namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface INamespaceImportConfiguration : ICodeConfiguration
{
    INamespaceImportConfiguration AsStatic();
    INamespaceImportConfiguration AsGlobal();
    INamespaceImportConfiguration WithAlias(string alias);
}

internal sealed class NamespaceImportConfiguration : CodeConfiguration, INamespaceImportConfiguration
{
    private readonly string _namespace;
    private bool _isGlobal;
    private bool _isStatic;
    private string? _alias;

    public NamespaceImportConfiguration(string @namespace)
    {
        _namespace = @namespace;
    }

    protected override int StartCapacity => 24;

    public INamespaceImportConfiguration AsGlobal()
    {
        _isGlobal = true;
        return this;
    }

    public INamespaceImportConfiguration AsStatic()
    {
        _isStatic = true;
        return this;
    }

    public INamespaceImportConfiguration WithAlias(string alias)
    {
        _alias = alias;
        return this;
    }

    public override void WriteTo(ISourceBuilder sourceBuilder)
    {
        if (_isGlobal)
            sourceBuilder.Append("global ");
        sourceBuilder.Append("using ");
        if (_isStatic)
            sourceBuilder.Append("static ");
        if (_alias is not null)
            sourceBuilder.Append($"{_alias} = ");
        sourceBuilder.Append(_namespace);
    }
}
