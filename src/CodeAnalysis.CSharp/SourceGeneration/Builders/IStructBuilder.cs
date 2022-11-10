using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IStructBuilder :
        ITypeMemberDeclarationBuilder<IStructBuilder>,
        INamespaceMemberDeclarationBuilder<IStructBuilder>,
        IConstructorDeclarationBuilder<IClassBuilder>
    {
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IStructBuilder
    {
    }
}