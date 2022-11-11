using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IInterfaceDeclarationBuilder<TBuilder>
    where TBuilder : IInterfaceDeclarationBuilder<TBuilder>
{
    TBuilder Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc);
}