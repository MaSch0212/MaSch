using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface INamespaceConfigurationFactory
{
    INamespaceConfiguration Namespace(string name);
}

partial class CodeConfigurationFactory : INamespaceConfigurationFactory
{
    public INamespaceConfiguration Namespace(string name)
    {
        return new NamespaceConfiguration(name);
    }
}