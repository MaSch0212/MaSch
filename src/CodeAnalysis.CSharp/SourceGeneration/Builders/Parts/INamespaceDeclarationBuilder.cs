using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a namespace.
    /// </summary>
    public interface INamespaceDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a namespace to the current context.
        /// </summary>
        /// <param name="namespaceConfiguration">The configuration of the namespace.</param>
        /// <param name="builderFunc">The function to add content to the namespace.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        INamespaceDeclarationBuilder Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc);
    }

    /// <inheritdoc cref="INamespaceDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface INamespaceDeclarationBuilder<T> : INamespaceDeclarationBuilder, ISourceBuilder<T>
        where T : INamespaceDeclarationBuilder<T>
    {
        /// <inheritdoc cref="INamespaceDeclarationBuilder.Append(INamespaceConfiguration, Action{INamespaceBuilder})"/>
        new T Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : INamespaceDeclarationBuilder
    {
        /// <inheritdoc/>
        INamespaceDeclarationBuilder INamespaceDeclarationBuilder.Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc)
            => Append(namespaceConfiguration, builderFunc);
    }
}