using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface ISourceFileBuilder :
        INamespaceDeclarationBuilder<ISourceFileBuilder>,
        INamespaceImportBuilder<ISourceFileBuilder>,
        ISourceBuilder<ISourceFileBuilder>
    {
        ISourceFileBuilder Append(INamespaceConfiguration namespaceConfiguration);
        ISourceFileBuilder Append(ICodeAttributeConfiguration codeAttributeConfiguration);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : ISourceFileBuilder
    {
        /// <inheritdoc/>
        ISourceFileBuilder ISourceFileBuilder.Append(INamespaceConfiguration namespaceConfiguration)
            => Append(namespaceConfiguration, null);

        /// <inheritdoc/>
        ISourceFileBuilder ISourceFileBuilder.Append(ICodeAttributeConfiguration codeAttributeConfiguration)
            => Append(codeAttributeConfiguration);

        /// <inheritdoc/>
        ISourceFileBuilder INamespaceDeclarationBuilder<ISourceFileBuilder>.Append(INamespaceConfiguration namespaceConfiguration, Action<INamespaceBuilder> builderFunc)
            => Append(namespaceConfiguration, builderFunc);

        /// <inheritdoc/>
        ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder>.Append(INamespaceImportConfiguration namespaceImportConfiguration)
            => Append(namespaceImportConfiguration);

        /// <inheritdoc/>
        ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(string value)
            => Append(value);

        /// <inheritdoc/>
        ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.Append(char value)
            => Append(value);

        /// <inheritdoc/>
        ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine()
            => AppendLine();

        /// <inheritdoc/>
        ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.AppendLine(string value)
            => AppendLine(value);

        /// <inheritdoc/>
        ISourceFileBuilder ISourceBuilder<ISourceFileBuilder>.EnsurePreviousLineEmpty()
            => EnsurePreviousLineEmpty();
    }
}