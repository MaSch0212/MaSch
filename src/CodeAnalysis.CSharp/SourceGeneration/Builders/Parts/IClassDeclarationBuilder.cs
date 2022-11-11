using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IClassDeclarationBuilder : ISourceBuilder
    {
        IClassDeclarationBuilder Append(Func<IClassConfigurationFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc);
    }

    public interface IClassDeclarationBuilder<TBuilder, TConfigFactory> : IClassDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IClassDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IClassConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IClassDeclarationBuilder
    {
        /// <inheritdoc/>
        IClassDeclarationBuilder IClassDeclarationBuilder.Append(Func<IClassConfigurationFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);
    }
}