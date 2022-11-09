using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IPropertyConfigurationFactory
{
    IPropertyConfiguration Property(string propertyTypeName, string propertyName);
}

partial class CodeConfigurationFactory : IPropertyConfigurationFactory
{
    public IPropertyConfiguration Property(string propertyTypeName, string propertyName)
    {
        return new PropertyConfiguration(propertyTypeName, propertyName);
    }
}
