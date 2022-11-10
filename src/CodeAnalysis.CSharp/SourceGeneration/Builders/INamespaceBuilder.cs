using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface INamespaceBuilder :
    INamespaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
    INamespaceImportBuilder<INamespaceBuilder>,
    INamespaceMemberDeclarationBuilder<INamespaceBuilder>,
    IClassDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>
{
}