using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IDelegateDeclarationBuilder<TBuilder>
    where TBuilder : IDelegateDeclarationBuilder<TBuilder>
{
    TBuilder Append(IDelegateConfiguration delegateConfiguration);
}