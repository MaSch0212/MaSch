using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IIndexerDeclarationBuilder<TBuilder>
    where TBuilder : IIndexerDeclarationBuilder<TBuilder>
{
    TBuilder Append(IIndexerConfiguration indexerConfiguration);
    TBuilder Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
    TBuilder Append(IReadOnlyIndexerConfiguration indexerConfiguration);
    TBuilder Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc);
    TBuilder Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc);
}