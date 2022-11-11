using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IEventDeclarationBuilder<TBuilder>
    where TBuilder : IEventDeclarationBuilder<TBuilder>
{
    TBuilder Append(IEventConfiguration eventConfiguration);
    TBuilder Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc);
}