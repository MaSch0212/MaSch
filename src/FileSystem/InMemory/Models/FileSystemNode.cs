using MaSch.Core;

namespace MaSch.FileSystem.InMemory.Models;

internal abstract class FileSystemNode
{
    protected FileSystemNode(RootNode root, DirectoryNode? parent, string name, FileAttributes attributes)
    {
        Name = Guard.NotNullOrEmpty(name);
        Root = ValidateRoot(root);
        ParentDirectory = parent;
        CreationTimeUtc = LastWriteTimeUtc = LastAccessTimeUtc = DateTime.UtcNow;
        Attributes = attributes;
    }

    public virtual string Path => System.IO.Path.Combine(Parent.Name, Name);
    public string Name { get; set; }
    public DateTime CreationTimeUtc { get; set; }
    public DateTime LastWriteTimeUtc { get; set; }
    public DateTime LastAccessTimeUtc { get; set; }
    public FileAttributes Attributes { get; set; }

    public RootNode Root { get; set; }
    public DirectoryNode? ParentDirectory { get; set; }
    public ContainerNode Parent => (ContainerNode?)ParentDirectory ?? Root;

    protected virtual RootNode ValidateRoot(RootNode root) => Guard.NotNull(root);
}
