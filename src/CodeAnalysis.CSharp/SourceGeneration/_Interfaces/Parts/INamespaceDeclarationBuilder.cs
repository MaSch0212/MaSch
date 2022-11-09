using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface INamespaceDeclarationBuilder : ISourceBuilder
{
    INamespaceDeclarationBuilder Append(Func<INamespaceConfigurationFactory, INamespaceConfiguration> createFunc);
    INamespaceDeclarationBuilder Append(Func<INamespaceConfigurationFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc);
}

public interface INamespaceDeclarationBuilder<TBuilder, TConfigFactory> : INamespaceDeclarationBuilder, ISourceBuilder<TBuilder>
    where TBuilder : INamespaceDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : INamespaceConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, INamespaceConfiguration> createFunc);
    TBuilder Append(Func<TConfigFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc);
}

public partial class SourceBuilder : INamespaceDeclarationBuilder
{
    INamespaceDeclarationBuilder INamespaceDeclarationBuilder.Append(Func<INamespaceConfigurationFactory, INamespaceConfiguration> createFunc)
        => Append(createFunc(_configurationFactory), null);

    INamespaceDeclarationBuilder INamespaceDeclarationBuilder.Append(Func<INamespaceConfigurationFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc)
        => Append(createFunc(_configurationFactory), builderFunc);

    private SourceBuilder Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder>? builderFunc)
    {
        if (builderFunc is not null)
            return AppendAsBlock(namespaceConfiguration, this, builderFunc);

        namespaceConfiguration.WriteTo(this);
        return Append(';').AppendLine();
    }
}