using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IRecordDeclarationBuilder<TBuilder, TConfigFactory>
    where TBuilder : IRecordDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IRecordConfgurationFactory
{
    TBuilder Append(Func<TConfigFactory, IRecordConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc);
}