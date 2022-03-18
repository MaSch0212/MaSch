using MaSch.FileSystem.InMemory.Models;

namespace MaSch.FileSystem.InMemory;

internal class InMemoryDirectoryService : DirectoryServiceBase
{
    private readonly InMemoryFileSystemService _fileSystem;

    public InMemoryDirectoryService(InMemoryFileSystemService fileSystem)
        : base(fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public override IDirectoryInfo CreateDirectory(string path)
    {
        if (!_fileSystem.TryGetRootAndSegments(path, out var rootNode, out var segments))
            throw new DirectoryNotFoundException($"Could not find a part of the path '{path}'.");

        ContainerNode currentNode = rootNode;
        bool startedCreating = false;
        foreach (var segment in segments)
        {
            if (startedCreating || !currentNode.Directories.TryGetByName(segment, out var dirNode))
            {
                startedCreating = true;
                dirNode = new DirectoryNode(rootNode, currentNode as DirectoryNode, segment);
                currentNode.Directories.Add(dirNode);
            }

            currentNode = dirNode;
        }

        return new InMemoryDirectoryInfo(_fileSystem, path, currentNode);
    }

    public override void Delete(string path, bool recursive)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public override bool Exists([NotNullWhen(true)] string? path)
    {
        throw new NotImplementedException();
    }

    public override DateTime GetCreationTimeUtc(string path)
    {
        throw new NotImplementedException();
    }

    public override string GetCurrentDirectory()
    {
        throw new NotImplementedException();
    }

    public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public override string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
    {
        throw new NotImplementedException();
    }

    public override IDirectoryInfo GetInfo(string path)
    {
        throw new NotImplementedException();
    }

    public override DateTime GetLastAccessTimeUtc(string path)
    {
        throw new NotImplementedException();
    }

    public override DateTime GetLastWriteTimeUtc(string path)
    {
        throw new NotImplementedException();
    }

    public override string[] GetLogicalDrives()
    {
        throw new NotImplementedException();
    }

    public override void Move(string sourceDirName, string destDirName)
    {
        throw new NotImplementedException();
    }

    public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
    {
        throw new NotImplementedException();
    }

    public override void SetCurrentDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
    {
        throw new NotImplementedException();
    }

    public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
    {
        throw new NotImplementedException();
    }
}
