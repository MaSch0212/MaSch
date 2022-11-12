using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IIndexerDeclarationBuilder : ISourceBuilder
    {
        IIndexerDeclarationBuilder Append(IIndexerConfiguration indexerConfiguration);
        IIndexerDeclarationBuilder Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        IIndexerDeclarationBuilder Append(IReadOnlyIndexerConfiguration indexerConfiguration);
        IIndexerDeclarationBuilder Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc);
        IIndexerDeclarationBuilder Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }

    public interface IIndexerDeclarationBuilder<T> : IIndexerDeclarationBuilder, ISourceBuilder<T>
        where T : IIndexerDeclarationBuilder<T>
    {
        new T Append(IIndexerConfiguration indexerConfiguration);
        new T Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        new T Append(IReadOnlyIndexerConfiguration indexerConfiguration);
        new T Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc);
        new T Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IIndexerDeclarationBuilder
    {
        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IIndexerConfiguration indexerConfiguration)
            => Append(indexerConfiguration, null, null);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(indexerConfiguration, setBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IReadOnlyIndexerConfiguration indexerConfiguration)
            => Append(indexerConfiguration, null, null);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc)
            => Append(indexerConfiguration, getBuilderFunc, null);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc)
            => Append(indexerConfiguration, null, setBuilderFunc);
    }
}