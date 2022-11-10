using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface IRecordBuilder :
    ITypeMemberDeclarationBuilder<IRecordBuilder>,
    INamespaceMemberDeclarationBuilder<IRecordBuilder>,
    IConstructorDeclarationBuilder<IClassBuilder>
{
}