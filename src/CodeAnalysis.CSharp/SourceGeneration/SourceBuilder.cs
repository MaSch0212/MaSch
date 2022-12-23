﻿using MaSch.CodeAnalysis.CSharp.Common;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

#pragma warning disable S3241 // Methods should not return values that are never used

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents an in-memory source file.
/// </summary>
public sealed partial class SourceBuilder
{
    /// <summary>
    /// The file header that is added to all source files generated using the <see cref="SourceBuilder"/> if <see cref="SourceBuilderOptions.IncludeFileHeader"/> is set to <c>true</c>.
    /// </summary>
    public static readonly string AutoGeneratedFileHeader = """
        //------------------------------------------------------------------------------
        // <auto-generated>
        //     This code was generated by a tool.
        //
        //     Changes to this file may cause incorrect behavior and will be lost if
        //     the code is regenerated.
        // </auto-generated>
        //------------------------------------------------------------------------------

        """;

    private const char IndentChar = ' ';

    private readonly StringBuilder _builder;
    private bool _isLineIndented = false;
    private bool _isLastLineEmpty;
    private bool _isCurrentLineEmpty;

    private SourceBuilder(SourceBuilderOptions options)
    {
        Options = options;
        if (options.IncludeFileHeader)
            _builder = new StringBuilder(AutoGeneratedFileHeader, options.Capacity);
        else
            _builder = new StringBuilder(options.Capacity);

        _isLastLineEmpty = true;
        _isCurrentLineEmpty = true;
    }

    /// <inheritdoc cref="ISourceBuilder.Options"/>
    public SourceBuilderOptions Options { get; }

    /// <inheritdoc cref="ISourceBuilder.CurrentIndentLevel"/>
    public int CurrentIndentLevel { get; set; }

    /// <inheritdoc cref="ISourceBuilder.CurrentTypeName"/>
    public string? CurrentTypeName { get; set; }

    /// <summary>
    /// Creates a new source file builder.
    /// </summary>
    /// <param name="options">The options to use for this <see cref="SourceBuilder"/>.</param>
    /// <returns>The newly created source file builder.</returns>
    public static ISourceFileBuilder Create(SourceBuilderOptions options)
    {
        return new SourceBuilder(options);
    }

    /// <summary>
    /// Creates a new source file builder using the default options.
    /// </summary>
    /// <returns>The newly created source file builder.</returns>
    public static ISourceFileBuilder Create()
    {
        return new SourceBuilder(SourceBuilderOptions.Default);
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

    /// <inheritdoc cref="ISourceBuilder.Append(string)" />
    public SourceBuilder Append(string value)
    {
        if (value is null || value.Length == 0)
            return this;

        var indentCount = CurrentIndentLevel * Options.IndentSize;
        var lineStartIndex = 0;

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] is '\r')
                continue;

            if (value[i] is '\n')
            {
                var lineLength = i - lineStartIndex;
                for (int j = i; j > 0 && value[j - 1] is '\r'; j--)
                    lineLength--;

                if (lineLength > 0)
                {
                    if (!_isLineIndented)
                        _ = _builder.Append(IndentChar, indentCount);
                    _ = _builder.Append(value, lineStartIndex, lineLength);
                }

                _isLastLineEmpty = _isCurrentLineEmpty;
                _ = _builder.AppendLine();
                _isCurrentLineEmpty = true;
                _isLineIndented = false;

                lineStartIndex = i + 1;
            }

