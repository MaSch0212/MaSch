using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IStructDeclarationBuilder<TBuilder>
    where TBuilder : IStructDeclarationBuilder<TBuilder>
{
    TBuilder Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc);
}