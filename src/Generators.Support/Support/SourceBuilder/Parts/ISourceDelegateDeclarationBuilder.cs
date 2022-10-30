namespace MaSch.Generators.Support
{
    public interface ISourceDelegateDeclarationBuilder<T> : ISourceBuilder
        where T : ISourceDelegateDeclarationBuilder<T>
    {

    }
}