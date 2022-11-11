using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IDelegateDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IDelegateDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IDelegateConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IDelegateConfiguration> createFunc);
}