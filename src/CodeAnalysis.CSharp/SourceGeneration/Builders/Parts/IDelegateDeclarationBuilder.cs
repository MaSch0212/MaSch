using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a delegate.
    /// </summary>
    public interface IDelegateDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a delegate to the current context.
        /// </summary>
        /// <param name="delegateConfiguration">The configuration of the delegate.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IDelegateDeclarationBuilder Append(IDelegateConfiguration delegateConfiguration);
    }

    /// <inheritdoc cref="IDelegateDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IDelegateDeclarationBuilder<T> : IDelegateDeclarationBuilder, ISourceBuilder<T>
        where T : IDelegateDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IDelegateDeclarationBuilder.Append(IDelegateConfiguration)"/>
        new T Append(IDelegateConfiguration delegateConfiguration);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IDelegateDeclarationBuilder
    {
        /// <inheritdoc/>
        IDelegateDeclarationBuilder IDelegateDeclarationBuilder.Append(IDelegateConfiguration delegateConfiguration)
            => Append(delegateConfiguration);
    }
}