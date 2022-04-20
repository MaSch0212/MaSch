using MaSch.Core;
using MaSch.FileSystem.FileSystemBuilder.Actions;

namespace MaSch.FileSystem.FileSystemBuilder;

internal class DirectoryBuilder : IDirectoryBuilder
{
    private readonly DirectoryCreateAction _action;

    public DirectoryBuilder(string fullPath)
    {
        _action = new DirectoryCreateAction(fullPath);
    }

    public IFileSystemAction Build()
    {
        return _action;
    }

    public IDirectoryBuilder WithAttributes(FileAttributes attributes)
    {
        Guard.NotUndefinedFlagInEnumValue(attributes);
        _action.Attributes = attributes | FileAttributes.Directory;
        return this;
    }

    public IDirectoryBuilder WithCreationTime(DateTime creationTime)
    {
        _action.CreationTimeUtc = creationTime.ToUniversalTime(DateTimeKind.Local);
        return this;
    }

    public IDirectoryBuilder WithCreationTimeUtc(DateTime creationTimeUtc)
    {
        _action.CreationTimeUtc = creationTimeUtc.ToUniversalTime(DateTimeKind.Utc);
        return this;
    }

    public IDirectoryBuilder WithDirectory(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new DirectoryCreateAction(fullPath));
        return this;
    }

    public IDirectoryBuilder WithDirectory(string path, Action<IDirectoryBuilder> directoryConfiguration)
    {
        Guard.NotNull(directoryConfiguration);
        var fullPath = GetFullPath(path);
        var builder = new DirectoryBuilder(fullPath);
        directoryConfiguration.Invoke(builder);
        _action.SubActionList.Add(builder.Build());
        return this;
    }

    public IDirectoryBuilder WithFile(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new FileCreateAction(fullPath));
        return this;
    }

    public IDirectoryBuilder WithFile(string path, Action<IFileBuilder> fileConfiguration)
    {
        Guard.NotNull(fileConfiguration);
        var fullPath = GetFullPath(path);
        var builder = new FileBuilder(fullPath);
        fileConfiguration.Invoke(builder);
        _action.SubActionList.Add(builder.Build());
        return this;
    }

    public IDirectoryBuilder WithLastAccessTime(DateTime lastAccessTime)
    {
        _action.LastAccessTimeUtc = lastAccessTime.ToUniversalTime(DateTimeKind.Local);
        return this;
    }

    public IDirectoryBuilder WithLastAccessTimeUtc(DateTime lastAccessTimeUtc)
    {
        _action.LastAccessTimeUtc = lastAccessTimeUtc.ToUniversalTime(DateTimeKind.Utc);
        return this;
    }

    public IDirectoryBuilder WithLastWriteTime(DateTime lastWriteTime)
    {
        _action.LastWriteTimeUtc = lastWriteTime.ToUniversalTime(DateTimeKind.Local);
        return this;
    }

    public IDirectoryBuilder WithLastWriteTimeUtc(DateTime lastWriteTimeUtc)
    {
        _action.LastWriteTimeUtc = lastWriteTimeUtc.ToUniversalTime(DateTimeKind.Utc);
        return this;
    }

    public IDirectoryBuilder WithoutDirectory(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new DirectoryDeleteAction(fullPath));
        return this;
    }

    public IDirectoryBuilder WithoutFile(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new FileDeleteAction(fullPath));
        return this;
    }

    private string GetFullPath(string path)
    {
        if (Path.IsPathRooted(path))
            throw new ArgumentException("The path cannot be rooted", nameof(path));
        return Path.Combine(_action.FullPath, path);
    }
}
