namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IClassBuilder :
    ITypeMemberDeclarationBuilder<IClassBuilder>,
    INamespaceMemberDeclarationBuilder<IClassBuilder>
{
}

public partial class SourceBuilder : IClassBuilder
{
    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IClassBuilder>.AppendClass<TParams>(string className, out IClassBuilder classBuilder, TParams @params, Action<IClassConfiguration, TParams> classConfiguration)
        => AppendClass(className, out classBuilder, @params, classConfiguration);

    /// <inheritdoc/>
    IClassBuilder INamespaceMemberDeclarationBuilder<IClassBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IClassBuilder>.AppendEnum<TParams>(string enumName, out IEnumBuilder enumBuilder, TParams @params, Action<IEnumConfiguration, TParams> enumConfiguration)
        => AppendEnum(enumName, out enumBuilder, @params, enumConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IClassBuilder>.AppendInterface<TParams>(string interfaceName, out IInterfaceBuilder interfaceBuilder, TParams @params, Action<IInterfaceConfguration, TParams> interfaceConfiguration)
        => AppendInterface(interfaceName, out interfaceBuilder, @params, interfaceConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IClassBuilder>.AppendRecord<TParams>(string recordName, out IRecordBuilder recordBuilder, TParams @params, Action<IRecordConfiguration, TParams> recordConfiguration)
        => AppendRecord(recordName, out recordBuilder, @params, recordConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IClassBuilder>.AppendStruct<TParams>(string structName, out IStructBuilder structBuilder, TParams @params, Action<IStructConfiguration, TParams> structConfiguration)
        => AppendStruct(structName, out structBuilder, @params, structConfiguration);
}