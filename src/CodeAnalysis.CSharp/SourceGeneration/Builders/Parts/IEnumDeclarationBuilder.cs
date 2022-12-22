using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build an enum.
    /// </summary>
    public interface IEnumDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends an enum to the current context.
        /// </summary>
        /// <param name="enumConfiguration">The configuration of the enum.</param>
        /// <param name="builderFunc">The function to add content to the enum.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IEnumDeclarationBuilder Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc);
    }

    /// <inheritdoc cref="IEnumDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IEnumDeclarationBuilder<T> : IEnumDeclarationBuilder, ISourceBuilder<T>
        where T : IEnumDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IEnumDeclarationBuilder.Append(IEnumConfiguration, Action{IEnumBuilder})"/>
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