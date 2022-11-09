using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IInterfaceDeclarationBuilder : ISourceBuilder
{
    IInterfaceDeclarationBuilder Append(Func<IInterfaceConfigurationFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc);
}

public interface IInterfaceDeclarationBuilder<TBuilder, TConfigFactory> : IInterfaceDeclarationBuilder
    where TBuilder : IInterfaceDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IInterfaceConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc);
}

public partial class SourceBuilder : IInterfaceDeclarationBuilder
{
    IInterfaceDeclarationBuilder IInterfaceDeclarationBuilder.Append(Func<IInterfaceConfigurationFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc)
        => Append(createFunc(_configurationFactory), builderFunc);

    private SourceBuilder Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
        => AppendAsBlock(interfaceConfguration, this, builderFunc);
}
