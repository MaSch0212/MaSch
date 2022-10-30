namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceMemberBuilder<T>
    where T : ISourceMemberBuilder<T>
{
    T WithAccessModifier(AccessModifier accessModifier);
    T WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ISourceCodeAttributeBuilder, TParams> attributeConfiguration);
}
