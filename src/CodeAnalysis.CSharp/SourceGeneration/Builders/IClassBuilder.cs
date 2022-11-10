using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IClassBuilder :
    ITypeMemberDeclarationBuilder<IClassBuilder>,
    INamespaceMemberDeclarationBuilder<IClassBuilder>,
    IFieldDeclarationBuilder<IClassBuilder, IClassMemberFactory>
    IConstructorDeclarationBuilder<IClassBuilder>
{
}