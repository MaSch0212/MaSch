using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IMethodDeclarationBuilder : ISourceBuilder
    {
        IMethodDeclarationBuilder Append(IMethodConfiguration methodConfiguration);
        IMethodDeclarationBuilder Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc);
    }

    public interface IMethodDeclarationBuilder<T> : IMethodDeclarationBuilder, ISourceBuilder<T>
        where T : IMethodDeclarationBuilder<T>
    {
        new T Append(IMethodConfiguration methodConfiguration);
        new T Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IMethodDeclarationBuilder
    {
        /// <inheritdoc/>
        IMethodDeclarationBuilder IMethodDeclarationBuilder.Append(IMethodConfiguration methodConfiguration)
            => Append(methodConfiguration, null);

        /// <inheritdoc/>
        IMethodDeclarationBuilder IMethodDeclarationBuilder.Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(methodConfiguration, builderFunc);
    }
}