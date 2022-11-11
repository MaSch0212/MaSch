using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IPropertyDeclarationBuilder<TBuilder>
    where TBuilder : IPropertyDeclarationBuilder<TBuilder>
{
    TBuilder Append(IPropertyConfiguration propertyConfiguration);
    TBuilder Append(IPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);
    TBuilder Append(IReadOnlyPropertyConfiguration propertyConfiguration);
    TBuilder Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc);
    TBuilder Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc);
}