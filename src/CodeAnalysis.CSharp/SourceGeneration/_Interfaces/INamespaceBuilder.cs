using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceBuilder :
    INamespaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
    INamespaceImportBuilder<INamespaceBuilder>,
    INamespaceMemberDeclarationBuilder<INamespaceBuilder>,
    IClassDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>
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

    INamespaceBuilder INamespaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, INamespaceConfiguration> createFunc)
        => Append(createFunc(_configurationFactory), null);

    INamespaceBuilder INamespaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc)
        => Append(createFunc(_configurationFactory), builderFunc);

    INamespaceBuilder IClassDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc)
        => Append(createFunc(_configurationFactory), builderFunc);
}