namespace MaSch.FileSystem.InMemory.Models;

internal class FileNode : FileSystemNode
{
    private readonly object _lock = new();
    private readonly List<Usage> _usages = new();

    public FileNode(RootNode root, DirectoryNode? parent, string name)
        : base(root, parent, name, FileAttributes.Normal)
    {
        Content = Array.Empty<byte>();
    }

    public byte[] Content { get; set; }

    public bool CanDelete
    {
        get
        {
            lock (_lock)
                return _usages.Count == 0 || _usages.Any(x => x.Share.HasFlag(FileShare.Delete));
        }
    }

    public FileNode Clone()
    {
        return new FileNode(Root, ParentDirectory, Name)
        {
            Content = Content.ToArray(),
            LastWriteTimeUtc = LastWriteTimeUtc,
            Attributes = Attributes,
        };
    }

    public IDisposable RegisterUsage(FileAccess access, FileShare share)
    {
        lock (_lock)
        {
            if (_usages.Count > 0)
            {
                var currentShare = _usages.Aggregate(FileShare.None, (a, b) => a | b.Share);
                if (((int)currentShare & (int)access) != (int)access)
                    throw new IOException("The file is already used by another process.");
            }

            var usage = new Usage(this, share);
            _usages.Add(usage);
            return usage;
        }
    }

    private sealed class Usage : IDisposable
    {
        private readonly FileNode _node;

        public Usage(FileNode node, FileShare share)
        {
            _node = node;
            Share = share;
        }

        public FileShare Share { get; }

        public void Dispose()
        {
            lock (_node._lock)
                _node._usages.Remove(this);
        }
    }
}
