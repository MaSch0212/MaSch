using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IFieldDeclarationBuilder<TBuilder>
    where TBuilder : IFieldDeclarationBuilder<TBuilder>
{
    TBuilder Append(IFieldConfiguration fieldConfiguration);
}