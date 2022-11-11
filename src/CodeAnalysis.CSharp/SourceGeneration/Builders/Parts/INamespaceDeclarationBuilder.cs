using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface INamespaceDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : INamespaceDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : INamespaceConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc);
}