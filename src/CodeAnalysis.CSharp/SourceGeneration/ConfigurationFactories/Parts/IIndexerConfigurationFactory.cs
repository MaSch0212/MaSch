using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

public interface IIndexerConfigurationFactory
{
    IIndexerConfiguration Indexer(string indexerType);
}

partial class CodeConfigurationFactory : IIndexerConfigurationFactory
{
    public IIndexerConfiguration Indexer(string indexerType)
    {
        return new IndexerConfiguration(indexerType);
    }
}
