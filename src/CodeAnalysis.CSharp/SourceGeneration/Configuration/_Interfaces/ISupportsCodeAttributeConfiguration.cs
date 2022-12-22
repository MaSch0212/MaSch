namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code element for which code attributes can be defined. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
public interface ISupportsCodeAttributeConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets a read-only list of code attributes attached to this <see cref="ISupportsCodeAttributeConfiguration"/>.
    /// </summary>
    IReadOnlyList<ICodeAttributeConfiguration> Attributes { get; }

    /// <summary>
    /// Adds a code attribute to this <see cref="ISupportsCodeAttributeConfiguration"/>.
    /// </summary>
    /// <param name="attributeTypeName">The type name of the code attribute to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ISupportsCodeAttributeConfiguration WithCodeAttribute(string attributeTypeName);

    /// <summary>
    /// Adds a code attribute to this <see cref="ISupportsCodeAttributeConfiguration"/>.
    /// </summary>
    /// <param name="attributeTypeName">The type name of the code attribute to add.</param>
    /// <param name="attributeConfiguration">A function to configure the added code attribute.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ISupportsCodeAttributeConfiguration WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration);
}

/// <inheritdoc cref="ISupportsCodeAttributeConfiguration"/>
/// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
public interface ISupportsCodeAttributeConfiguration<T> : ISupportsCodeAttributeConfiguration
    where T : ISupportsCodeAttributeConfiguration<T>
{
    /// <inheritdoc cref="ISupportsCodeAttributeConfiguration.WithCodeAttribute(string)"/>
    new T WithCodeAttribute(string attributeTypeName);

    /// <inheritdoc cref="ISupportsCodeAttributeConfiguration.WithCodeAttribute(string, Action{ICodeAttributeConfiguration})"/>
    new T WithCodeAttribute(string attributeTypeName, Action<ICodeAttributeConfiguration> attributeConfiguration);
}