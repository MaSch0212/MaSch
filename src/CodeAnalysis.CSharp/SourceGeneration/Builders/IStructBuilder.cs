using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IStructBuilder :
        IFieldDeclarationBuilder<IStructBuilder>,
        IDelegateDeclarationBuilder<IStructBuilder>,
        IPropertyDeclarationBuilder<IStructBuilder>,
        IMethodDeclarationBuilder<IStructBuilder>,
        IEventDeclarationBuilder<IStructBuilder>,
        IIndexerDeclarationBuilder<IStructBuilder>,
        IConstructorDeclarationBuilder<IStructBuilder>,
        IEnumDeclarationBuilder<IStructBuilder>,
        IInterfaceDeclarationBuilder<IStructBuilder>,
        IClassDeclarationBuilder<IStructBuilder>,
        IStructDeclarationBuilder<IStructBuilder>,
        IRecordDeclarationBuilder<IStructBuilder>,
        ISourceBuilder<IStructBuilder>
    {
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IStructBuilder
    {
        /// <inheritdoc/>
        IStructBuilder IFieldDeclarationBuilder<IStructBuilder>.Append(IFieldConfiguration fieldConfiguration)
            => Append(fieldConfiguration);

        /// <inheritdoc/>
        IStructBuilder IDelegateDeclarationBuilder<IStructBuilder>.Append(IDelegateConfiguration delegateConfiguration)
            => Append(delegateConfiguration);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder>.Append(IPropertyConfiguration propertyConfiguration)
            => Append(propertyConfiguration, null, null);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder>.Append(IPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(propertyConfiguration, getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration)
            => Append(propertyConfiguration, null, null);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder>.Append(IReadOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> getBuilderFunc)
            => Append(propertyConfiguration, getBuilderFunc, null);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder>.Append(IWriteOnlyPropertyConfiguration propertyConfiguration, Action<ISourceBuilder> setBuilderFunc)
            => Append(propertyConfiguration, null, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IMethodDeclarationBuilder<IStructBuilder>.Append(IMethodConfiguration methodConfiguration)
            => Append(methodConfiguration, null);

        /// <inheritdoc/>
        IStructBuilder IMethodDeclarationBuilder<IStructBuilder>.Append(IMethodConfiguration methodConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(methodConfiguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder IEventDeclarationBuilder<IStructBuilder>.Append(IEventConfiguration eventConfiguration)
            => Append(eventConfiguration, null, null);

        /// <inheritdoc/>
        IStructBuilder IEventDeclarationBuilder<IStructBuilder>.Append(IEventConfiguration eventConfiguration, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(eventConfiguration, addBuilderFunc, removeBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder>.Append(IIndexerConfiguration indexerConfiguration)
            => Append(indexerConfiguration, null, null);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder>.Append(IIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(indexerConfiguration, getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder>.Append(IReadOnlyIndexerConfiguration indexerConfiguration)
            => Append(indexerConfiguration, null, null);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder>.Append(IReadOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> getBuilderFunc)
            => Append(indexerConfiguration, getBuilderFunc, null);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder>.Append(IWriteOnlyIndexerConfiguration indexerConfiguration, Action<ISourceBuilder> setBuilderFunc)
            => Append(indexerConfiguration, null, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IConstructorDeclarationBuilder<IStructBuilder>.Append(IConstructorConfiguration constructorConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(constructorConfiguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder IConstructorDeclarationBuilder<IStructBuilder>.Append(IStaticConstructorConfiguration staticConstructorConfiguration, Action<ISourceBuilder> builderFunc)
            => Append(staticConstructorConfiguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder IEnumDeclarationBuilder<IStructBuilder>.Append(IEnumConfiguration enumConfiguration, Action<IEnumBuilder> builderFunc)
            => Append(enumConfiguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder IInterfaceDeclarationBuilder<IStructBuilder>.Append(IInterfaceConfguration interfaceConfguration, Action<IInterfaceBuilder> builderFunc)
            => Append(interfaceConfguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder IClassDeclarationBuilder<IStructBuilder>.Append(IClassConfiguration classConfiguration, Action<IClassBuilder> builderFunc)
            => Append(classConfiguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder IStructDeclarationBuilder<IStructBuilder>.Append(IStructConfiguration structConfiguration, Action<IStructBuilder> builderFunc)
            => Append(structConfiguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder IRecordDeclarationBuilder<IStructBuilder>.Append(IRecordConfiguration recordConfiguration)
            => Append(recordConfiguration, null);

        /// <inheritdoc/>
        IStructBuilder IRecordDeclarationBuilder<IStructBuilder>.Append(IRecordConfiguration recordConfiguration, Action<IRecordBuilder> builderFunc)
            => Append(recordConfiguration, builderFunc);

        /// <inheritdoc/>
        IStructBuilder ISourceBuilder<IStructBuilder>.Append(string value)
            => Append(value);

        /// <inheritdoc/>
        IStructBuilder ISourceBuilder<IStructBuilder>.Append(char value)
            => Append(value);

        /// <inheritdoc/>
        IStructBuilder ISourceBuilder<IStructBuilder>.AppendLine()
            => AppendLine();

        /// <inheritdoc/>
        IStructBuilder ISourceBuilder<IStructBuilder>.AppendLine(string value)
            => AppendLine(value);

        /// <inheritdoc/>
        IStructBuilder ISourceBuilder<IStructBuilder>.EnsurePreviousLineEmpty()
            => EnsurePreviousLineEmpty();
    }
}