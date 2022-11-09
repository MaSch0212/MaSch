using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

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