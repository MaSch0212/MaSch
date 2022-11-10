using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IFileMemberFactory :
    INamespaceConfigurationFactory,
    INamespaceImportConfigurationFactory
{
    ICodeAttributeConfiguration AssemblyAttribute(string attributeTypeName);
}

partial class CodeConfigurationFactory : IFileMemberFactory
{
    public ICodeAttributeConfiguration AssemblyAttribute(string attributeTypeName)
    {
        return new CodeAttributeConfiguration(attributeTypeName).OnAssembly();
    }
}