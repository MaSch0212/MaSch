using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a constructor.
    /// </summary>
    public interface IConstructorDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a constructor to the current context.
        /// </summary>
        /// <param name="constructorConfiguration">The configuration of the constructor.</param>
        /// <param name="builderFunc">The function to add content to the constructor.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IConstructorDeclarationBuilder Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc);

        /// <summary>
        /// Appends a static constructor to the current context.
        /// </summary>
        /// <param name="staticConstructorConfiguration">The configuration of the static constructor.</param>
        /// <param name="builderFunc">The function to add content to the static constructor.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IConstructorDeclarationBuilder Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc);
    }

    /// <inheritdoc cref="IConstructorDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IConstructorDeclarationBuilder<T> : IConstructorDeclarationBuilder, ISourceBuilder<T>
        where T : IConstructorDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IConstructorDeclarationBuilder.Append(IConstructorConfiguration, Action{ISourceBuilder})"/>
        new T Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc);

        /// <inheritdoc cref="IConstructorDeclarationBuilder.Append(IStaticConstructorConfiguration, Action{ISourceBuilder})"/>
        new T Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IConstructorDeclarationBuilder
    {
        /// <inheritdoc/>
        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(constructorConfiguration, builderFunc);

        /// <inheritdoc/>
        IConstructorDeclarationBuilder IConstructorDeclarationBuilder.Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(staticConstructorConfiguration, builderFunc);
    }
}