using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface IClassBuilder :
    IFieldDeclarationBuilder<IClassBuilder>,
    IDelegateDeclarationBuilder<IClassBuilder>,
    IPropertyDeclarationBuilder<IClassBuilder>,
    IMethodDeclarationBuilder<IClassBuilder>,
    IEventDeclarationBuilder<IClassBuilder>,
    IIndexerDeclarationBuilder<IClassBuilder>,
    IConstructorDeclarationBuilder<IClassBuilder>,
    IFinalizerDeclarationBuilder<IClassBuilder>,
    IEnumDeclarationBuilder<IClassBuilder>,
    IInterfaceDeclarationBuilder<IClassBuilder>,
    IClassDeclarationBuilder<IClassBuilder>,
    IStructDeclarationBuilder<IClassBuilder>,
    IRecordDeclarationBuilder<IClassBuilder>
{
}

partial class SourceBuilder : IClassBuilder
{
    /// <inheritdoc/>
    IClassBuilder IFieldDeclarationBuilder<IClassBuilder>.Append(IFieldConfiguration fieldConfiguration)
        => Append(fieldConfiguration);

    /// <inheritdoc/>
    IClassBuilder IDelegateDeclarationBuilder<IClassBuilder>.Append(IDelegateConfiguration delegateConfiguration)
        => Append(delegateConfiguration);

    /// <inheritdoc/>
    IClassBuilder IPropertyDeclarationBuilder<IClassBuilder>.Append(IPropertyConfiguration propertyConfiguration)
        => Append(propertyConfiguration, null, null);

    /// <inheritdoc/>
    IClassBuilder IPropertyDeclarationBuilder<IClassBuilder>.Append(IPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
        => Append(propertyConfiguration, getBuilderFunc, setBuilderFunc);

    /// <inheritdoc/>
    IClassBuilder IPropertyDeclarationBuilder<IClassBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration)
        => Append(propertyConfiguration, null, null);

    /// <inheritdoc/>
    IClassBuilder IPropertyDeclarationBuilder<IClassBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc)
        => Append(propertyConfiguration, getBuilderFunc, null);

    /// <inheritdoc/>
    IClassBuilder IPropertyDeclarationBuilder<IClassBuilder>.Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc)
        => Append(propertyConfiguration, null, setBuilderFunc);

    /// <inheritdoc/>
    IClassBuilder IMethodDeclarationBuilder<IClassBuilder>.Append(IMethodConfiguration methodConfiguration)
        => Append(methodConfiguration, null);

    /// <inheritdoc/>
    IClassBuilder IMethodDeclarationBuilder<IClassBuilder>.Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(methodConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IEventDeclarationBuilder<IClassBuilder>.Append(IEventConfiguration eventConfiguration)
        => Append(eventConfiguration, null, null);

    /// <inheritdoc/>
    IClassBuilder IEventDeclarationBuilder<IClassBuilder>.Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
        => Append(eventConfiguration, addBuilderFunc, removeBuilderFunc);

    /// <inheritdoc/>
    IClassBuilder IIndexerDeclarationBuilder<IClassBuilder>.Append(IIndexerConfiguration indexerConfiguration)
        => Append(indexerConfiguration, null, null);

    /// <inheritdoc/>
    IClassBuilder IIndexerDeclarationBuilder<IClassBuilder>.Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
        => Append(indexerConfiguration, getBuilderFunc, setBuilderFunc);

    /// <inheritdoc/>
    IClassBuilder IIndexerDeclarationBuilder<IClassBuilder>.Append(IReadOnlyIndexerConfiguration indexerConfiguration)
        => Append(indexerConfiguration, null, null);

    /// <inheritdoc/>
    IClassBuilder IIndexerDeclarationBuilder<IClassBuilder>.Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc)
        => Append(indexerConfiguration, getBuilderFunc, null);

    /// <inheritdoc/>
    IClassBuilder IIndexerDeclarationBuilder<IClassBuilder>.Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc)
        => Append(indexerConfiguration, null, setBuilderFunc);

    /// <inheritdoc/>
    IClassBuilder IConstructorDeclarationBuilder<IClassBuilder>.Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(constructorConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IConstructorDeclarationBuilder<IClassBuilder>.Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(staticConstructorConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IFinalizerDeclarationBuilder<IClassBuilder>.Append(IFinalizerConfiguration finalizerConfiguration, Action<ISourceBuilder> builderFunc)
        => Append(finalizerConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IEnumDeclarationBuilder<IClassBuilder>.Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
        => Append(enumConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IInterfaceDeclarationBuilder<IClassBuilder>.Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
        => Append(interfaceConfguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IClassDeclarationBuilder<IClassBuilder>.Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc)
        => Append(classConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IStructDeclarationBuilder<IClassBuilder>.Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
        => Append(structConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder IRecordDeclarationBuilder<IClassBuilder>.Append(IRecordConfiguration recordConfiguration)
        => Append(recordConfiguration, null);

    /// <inheritdoc/>
    IClassBuilder IRecordDeclarationBuilder<IClassBuilder>.Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc)
        => Append(recordConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.Append(string value)
        => Append(value);

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.Append(char value)
        => Append(value);

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.Append(IRegionConfiguration regionConfiguration, Action<IClassBuilder> builderFunc)
        => Append(regionConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.Append(ICodeBlockConfiguration codeBlockConfiguration, Action<IClassBuilder> builderFunc)
        => Append(codeBlockConfiguration, builderFunc);

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.AppendLine()
        => AppendLine();

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.AppendLine(string value)
        => AppendLine(value);

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.EnsureCurrentLineEmpty()
        => EnsureCurrentLineEmpty();

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.EnsurePreviousLineEmpty()
        => EnsurePreviousLineEmpty();

    /// <inheritdoc/>
    IClassBuilder ISourceBuilder<IClassBuilder>.Indent(Action<IClassBuilder> builderFunc)
        => Indent(builderFunc);
}