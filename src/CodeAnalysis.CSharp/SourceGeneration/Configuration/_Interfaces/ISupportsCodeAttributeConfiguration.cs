namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ISupportsCodeAttributeConfiguration : ICodeConfiguration
{
    ISupportsCodeAttributeConfiguration WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration);
}

public interface ISupportsCodeAttributeConfiguration<T> : ISupportsCodeAttributeConfiguration
    where T : ISupportsCodeAttributeConfiguration<T>
{
    new T WithCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration);
}
