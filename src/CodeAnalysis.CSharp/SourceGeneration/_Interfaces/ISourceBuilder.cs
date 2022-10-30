namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceBuilder
{
    /// <summary>
    /// Gets the size of the indentation.
    /// </summary>
    int IndentSize { get; }

    /// <summary>
    /// Gets or sets the current indentation level.
    /// </summary>
    int CurrentIndentLevel { get; set; }

    /// <summary>
    /// Appends a new region to the source file.
    /// </summary>
    /// <param name="regionName">Name of the region.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the region when disposed.</returns>
    SourceBuilderCodeBlock AppendRegion(string regionName);

    /// <summary>
    /// Appends a new code block to the source file.
    /// </summary>
    /// <param name="blockLine">The line before the code block.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    SourceBuilderCodeBlock AppendBlock(string blockLine);

    /// <summary>
    /// Appends a new code block to the source file.
    /// </summary>
    /// <param name="blockLine">The line before the code block.</param>
    /// <param name="addSemicolon">if <c>true</c> adds a semicolon after the end of the block.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    SourceBuilderCodeBlock AppendBlock(string blockLine, bool addSemicolon);

    /// <summary>
    /// Appends a new code block to the source file.
    /// </summary>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    SourceBuilderCodeBlock AppendBlock();

    /// <summary>
    /// Appends a new code block to the source file.
    /// </summary>
    /// <param name="addSemicolon">if <c>true</c> adds a semicolon after the end of the block.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    SourceBuilderCodeBlock AppendBlock(bool addSemicolon);

    /// <summary>
    /// Adds one indentation level. If the current line already contains characters, only subsequent lines are affected.
    /// </summary>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    SourceBuilderCodeBlock Indent();

    /// <summary>
    /// Appends the default line terminator to the end of the current <see cref="SourceBuilder"/> object.
    /// </summary>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceBuilder AppendLine();

    /// <summary>
    /// Appends a copy of the specified string followed by the default line terminator to the end of the current <see cref="SourceBuilder"/> object.
    /// </summary>
    /// <param name="value">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceBuilder AppendLine(string value);

    /// <summary>
    /// Ensures that the line above the current line is empty or consists only of whitespace characters.
    /// </summary>
    /// <returns>A value indicating whether the line above the current line is empty.</returns>
    ISourceBuilder EnsurePreviousLineEmpty();

    /// <summary>
    /// Appends a copy of the specified string to this instance.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceBuilder Append(string value);

    /// <summary>
    /// Appends the string representation of a specified <see cref="char"/> object to this instance.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceBuilder Append(char value);

    /// <summary>
    /// Converts the value of this instance to a <see cref="SourceText"/>.
    /// </summary>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="checksumAlgorithm">The source hash algorithm to use.</param>
    /// <returns>A <see cref="SourceText"/> whose value is the same as this instance.</returns>
    SourceText ToSourceText(Encoding? encoding = null, SourceHashAlgorithm checksumAlgorithm = SourceHashAlgorithm.Sha1);

    T As<T>()
        where T : ISourceBuilder;
}

public interface ISourceBuilder<T> : ISourceBuilder
    where T : ISourceBuilder<T>
{
    /// <inheritdoc cref="ISourceBuilder.AppendLine()" />
    new T AppendLine();

    /// <inheritdoc cref="ISourceBuilder.AppendLine(string)" />
    new T AppendLine(string value);

    /// <inheritdoc cref="ISourceBuilder.EnsurePreviousLineEmpty()" />
    new T EnsurePreviousLineEmpty();

    /// <inheritdoc cref="ISourceBuilder.Append(string)" />
    new T Append(string value);

    /// <inheritdoc cref="ISourceBuilder.Append(char)" />
    new T Append(char value);
}

[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Already has documentation")]
public partial class SourceBuilder : ISourceBuilder
{
    /// <inheritdoc/>
    int ISourceBuilder.IndentSize => IndentSize;

    /// <inheritdoc/>
    int ISourceBuilder.CurrentIndentLevel
    {
        get => CurrentIndentLevel;
        set => CurrentIndentLevel = value;
    }

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.Append(string value) => Append(value);

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.Append(char value) => Append(value);

    /// <inheritdoc/>
    SourceBuilderCodeBlock ISourceBuilder.AppendBlock(string blockLine) => AppendBlock(blockLine);

    /// <inheritdoc/>
    SourceBuilderCodeBlock ISourceBuilder.AppendBlock(string blockLine, bool addSemicolon) => AppendBlock(blockLine, addSemicolon);

    /// <inheritdoc/>
    SourceBuilderCodeBlock ISourceBuilder.AppendBlock() => AppendBlock();

    /// <inheritdoc/>
    SourceBuilderCodeBlock ISourceBuilder.AppendBlock(bool addSemicolon) => AppendBlock(addSemicolon);

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.AppendLine() => AppendLine();

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.AppendLine(string value) => AppendLine(value);

    /// <inheritdoc/>
    SourceBuilderCodeBlock ISourceBuilder.AppendRegion(string regionName) => AppendRegion(regionName);

    /// <inheritdoc/>
    T ISourceBuilder.As<T>() => (T)(ISourceBuilder)this;

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.EnsurePreviousLineEmpty() => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    SourceBuilderCodeBlock ISourceBuilder.Indent() => Indent();

    /// <inheritdoc/>
    SourceText ISourceBuilder.ToSourceText(Encoding? encoding, SourceHashAlgorithm checksumAlgorithm) => ToSourceText(encoding, checksumAlgorithm);
}