namespace MaSch.FileSystem.InMemory.Models;

internal class FileNode : FileSystemNode
{
    public FileNode(RootNode root, DirectoryNode? parent, string name)
        : base(root, parent, name, FileAttributes.Normal)
    {
        Content = Array.Empty<byte>();
    }

    public byte[] Content { get; set; }

    public FileNode Clone()
    {
        return new FileNode(Root, ParentDirectory, Name)
        {
            Content = Content.ToArray(),
            LastWriteTimeUtc = LastWriteTimeUtc,
            Attributes = Attributes,
        };
    }
}
