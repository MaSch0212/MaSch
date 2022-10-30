using MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;
using MaSch.CodeAnalysis.CSharp.SourceGeneration.Parts;

namespace MaSch.CodeAnalysis.CSharp.SourceGeneration;

public interface ISourceFileBuilder :
    ISourceBuilder<ISourceFileBuilder>,
    ISourceNamespaceImportBuilder<ISourceFileBuilder>,
    ISourceNamespaceDeclarationBuilder<ISourceFileBuilder>
{
    ISourceFileBuilder AppendFileNamespace(string @namespace);
    ISourceFileBuilder AppendAssemblyCodeAttribute<TParams>(string attributeTypeName, TParams @params, Action<ISourceCodeAttributeBuilder, TParams> attributeConfiguration);
}
