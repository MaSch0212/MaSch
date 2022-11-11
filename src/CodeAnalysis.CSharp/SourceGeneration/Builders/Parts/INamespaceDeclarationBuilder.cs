using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface INamespaceDeclarationBuilder<TBuilder>
    where TBuilder : INamespaceDeclarationBuilder<TBuilder>
{
    TBuilder Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc);
}