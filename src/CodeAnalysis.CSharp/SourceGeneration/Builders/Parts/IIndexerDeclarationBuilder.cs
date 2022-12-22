using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build an indexer.
    /// </summary>
    public interface IIndexerDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends an indexer to the current context.
        /// </summary>
        /// <param name="indexerConfiguration">The configuration of the indexer.</param>
        /// <param name="getBuilderFunc">The function to add content to the get method of the indexer.</param>
        /// <param name="setBuilderFunc">The function to add content to the set method of the indexer.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IIndexerDeclarationBuilder Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);

        /// <summary>
        /// Appends a read-only indexer to the current context.
        /// </summary>
        /// <param name="indexerConfiguration">The configuration of the indexer.</param>
        /// <param name="getBuilderFunc">The function to add content to the get method of the indexer.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IIndexerDeclarationBuilder Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc);

        /// <summary>
        /// Appends a write-only indexer to the current context.
        /// </summary>
        /// <param name="indexerConfiguration">The configuration of the indexer.</param>
        /// <param name="setBuilderFunc">The function to add content to the set method of the indexer.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IIndexerDeclarationBuilder Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }

    /// <inheritdoc cref="IIndexerDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IIndexerDeclarationBuilder<T> : IIndexerDeclarationBuilder, ISourceBuilder<T>
        where T : IIndexerDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IIndexerDeclarationBuilder.Append(IIndexerConfiguration, Action{ISourceBuilder}, Action{ISourceBuilder})"/>
        new T Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);

        /// <inheritdoc cref="IIndexerDeclarationBuilder.Append(IReadOnlyIndexerConfiguration, Action{ISourceBuilder})"/>
        new T Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc);

        /// <inheritdoc cref="IIndexerDeclarationBuilder.Append(IWriteOnlyIndexerConfiguration, Action{ISourceBuilder})"/>
        new T Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IIndexerDeclarationBuilder
    {
        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(indexerConfiguration, setBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc)
            => Append(indexerConfiguration, getBuilderFunc, null);

        /// <inheritdoc/>
        IIndexerDeclarationBuilder IIndexerDeclarationBuilder.Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc)
            => Append(indexerConfiguration, null, setBuilderFunc);
    }
}