            if (!char.IsWhiteSpace(value[i]) && value[i] is not '{')
                _isCurrentLineEmpty = false;
        }

        if (!_isLineIndented && lineStartIndex < value.Length)
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
        if (value is '\r')
            return this;

        if (!_isLineIndented)
        {
            _builder.Append(IndentChar, CurrentIndentLevel * Options.IndentSize);
            _isLineIndented = true;
        }

        _builder.Append(value);
        if (value is '\n')
        {
            _isLastLineEmpty = _isCurrentLineEmpty;
            _isCurrentLineEmpty = true;
            _isLineIndented = false;
        }
        else if (value is not '{')
        {
            _isCurrentLineEmpty = false;
        }

        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.Append(IRegionConfiguration, Action{ISourceBuilder})" />
    public SourceBuilder Append(IRegionConfiguration regionConfiguration, Action<SourceBuilder> builderFunc)
    {
        regionConfiguration.WriteStartTo(this);
        builderFunc(this);
        regionConfiguration.WriteEndTo(this);
        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.Append(ICodeBlockConfiguration, Action{ISourceBuilder})" />
    public SourceBuilder Append(ICodeBlockConfiguration codeBlockConfiguration, Action<SourceBuilder> builderFunc)
    {
        codeBlockConfiguration.WriteStartTo(this);
        Indent(builderFunc);
        codeBlockConfiguration.WriteEndTo(this);
        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.EnsurePreviousLineEmpty()" />
    public SourceBuilder EnsurePreviousLineEmpty()
    {
        if (_isLineIndented)
            AppendLine();
        if (!_isLastLineEmpty)
            AppendLine();
        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.EnsureCurrentLineEmpty()" />
    public SourceBuilder EnsureCurrentLineEmpty()
    {
        if (_isLineIndented)
            AppendLine();
        return this;
    }

    /// <inheritdoc cref="ISourceBuilder.Indent(Action{ISourceBuilder})" />
    public SourceBuilder Indent(Action<SourceBuilder> builderFunc)
    {
        var lastTypeName = CurrentTypeName;
        CurrentIndentLevel++;

        try
        {
            builderFunc(this);
        }
        finally
        {
            CurrentIndentLevel--;
            CurrentTypeName = lastTypeName;
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

    private SourceBuilder AppendAsBlock(ICodeConfiguration configuration, Action<SourceBuilder> builderFunc)
    {
        configuration.WriteTo(this);
        Append(CodeConfiguration.Block(), _ =>
        {
            if (configuration is ITypeConfiguration typeConfiguration)
                CurrentTypeName = typeConfiguration.MemberNameWithoutGenericParameters;
            builderFunc(this);
        });

        return this;
    }

    private SourceBuilder AppendAsExpression<T>(ICodeConfiguration configuration, T builder, Action<T> builderFunc, bool appendLineBreak)
        where T : ISourceBuilder
    {
        configuration.WriteTo(this);
        Indent(_ =>
        {
            if (appendLineBreak)
                AppendLine();
            else
                Append(' ');
            Append("=> ");
            builderFunc(builder);
            Append(';').AppendLine();
        });

        return this;
    }

    private SourceBuilder AppendWithLineTerminator(ICodeConfiguration configuration, bool appendLine = true)
    {
        configuration.WriteTo(this);
        Append(';');
        if (appendLine)
            AppendLine();
        return this;
    }

    private SourceBuilder EnsurePreviousLineEmpty(ICodeConfiguration configuration, bool condition)
    {
        if (!condition && Options.EnsureEmptyLineBeforeMembersWithComments && configuration is ISupportsLineCommentsConfiguration supportsLineCommentsConfiguration)
            condition = supportsLineCommentsConfiguration.Comments.Count > 0;
        if (!condition && Options.EnsureEmptyLineBeforeMembersWithAttributes && configuration is ISupportsCodeAttributeConfiguration supportsCodeAttributeConfiguration)
            condition = supportsCodeAttributeConfiguration.Attributes.Count > 0;
        return condition ? EnsurePreviousLineEmpty() : this;
    }

    private SourceBuilder Append(INamespaceImportConfiguration namespaceImportConfiguration)
        => AppendWithLineTerminator(namespaceImportConfiguration);

    private SourceBuilder Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder>? builderFunc)
    {
        EnsurePreviousLineEmpty(namespaceConfiguration, Options.EnsureEmptyLineBeforeNamespaces);

        if (builderFunc is null)
            return AppendWithLineTerminator(namespaceConfiguration);

        return AppendAsBlock(namespaceConfiguration, builderFunc);
    }

    private SourceBuilder Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc)
        => EnsurePreviousLineEmpty(classConfiguration, Options.EnsureEmptyLineBeforeTypes).AppendAsBlock(classConfiguration, builderFunc);

    private SourceBuilder Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
        => EnsurePreviousLineEmpty(interfaceConfguration, Options.EnsureEmptyLineBeforeTypes).AppendAsBlock(interfaceConfguration, builderFunc);

    private SourceBuilder Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder>? builderFunc)
    {
        EnsurePreviousLineEmpty(recordConfiguration, Options.EnsureEmptyLineBeforeTypes);

        if (builderFunc is null)
            return AppendWithLineTerminator(recordConfiguration);

        return AppendAsBlock(recordConfiguration, builderFunc);
    }

    private SourceBuilder Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
        => EnsurePreviousLineEmpty(structConfiguration, Options.EnsureEmptyLineBeforeTypes).AppendAsBlock(structConfiguration, builderFunc);

    private SourceBuilder Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
        => EnsurePreviousLineEmpty(enumConfiguration, Options.EnsureEmptyLineBeforeTypes).AppendAsBlock(enumConfiguration, builderFunc);

    private SourceBuilder Append(IDelegateConfiguration delegateConfiguration)
        => EnsurePreviousLineEmpty(delegateConfiguration, Options.EnsureEmptyLineBeforeTypes).AppendWithLineTerminator(delegateConfiguration);

    private SourceBuilder Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
        => EnsurePreviousLineEmpty(constructorConfiguration, Options.EnsureEmptyLineBeforeConstructors).AppendAs(constructorConfiguration.BodyType, constructorConfiguration, builderFunc);

    private SourceBuilder Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
        => EnsurePreviousLineEmpty(staticConstructorConfiguration, Options.EnsureEmptyLineBeforeConstructors).AppendAs(staticConstructorConfiguration.BodyType, staticConstructorConfiguration, builderFunc);

    private SourceBuilder Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc)
        => EnsurePreviousLineEmpty(finalizerConfiguration, Options.EnsureEmptyLineBeforeConstructors).AppendAs(finalizerConfiguration.BodyType, finalizerConfiguration, builderFunc);

    private SourceBuilder Append(IFieldConfiguration fieldConfiguration)
        => EnsurePreviousLineEmpty(fieldConfiguration, Options.EnsureEmptyLineBeforeFields).AppendWithLineTerminator(fieldConfiguration);

    private SourceBuilder Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder>? addMethodBuilderFunc, Action<ISourceBuilder>? removeMethodBuilderFunc)
    {
        EnsurePreviousLineEmpty(eventConfiguration, Options.EnsureEmptyLineBeforeProperties);

        if (addMethodBuilderFunc is null || removeMethodBuilderFunc is null)
            return AppendWithLineTerminator(eventConfiguration);

        eventConfiguration.WriteTo(this);
        Append(CodeConfiguration.Block(), _ =>
        {
            Append(eventConfiguration.AddMethod, addMethodBuilderFunc);
            Append(eventConfiguration.RemoveMethod, removeMethodBuilderFunc);
        });

        return this;
    }

    private SourceBuilder Append(IEventMethodConfiguration eventMethodConfiguration, Action<ISourceBuilder> builderFunc)
        => AppendAs(eventMethodConfiguration.BodyType, eventMethodConfiguration, builderFunc);

    private SourceBuilder Append(IReadWritePropertyConfiguration propertyConfiguration, Action<ISourceBuilder>? getBuilderFunc, Action<ISourceBuilder>? setBuilderFunc)
        => Append(propertyConfiguration, propertyConfiguration.GetMethod, propertyConfiguration.SetMethod, propertyConfiguration.Value, getBuilderFunc, setBuilderFunc);

    private SourceBuilder Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder>? getBuilderFunc)
        => Append(propertyConfiguration, propertyConfiguration.GetMethod, null, propertyConfiguration.Value, getBuilderFunc, null);

    private SourceBuilder Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder>? setBuilderFunc)
        => Append(propertyConfiguration, null, propertyConfiguration.SetMethod, null, null, setBuilderFunc);

    private SourceBuilder Append(IReadWriteIndexerConfiguration indexerConfiguration, Action<ISourceBuilder>? getBuilderFunc, Action<ISourceBuilder>? setBuilderFunc)
        => Append(indexerConfiguration, indexerConfiguration.GetMethod, indexerConfiguration.SetMethod, null, getBuilderFunc, setBuilderFunc);

    private SourceBuilder Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder>? getBuilderFunc)
        => Append(indexerConfiguration, indexerConfiguration.GetMethod, null, null, getBuilderFunc, null);

    private SourceBuilder Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder>? setBuilderFunc)
        => Append(indexerConfiguration, null, indexerConfiguration.SetMethod, null, null, setBuilderFunc);

    private SourceBuilder Append(
        IPropertyConfiguration propertyConfiguration,
        IPropertyMethodConfiguration? getMethod,
        IPropertyMethodConfiguration? setMethod,
        string? value,
        Action<ISourceBuilder>? getBuilderFunc,
        Action<ISourceBuilder>? setBuilderFunc)
    {
        EnsurePreviousLineEmpty(propertyConfiguration, Options.EnsureEmptyLineBeforeProperties);

        if (getMethod is not null && setMethod is null &&
            getMethod.BodyType is MethodBodyType.Expression or MethodBodyType.ExpressionNewLine &&
            getMethod.AccessModifier == AccessModifier.Default &&
            getMethod.Attributes.Count == 0 &&
            getBuilderFunc is not null)
        {
            return AppendAsExpression(propertyConfiguration, this, getBuilderFunc, getMethod.BodyType is MethodBodyType.ExpressionNewLine);
        }

        propertyConfiguration.WriteTo(this);
        var multiline =
            getMethod?.Attributes.Count > 0 ||
            setMethod?.Attributes.Count > 0 ||
            getBuilderFunc is not null ||
            setBuilderFunc is not null;

        var blockStyle = multiline
            ? CodeBlockStyle.Default & ~CodeBlockStyle.AppendLineAfterBlock
            : CodeBlockStyle.EnsureBracketSpacing;

        Append(CodeConfiguration.Block().WithStyle(blockStyle), _ =>
        {
            if (getMethod is not null)
                Append(getMethod, getBuilderFunc, multiline);

            if (getMethod is not null && setMethod is not null && !multiline)
                Append(' ');

            if (setMethod is not null)
                Append(setMethod, setBuilderFunc, multiline);
        });

        if (getBuilderFunc is null && setBuilderFunc is null && value is not null)
        {
            Append(" = ");
            Append(value);
            Append(';');
        }

        AppendLine();

        return this;
    }

    private SourceBuilder Append(IPropertyMethodConfiguration propertyMethodConfiguration, Action<ISourceBuilder>? builderFunc, bool multiline)
        => AppendAs(propertyMethodConfiguration.BodyType, propertyMethodConfiguration, builderFunc, multiline);

    private SourceBuilder Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder>? builderFunc)
        => EnsurePreviousLineEmpty(methodConfiguration, Options.EnsureEmptyLineBeforeMethods).AppendAs(methodConfiguration.BodyType, methodConfiguration, builderFunc);

    private SourceBuilder Append(IEnumValueConfiguration enumValueConfiguration)
    {
        EnsurePreviousLineEmpty(enumValueConfiguration, Options.EnsureEmptyLineBeforeEnumValues);
        enumValueConfiguration.WriteTo(this);
        return Append(',').AppendLine();
    }

    private SourceBuilder Append(ICodeAttributeConfiguration codeAttributeConfiguration, bool appendLine = true)
    {
        codeAttributeConfiguration.WriteTo(this);
        return appendLine ? AppendLine() : Append(' ');
    }

    private SourceBuilder AppendAs(MethodBodyType bodyType, ICodeConfiguration codeConfiguration, Action<ISourceBuilder> builderFunc, bool appendLineAfterTerminator = true)
    {
        if (builderFunc is null)
            return AppendWithLineTerminator(codeConfiguration, appendLineAfterTerminator);

        if (bodyType is MethodBodyType.Expression or MethodBodyType.ExpressionNewLine)
            return AppendAsExpression(codeConfiguration, this, builderFunc, bodyType is MethodBodyType.ExpressionNewLine);

        return AppendAsBlock(codeConfiguration, builderFunc);
    }
}