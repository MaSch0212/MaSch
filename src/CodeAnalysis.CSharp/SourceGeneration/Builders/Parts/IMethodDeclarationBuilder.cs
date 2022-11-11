using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IMethodDeclarationBuilder : ISourceBuilder
    {
        IMethodDeclarationBuilder Append(Func<IMethodConfigurationFactory, IMethodConfiguration> createFunc);
        IMethodDeclarationBuilder Append(Func<IMethodConfigurationFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
    }

    public interface IMethodDeclarationBuilder<TBuilder, TConfigFactory> : IMethodDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IMethodDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IMethodConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IMethodConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IMethodDeclarationBuilder
    {
        /// <inheritdoc/>
        IMethodDeclarationBuilder IMethodDeclarationBuilder.Append(Func<IMethodConfigurationFactory, IMethodConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IMethodDeclarationBuilder IMethodDeclarationBuilder.Append(Func<IMethodConfigurationFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);
    }
}