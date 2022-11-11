using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IPropertyDeclarationBuilder : ISourceBuilder
    {
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
        IPropertyDeclarationBuilder Append(Func<IPropertyConfigurationFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
    }

    public interface IPropertyDeclarationBuilder<TBuilder, TConfigFactory> : IPropertyDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IPropertyDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IPropertyConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IPropertyConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
        TBuilder Append(Func<TConfigFactory, IReadOnlyPropertyConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc);
        TBuilder Append(Func<TConfigFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IPropertyDeclarationBuilder
    {
        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(Func<IPropertyConfigurationFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);
    }
}