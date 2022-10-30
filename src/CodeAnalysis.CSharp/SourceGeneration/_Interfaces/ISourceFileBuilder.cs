namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceFileBuilder :
    ISourceBuilder<ISourceFileBuilder>,
    INamespaceImportBuilder<ISourceFileBuilder>,
    INamespaceDeclarationBuilder<ISourceFileBuilder>
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
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendNamespaceImport(string @namespace) => AppendNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendNamespaceImport(string @namespace, string alias) => AppendNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendStaticNamespaceImport(string @namespace) => AppendStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalNamespaceImport(string @namespace) => AppendGlobalNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalNamespaceImport(string @namespace, string alias) => AppendGlobalNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.AppendGlobalStaticNamespaceImport(string @namespace) => AppendGlobalStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.AppendFileNamespace(string @namespace) => AppendFileNamespace(@namespace);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ISourceCodeAttributeBuilder, TParams> attributeConfiguration) => AppendAssemblyCodeAttribute(attributeTypeName, @params, attributeConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceDeclarationBuilder<ISourceFileBuilder>.AppendNamespace(string @namespace, out INamespaceBuilder namespaceBuilder) => AppendNamespace(@namespace, out namespaceBuilder);
}