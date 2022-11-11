using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IRecordBuilder :
        IFieldDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IDelegateDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IPropertyDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IMethodDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IEventDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IIndexerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IConstructorDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IFinalizerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IEnumDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IInterfaceDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IClassDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IStructDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        IRecordDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>,
        ISourceBuilder<IRecordBuilder>
    {
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IRecordBuilder
    {
        /// <inheritdoc/>
        IRecordBuilder IFieldDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IFieldConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        IRecordBuilder IDelegateDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IDelegateConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IReadOnlyPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IRecordBuilder IPropertyDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IRecordBuilder IMethodDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IMethodConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IRecordBuilder IMethodDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IEventDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IEventConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IRecordBuilder IEventDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(createFunc(_configurationFactory), addBuilderFunc, removeBuilderFunc);

        /// <inheritdoc/>
        IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IReadOnlyIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IRecordBuilder IIndexerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IRecordBuilder IConstructorDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IConstructorDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IFinalizerDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IEnumDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IInterfaceDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IClassDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IStructDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder IRecordDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IRecordConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IRecordBuilder IRecordDeclarationBuilder<IRecordBuilder, IRecordMemberFactory>.Append(Func<IRecordMemberFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IRecordBuilder ISourceBuilder<IRecordBuilder>.Append(string value)
            => Append(value);

        /// <inheritdoc/>
        IRecordBuilder ISourceBuilder<IRecordBuilder>.Append(char value)
            => Append(value);

        /// <inheritdoc/>
        IRecordBuilder ISourceBuilder<IRecordBuilder>.AppendLine()
            => AppendLine();

        /// <inheritdoc/>
        IRecordBuilder ISourceBuilder<IRecordBuilder>.AppendLine(string value)
            => AppendLine(value);

        /// <inheritdoc/>
        IRecordBuilder ISourceBuilder<IRecordBuilder>.EnsurePreviousLineEmpty()
            => EnsurePreviousLineEmpty();
    }
}