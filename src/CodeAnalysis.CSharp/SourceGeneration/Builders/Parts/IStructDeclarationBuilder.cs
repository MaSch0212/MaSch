using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IStructDeclarationBuilder : ISourceBuilder
    {
        IStructDeclarationBuilder Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc);
    }

    public interface IStructDeclarationBuilder<T> : IStructDeclarationBuilder, ISourceBuilder<T>
        where T : IStructDeclarationBuilder<T>
    {
        new T Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IStructDeclarationBuilder
    {
        /// <inheritdoc/>
        IStructDeclarationBuilder IStructDeclarationBuilder.Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
            => Append(structConfiguration, builderFunc);
    }
}