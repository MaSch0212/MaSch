using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface INamespaceImportConfigurationFactory
{
    INamespaceImportConfiguration NamespaceImport(string @namespace);

    /// <inheritdoc cref="NamespaceImport(string)"/>
    INamespaceImportConfiguration Using(string @namespace);
}

partial class CodeConfigurationFactory : INamespaceImportConfigurationFactory
{
    public INamespaceImportConfiguration NamespaceImport(string @namespace)
    {
        return new NamespaceImportConfiguration(@namespace);
    }

    public INamespaceImportConfiguration Using(string @namespace)
    {
        return new NamespaceImportConfiguration(@namespace);
    }
}