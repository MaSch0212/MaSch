using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IDelegateDeclarationBuilder : ISourceBuilder
    {
        IDelegateDeclarationBuilder Append(Func<IDelegateConfigurationFactory, IDelegateConfiguration> createFunc);
    }

    public interface IDelegateDeclarationBuilder<TBuilder, TConfigFactory> : IDelegateDeclarationBuilder, ISourceBuilder<TBuilder>
        where TBuilder : IDelegateDeclarationBuilder<TBuilder, TConfigFactory>
        where TConfigFactory : IDelegateConfigurationFactory
    {
        TBuilder Append(Func<TConfigFactory, IDelegateConfiguration> createFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IDelegateDeclarationBuilder
    {
        IDelegateDeclarationBuilder IDelegateDeclarationBuilder.Append(Func<IDelegateConfigurationFactory, IDelegateConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        private SourceBuilder Append(IDelegateConfiguration delegateConfiguration)
            => AppendWithLineTerminator(delegateConfiguration);
    }
}