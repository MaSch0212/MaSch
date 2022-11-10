using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface INamespaceImportBuilder : ISourceBuilder
    {
        INamespaceImportBuilder Append(Func<INamespaceImportConfigurationFactory, INamespaceImportConfiguration> createFunc);
    }

    public interface INamespaceImportBuilder<TBuilder, TConfigFactory> : INamespaceImportBuilder, ISourceBuilder<TBuilder>
        where TBuilder : INamespaceImportBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : INamespaceImportConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, INamespaceImportConfiguration> createFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : INamespaceImportBuilder
    {
        INamespaceImportBuilder INamespaceImportBuilder.Append(Func<INamespaceImportConfigurationFactory, INamespaceImportConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        private SourceBuilder Append(INamespaceImportConfiguration namespaceImportConfiguration)
            => AppendWithLineTerminator(namespaceImportConfiguration);
    }
}