using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceImportBuilder : ISourceBuilder
{
    INamespaceImportBuilder Append(Func<INamespaceImportConfigurationFactory, INamespaceImportConfiguration> createFunc);
}

public interface INamespaceImportBuilder<TBuilder, TConfigFactory> : INamespaceImportBuilder
    where TBuilder : INamespaceImportBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : INamespaceImportConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, INamespaceImportConfiguration> createFunc);
}

public partial class SourceBuilder : INamespaceImportBuilder
{
    INamespaceImportBuilder INamespaceImportBuilder.Append(Func<INamespaceImportConfigurationFactory, INamespaceImportConfiguration> createFunc)
        => Append(createFunc(_configurationFactory));

    private SourceBuilder Append(INamespaceImportConfiguration namespaceImportConfiguration)
    {
        namespaceImportConfiguration.WriteTo(this);
        return Append(';').AppendLine();
    }
}