using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceBuilder
{
    /// <summary>
    /// Gets the options of this <see cref="ISourceBuilder"/>.
    /// </summary>
    SourceBuilderOptions Options { get; }

    /// <summary>
    /// Gets the length of the current <see cref="ISourceBuilder"/> object.
    /// </summary>
    int Length { get; }

    /// <summary>
    /// Gets or sets the current indentation level.
    /// </summary>
    int CurrentIndentLevel { get; set; }

    /// <summary>
    /// Gets or sets the name of the currently generated type.
    /// </summary>
    string? CurrentTypeName { get; set; }

    /// <summary>
    /// Gets the character at the specified index.
    /// </summary>
    /// <param name="index">The index of the character to get.</param>
    /// <returns>The character at <paramref name="index"/>.</returns>
    char this[int index] { get; }

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
    /// Adds a region. Will ensure the region is placed in its own line.
    /// </summary>
    /// <param name="regionConfiguration">The region configuration.</param>
    /// <param name="builderFunc">The function to add indented content.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceBuilder Append(IRegionConfiguration regionConfiguration, Action<ISourceBuilder> builderFunc);

    /// <summary>
    /// Adds a code block.
    /// </summary>
    /// <param name="codeBlockConfiguration">The block configuration.</param>
    /// <param name="builderFunc">The function to add indented content.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceBuilder Append(ICodeBlockConfiguration codeBlockConfiguration, Action<ISourceBuilder> builderFunc);

    /// <summary>
    /// Ensures that the line above the current line is empty or consists only of whitespace characters.
    /// </summary>
    /// <returns>A value indicating whether the line above the current line is empty.</returns>
    ISourceBuilder EnsurePreviousLineEmpty();

    /// <summary>
    /// Ensures that the current line is empty or consists only of whitespace characters.
    /// </summary>
    /// <returns>A value indicating whether the current line is empty.</returns>
    ISourceBuilder EnsureCurrentLineEmpty();

    /// <summary>
    /// Adds one indentation level. If the current line already contains characters, only subsequent lines are affected.
    /// </summary>
    /// <param name="builderFunc">The function to add indented content.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceBuilder Indent(Action<ISourceBuilder> builderFunc);

    /// <summary>
    /// Converts the value of this instance to a <see cref="SourceText"/>.
    /// </summary>
    /// <param name="encoding">The encoding to use.</param>
    /// <param name="checksumAlgorithm">The source hash algorithm to use.</param>
    /// <returns>A <see cref="SourceText"/> whose value is the same as this instance.</returns>
    SourceText ToSourceText(Encoding? encoding = null, SourceHashAlgorithm checksumAlgorithm = SourceHashAlgorithm.Sha1);

    /// <summary>
    /// Changes the type of this <see cref="ISourceBuilder"/> to another source builder interface.
    /// </summary>
    /// <typeparam name="T">The source builder interface to change to.</typeparam>
    /// <returns>The builder implementing <typeparamref name="T"/>.</returns>
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

    /// <inheritdoc cref="ISourceBuilder.Append(string)" />
    new T Append(string value);

    /// <inheritdoc cref="ISourceBuilder.Append(char)" />
    new T Append(char value);

    /// <inheritdoc cref="ISourceBuilder.Append(IRegionConfiguration, Action{ISourceBuilder})" />
    T Append(IRegionConfiguration regionConfiguration, Action<T> builderFunc);

    /// <inheritdoc cref="ISourceBuilder.Append(ICodeBlockConfiguration, Action{ISourceBuilder})" />
    T Append(ICodeBlockConfiguration codeBlockConfiguration, Action<T> builderFunc);

    /// <inheritdoc cref="ISourceBuilder.EnsurePreviousLineEmpty()" />
    new T EnsurePreviousLineEmpty();

    /// <inheritdoc cref="ISourceBuilder.EnsureCurrentLineEmpty()" />
    new T EnsureCurrentLineEmpty();

    /// <inheritdoc cref="ISourceBuilder.Indent(Action{ISourceBuilder})" />
    T Indent(Action<T> builderFunc);
}

[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Already has documentation")]
public partial class SourceBuilder : ISourceBuilder
{
    /// <inheritdoc/>
    SourceBuilderOptions ISourceBuilder.Options => Options;

    /// <inheritdoc/>
    int ISourceBuilder.CurrentIndentLevel
    {
        get => CurrentIndentLevel;
        set => CurrentIndentLevel = value;
    }

    /// <inheritdoc/>
    public int Length => _builder.Length;

    /// <inheritdoc/>
    public char this[int index] => _builder[index];

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.Append(string value) => Append(value);

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.Append(char value) => Append(value);

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.Append(IRegionConfiguration regionConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(regionConfiguration, builderFunc);

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.Append(ICodeBlockConfiguration codeBlockConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(codeBlockConfiguration, builderFunc);

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.AppendLine() => AppendLine();

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.AppendLine(string value) => AppendLine(value);

    /// <inheritdoc/>
    T ISourceBuilder.As<T>() => As<T>();

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.EnsurePreviousLineEmpty() => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.EnsureCurrentLineEmpty() => EnsureCurrentLineEmpty();

    /// <inheritdoc/>
    ISourceBuilder ISourceBuilder.Indent(Action<ISourceBuilder> builderFunc) => Indent(builderFunc);

    /// <inheritdoc/>
    SourceText ISourceBuilder.ToSourceText(Encoding? encoding, SourceHashAlgorithm checksumAlgorithm) => ToSourceText(encoding, checksumAlgorithm);
}