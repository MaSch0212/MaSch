namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IRecordBuilder :
    ITypeMemberDeclarationBuilder<IRecordBuilder>,
    INamespaceMemberDeclarationBuilder<IRecordBuilder>
{
}

public partial class SourceBuilder : IRecordBuilder
{
    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IRecordBuilder>.AppendClass<TParams>(string className, out IClassBuilder classBuilder, TParams @params, Action<IClassConfiguration, TParams> classConfiguration)
        => AppendClass(className, out classBuilder, @params, classConfiguration);

    /// <inheritdoc/>
    IRecordBuilder INamespaceMemberDeclarationBuilder<IRecordBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IRecordBuilder>.AppendEnum<TParams>(string enumName, out IEnumBuilder enumBuilder, TParams @params, Action<IEnumConfiguration, TParams> enumConfiguration)
        => AppendEnum(enumName, out enumBuilder, @params, enumConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IRecordBuilder>.AppendInterface<TParams>(string interfaceName, out IInterfaceBuilder interfaceBuilder, TParams @params, Action<IInterfaceConfguration, TParams> interfaceConfiguration)
        => AppendInterface(interfaceName, out interfaceBuilder, @params, interfaceConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IRecordBuilder>.AppendRecord<TParams>(string recordName, out IRecordBuilder recordBuilder, TParams @params, Action<IRecordConfiguration, TParams> recordConfiguration)
        => AppendRecord(recordName, out recordBuilder, @params, recordConfiguration);

    /// <inheritdoc/>
    SourceBuilderCodeBlock INamespaceMemberDeclarationBuilder<IRecordBuilder>.AppendStruct<TParams>(string structName, out IStructBuilder structBuilder, TParams @params, Action<IStructConfiguration, TParams> structConfiguration)
        => AppendStruct(structName, out structBuilder, @params, structConfiguration);
}