namespace MaSch.FileSystem.InMemory.Models;

internal class DirectoryNode : ContainerNode
{
    public DirectoryNode(RootNode root, DirectoryNode? parent, string name)
        : base(root, parent, name)
    {
    }
}
