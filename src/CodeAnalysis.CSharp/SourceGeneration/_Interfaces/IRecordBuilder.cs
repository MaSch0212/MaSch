using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IRecordBuilder :
    ITypeMemberDeclarationBuilder<IRecordBuilder>,
    INamespaceMemberDeclarationBuilder<IRecordBuilder>,
    IConstructorDeclarationBuilder<IClassBuilder>
{
}

public partial class SourceBuilder : IRecordBuilder
{
    /// <inheritdoc/>
    IRecordBuilder INamespaceMemberDeclarationBuilder<IRecordBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);

    /// <inheritdoc/>
    IRecordBuilder ITypeMemberDeclarationBuilder<IRecordBuilder>.AppendField<TParams>(string fieldTypeName, string fieldName, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration)
        => AppendField(fieldTypeName, fieldName, @params, fieldConfiguration);

    /// <inheritdoc/>
    IRecordBuilder ITypeMemberDeclarationBuilder<IRecordBuilder>.AppendMethod<TParams>(string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        => AppendMethod(methodName, @params, methodConfiguration);
}