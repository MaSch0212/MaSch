using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IPropertyConfigurationFactory
{
    IPropertyConfigurationBase Property(string propertyTypeName, string propertyName);
}

partial class CodeConfigurationFactory : IPropertyConfigurationFactory
{
    public IPropertyConfigurationBase Property(string propertyTypeName, string propertyName)
    {
        return new PropertyConfiguration(propertyTypeName, propertyName);
    }
}
