using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IRecordDeclarationBuilder : ISourceBuilder
    {
        IRecordDeclarationBuilder Append(Func<IRecordConfgurationFactory, IRecordConfiguration> createFunc);
        IRecordDeclarationBuilder Append(Func<IRecordConfgurationFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc);
    }

    public interface IRecordDeclarationBuilder<TBuilder, TConfigFactory> : IRecordDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IRecordDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IRecordConfgurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IRecordConfiguration> createFunc);
        TBuilder Append(Func<TConfigFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IRecordDeclarationBuilder
    {
        /// <inheritdoc/>
        IRecordDeclarationBuilder IRecordDeclarationBuilder.Append(Func<IRecordConfgurationFactory, IRecordConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IRecordDeclarationBuilder IRecordDeclarationBuilder.Append(Func<IRecordConfgurationFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);
    }
}