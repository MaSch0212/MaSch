using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IEnumDeclarationBuilder : ISourceBuilder
{
    IEnumDeclarationBuilder Append(Func<IEnumConfigurationFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc);
}

public interface IEnumDeclarationBuilder<TBuilder, TConfigFactory> : IEnumDeclarationBuilder
    where TBuilder : IEnumDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IEnumConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc);
}

public partial class SourceBuilder : IEnumDeclarationBuilder
{
    IEnumDeclarationBuilder IEnumDeclarationBuilder.Append(Func<IEnumConfigurationFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc)
        => Append(createFunc(_configurationFactory), builderFunc);

    private SourceBuilder Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
        => AppendAsBlock(enumConfiguration, this, builderFunc);
}