using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a field.
    /// </summary>
    public interface IFieldDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a field to the current context.
        /// </summary>
        /// <param name="fieldConfiguration">The configuration of the field.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IFieldDeclarationBuilder Append(IFieldConfiguration fieldConfiguration);
    }

    /// <inheritdoc cref="IFieldDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IFieldDeclarationBuilder<T> : IFieldDeclarationBuilder, ISourceBuilder<T>
        where T : IFieldDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IFieldDeclarationBuilder.Append(IFieldConfiguration)"/>
        new T Append(IFieldConfiguration fieldConfiguration);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IFieldDeclarationBuilder
    {
        /// <inheritdoc/>
        IFieldDeclarationBuilder IFieldDeclarationBuilder.Append(IFieldConfiguration fieldConfiguration)
            => Append(fieldConfiguration);
    }
}