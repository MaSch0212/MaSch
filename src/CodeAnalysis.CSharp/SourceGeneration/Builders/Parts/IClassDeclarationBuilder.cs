using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IClassDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IClassDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IClassConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc);
}