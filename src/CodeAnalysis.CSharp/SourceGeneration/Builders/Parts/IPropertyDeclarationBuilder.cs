using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    /// <summary>
    /// Represents a <see cref="ISourceBuilder"/> used to build a property.
    /// </summary>
    public interface IPropertyDeclarationBuilder : ISourceBuilder
    {
        /// <summary>
        /// Appends a property to the current context.
        /// </summary>
        /// <param name="propertyConfiguration">The configuration of the property.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IPropertyDeclarationBuilder Append(IReadWritePropertyConfiguration propertyConfiguration);

        /// <summary>
        /// Appends a property with custom get and set logic to the current context.
        /// </summary>
        /// <param name="propertyConfiguration">The configuration of the property.</param>
        /// <param name="getBuilderFunc">The function to add content to the get method of the property.</param>
        /// <param name="setBuilderFunc">The function to add content to the set method of the property.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IPropertyDeclarationBuilder Append(IReadWritePropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);

        /// <summary>
        /// Appends a read-only property to the current context.
        /// </summary>
        /// <param name="propertyConfiguration">The configuration of the property.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IPropertyDeclarationBuilder Append(IReadOnlyPropertyConfiguration propertyConfiguration);

        /// <summary>
        /// Appends a read-only property with custom get logic to the current context.
        /// </summary>
        /// <param name="propertyConfiguration">The configuration of the property.</param>
        /// <param name="getBuilderFunc">The function to add content to the get method of the property.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IPropertyDeclarationBuilder Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc);

        /// <summary>
        /// Appends a write-only property to the current context.
        /// </summary>
        /// <param name="propertyConfiguration">The configuration of the property.</param>
        /// <param name="setBuilderFunc">The function to add content to the set method of the property.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        IPropertyDeclarationBuilder Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }

    /// <inheritdoc cref="IPropertyDeclarationBuilder"/>
    /// <typeparam name="T">The type of <see cref="ISourceBuilder{T}"/>.</typeparam>
    public interface IPropertyDeclarationBuilder<T> : IPropertyDeclarationBuilder, ISourceBuilder<T>
        where T : IPropertyDeclarationBuilder<T>
    {
        /// <inheritdoc cref="IPropertyDeclarationBuilder.Append(IReadWritePropertyConfiguration)"/>
        new T Append(IReadWritePropertyConfiguration propertyConfiguration);

        /// <inheritdoc cref="IPropertyDeclarationBuilder.Append(IReadWritePropertyConfiguration, Action{ISourceBuilder}, Action{ISourceBuilder})"/>
        new T Append(IReadWritePropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc);

        /// <inheritdoc cref="IPropertyDeclarationBuilder.Append(IReadOnlyPropertyConfiguration)"/>
        new T Append(IReadOnlyPropertyConfiguration propertyConfiguration);

        /// <inheritdoc cref="IPropertyDeclarationBuilder.Append(IReadOnlyPropertyConfiguration, Action{ISourceBuilder})"/>
        new T Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc);

        /// <inheritdoc cref="IPropertyDeclarationBuilder.Append(IWriteOnlyPropertyConfiguration, Action{ISourceBuilder})"/>
        new T Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IPropertyDeclarationBuilder
    {
        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IReadWritePropertyConfiguration propertyConfiguration)
            => Append(propertyConfiguration, null, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IReadWritePropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(propertyConfiguration, getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IReadOnlyPropertyConfiguration propertyConfiguration)
            => Append(propertyConfiguration, null);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc)
            => Append(propertyConfiguration, getBuilderFunc);

        /// <inheritdoc/>
        IPropertyDeclarationBuilder IPropertyDeclarationBuilder.Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc)
            => Append(propertyConfiguration, setBuilderFunc);
    }
}