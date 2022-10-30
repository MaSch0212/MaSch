namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceFileBuilder :
    ISourceBuilder<ISourceFileBuilder>,
    ISourceNamespaceImportBuilder<ISourceFileBuilder>,
    ISourceNamespaceDeclarationBuilder<ISourceFileBuilder>
{
    ISourceFileBuilder AppendFileNamespace(string @namespace);
    ISourceFileBuilder AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ISourceCodeAttributeBuilder, TParams> attributeConfiguration);
}

public partial class SourceBuilder : ISourceFileBuilder
{
    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine() => AppendLine();

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine(string value) => AppendLine(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.EnsurePreviousLineEmpty() => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(string value) => Append(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(char value) => Append(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceNamespaceImportBuilder<ISourceFileBuilder>.AppendNamespaceImport(string @namespace) => AppendNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceNamespaceImportBuilder<ISourceFileBuilder>.AppendNamespaceImport(string @namespace, string alias) => AppendNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceNamespaceImportBuilder<ISourceFileBuilder>.AppendStaticNamespaceImport(string @namespace) => AppendStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceNamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalNamespaceImport(string @namespace) => AppendGlobalNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceNamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalNamespaceImport(string @namespace, string alias) => AppendGlobalNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceNamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalStaticNamespaceImport(string @namespace) => AppendGlobalStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.AppendFileNamespace(string @namespace) => AppendFileNamespace(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ISourceCodeAttributeBuilder, TParams> attributeConfiguration) => AppendAssemblyCodeAttribute(attributeTypeName, @params, attributeConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock ISourceNamespaceDeclarationBuilder<ISourceFileBuilder>.AppendNamespace(string @namespace, out ISourceNamespaceBuilder namespaceBuilder) => AppendNamespace(@namespace, out namespaceBuilder);
}