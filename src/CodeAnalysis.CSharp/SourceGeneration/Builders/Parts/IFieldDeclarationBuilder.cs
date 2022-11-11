using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IFieldDeclarationBuilder : ISourceBuilder
    {
        IFieldDeclarationBuilder Append(Func<IFieldConfigurationFactory, IFieldConfiguration> createFunc);
    }

    public interface IFieldDeclarationBuilder<TBuilder, TConfigFactory> : IFieldDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IFieldDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IFieldConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IFieldConfiguration> createFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IFieldDeclarationBuilder
    {
        /// <inheritdoc/>
        IFieldDeclarationBuilder IFieldDeclarationBuilder.Append(Func<IFieldConfigurationFactory, IFieldConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));
    }
}