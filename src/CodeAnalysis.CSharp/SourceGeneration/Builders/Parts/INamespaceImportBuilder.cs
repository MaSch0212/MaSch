using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface INamespaceImportBuilder : ISourceBuilder
    {
        INamespaceImportBuilder Append(INamespaceImportConfiguration namespaceImportConfiguration);
    }

    public interface INamespaceImportBuilder<T> : INamespaceImportBuilder, ISourceBuilder<T>
        where T : INamespaceImportBuilder<T>
    {
        new T Append(INamespaceImportConfiguration namespaceImportConfiguration);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : INamespaceImportBuilder
    {
        /// <inheritdoc/>
        INamespaceImportBuilder INamespaceImportBuilder.Append(INamespaceImportConfiguration namespaceImportConfiguration)
            => Append(namespaceImportConfiguration);
    }
}