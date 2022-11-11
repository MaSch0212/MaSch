using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface INamespaceImportBuilder<TBuilder, TConfigFactory>
        where TBuilder : INamespaceImportBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : INamespaceImportConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, INamespaceImportConfiguration> createFunc);
    }
}