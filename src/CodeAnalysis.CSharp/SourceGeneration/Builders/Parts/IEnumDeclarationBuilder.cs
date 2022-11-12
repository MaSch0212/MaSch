using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IEnumDeclarationBuilder : ISourceBuilder
    {
        IEnumDeclarationBuilder Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc);
    }

    public interface IEnumDeclarationBuilder<T> : IEnumDeclarationBuilder, ISourceBuilder<T>
        where T : IEnumDeclarationBuilder<T>
    {
        new T Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IEnumDeclarationBuilder
    {
        /// <inheritdoc/>
        IEnumDeclarationBuilder IEnumDeclarationBuilder.Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
            => Append(enumConfiguration, builderFunc);
    }
}