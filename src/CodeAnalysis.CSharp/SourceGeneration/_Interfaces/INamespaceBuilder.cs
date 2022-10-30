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
    SourceBuilderCodeBlock INamespaceDeclarationBuilder<INamespaceBuilder>.AppendNamespace(string @namespace, out INamespaceBuilder namespaceBuilder) => AppendNamespace(@namespace, out namespaceBuilder);

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
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<INamespaceBuilder>.AppendClass<TParams>(string className, out IClassBuilder classBuilder, TParams @params, Action<IClassConfiguration, TParams> classConfiguration)
        => AppendClass(className, out classBuilder, @params, classConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<INamespaceBuilder>.AppendRecord<TParams>(string recordName, out IRecordBuilder recordBuilder, TParams @params, Action<IRecordConfiguration, TParams> recordConfiguration)
        => AppendRecord(recordName, out recordBuilder, @params, recordConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<INamespaceBuilder>.AppendInterface<TParams>(string interfaceName, out IInterfaceBuilder interfaceBuilder, TParams @params, Action<IInterfaceConfguration, TParams> interfaceConfiguration)
        => AppendInterface(interfaceName, out interfaceBuilder, @params, interfaceConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<INamespaceBuilder>.AppendStruct<TParams>(string structName, out IStructBuilder structBuilder, TParams @params, Action<IStructConfiguration, TParams> structConfiguration)
        => AppendStruct(structName, out structBuilder, @params, structConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<INamespaceBuilder>.AppendEnum<TParams>(string enumName, out IEnumBuilder enumBuilder, TParams @params, Action<IEnumConfiguration, TParams> enumConfiguration)
        => AppendEnum(enumName, out enumBuilder, @params, enumConfiguration);

    /// <inheritdoc/>
    INamespaceBuilder INamespaceMemberDeclarationBuilder<INamespaceBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);
}