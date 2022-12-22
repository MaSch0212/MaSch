using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a method.
    /// </summary>
    public interface IMethodDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a method to the current context.
        /// </summary>
        /// <param name="methodConfiguration">The configuration of the method.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IMethodDeclarationBuilder Append(IMethodConfiguration methodConfiguration);

        /// <summary>
        /// Appends a method to the current context.
        /// </summary>
        /// <param name="methodConfiguration">The configuration of the method.</param>
        /// <param name="builderFunc">The function to add content to the method.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IMethodDeclarationBuilder Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc);
    }

    /// <inheritdoc cref="IMethodDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IMethodDeclarationBuilder<T> : IMethodDeclarationBuilder, ISourceBuilder<T>
        where T : IMethodDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IMethodDeclarationBuilder.Append(IMethodConfiguration)"/>
        new T Append(IMethodConfiguration methodConfiguration);

        /// <inheritdoc cref="IMethodDeclarationBuilder.Append(IMethodConfiguration, Action{ISourceBuilder})"/>
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