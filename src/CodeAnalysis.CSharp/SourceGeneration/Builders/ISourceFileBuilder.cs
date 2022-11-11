using MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.ConfigurationFactories;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders
{
    public interface ISourceFileBuilder :
        INamespaceDeclarationBuilder<ISourceFileBuilder, IFileMemberFactory>,
        INamespaceImportBuilder<ISourceFileBuilder, IFileMemberFactory>,
        ISourceBuilder<ISourceFileBuilder>
    {
        ISourceFileBuilder Append(Func<IFileMemberFactory, INamespaceConfiguration> createFunc);
        ISourceFileBuilder Append(Func<IFileMemberFactory, ICodeAttributeConfiguration> createFunc);
    }
}

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration
{
    partial class SourceBuilder : ISourceFileBuilder
    {
        /// <inheritdoc/>
        ISourceFileBuilder ISourceFileBuilder.Append(Func<IFileMemberFactory, INamespaceConfiguration> createFunc)
            => Append(createFunc(_configurationFactory), null);

        /// <inheritdoc/>
        ISourceFileBuilder ISourceFileBuilder.Append(Func<IFileMemberFactory, ICodeAttributeConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

        /// <inheritdoc/>
        ISourceFileBuilder INamespaceDeclarationBuilder<ISourceFileBuilder, IFileMemberFactory>.Append(Func<IFileMemberFactory, INamespaceConfiguration> createFunc, Action<INamespaceBuilder> builderFunc)
            => Append(createFunc(_configurationFactory), builderFunc);

        /// <inheritdoc/>
        ISourceFileBuilder INamespaceImportBuilder<ISourceFileBuilder, IFileMemberFactory>.Append(Func<IFileMemberFactory, INamespaceImportConfiguration> createFunc)
            => Append(createFunc(_configurationFactory));

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