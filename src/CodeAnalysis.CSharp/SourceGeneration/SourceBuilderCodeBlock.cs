namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents a code block, that has been opened in the <see cref="SourceBuilder"/>.
/// When disposed the indent in the source <see cref="SourceBuilder"/> is reduced again.
/// </summary>
public readonly struct SourceBuilderCodeBlock : IDisposable
{
    private readonly ISourceBuilder _builder;
    private readonly string? _endContent;
    private readonly bool _changeIndent;
    private readonly string? _lastTypeName;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceBuilderCodeBlock"/> struct.
    /// </summary>
    /// <param name="builder">The <see cref="SourceBuilder"/> that owns the <see cref="SourceBuilderCodeBlock"/>.</param>
    /// <param name="endContent">The text that should be appended after the block is disposed.</param>
    /// <param name="changeIndent">Determines whether indentation should be changed.</param>
    public SourceBuilderCodeBlock(ISourceBuilder builder, string? endContent, bool changeIndent)
    {
        _builder = builder;
        _endContent = endContent;
        _changeIndent = changeIndent;
        _lastTypeName = builder.CurrentTypeName;

        if (changeIndent)
            _builder.CurrentIndentLevel++;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_changeIndent)
            _builder.CurrentIndentLevel--;
        if (_endContent is not null)
        {
            if (_builder is SourceBuilder sourceBuilder && !sourceBuilder.IsCurrentLineEmpty)
                _builder.AppendLine();
            _ = _builder.AppendLine(_endContent);
        }

        _builder.CurrentTypeName = _lastTypeName;
    }
}