namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ISupportsCodeAttributeConfiguration : ICodeConfiguration
{
    ISupportsCodeAttributeConfiguration WithCodeAttribute(string attributeTypeName);
    ISupportsCodeAttributeConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration);
}

public interface ISupportsCodeAttributeConfiguration<T> : ISupportsCodeAttributeConfiguration
    where T : ISupportsCodeAttributeConfiguration<T>
{
    new T WithCodeAttribute(string attributeTypeName);
    new T WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration);
}