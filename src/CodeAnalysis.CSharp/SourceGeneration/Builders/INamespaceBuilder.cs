using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents a <see cref="ISourceBuilder"/> used to build the content of a namespace.
/// </summary>
public interface INamespaceBuilder :
    INamespaceDeclarationBuilder<INamespaceBuilder>,
    IDelegateDeclarationBuilder<INamespaceBuilder>,
    IEnumDeclarationBuilder<INamespaceBuilder>,
    IInterfaceDeclarationBuilder<INamespaceBuilder>,
    IClassDeclarationBuilder<INamespaceBuilder>,
    IStructDeclarationBuilder<INamespaceBuilder>,
    IRecordDeclarationBuilder<INamespaceBuilder>
{
}

partial class SourceBuilder : INamespaceBuilder
{
    /// <inheritdoc/>
    INamespaceBuilder INamespaceDeclarationBuilder<INamespaceBuilder>.Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc)
        => Append(namespaceConfiguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder IDelegateDeclarationBuilder<INamespaceBuilder>.Append(IDelegateConfiguration delegateConfiguration)
        => Append(delegateConfiguration);

    /// <inheritdoc/>
    INamespaceBuilder IEnumDeclarationBuilder<INamespaceBuilder>.Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
        => Append(enumConfiguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder IInterfaceDeclarationBuilder<INamespaceBuilder>.Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
        => Append(interfaceConfguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder IClassDeclarationBuilder<INamespaceBuilder>.Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc)
        => Append(classConfiguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder IStructDeclarationBuilder<INamespaceBuilder>.Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
        => Append(structConfiguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder IRecordDeclarationBuilder<INamespaceBuilder>.Append(IRecordConfiguration recordConfiguration)
        => Append(recordConfiguration, null);

    /// <inheritdoc/>
    INamespaceBuilder IRecordDeclarationBuilder<INamespaceBuilder>.Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc)
        => Append(recordConfiguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.Append(string value)
        => Append(value);

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.Append(char value)
        => Append(value);

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.Append(IRegionConfiguration regionConfiguration, Action<INamespaceBuilder> builderFunc)
        => Append(regionConfiguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.Append(ICodeBlockConfiguration codeBlockConfiguration, Action<INamespaceBuilder> builderFunc)
        => Append(codeBlockConfiguration, builderFunc);

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.AppendLine()
        => AppendLine();

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.AppendLine(string value)
        => AppendLine(value);

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.EnsureCurrentLineEmpty()
        => EnsureCurrentLineEmpty();

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.EnsurePreviousLineEmpty()
        => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    INamespaceBuilder ISourceBuilder<INamespaceBuilder>.Indent(Action<INamespaceBuilder> builderFunc)
        => Indent(builderFunc);
}