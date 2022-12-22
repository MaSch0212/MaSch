using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build an interface.
    /// </summary>
    public interface IInterfaceDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends an interface to the current context.
        /// </summary>
        /// <param name="interfaceConfguration">The configuration of the interface.</param>
        /// <param name="builderFunc">The function to add content to the interface.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IInterfaceDeclarationBuilder Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc);
    }

    /// <inheritdoc cref="IInterfaceDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IInterfaceDeclarationBuilder<T> : IInterfaceDeclarationBuilder, ISourceBuilder<T>
        where T : IInterfaceDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IInterfaceDeclarationBuilder.Append(IInterfaceConfguration, Action{IInterfaceBuilder})"/>
        new T Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IInterfaceDeclarationBuilder
    {
        /// <inheritdoc/>
        IInterfaceDeclarationBuilder IInterfaceDeclarationBuilder.Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
            => Append(interfaceConfguration, builderFunc);
    }
}