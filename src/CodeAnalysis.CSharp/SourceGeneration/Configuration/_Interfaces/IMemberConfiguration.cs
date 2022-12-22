namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code element that can be a member of a namespace, class, interface, struct or record. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
public interface IMemberConfiguration : ISupportsCodeAttributeConfiguration, ISupportsAccessModifierConfiguration, ISupportsLineCommentsConfiguration
{
    /// <summary>
    /// Gets the name of this <see cref="IMemberConfiguration"/>.
    /// </summary>
    string MemberName { get; }

    /// <summary>
    /// Gets the keywords defined for this <see cref="IMemberConfiguration"/>.
    /// </summary>
    MemberKeyword Keywords { get; }

    /// <summary>
    /// Adds keyword(s) to this <see cref="IMemberConfiguration"/>.
    /// </summary>
    /// <param name="keyword">The keyword(s) to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    IMemberConfiguration WithKeyword(MemberKeyword keyword);
}

/// <inheritdoc cref="IMemberConfiguration"/>
/// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
public interface IMemberConfiguration<T> : IMemberConfiguration, ISupportsCodeAttributeConfiguration<T>, ISupportsAccessModifierConfiguration<T>, ISupportsLineCommentsConfiguration<T>
    where T : IMemberConfiguration<T>
{
    /// <inheritdoc cref="IMemberConfiguration.WithKeyword(MemberKeyword)"/>
    new T WithKeyword(MemberKeyword keyword);
}