namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code element that can derives from another type. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
public interface ISupportsInheritanceConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets the base type of this <see cref="ISupportsInheritanceConfiguration"/>. Returns <c>null</c> if the <see cref="ISupportsInheritanceConfiguration"/> does not derives from another type.
    /// </summary>
    string? BaseType { get; }

    /// <summary>
    /// Sets the base type of this <see cref="ISupportsInheritanceConfiguration"/>.
    /// </summary>
    /// <param name="typeName">The type name to use as the base type.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ISupportsInheritanceConfiguration DerivesFrom(string typeName);
}

/// <inheritdoc cref="ISupportsInheritanceConfiguration"/>
/// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
public interface ISupportsInheritanceConfiguration<T> : ISupportsInheritanceConfiguration
    where T : ISupportsInheritanceConfiguration<T>
{
    /// <inheritdoc cref="ISupportsInheritanceConfiguration.DerivesFrom(string)"/>
    new T DerivesFrom(string typeName);
}
