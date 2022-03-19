using MaSch.Core;
using MaSch.FileSystem.InMemory.Models;

namespace MaSch.FileSystem.InMemory;

internal class InMemoryFileInfo : FileInfoBase
{
    private readonly InMemoryFileSystemService _fileSystem;
    private FileNode? _node;

    public InMemoryFileInfo(InMemoryFileSystemService fileSystem, string path)
        : base(fileSystem, path)
    {
        _fileSystem = fileSystem;
        Refresh();
    }

    public InMemoryFileInfo(InMemoryFileSystemService fileSystem, string path, FileNode node)
        : base(fileSystem, path)
    {
        _fileSystem = fileSystem;
        _node = Guard.NotNull(node);
    }

    public override long Length => EnsureNode().Content.Length;
    public override bool IsReadOnly => EnsureNode().Attributes.HasFlag(FileAttributes.ReadOnly);

    public override void Refresh()
    {
        if (_fileSystem.TryGetNode<FileNode>(FullName, out var node))
            _node = node;
    }

    private FileNode EnsureNode()
    {
        if (_node == null)
        {
            if (_fileSystem.TryGetNode<FileNode>(FullName, out var node))
                _node = node;
            else
                throw new FileNotFoundException($"Could not find a part of the path '{FullName}'.");
        }

        return _node;
    }
}
