using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents a <see cref="ISourceBuilder"/> used to build the content of an interface.
/// </summary>
public interface IInterfaceBuilder :
    IDelegateDeclarationBuilder<IInterfaceBuilder>,
    IPropertyDeclarationBuilder<IInterfaceBuilder>,
    IMethodDeclarationBuilder<IInterfaceBuilder>,
    IEventDeclarationBuilder<IInterfaceBuilder>,
    IIndexerDeclarationBuilder<IInterfaceBuilder>,
    IEnumDeclarationBuilder<IInterfaceBuilder>,
    IInterfaceDeclarationBuilder<IInterfaceBuilder>,
    IClassDeclarationBuilder<IInterfaceBuilder>,
    IStructDeclarationBuilder<IInterfaceBuilder>,
    IRecordDeclarationBuilder<IInterfaceBuilder>
{
}

partial class SourceBuilder : IInterfaceBuilder
{
    /// <inheritdoc/>
    IInterfaceBuilder IDelegateDeclarationBuilder<IInterfaceBuilder>.Append(IDelegateConfiguration delegateConfiguration)
        => Append(delegateConfiguration);

    /// <inheritdoc/>
    IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder>.Append(IPropertyConfiguration propertyConfiguration)
        => Append(propertyConfiguration, null, null);

    /// <inheritdoc/>
    IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder>.Append(IPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
        => Append(propertyConfiguration, getBuilderFunc, setBuilderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration)
        => Append(propertyConfiguration, null, null);

    /// <inheritdoc/>
    IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc)
        => Append(propertyConfiguration, getBuilderFunc, null);

    /// <inheritdoc/>
    IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder>.Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc)
        => Append(propertyConfiguration, null, setBuilderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IMethodDeclarationBuilder<IInterfaceBuilder>.Append(IMethodConfiguration methodConfiguration)
        => Append(methodConfiguration, null);

    /// <inheritdoc/>
    IInterfaceBuilder IMethodDeclarationBuilder<IInterfaceBuilder>.Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(methodConfiguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IEventDeclarationBuilder<IInterfaceBuilder>.Append(IEventConfiguration eventConfiguration)
        => Append(eventConfiguration, null, null);

    /// <inheritdoc/>
    IInterfaceBuilder IEventDeclarationBuilder<IInterfaceBuilder>.Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
        => Append(eventConfiguration, addBuilderFunc, removeBuilderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder>.Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
        => Append(indexerConfiguration, getBuilderFunc, setBuilderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder>.Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc)
        => Append(indexerConfiguration, getBuilderFunc, null);

    /// <inheritdoc/>
    IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder>.Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc)
        => Append(indexerConfiguration, null, setBuilderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IEnumDeclarationBuilder<IInterfaceBuilder>.Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
        => Append(enumConfiguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IInterfaceDeclarationBuilder<IInterfaceBuilder>.Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
        => Append(interfaceConfguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IClassDeclarationBuilder<IInterfaceBuilder>.Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc)
        => Append(classConfiguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IStructDeclarationBuilder<IInterfaceBuilder>.Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
        => Append(structConfiguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder IRecordDeclarationBuilder<IInterfaceBuilder>.Append(IRecordConfiguration recordConfiguration)
        => Append(recordConfiguration, null);

    /// <inheritdoc/>
    IInterfaceBuilder IRecordDeclarationBuilder<IInterfaceBuilder>.Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc)
        => Append(recordConfiguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.Append(string value)
        => Append(value);

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.Append(char value)
        => Append(value);

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.Append(IRegionConfiguration regionConfiguration, Action<IInterfaceBuilder> builderFunc)
        => Append(regionConfiguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.Append(ICodeBlockConfiguration codeBlockConfiguration, Action<IInterfaceBuilder> builderFunc)
        => Append(codeBlockConfiguration, builderFunc);

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.AppendLine()
        => AppendLine();

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.AppendLine(string value)
        => AppendLine(value);

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.EnsureCurrentLineEmpty()
        => EnsureCurrentLineEmpty();

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.EnsurePreviousLineEmpty()
        => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.Indent(Action<IInterfaceBuilder> builderFunc)
        => Indent(builderFunc);
}