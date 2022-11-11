using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IClassBuilder :
        IFieldDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IDelegateDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IPropertyDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IMethodDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IEventDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IIndexerDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IConstructorDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IFinalizerDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IEnumDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IInterfaceDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IClassDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IStructDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        IRecordDeclarationBuilder<IClassBuilder, IClassMemberFactory>,
        ISourceBuilder<IClassBuilder>
    {
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IClassBuilder
    {
        /// <inheritdoc/>
        IClassBuilder IFieldDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IFieldConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        IClassBuilder IDelegateDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IDelegateConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        IClassBuilder IPropertyDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IClassBuilder IPropertyDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IClassBuilder IPropertyDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IReadOnlyPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IClassBuilder IPropertyDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IClassBuilder IPropertyDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IClassBuilder IMethodDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IMethodConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IClassBuilder IMethodDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IEventDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IEventConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IClassBuilder IEventDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(createFunc(_configurationFactory), addBuilderFunc, removeBuilderFunc);

        /// <inheritdoc/>
        IClassBuilder IIndexerDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IClassBuilder IIndexerDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IClassBuilder IIndexerDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IReadOnlyIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IClassBuilder IIndexerDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IClassBuilder IIndexerDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IClassBuilder IConstructorDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IConstructorDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IFinalizerDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IFinalizerConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IEnumDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IInterfaceDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IClassDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IStructDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder IRecordDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IRecordConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IClassBuilder IRecordDeclarationBuilder<IClassBuilder, IClassMemberFactory>.Append(Func<IClassMemberFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IClassBuilder ISourceBuilder<IClassBuilder>.Append(string value)
            => Append(value);

        /// <inheritdoc/>
        IClassBuilder ISourceBuilder<IClassBuilder>.Append(char value)
            => Append(value);

        /// <inheritdoc/>
        IClassBuilder ISourceBuilder<IClassBuilder>.AppendLine()
            => AppendLine();

        /// <inheritdoc/>
        IClassBuilder ISourceBuilder<IClassBuilder>.AppendLine(string value)
            => AppendLine(value);

        /// <inheritdoc/>
        IClassBuilder ISourceBuilder<IClassBuilder>.EnsurePreviousLineEmpty()
            => EnsurePreviousLineEmpty();
    }
}