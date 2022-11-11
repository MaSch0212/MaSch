using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IFieldDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IFieldDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IFieldConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IFieldConfiguration> createFunc);
}