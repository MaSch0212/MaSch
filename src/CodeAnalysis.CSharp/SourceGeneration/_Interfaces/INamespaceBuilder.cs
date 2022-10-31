using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceBuilder :
    INamespaceDeclarationBuilder<INamespaceBuilder>,
    INamespaceImportBuilder<INamespaceBuilder>,
    INamespaceMemberDeclarationBuilder<INamespaceBuilder>
{
}

public partial class SourceBuilder : INamespaceBuilder
{
    /// <inheritdoc/>
    INamespaceBuilder INamespaceImportBuilder<INamespaceBuilder>.AppendNamespaceImport(string @namespace) => AppendNamespaceImport(@namespace);

    /// <inheritdoc/>
    INamespaceBuilder INamespaceImportBuilder<INamespaceBuilder>.AppendNamespaceImport(string @namespace, string alias) => AppendNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    INamespaceBuilder INamespaceImportBuilder<INamespaceBuilder>.AppendStaticNamespaceImport(string @namespace) => AppendStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    INamespaceBuilder INamespaceImportBuilder<INamespaceBuilder>.AppendGlobalNamespaceImport(string @namespace) => AppendGlobalNamespaceImport(@namespace);

    /// <inheritdoc/>
    INamespaceBuilder INamespaceImportBuilder<INamespaceBuilder>.AppendGlobalNamespaceImport(string @namespace, string alias) => AppendGlobalNamespaceImport(@namespace, alias);

    /// <inheritdoc/>
    INamespaceBuilder INamespaceImportBuilder<INamespaceBuilder>.AppendGlobalStaticNamespaceImport(string @namespace) => AppendGlobalStaticNamespaceImport(@namespace);

    /// <inheritdoc/>
    INamespaceBuilder INamespaceMemberDeclarationBuilder<INamespaceBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);
}