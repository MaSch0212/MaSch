namespace MaSch.Generators.Support
{
    public interface ISourceNamespaceImportBuilder<T> : ISourceBuilder
        where T : ISourceNamespaceImportBuilder<T>
    {
        T AppendNamespaceImport(string @namespace);
        T AppendNamespaceImport(string @namespace, string alias);
        T AppendStaticNamespaceImport(string @namespace);
        T AppendGlobalNamespaceImport(string @namespace);
        T AppendGlobalNamespaceImport(string @namespace, string alias);
        T AppendGlobalStaticNamespaceImport(string @namespace);
    }
}