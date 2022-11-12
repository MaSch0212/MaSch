using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IFinalizerDeclarationBuilder : ISourceBuilder
    {
        IFinalizerDeclarationBuilder Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc);
    }

    public interface IFinalizerDeclarationBuilder<T> : IFinalizerDeclarationBuilder, ISourceBuilder<T>
        where T : IFinalizerDeclarationBuilder<T>
    {
        new T Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IFinalizerDeclarationBuilder
    {
        /// <inheritdoc/>
        IFinalizerDeclarationBuilder IFinalizerDeclarationBuilder.Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(finalizerConfiguration, builderFunc);
    }
}