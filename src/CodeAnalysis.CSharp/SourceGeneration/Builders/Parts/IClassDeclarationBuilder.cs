using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IClassDeclarationBuilder : ISourceBuilder
    {
        IClassDeclarationBuilder Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc);
    }

    public interface IClassDeclarationBuilder<T> : IClassDeclarationBuilder, ISourceBuilder<T>
        where T : IClassDeclarationBuilder<T>
    {
        new T Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IClassDeclarationBuilder
    {
        /// <inheritdoc/>
        IClassDeclarationBuilder IClassDeclarationBuilder.Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc)
            => Append(classConfiguration, builderFunc);
    }
}