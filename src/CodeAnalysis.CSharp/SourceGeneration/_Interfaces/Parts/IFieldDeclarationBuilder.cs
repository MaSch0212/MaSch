using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IFieldDeclarationBuilder : ISourceBuilder
{
    IFieldDeclarationBuilder Append(Func<IFieldConfigurationFactory, IFieldConfiguration> createFunc);
}

public interface IFieldDeclarationBuilder<TBuilder, TConfigFactory> : IFieldDeclarationBuilder
    where TBuilder : IFieldDeclarationBuilder<TBuilder, TConfigFactory>
    where TConfigFactory : IFieldConfigurationFactory
{
    TBuilder Append(Func<TConfigFactory, IFieldConfiguration> createFunc);
}

public partial class SourceBuilder : IFieldDeclarationBuilder
{
    IFieldDeclarationBuilder IFieldDeclarationBuilder.Append(Func<IFieldConfigurationFactory, IFieldConfiguration> createFunc)
        => Append(createFunc(_configurationFactory));

    private SourceBuilder Append(IFieldConfiguration fieldConfiguration)
    {
        fieldConfiguration.WriteTo(this);
        return Append(';').AppendLine();
    }
}