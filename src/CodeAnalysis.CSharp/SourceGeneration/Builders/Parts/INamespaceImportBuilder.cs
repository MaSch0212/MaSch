using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface INamespaceImportBuilder<TBuilder>
    where TBuilder : INamespaceImportBuilder<TBuilder>
{
    TBuilder Append(INamespaceImportConfiguration namespaceImportConfiguration);
}