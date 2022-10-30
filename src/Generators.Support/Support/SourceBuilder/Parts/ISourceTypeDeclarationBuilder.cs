namespace MaSch.Generators.Support
{
    public interface ISourceTypeDeclarationBuilder<T> : ISourceBuilder
        where T : ISourceTypeDeclarationBuilder<T>
    {

    }
}