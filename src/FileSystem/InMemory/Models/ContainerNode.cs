namespace MaSch.FileSystem.InMemory.Models;

internal abstract class ContainerNode : FileSystemNode
{
    protected ContainerNode(RootNode root, DirectoryNode? parent, string name)
        : base(root, parent, name, FileAttributes.Directory)
    {
    }

    public List<DirectoryNode> Directories { get; } = new();
    public List<FileNode> Files { get; } = new();
}