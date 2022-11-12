using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface INamespaceDeclarationBuilder : ISourceBuilder
    {
        INamespaceDeclarationBuilder Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc);
    }

    public interface INamespaceDeclarationBuilder<T> : INamespaceDeclarationBuilder, ISourceBuilder<T>
        where T : INamespaceDeclarationBuilder<T>
    {
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