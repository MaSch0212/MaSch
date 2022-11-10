using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IInterfaceBuilder :
    ITypeMemberDeclarationBuilder<IInterfaceBuilder>,
    INamespaceMemberDeclarationBuilder<IInterfaceBuilder>
{
}