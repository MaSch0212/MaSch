using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IFieldConfigurationFactory
{
    IFieldConfiguration Field(string fieldTypeName, string fieldName);
}

partial class CodeConfigurationFactory : IFieldConfigurationFactory
{
    public IFieldConfiguration Field(string fieldTypeName, string fieldName)
    {
        return new FieldConfiguration(fieldTypeName, fieldName);
    }
}
