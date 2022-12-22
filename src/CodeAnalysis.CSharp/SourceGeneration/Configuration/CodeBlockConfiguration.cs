namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a code block code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface ICodeBlockConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets the prefix of the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    /// <remarks>
    /// The prefix is written right before the opening bracket. This is useful for cases where the code block is associated with another code element (like if-statement, class, namespace, ...).
    /// <code>
    /// var myVar = new string[] // &lt;-- This line is the prefix
    /// {
    ///     "Item1",
    ///     "Item2",
    /// };
    /// </code>
    /// </remarks>
    string? BlockPrefix { get; }

    /// <summary>
    /// Gets the suffix of the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    /// <remarks>
    /// The suffix is written right after the closing bracket. This is useful for cases where the code block is associated with another code element that is assigned to variable or given das a parameter (like array, list, ...).
    /// <code>
    /// var myVar = new string[]
    /// {
    ///     "Item1",
    ///     "Item2",
    /// }; // The ';' is the suffix
    /// </code>
    /// </remarks>
    string? BlockSuffix { get; }

    /// <summary>
    /// Gets the opening bracket of the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    string OpeningBracket { get; }

    /// <summary>
    /// Gets the closing bracket of the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    string ClosingBracket { get; }

    /// <summary>
    /// Gets the spacing style for the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    CodeBlockStyle Style { get; }

    /// <summary>
    /// Sets the opening and closing brackets of the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    /// <param name="openingBracket">The opening bracket to use.</param>
    /// <param name="closingBracket">The closing bracket to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ICodeBlockConfiguration WithBrackets(string openingBracket, string closingBracket);

    /// <summary>
    /// Sets the spacing style for the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    /// <param name="style">The spacing style to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ICodeBlockConfiguration WithStyle(CodeBlockStyle style);

    /// <summary>
    /// Sets the prefix of the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    /// <param name="blockPrefix">The prefix to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ICodeBlockConfiguration WithPrefix(string? blockPrefix);

    /// <summary>
    /// Sets the suffix of the code block represented by this <see cref="ICodeBlockConfiguration"/>.
    /// </summary>
    /// <param name="blockSuffix">The suffix to use.</param>
    /// <returns>A reference to this instance after the operation has completed.</returns>
    ICodeBlockConfiguration WithSuffix(string? blockSuffix);

    /// <summary>
    /// Writes the start (prefix & opening bracket) of the code block represented by this <see cref="ICodeConfiguration"/> to the target <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
    void WriteStartTo(ISourceBuilder sourceBuilder);

    /// <summary>
    /// Writes the end (cloding bracket & suffix) of the code block represented by this <see cref="ICodeConfiguration"/> to the target <see cref="ISourceBuilder"/>.
    /// </summary>
    /// <param name="sourceBuilder">The <see cref="ISourceBuilder"/> to write the code to.</param>
    void WriteEndTo(ISourceBuilder sourceBuilder);
}

internal sealed class CodeBlockConfiguration : ICodeBlockConfiguration
{
    public CodeBlockConfiguration()
    {
    }

    public CodeBlockConfiguration(string? blockPrefix)
    {
        BlockPrefix = blockPrefix;
    }

    public CodeBlockConfiguration(string? blockPrefix, string? blockSuffix)
    {
        BlockPrefix = blockPrefix;
        BlockSuffix = blockSuffix;
    }

    public string? BlockPrefix { get; private set; }
    public string? BlockSuffix { get; private set; }
    public string OpeningBracket { get; private set; } = "{";
    public string ClosingBracket { get; private set; } = "}";
    public CodeBlockStyle Style { get; private set; } = CodeBlockStyle.Default;

    public ICodeBlockConfiguration WithBrackets(string openingBracket, string closingBracket)
    {
        OpeningBracket = openingBracket;
        ClosingBracket = closingBracket;
        return this;
    }

    public ICodeBlockConfiguration WithPrefix(string? blockPrefix)
    {
        BlockPrefix = blockPrefix;
        return this;
    }

    public ICodeBlockConfiguration WithStyle(CodeBlockStyle style)
    {
        Style = style;
        return this;
    }

    public ICodeBlockConfiguration WithSuffix(string? blockSuffix)
    {
        BlockSuffix = blockSuffix;
        return this;
    }

    public void WriteStartTo(ISourceBuilder sourceBuilder)
    {
        if (BlockPrefix is not null)
        {
            if (Style.HasFlag(CodeBlockStyle.EnsureBlockPrefixOnEmptyLine))
                sourceBuilder.EnsureCurrentLineEmpty();
            sourceBuilder.Append(BlockPrefix);
        }

        if (Style.HasFlag(CodeBlockStyle.EnsureOpeningBracketOnEmptyLine))
            sourceBuilder.EnsureCurrentLineEmpty();
        else if (Style.HasFlag(CodeBlockStyle.EnsureBracketSpacing))
            EnsurePreviousCharacterWhitespace(sourceBuilder);

        sourceBuilder.Append(OpeningBracket);

        if (Style.HasFlag(CodeBlockStyle.AppendLineAfterOpeningBracket))
            sourceBuilder.AppendLine();
        else if (Style.HasFlag(CodeBlockStyle.EnsureBracketSpacing))
            sourceBuilder.Append(' ');
    }

    public void WriteEndTo(ISourceBuilder sourceBuilder)
    {
        if (Style.HasFlag(CodeBlockStyle.EnsureClosingBracketOnEmptyLine))
            sourceBuilder.EnsureCurrentLineEmpty();
        else if (Style.HasFlag(CodeBlockStyle.EnsureBracketSpacing))
            EnsurePreviousCharacterWhitespace(sourceBuilder);

        sourceBuilder.Append(ClosingBracket);

        if (BlockSuffix is not null)
        {
            if (Style.HasFlag(CodeBlockStyle.EnsureBlockSuffixOnEmptyLine))
                sourceBuilder.EnsureCurrentLineEmpty();
            sourceBuilder.Append(BlockSuffix);
        }

        if (Style.HasFlag(CodeBlockStyle.AppendLineAfterBlock))
            sourceBuilder.AppendLine();
    }

    public void WriteTo(ISourceBuilder sourceBuilder)
    {
        WriteStartTo(sourceBuilder);
        WriteEndTo(sourceBuilder);
    }

    private static void EnsurePreviousCharacterWhitespace(ISourceBuilder sourceBuilder)
    {
        if (sourceBuilder.Length == 0)
            return;
        if (!char.IsWhiteSpace(sourceBuilder[sourceBuilder.Length - 1]))
            sourceBuilder.Append(' ');
    }
}
