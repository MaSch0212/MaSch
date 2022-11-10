using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IStructDeclarationBuilder : ISourceBuilder
    {
        IStructDeclarationBuilder Append(Func<IStructConfigurationFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc);
    }

    public interface IStructDeclarationBuilder<TBuilder, TConfigFactory> : IStructDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IStructDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IStructConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IStructDeclarationBuilder
    {
        IStructDeclarationBuilder IStructDeclarationBuilder.Append(Func<IStructConfigurationFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        private SourceBuilder Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
            => AppendAsBlock(structConfiguration, this, builderFunc);
    }
}
