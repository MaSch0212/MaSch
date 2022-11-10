using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Builders;

public interface ISourceFileBuilder :
    ISourceBuilder<ISourceFileBuilder>,
    INamespaceImportBuilder<ISourceFileBuilder>,
    INamespaceDeclarationBuilder<ISourceFileBuilder, IFileMemberFactory>
{
    ISourceFileBuilder AppendFileNamespace(string @namespace);
    ISourceFileBuilder AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ICodeAttributeConfiguration, TParams> attributeConfiguration);
}