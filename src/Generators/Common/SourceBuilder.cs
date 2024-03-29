﻿namespace MaSch.Generators.Common;

/// <summary>
/// Represents a in-memory source file.
/// </summary>
public class SourceBuilder
{
    private const int IdentCharCount = 4;

    private readonly StringBuilder _builder;
    private int _currentIndent = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceBuilder"/> class.
    /// </summary>
    public SourceBuilder()
    {
        _builder = new StringBuilder(@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

");
    }

    /// <summary>
    /// Adds a new region to the source file.
    /// </summary>
    /// <param name="regionName">Name of the region.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the region when disposed.</returns>
    public IDisposable AddRegion(string regionName)
    {
        _ = AppendLine($"#region {regionName}");
        return new CodeBlock(this, "#endregion", false);
    }

    /// <summary>
    /// Adds a new code block to the source file.
    /// </summary>
    /// <param name="blockLine">The line before the code block.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    public IDisposable AddBlock(string blockLine)
    {
        return AddBlock(blockLine, false);
    }

    /// <summary>
    /// Adds a new code block to the source file.
    /// </summary>
    /// <param name="blockLine">The line before the code block.</param>
    /// <param name="addSemicolon">if <c>true</c> adds a semicolon after the end of the block.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    public IDisposable AddBlock(string blockLine, bool addSemicolon)
    {
        _ = AppendLine(blockLine);
        return AddBlock(addSemicolon);
    }

    /// <summary>
    /// Adds a new code block to the source file.
    /// </summary>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    public IDisposable AddBlock()
    {
        return AddBlock(false);
    }

    /// <summary>
    /// Adds a new code block to the source file.
    /// </summary>
    /// <param name="addSemicolon">if <c>true</c> adds a semicolon after the end of the block.</param>
    /// <returns>Returns an <see cref="IDisposable"/> object, which closes the code block when disposed.</returns>
    public IDisposable AddBlock(bool addSemicolon)
    {
        _ = AppendLine("{");
        _currentIndent++;
        return new CodeBlock(this, addSemicolon ? "};" : "}", true);
    }

    /// <summary>
    /// Appends the default line terminator to the end of the current <see cref="SourceBuilder"/> object.
    /// </summary>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public SourceBuilder AppendLine()
    {
        return Append(Environment.NewLine);
    }

    /// <summary>
    /// Appends a copy of the specified string followed by the default line terminator to the end of the current <see cref="SourceBuilder"/> object.
    /// </summary>
    /// <param name="value">The string to append.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public SourceBuilder AppendLine(string value)
    {
        return Append(value + Environment.NewLine);
    }

    /// <summary>
    /// Appends a copy of the specified string to this instance.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    public SourceBuilder Append(string value)
    {
        var indent = new string(' ', _currentIndent * IdentCharCount);
        var lines = value.Replace("\r", string.Empty).Split(new[] { '\n' }, StringSplitOptions.None);
        foreach (var line in lines.Take(lines.Length - 1))
        {
            if (string.IsNullOrWhiteSpace(line))
                _ = _builder.AppendLine();
            else
                _ = _builder.Append(indent).AppendLine(line);
        }

        if (lines.Length > 1 && !string.IsNullOrWhiteSpace(lines[^1]))
            _ = _builder.Append(indent);
        _ = _builder.Append(lines[^1]);
        return this;
    }

    /// <summary>
    /// Converts the value of this instance to a System.String.
    /// </summary>
    /// <returns>A string whose value is the same as this instance.</returns>
    public override string ToString()
    {
        return _builder.ToString();
    }

    private sealed class CodeBlock : IDisposable
    {
        private readonly SourceBuilder _builder;
        private readonly string _endContent;
        private readonly bool _changeIndent;

        public CodeBlock(SourceBuilder builder, string endContent, bool changeIndent)
        {
            _builder = builder;
            _endContent = endContent;
            _changeIndent = changeIndent;
        }

        public void Dispose()
        {
            if (_changeIndent)
                _builder._currentIndent--;
            _ = _builder.AppendLine(_endContent);
        }
    }
}
