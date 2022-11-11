using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IInterfaceDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IInterfaceDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IInterfaceConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc);
}