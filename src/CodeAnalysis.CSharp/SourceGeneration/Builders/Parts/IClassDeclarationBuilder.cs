using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a class.
    /// </summary>
    public interface IClassDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a class to the current context.
        /// </summary>
        /// <param name="classConfiguration">The configuration of the class.</param>
        /// <param name="builderFunc">The function to add content to the class.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IClassDeclarationBuilder Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc);
    }

    /// <inheritdoc cref="IClassDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IClassDeclarationBuilder<T> : IClassDeclarationBuilder, ISourceBuilder<T>
        where T : IClassDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IClassDeclarationBuilder.Append(IClassConfiguration, Action{IClassBuilder})"/>
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