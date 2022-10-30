namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISupportsCodeAttributeConfiguration<T> : ICodeConfiguration
    where T : ISupportsCodeAttributeConfiguration<T>
{
    T WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration);
}
