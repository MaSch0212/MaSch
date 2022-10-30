namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceImportBuilder<T> : ISourceBuilder
    where T : INamespaceImportBuilder<T>
{
    T AppendNamespaceImport(string @namespace);
    T AppendNamespaceImport(string @namespace, string alias);
    T AppendStaticNamespaceImport(string @namespace);
    T AppendGlobalNamespaceImport(string @namespace);
    T AppendGlobalNamespaceImport(string @namespace, string alias);
    T AppendGlobalStaticNamespaceImport(string @namespace);
}