namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

public interface ICodeBlockConfiguration : ICodeConfiguration
{
    string? BlockPrefix { get; }
    string? BlockSuffix { get; }
    string OpeningBracket { get; }
    string ClosingBracket { get; }
    CodeBlockStyle Style { get; }

    ICodeBlockConfiguration WithBrackets(string openingBracket, string closingBracket);
    ICodeBlockConfiguration WithStyle(CodeBlockStyle style);
    ICodeBlockConfiguration WithPrefix(string? blockPrefix);
    ICodeBlockConfiguration WithSuffix(string? blockSuffix);

    void WriteStartTo(ISourceBuilder sourceBuilder);
    void WriteEndTo(ISourceBuilder sourceBuilder);
}

internal class CodeBlockConfiguration : ICodeBlockConfiguration
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
