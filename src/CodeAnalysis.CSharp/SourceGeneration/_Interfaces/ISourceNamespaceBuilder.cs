using MaSch.CodeAnalysis.CSharp.SourceGeneration.Parts;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceNamespaceBuilder :
    ISourceNamespaceDeclarationBuilder<ISourceNamespaceBuilder>,
    ISourceNamespaceImportBuilder<ISourceNamespaceBuilder>
{
}