using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IInterfaceDeclarationBuilder : ISourceBuilder
    {
        IInterfaceDeclarationBuilder Append(Func<IInterfaceConfigurationFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc);
    }

    public interface IInterfaceDeclarationBuilder<TBuilder, TConfigFactory> : IInterfaceDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IInterfaceDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IInterfaceConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IInterfaceDeclarationBuilder
    {
        /// <inheritdoc/>
        IInterfaceDeclarationBuilder IInterfaceDeclarationBuilder.Append(Func<IInterfaceConfigurationFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);
    }
}
