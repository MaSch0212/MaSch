using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface INamespaceBuilder :
        INamespaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
        IDelegateDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
        IEnumDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
        IInterfaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
        IClassDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
        IStructDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
        IRecordDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>,
        ISourceBuilder<INamespaceBuilder>
    {
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : INamespaceBuilder
    {
        /// <inheritdoc/>
        INamespaceBuilder INamespaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        INamespaceBuilder IDelegateDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IDelegateConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        INamespaceBuilder IEnumDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IEnumConfiguration> createFunc, Action<IEnumBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        INamespaceBuilder IInterfaceDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IInterfaceConfguration> createFunc, Action<IInterfaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        INamespaceBuilder IClassDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IClassConfiguration> createFunc, Action<IClassBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        INamespaceBuilder IStructDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IStructConfiguration> createFunc, Action<IStructBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        INamespaceBuilder IRecordDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IRecordConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        INamespaceBuilder IRecordDeclarationBuilder<INamespaceBuilder, INamespaceMemberFactory>.Append(Func<INamespaceMemberFactory, IRecordConfiguration> createFunc, Action<IRecordBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        INamespaceBuilder ISourceBuilder<INamespaceBuilder>.Append(string value)
            => Append(value);

        /// <inheritdoc/>
        INamespaceBuilder ISourceBuilder<INamespaceBuilder>.Append(char value)
            => Append(value);

        /// <inheritdoc/>
        INamespaceBuilder ISourceBuilder<INamespaceBuilder>.AppendLine()
            => AppendLine();

        /// <inheritdoc/>
        INamespaceBuilder ISourceBuilder<INamespaceBuilder>.AppendLine(string value)
            => AppendLine(value);

        /// <inheritdoc/>
        INamespaceBuilder ISourceBuilder<INamespaceBuilder>.EnsurePreviousLineEmpty()
            => EnsurePreviousLineEmpty();
    }
}