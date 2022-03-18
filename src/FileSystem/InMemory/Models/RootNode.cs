namespace MaSch.FileSystem.InMemory.Models;

internal class RootNode : ContainerNode
{
    public RootNode(string name)
        : base(null!, null, name)
    {
        Root = this;
    }

    protected override RootNode ValidateRoot(RootNode root) => root;
}
