﻿using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents a <see cref="ISourceBuilder"/> used to build the content of a source file.
/// </summary>
public interface ISourceFileBuilder :
    INamespaceDeclarationBuilder<ISourceFileBuilder>,
    INamespaceImportBuilder<ISourceFileBuilder>
{
    /// <summary>
    /// Appends a file-scoped namespace to the current source file.
    /// </summary>
    /// <param name="namespaceConfiguration">The configuration of the file-scoped namespace.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceFileBuilder Append(INamespaceConfiguration namespaceConfiguration);

    /// <summary>
    /// Appends a code attribute to the current source file.
    /// </summary>
    /// <param name="codeAttributeConfiguration">The configuration of the code attribute.</param>
    /// <returns>A reference to this instance after the append operation has completed.</returns>
    ISourceFileBuilder Append(ICodeAttributeConfiguration codeAttributeConfiguration);
}

partial class SourceBuilder : ISourceFileBuilder
{
    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.Append(INamespaceConfiguration namespaceConfiguration)
        => Append(namespaceConfiguration, null);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceFileBuilder.Append(ICodeAttributeConfiguration codeAttributeConfiguration)
        => Append(codeAttributeConfiguration);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceDeclarationBuilder<ISourceFileBuilder>.Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc)
        => Append(namespaceConfiguration, builderFunc);

    /// <inheritdoc/>
    ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.Append(INamespaceImportConfiguration namespaceImportConfiguration)
        => Append(namespaceImportConfiguration);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(string value)
        => Append(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(char value)
        => Append(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(IRegionConfiguration regionConfiguration, Action<ISourceFileBuilder> builderFunc)
        => Append(regionConfiguration, builderFunc);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(ICodeBlockConfiguration codeBlockConfiguration, Action<ISourceFileBuilder> builderFunc)
        => Append(codeBlockConfiguration, builderFunc);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine()
        => AppendLine();

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine(string value)
        => AppendLine(value);

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.EnsureCurrentLineEmpty()
        => EnsureCurrentLineEmpty();

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.EnsurePreviousLineEmpty()
        => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Indent(Action<ISourceFileBuilder> builderFunc)
        => Indent(builderFunc);
}