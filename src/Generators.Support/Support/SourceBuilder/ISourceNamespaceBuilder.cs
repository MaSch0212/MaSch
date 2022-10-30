namespace MaSch.Generators.Support
{
    public interface ISourceNamespaceBuilder :
        ISourceNamespaceDeclarationBuilder<ISourceNamespaceBuilder>,
        ISourceNamespaceImportBuilder<ISourceNamespaceBuilder>
    {
    }
}