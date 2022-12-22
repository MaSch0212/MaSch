namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code element for which comments can be defined. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
public interface ISupportsLineCommentsConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets a read-only list of comments attached to this <see cref="ISupportsLineCommentsConfiguration"/>.
    /// </summary>
    IReadOnlyList<ICommentConfiguration> Comments { get; }

    /// <summary>
    /// Adds a line comment (<see cref="CommentType.Line"/>) to this <see cref="ISupportsLineCommentsConfiguration"/>.
    /// </summary>
    /// <param name="comment">The comment to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ISupportsLineCommentsConfiguration WithLineComment(string comment);

    /// <summary>
    /// Adds a block comment (<see cref="CommentType.Block"/>) to this <see cref="ISupportsLineCommentsConfiguration"/>.
    /// </summary>
    /// <param name="comment">The comment to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ISupportsLineCommentsConfiguration WithBlockComment(string comment);

    /// <summary>
    /// Adds a XML documentation comment (<see cref="CommentType.Doc"/>) to this <see cref="ISupportsLineCommentsConfiguration"/>.
    /// </summary>
    /// <param name="comment">The comment to add.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ISupportsLineCommentsConfiguration WithDocComment(string comment);
}

/// <inheritdoc cref="ISupportsLineCommentsConfiguration"/>
/// <typeparam name="T">The type of <see cref="ICodeConfiguration"/>.</typeparam>
public interface ISupportsLineCommentsConfiguration<T> : ISupportsLineCommentsConfiguration
    where T : ISupportsLineCommentsConfiguration<T>
{
    /// <inheritdoc cref="ISupportsLineCommentsConfiguration.WithLineComment(string)"/>
    new T WithLineComment(string comment);

    /// <inheritdoc cref="ISupportsLineCommentsConfiguration.WithBlockComment(string)"/>
    new T WithBlockComment(string comment);

    /// <inheritdoc cref="ISupportsLineCommentsConfiguration.WithDocComment(string)"/>
    new T WithDocComment(string comment);
}