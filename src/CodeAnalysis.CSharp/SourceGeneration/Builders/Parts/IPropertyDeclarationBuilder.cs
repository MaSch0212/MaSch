using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IPropertyDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IPropertyDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IPropertyConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IPropertyConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
    TBuilder Append(Func<TConfigFactory, IReadOnlyPropertyConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
    TBuilder Append(Func<TConfigFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
}