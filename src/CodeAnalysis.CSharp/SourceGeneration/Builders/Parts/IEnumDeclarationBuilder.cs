using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IEnumDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IEnumDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IEnumConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc);
}