using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface IInterfaceBuilder :
        IDelegateDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IPropertyDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IMethodDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IEventDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IIndexerDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IEnumDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IInterfaceDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IClassDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IStructDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        IRecordDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>,
        ISourceBuilder<IInterfaceBuilder>
    {
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : IInterfaceBuilder
    {
        /// <inheritdoc/>
        IInterfaceBuilder IDelegateDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IDelegateConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IReadOnlyPropertyConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IReadOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IInterfaceBuilder IPropertyDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IWriteOnlyPropertyConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IMethodDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IMethodConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IInterfaceBuilder IMethodDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IMethodConfiguration> createFunc, Action<ISourceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IEventDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IEventConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IInterfaceBuilder IEventDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IEventConfiguration> createFunc, Action<ISourceBuilder> addBuilderFunc, Action<ISourceBuilder> removeBuilderFunc)
            => Append(createFunc(_configurationFactory), addBuilderFunc, removeBuilderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, setBuilderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IReadOnlyIndexerConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null, null);

        /// <inheritdoc/>
        IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IReadOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> getBuilderFunc)
            => Append(createFunc(_configurationFactory), getBuilderFunc, null);

        /// <inheritdoc/>
        IInterfaceBuilder IIndexerDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IWriteOnlyIndexerConfiguration> createFunc, Action<ISourceBuilder> setBuilderFunc)
            => Append(createFunc(_configurationFactory), null, setBuilderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IEnumDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IInterfaceDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IClassDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IStructDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder IRecordDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IRecordConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        IInterfaceBuilder IRecordDeclarationBuilder<IInterfaceBuilder, IInterfaceMemberFactory>.Append(Func<IInterfaceMemberFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.Append(string value)
            => Append(value);

        /// <inheritdoc/>
        IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.Append(char value)
            => Append(value);

        /// <inheritdoc/>
        IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.AppendLine()
            => AppendLine();

        /// <inheritdoc/>
        IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.AppendLine(string value)
            => AppendLine(value);

        /// <inheritdoc/>
        IInterfaceBuilder ISourceBuilder<IInterfaceBuilder>.EnsurePreviousLineEmpty()
            => EnsurePreviousLineEmpty();
    }
}