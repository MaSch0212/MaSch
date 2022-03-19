using MaSch.Core;

namespace MaSch.FileSystem.InMemory.Models;

internal abstract class ContainerNode : FileSystemNode
{
    protected ContainerNode(RootNode root, DirectoryNode? parent, string name)
        : base(root, parent, name, FileAttributes.Directory)
    {
        ChildNodes = new FileSystemNodeCollection(this);
    }

    public List<DirectoryNode> Directories { get; } = new();
    public List<FileNode> Files { get; } = new();
    public ICollection<FileSystemNode> ChildNodes { get; }

    public IEnumerable<DirectoryNode> GetAllSubDirectories() => GetAllSubDirectories(int.MaxValue);
    public IEnumerable<DirectoryNode> GetAllSubDirectories(int maxRecursionLevel) => GetAllSubDirectories(0, maxRecursionLevel);

    public IEnumerable<FileNode> GetAllFiles() => GetAllFiles(int.MaxValue);
    public IEnumerable<FileNode> GetAllFiles(int maxRecursionLevel) => GetAllFiles(0, maxRecursionLevel);

    public IEnumerable<FileSystemNode> GetAllFileSystemNodes() => GetAllFileSystemNodes(int.MaxValue);
    public IEnumerable<FileSystemNode> GetAllFileSystemNodes(int maxRecursionLevel) => GetAllFileSystemNodes(0, maxRecursionLevel);

    private IEnumerable<DirectoryNode> GetAllSubDirectories(int currentRecusrionLevel, int maxRecursionLevel)
    {
        if (currentRecusrionLevel < maxRecursionLevel)
            return Directories.Concat(Directories.SelectMany(x => x.GetAllSubDirectories(currentRecusrionLevel + 1, maxRecursionLevel)));
        else
            return Directories;
    }

    private IEnumerable<FileNode> GetAllFiles(int currentRecusrionLevel, int maxRecursionLevel)
    {
        if (currentRecusrionLevel < maxRecursionLevel)
            return Files.Concat(Directories.SelectMany(x => x.GetAllFiles(currentRecusrionLevel + 1, maxRecursionLevel)));
        else
            return Files;
    }

    private IEnumerable<FileSystemNode> GetAllFileSystemNodes(int currentRecusrionLevel, int maxRecursionLevel)
    {
        if (currentRecusrionLevel < maxRecursionLevel)
            return ChildNodes.Concat(GetAllFileSystemNodes(currentRecusrionLevel + 1, maxRecursionLevel));
        else
            return ChildNodes;
    }

    private sealed class FileSystemNodeCollection : ICollection<FileSystemNode>
    {
        private readonly ContainerNode _node;

        public FileSystemNodeCollection(ContainerNode node)
        {
            _node = node;
        }

        public int Count => _node.Directories.Count + _node.Files.Count;
        public bool IsReadOnly => false;

        public void Add(FileSystemNode item)
        {
            if (item is DirectoryNode directoryNode)
                _node.Directories.Add(directoryNode);
            else if (item is FileNode fileNode)
                _node.Files.Add(fileNode);
            else
                throw new ArgumentException($"The item needs to be either a DirectoryNode or FileNode.", nameof(item));
        }

        public void Clear()
        {
            _node.Directories.Clear();
            _node.Files.Clear();
        }

        public bool Contains(FileSystemNode item)
        {
            return
                (item is DirectoryNode directoryNode && _node.Directories.Contains(directoryNode)) ||
                (item is FileNode fileNode && _node.Files.Contains(fileNode));
        }

        public void CopyTo(FileSystemNode[] array, int arrayIndex)
        {
            Guard.NotSmallerThan(arrayIndex, 0);
            if (array.Length < arrayIndex + _node.Directories.Count + _node.Files.Count)
                throw new ArgumentOutOfRangeException(nameof(array));

            for (int i = 0; i < _node.Directories.Count; i++)
                array[arrayIndex + i] = _node.Directories[i];
            for (int i = 0; i < _node.Files.Count; i++)
                array[arrayIndex + i + _node.Directories.Count] = _node.Files[i];
        }

        public IEnumerator<FileSystemNode> GetEnumerator()
        {
            foreach (var item in _node.Directories)
                yield return item;
            foreach (var item in _node.Files)
                yield return item;
        }

        public bool Remove(FileSystemNode item)
        {
            if (item is DirectoryNode directoryNode)
                return _node.Directories.Remove(directoryNode);
            else if (item is FileNode fileNode)
                return _node.Files.Remove(fileNode);
            else
                throw new ArgumentException($"The item needs to be either a DirectoryNode or FileNode.", nameof(item));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}