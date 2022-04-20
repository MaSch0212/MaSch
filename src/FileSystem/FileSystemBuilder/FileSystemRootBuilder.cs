using MaSch.Core;
using MaSch.FileSystem.FileSystemBuilder.Actions;

namespace MaSch.FileSystem.FileSystemBuilder;

internal class FileSystemRootBuilder : IFileSystemRootBuilder
{
    private readonly FileSystemRootCreateAction _action;

    public FileSystemRootBuilder(string rootName)
    {
        _action = new FileSystemRootCreateAction(rootName);
    }

    public IFileSystemAction Build()
    {
        return _action;
    }

    public IFileSystemRootBuilder WithDirectory(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new DirectoryCreateAction(fullPath));
        return this;
    }

    public IFileSystemRootBuilder WithDirectory(string path, Action<IDirectoryBuilder> directoryConfiguration)
    {
        Guard.NotNull(directoryConfiguration);
        var fullPath = GetFullPath(path);
        var builder = new DirectoryBuilder(fullPath);
        directoryConfiguration.Invoke(builder);
        _action.SubActionList.Add(builder.Build());
        return this;
    }

    public IFileSystemRootBuilder WithFile(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new FileCreateAction(fullPath));
        return this;
    }

    public IFileSystemRootBuilder WithFile(string path, Action<IFileBuilder> fileConfiguration)
    {
        Guard.NotNull(fileConfiguration);
        var fullPath = GetFullPath(path);
        var builder = new FileBuilder(fullPath);
        fileConfiguration.Invoke(builder);
        _action.SubActionList.Add(builder.Build());
        return this;
    }

    public IFileSystemRootBuilder WithoutDirectory(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new DirectoryDeleteAction(fullPath));
        return this;
    }

    public IFileSystemRootBuilder WithoutFile(string path)
    {
        var fullPath = GetFullPath(path);
        _action.SubActionList.Add(new FileDeleteAction(fullPath));
        return this;
    }

    private string GetFullPath(string path)
    {
        if (Path.IsPathRooted(path))
            throw new ArgumentException("The path cannot be rooted", nameof(path));
        return Path.Combine(_action.RootName, path);
    }
}
