using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IInterfaceBuilder :
    ITypeMemberDeclarationBuilder<IInterfaceBuilder>,
    INamespaceMemberDeclarationBuilder<IInterfaceBuilder>
{
}

public partial class SourceBuilder : IInterfaceBuilder
{
    /// <inheritdoc/>
    IInterfaceBuilder INamespaceMemberDeclarationBuilder<IInterfaceBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);

    /// <inheritdoc/>
    IInterfaceBuilder ITypeMemberDeclarationBuilder<IInterfaceBuilder>.AppendField<TParams>(string fieldTypeName, string fieldName, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration)
        => AppendField(fieldTypeName, fieldName, @params, fieldConfiguration);

    /// <inheritdoc/>
    IInterfaceBuilder ITypeMemberDeclarationBuilder<IInterfaceBuilder>.AppendMethod<TParams>(string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        => AppendMethod(methodName, @params, methodConfiguration);
}