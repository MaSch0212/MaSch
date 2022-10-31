using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IClassBuilder :
    ITypeMemberDeclarationBuilder<IClassBuilder>,
    INamespaceMemberDeclarationBuilder<IClassBuilder>,
    IConstructorDeclarationBuilder<IClassBuilder>
{
}

public partial class SourceBuilder : IClassBuilder
{
    /// <inheritdoc/>
    IClassBuilder INamespaceMemberDeclarationBuilder<IClassBuilder>.AppendDelegate<TParams>(string delegateName, TParams @params, Action<IDelegateConfiguration, TParams> delegateConfiguration)
        => AppendDelegate(delegateName, @params, delegateConfiguration);

    /// <inheritdoc/>
    IClassBuilder ITypeMemberDeclarationBuilder<IClassBuilder>.AppendField<TParams>(string fieldTypeName, string fieldName, TParams @params, Action<IFieldConfiguration, TParams> fieldConfiguration)
        => AppendField(fieldTypeName, fieldName, @params, fieldConfiguration);

    /// <inheritdoc/>
    IClassBuilder ITypeMemberDeclarationBuilder<IClassBuilder>.AppendMethod<TParams>(string methodName, TParams @params, Action<IMethodConfiguration, TParams> methodConfiguration)
        => AppendMethod(methodName, @params, methodConfiguration);
}