using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IStructBuilder :
        IFieldDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IDelegateDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IPropertyDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IMethodDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IEventDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IIndexerDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IConstructorDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IEnumDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IInterfaceDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IClassDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IStructDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        IRecordDeclarationBuilder<IStructBuilder, IStructMemberFactory>,
        ISourceBuilder<IStructBuilder>
    {
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    public partial class SourceBuilder : IStructBuilder
    {
        /// <inheritdoc/>
        IStructBuilder IFieldDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IFieldConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        IStructBuilder IDelegateDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IDelegateConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IReadOnlyPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IStructBuilder IPropertyDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IMethodDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IMethodConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IStructBuilder IMethodDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IStructBuilder IEventDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IEventConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IStructBuilder IEventDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(createFunc(_configurationFactory), addBuilderFunc, removeBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IReadOnlyIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IStructBuilder IIndexerDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IStructBuilder IConstructorDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IStructBuilder IConstructorDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IStaticConstructorConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IStructBuilder IEnumDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IStructBuilder IInterfaceDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IStructBuilder IClassDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IStructBuilder IStructDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IStructBuilder IRecordDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IRecordConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IStructBuilder IRecordDeclarationBuilder<IStructBuilder, IStructMemberFactory>.Append(Func<IStructMemberFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

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