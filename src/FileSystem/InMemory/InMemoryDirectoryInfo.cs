using MaSch.Core;
using MaSch.FileSystem.InMemory.Models;

namespace MaSch.FileSystem.InMemory;

internal class InMemoryDirectoryInfo : DirectoryInfoBase
{
    private readonly ContainerNode? _node;

    public InMemoryDirectoryInfo(InMemoryFileSystemService fileSystem, string path)
        : base(fileSystem, path)
    {
        if (fileSystem.TryGetNode(path, out var node) && node is ContainerNode dirNode)
            _node = dirNode;
    }

    public InMemoryDirectoryInfo(InMemoryFileSystemService fileSystem, string path, ContainerNode node)
        : base(fileSystem, path)
    {
        _node = Guard.NotNull(node);
    }

    public override FileAttributes Attributes
    {
        get => _node?.Attributes ?? (FileAttributes)(-1);
        set
        {
            if (_node is null)
                throw new FileNotFoundException($"Could not find file '{FullName}'.");
            _node.Attributes = value;
        }
    }

    public override void Refresh()
    {
        // Nothing to do here
    }
}
