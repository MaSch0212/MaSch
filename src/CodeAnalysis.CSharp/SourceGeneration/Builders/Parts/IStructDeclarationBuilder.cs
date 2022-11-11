using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IStructDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IStructDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IStructConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc);
}