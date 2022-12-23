using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

/// <summary>
/// Represents a <see cref="ISourceBuilder"/> used to build the content of a record.
/// </summary>
public interface IRecordBuilder :
    IFieldDeclarationBuilder<IRecordBuilder>,
    IDelegateDeclarationBuilder<IRecordBuilder>,
    IPropertyDeclarationBuilder<IRecordBuilder>,
    IMethodDeclarationBuilder<IRecordBuilder>,
    IEventDeclarationBuilder<IRecordBuilder>,
    IIndexerDeclarationBuilder<IRecordBuilder>,
    IConstructorDeclarationBuilder<IRecordBuilder>,
    IFinalizerDeclarationBuilder<IRecordBuilder>,
    IEnumDeclarationBuilder<IRecordBuilder>,
    IInterfaceDeclarationBuilder<IRecordBuilder>,
    IClassDeclarationBuilder<IRecordBuilder>,
    IStructDeclarationBuilder<IRecordBuilder>,
    IRecordDeclarationBuilder<IRecordBuilder>
{
}

partial class SourceBuilder : IRecordBuilder
{
    /// <inheritdoc/>
    IRecordBuilder IFieldDeclarationBuilder<IRecordBuilder>.Append(IFieldConfiguration fieldConfiguration)
        => Append(fieldConfiguration);

    /// <inheritdoc/>
    IRecordBuilder IDelegateDeclarationBuilder<IRecordBuilder>.Append(IDelegateConfiguration delegateConfiguration)
        => Append(delegateConfiguration);

    /// <inheritdoc/>
    IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder>.Append(IReadWritePropertyConfiguration propertyConfiguration)
        => Append(propertyConfiguration, null, null);

    /// <inheritdoc/>
    IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder>.Append(IReadWritePropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
        => Append(propertyConfiguration, getBuilderFunc, setBuilderFunc);

    /// <inheritdoc/>
    IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration)
        => Append(propertyConfiguration, null);

    /// <inheritdoc/>
    IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc)
        => Append(propertyConfiguration, getBuilderFunc);

    /// <inheritdoc/>
    IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder>.Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc)
        => Append(propertyConfiguration, setBuilderFunc);

    /// <inheritdoc/>
    IRecordBuilder IMethodDeclarationBuilder<IRecordBuilder>.Append(IMethodConfiguration methodConfiguration)
        => Append(methodConfiguration, null);

    /// <inheritdoc/>
    IRecordBuilder IMethodDeclarationBuilder<IRecordBuilder>.Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(methodConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IEventDeclarationBuilder<IRecordBuilder>.Append(IEventConfiguration eventConfiguration)
        => Append(eventConfiguration, null, null);

    /// <inheritdoc/>
    IRecordBuilder IEventDeclarationBuilder<IRecordBuilder>.Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
        => Append(eventConfiguration, addBuilderFunc, removeBuilderFunc);

    /// <inheritdoc/>
    IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder>.Append(IReadWriteIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
        => Append(indexerConfiguration, getBuilderFunc, setBuilderFunc);

    /// <inheritdoc/>
    IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder>.Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc)
        => Append(indexerConfiguration, getBuilderFunc);

    /// <inheritdoc/>
    IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder>.Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc)
        => Append(indexerConfiguration, setBuilderFunc);

    /// <inheritdoc/>
    IRecordBuilder IConstructorDeclarationBuilder<IRecordBuilder>.Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(constructorConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IConstructorDeclarationBuilder<IRecordBuilder>.Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(staticConstructorConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IFinalizerDeclarationBuilder<IRecordBuilder>.Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(finalizerConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IEnumDeclarationBuilder<IRecordBuilder>.Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
        => Append(enumConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IInterfaceDeclarationBuilder<IRecordBuilder>.Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
        => Append(interfaceConfguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IClassDeclarationBuilder<IRecordBuilder>.Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc)
        => Append(classConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IStructDeclarationBuilder<IRecordBuilder>.Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
        => Append(structConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder IRecordDeclarationBuilder<IRecordBuilder>.Append(IRecordConfiguration recordConfiguration)
        => Append(recordConfiguration, null);

    /// <inheritdoc/>
    IRecordBuilder IRecordDeclarationBuilder<IRecordBuilder>.Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc)
        => Append(recordConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.Append(string value)
        => Append(value);

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.Append(char value)
        => Append(value);

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.Append(IRegionConfiguration regionConfiguration, Action<IRecordBuilder> builderFunc)
        => Append(regionConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.Append(ICodeBlockConfiguration codeBlockConfiguration, Action<IRecordBuilder> builderFunc)
        => Append(codeBlockConfiguration, builderFunc);

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.AppendLine()
        => AppendLine();

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.AppendLine(string value)
        => AppendLine(value);

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.EnsureCurrentLineEmpty()
        => EnsureCurrentLineEmpty();

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.EnsurePreviousLineEmpty()
        => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    IRecordBuilder ISourceBuilder<IRecordBuilder>.Indent(Action<IRecordBuilder> builderFunc)
        => Indent(builderFunc);
}