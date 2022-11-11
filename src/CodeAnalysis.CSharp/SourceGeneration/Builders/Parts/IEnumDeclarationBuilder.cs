using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IEnumDeclarationBuilder : ISourceBuilder
    {
        IEnumDeclarationBuilder Append(Func<IEnumConfigurationFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc);
    }

    public interface IEnumDeclarationBuilder<TBuilder, TConfigFactory> : IEnumDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IEnumDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IEnumConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IEnumDeclarationBuilder
    {
        /// <inheritdoc/>
        IEnumDeclarationBuilder IEnumDeclarationBuilder.Append(Func<IEnumConfigurationFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);
    }
}