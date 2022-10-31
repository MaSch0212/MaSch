namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceImportBuilder : ISourceBuilder
{
    INamespaceImportBuilder AppendNamespaceImport(string @namespace);
    INamespaceImportBuilder AppendNamespaceImport(string @namespace, string alias);
    INamespaceImportBuilder AppendStaticNamespaceImport(string @namespace);
    INamespaceImportBuilder AppendGlobalNamespaceImport(string @namespace);
    INamespaceImportBuilder AppendGlobalNamespaceImport(string @namespace, string alias);
    INamespaceImportBuilder AppendGlobalStaticNamespaceImport(string @namespace);
}

public interface INamespaceImportBuilder<T> : INamespaceImportBuilder
    where T : INamespaceImportBuilder<T>
{
    new T AppendNamespaceImport(string @namespace);
    new T AppendNamespaceImport(string @namespace, string alias);
    new T AppendStaticNamespaceImport(string @namespace);
    new T AppendGlobalNamespaceImport(string @namespace);
    new T AppendGlobalNamespaceImport(string @namespace, string alias);
    new T AppendGlobalStaticNamespaceImport(string @namespace);
}

public partial class SourceBuilder : INamespaceImportBuilder
{
    /// <inheritdoc/>
    INamespaceImportBuilder INamespaceImportBuilder.AppendGlobalNamespaceImport(string @namespace)
        => AppendGlobalNamespaceImport(@namespace);

    /// <inheritdoc/>
    INamespaceImportBuilder INamespaceImportBuilder.AppendGlobalNamespaceImport(string @namespace, string alias)
        => AppendGlobalNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    INamespaceImportBuilder INamespaceImportBuilder.AppendGlobalStaticNamespaceImport(string @namespace)
        => AppendGlobalStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    INamespaceImportBuilder INamespaceImportBuilder.AppendNamespaceImport(string @namespace)
        => AppendNamespaceImport(@namespace);

    /// <inheritdoc/>
    INamespaceImportBuilder INamespaceImportBuilder.AppendNamespaceImport(string @namespace, string alias)
        => AppendNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    INamespaceImportBuilder INamespaceImportBuilder.AppendStaticNamespaceImport(string @namespace)
        => AppendStaticNamespaceImport(@namespace);
}