﻿using MaSch.FileSystem.InMemory.Models;

namespace MaSch.FileSystem.InMemory;

public class InMemoryFileService : FileServiceBase
{
    private readonly InMemoryFileSystemService _fileSystem;

    public InMemoryFileService(InMemoryFileSystemService fileSystem)
        : base(fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public override void Copy(string sourceFileName, string destFileName, bool overwrite)
    {
        var sourceNode = GetFileNode(sourceFileName);
        if (_fileSystem.TryGetNode(destFileName, out var destNode) && destNode is not FileNode)
            throw new IOException($"The target file '{destFileName}' is a directory, not a file.");
        if (!overwrite && destNode is not null)
            throw new IOException($"The file '{destFileName}' already exists.");
        if (!_fileSystem.TryGetNode<ContainerNode>(Path.GetDirectoryName(destFileName), out var destParentNode))
            throw new DirectoryNotFoundException($"Could not find a part of the path '{Path.GetDirectoryName(destFileName)}'.");

        var destFileNode = sourceNode.Clone();

        if (destNode is not null)
        {
            destNode.Parent.Files.Remove((FileNode)destNode);
            destNode.ParentDirectory = null;
        }

        destFileNode.Name = Path.GetFileName(destFileName);
        destFileNode.Root = destParentNode.Root;
        destFileNode.ParentDirectory = destParentNode as DirectoryNode;
        destParentNode.Files.Add(destFileNode);
    }

    public override void Delete(string path)
    {
        var node = GetFileNode(path);
        node.Parent.Files.Remove(node);
        node.ParentDirectory = null;
    }

    public override bool Exists([NotNullWhen(true)] string? path)
    {
        return _fileSystem.TryGetNode<FileNode>(path, out _);
    }

    public override FileAttributes GetAttributes(string path)
    {
        return GetFileNode(path).Attributes;
    }

    public override DateTime GetCreationTimeUtc(string path)
    {
        return GetFileNode(path).CreationTimeUtc;
    }

    public override IFileInfo GetInfo(string path)
    {
        return new InMemoryFileInfo(_fileSystem, path);
    }

    public override DateTime GetLastAccessTimeUtc(string path)
    {
        return GetFileNode(path).LastAccessTimeUtc;
    }

    public override DateTime GetLastWriteTimeUtc(string path)
    {
        return GetFileNode(path).LastWriteTimeUtc;
    }

    public override void Move(string sourceFileName, string destFileName, bool overwrite)
    {
        var sourceNode = GetFileNode(sourceFileName);
        if (_fileSystem.TryGetNode(destFileName, out var destNode) && destNode is not FileNode)
            throw new IOException($"The target file '{destFileName}' is a directory, not a file.");
        if (!overwrite && destNode is not null)
            throw new IOException($"The file '{destFileName}' already exists.");
        if (!_fileSystem.TryGetNode<ContainerNode>(Path.GetDirectoryName(destFileName), out var destParentNode))
            throw new DirectoryNotFoundException($"Could not find a part of the path '{Path.GetDirectoryName(destFileName)}'.");

        if (destNode is not null)
        {
            destNode.Parent.Files.Remove((FileNode)destNode);
            destNode.ParentDirectory = null;
        }

        if (sourceNode.Parent != destParentNode)
        {
            sourceNode.Parent.Files.Remove(sourceNode);
            sourceNode.Root = destParentNode.Root;
            sourceNode.ParentDirectory = destParentNode as DirectoryNode;
            destParentNode.Files.Add(sourceNode);
        }

        var fileName = Path.GetFileName(destFileName);
        if (sourceNode.Name != fileName)
            sourceNode.Name = fileName;
    }

    public override Stream Open(string path, FileStreamOptions options)
    {
        return Open(path, options.Mode, options.Access, options.Share, options.BufferSize, options.Options);
    }

    public override void SetAttributes(string path, FileAttributes fileAttributes)
    {
        GetFileNode(path).Attributes = fileAttributes;
    }

    public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
    {
        GetFileNode(path).CreationTimeUtc = creationTimeUtc;
    }

    public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
    {
        GetFileNode(path).LastAccessTimeUtc = lastAccessTimeUtc;
    }

    public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
    {
        GetFileNode(path).LastWriteTimeUtc = lastWriteTimeUtc;
    }

    protected override Stream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
    {
        if (_fileSystem.TryGetNode(path, out var node) && node is not FileNode)
            throw new IOException($"The target file '{path}' is a directory, not a file.");
        var fileNode = node as FileNode;

        if (mode == FileMode.Open && fileNode is null)
            throw new FileNotFoundException($"Could not find a part of the path '{path}'.");
        if (mode == FileMode.CreateNew && fileNode is not null)
            throw new IOException($"File '{path}' already exists.");
        if (mode == FileMode.Create && fileNode is not null)
            fileNode.Content = Array.Empty<byte>();

        if (fileNode is null)
        {
            if (!_fileSystem.TryGetNode<ContainerNode>(Path.GetDirectoryName(path), out var destParentNode))
                throw new DirectoryNotFoundException($"Could not find a part of the path '{Path.GetDirectoryName(path)}'.");
            fileNode = new FileNode(destParentNode.Root, destParentNode as DirectoryNode, Path.GetFileName(path));
            destParentNode.Files.Add(fileNode);
        }

        var stream = new MemoryFileStream(fileNode, access);
        if (mode == FileMode.Append)
        {
            stream.Seek(0, SeekOrigin.End);
        }

        return stream;
    }

    private FileNode GetFileNode(string? path)
    {
        if (!_fileSystem.TryGetNode<FileNode>(path, out var node))
            throw new FileNotFoundException($"Could not find a part of the path '{path}'.");
        return node;
    }

    private sealed class MemoryFileStream : MemoryStream
    {
        private readonly FileNode _node;

        public MemoryFileStream(FileNode node, FileAccess access)
            : base(node.Content, access.HasFlag(FileAccess.Write))
        {
            _node = node;
        }

        public override void Flush()
        {
            base.Flush();
            _node.Content = ToArray();
        }

        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            await base.FlushAsync(cancellationToken);
            _node.Content = ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            finally
            {
                if (disposing)
                {
                    _node.Content = ToArray();
                }
            }
        }
    }
}
