using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IDelegateDeclarationBuilder : ISourceBuilder
{
    IDelegateDeclarationBuilder Append(Func<IDelegateConfigurationFactory, IDelegateConfiguration> createFunc);
}

public interface IDelegateDeclarationBuilder<TBuilder, TConfigFactory> : IDelegateDeclarationBuilder
    where TBuilder : IDelegateDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IDelegateConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IDelegateConfiguration> createFunc);
}

public partial class SourceBuilder : IDelegateDeclarationBuilder
{
    IDelegateDeclarationBuilder IDelegateDeclarationBuilder.Append(Func<IDelegateConfigurationFactory, IDelegateConfiguration> createFunc)
        => Append(createFunc(_configurationFactory));

    private SourceBuilder Append(IDelegateConfiguration delegateConfiguration)
    {
        delegateConfiguration.WriteTo(this);
        return Append(';').AppendLine();
    }
}