﻿using MaSch.CodeAnalysis.CSharp.Common;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents an in-memory source file.
/// </summary>
public sealed partial class SourceBuilder
{
    /// <summary>
    /// The file header that is added to all source files generated using the <see cref="SourceBuilder"/>.
    /// </summary>
    public static readonly string AutoGeneratedFileHeader = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

";

    private const char IndentChar = ' ';
    private const int StartCapacity = 16 * 1024; // 16KB

    private readonly StringBuilder _builder;
    private readonly CodeConfigurationFactory _configurationFactory = new();
    private bool _isLineIndented = false;
    private bool _isLastLineEmpty;

    private SourceBuilder(int indentSize, int capacity, bool autoAddFileHeader)
    {
        IndentSize = indentSize;
        if (autoAddFileHeader)
            _builder = new StringBuilder(AutoGeneratedFileHeader, capacity);
        else
            _builder = new StringBuilder(capacity);

        _isLastLineEmpty = true;
    }

    /// <inheritdoc cref="ISourceBuilder.IndentSize"/>
    public int IndentSize { get; }

    /// <inheritdoc cref="ISourceBuilder.CurrentIndentLevel"/>
    public int CurrentIndentLevel { get; set; }

    /// <summary>
    /// Creates a new source file builder.
    /// </summary>
    /// <param name="indentSize">The number of whitespace characters to use when indenting.</param>
    /// <param name="capacity">The suggested starting size of the <see cref="SourceBuilder"/>.</param>
    /// <param name="autoAddFileHeader">Determines whether a file header should be automatically added that informs users and IDEs, that the source is auto-generated (see <see cref="AutoGeneratedFileHeader"/>).</param>
    /// <returns>The newly created source file builder.</returns>
    public static ISourceFileBuilder Create(int indentSize = 4, int capacity = StartCapacity, bool autoAddFileHeader = true)
    {
        return new SourceBuilder(indentSize, capacity, autoAddFileHeader);
    }

    /// <inheritdoc cref="ISourceBuilder.AppendRegion(string)" />
    public SourceBuilderCodeBlock AppendRegion(string regionName)
    {
        _ = AppendLine($"#region {regionName}");
        return new SourceBuilderCodeBlock(this, "#endregion", false);
    }

    /// <inheritdoc cref="ISourceBuilder.AppendBlock(string)" />
    public SourceBuilderCodeBlock AppendBlock(string blockLine)
    {
        return AppendBlock(blockLine, false);
    }

    /// <inheritdoc cref="ISourceBuilder.AppendBlock(string, bool)" />
    public SourceBuilderCodeBlock AppendBlock(string blockLine, bool addSemicolon)
    {
        _ = AppendLine(blockLine);
        return AppendBlock(addSemicolon);
    }

    /// <inheritdoc cref="ISourceBuilder.AppendBlock()" />
    public SourceBuilderCodeBlock AppendBlock()
    {
        return AppendBlock(false);
    }

    /// <inheritdoc cref="ISourceBuilder.AppendBlock(bool)" />
    public SourceBuilderCodeBlock AppendBlock(bool addSemicolon)
    {
        _ = AppendLine("{");
        return new SourceBuilderCodeBlock(this, addSemicolon ? "};" : "}", true);
    }

    /// <inheritdoc cref="ISourceBuilder.Indent()" />
    public SourceBuilderCodeBlock Indent()
    {
        return new SourceBuilderCodeBlock(this, null, true);
    }

    /// <inheritdoc cref="ISourceBuilder.AppendLine()" />
    public SourceBuilder AppendLine()
    {
        return Append(Environment.NewLine);
    }

    /// <inheritdoc cref="ISourceBuilder.AppendLine(string)" />
    public SourceBuilder AppendLine(string value)
    {
        return Append(value + Environment.NewLine);
    }

    /// <inheritdoc cref="ISourceBuilder.EnsurePreviousLineEmpty()" />
    public SourceBuilder EnsurePreviousLineEmpty()
    {
        if (!_isLastLineEmpty)
            AppendLine();
        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.Append(string)" />
    public SourceBuilder Append(string value)
    {
        if (value is null || value.Length == 0)
            return this;

        if (CurrentIndentLevel == 0)
        {
            _builder.Append(value);
            return this;
        }

        var indentCount = CurrentIndentLevel * IndentSize;
        var lineStartIndex = 0;
        var lineIsEmpty = !_isLineIndented;

        for (int i = 0; i < value.Length - 1; i++)
        {
            if (value[i] is '\r')
                i++;

            if (value[i] is '\n')
            {
                var lineLength = i - lineStartIndex;
                if (i > 0 && value[i - 1] is '\r')
                    lineLength--;

                if (lineLength > 0)
                {
                    if (!_isLineIndented)
                        _ = _builder.Append(IndentChar, indentCount);
                    _ = _builder.Append(value, lineStartIndex, lineLength);
                }

                _isLastLineEmpty = lineIsEmpty;
                _ = _builder.AppendLine();
                _isLineIndented = false;

                lineStartIndex = i + 1;
                lineIsEmpty = true;
            }

            if (!char.IsWhiteSpace(value[i]) && value[i] is not '{')
                lineIsEmpty = false;
        }

        if (!_isLineIndented && lineStartIndex < value.Length - 1)
        {
            _ = _builder.Append(IndentChar, indentCount);
            _isLineIndented = true;
        }

        _ = _builder.Append(value, lineStartIndex, value.Length - lineStartIndex);
        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.Append(char)" />
    public SourceBuilder Append(char value)
    {
        if (CurrentIndentLevel > 0 && !_isLineIndented)
        {
            _builder.Append(IndentChar, CurrentIndentLevel * IndentSize);
            _isLineIndented = CurrentIndentLevel > 0;
        }

        _builder.Append(value);
        if (value is '\n')
        {
            _isLastLineEmpty = !_isLineIndented;
            _isLineIndented = false;
        }

        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.As{T}" />
    public T As<T>()
        where T : ISourceBuilder
        => (T)(ISourceBuilder)this;

    /// <inheritdoc cref="ISourceBuilder.ToSourceText(Encoding?, SourceHashAlgorithm)"/>
    public SourceText ToSourceText(Encoding? encoding = null, SourceHashAlgorithm checksumAlgorithm = SourceHashAlgorithm.Sha1)
    {
        encoding ??= Encoding.UTF8;
        const int LargeObjectHeapLimitInChars = 40 * 1024;
        if (_builder.Length >= LargeObjectHeapLimitInChars)
        {
            using var reader = new StringBuilderReader(_builder);
            return SourceText.From(reader, _builder.Length, encoding, checksumAlgorithm);
        }

        return SourceText.From(_builder.ToString(), encoding, checksumAlgorithm);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return _builder.ToString();
    }

    private SourceBuilder AppendAsBlock<T>(ICodeConfiguration configuration, T builder, Action<T> builderFunc)
        where T : ISourceBuilder
    {
        configuration.WriteTo(this);
        using (AppendLine().AppendBlock())
            builderFunc(builder);
        return this;
    }

    private SourceBuilder AppendAsExpression<T>(ICodeConfiguration configuration, T builder, Action<T> builderFunc, bool appendLineBreak)
        where T : ISourceBuilder
    {
        configuration.WriteTo(this);
        using (Indent())
        {
            if (appendLineBreak)
                AppendLine();
            else
                Append(' ');
            Append("=> ");
            builderFunc(builder);
        }

        return this;
    }

    private SourceBuilder AppendWithLineTerminator(ICodeConfiguration configuration)
    {
        configuration.WriteTo(this);
        return Append(';').AppendLine();
    }
}