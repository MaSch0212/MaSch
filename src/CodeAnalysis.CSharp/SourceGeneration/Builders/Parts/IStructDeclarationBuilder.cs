using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a struct.
    /// </summary>
    public interface IStructDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a struct to the current context.
        /// </summary>
        /// <param name="structConfiguration">The configuration of the struct.</param>
        /// <param name="builderFunc">The function to add content to the struct.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IStructDeclarationBuilder Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc);
    }

    /// <inheritdoc cref="IStructDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IStructDeclarationBuilder<T> : IStructDeclarationBuilder, ISourceBuilder<T>
        where T : IStructDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IStructDeclarationBuilder.Append(IStructConfiguration, Action{IStructBuilder})"/>
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