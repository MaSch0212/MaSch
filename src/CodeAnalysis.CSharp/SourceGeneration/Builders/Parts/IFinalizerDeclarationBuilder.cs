using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a finalizer.
    /// </summary>
    public interface IFinalizerDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a finalizer to the current context.
        /// </summary>
        /// <param name="finalizerConfiguration">The configuration of the finalizer.</param>
        /// <param name="builderFunc">The function to add content to the finalizer.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IFinalizerDeclarationBuilder Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc);
    }

    /// <inheritdoc cref="IFinalizerDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IFinalizerDeclarationBuilder<T> : IFinalizerDeclarationBuilder, ISourceBuilder<T>
        where T : IFinalizerDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IFinalizerDeclarationBuilder.Append(IFinalizerConfiguration, Action{ISourceBuilder})"/>
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