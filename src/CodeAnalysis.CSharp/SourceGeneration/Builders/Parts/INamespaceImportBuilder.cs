using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a namespace import.
    /// </summary>
    public interface INamespaceImportBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a namespace import to the current context.
        /// </summary>
        /// <param name="namespaceImportConfiguration">The configuration of the namespace import.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        INamespaceImportBuilder Append(INamespaceImportConfiguration namespaceImportConfiguration);
    }

    /// <inheritdoc cref="INamespaceImportBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface INamespaceImportBuilder<T> : INamespaceImportBuilder, ISourceBuilder<T>
        where T : INamespaceImportBuilder<T>
    {
        /// <inheritdoc cref="INamespaceImportBuilder.Append(INamespaceImportConfiguration)"/>
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