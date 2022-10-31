using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IStructBuilder :
    ITypeMemberDeclarationBuilder<IStructBuilder>,
    INamespaceMemberDeclarationBuilder<IStructBuilder>,
    IConstructorDeclarationBuilder<IClassBuilder>
{
}

public partial class SourceBuilder : IStructBuilder
{
    /// <inheritdoc/>
    IStructBuilder INamespaceMemberDeclarationBuilder<IStructBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);

    /// <inheritdoc/>
    IStructBuilder ITypeMemberDeclarationBuilder<IStructBuilder>.AppendField<TParams>(string fieldTypeName, string fieldName, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration)
        => AppendField(fieldTypeName, fieldName, @params, fieldConfiguration);

    /// <inheritdoc/>
    IStructBuilder ITypeMemberDeclarationBuilder<IStructBuilder>.AppendMethod<TParams>(string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        => AppendMethod(methodName, @params, methodConfiguration);
}