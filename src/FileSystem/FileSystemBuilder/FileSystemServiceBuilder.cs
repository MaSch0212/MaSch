using MaSch.Core;
using MaSch.FileSystem.FileSystemBuilder;
using MaSch.FileSystem.FileSystemBuilder.Actions;

namespace MaSch.FileSystem;

public class FileSystemServiceBuilder : IFileSystemServiceBuilder<FileSystemServiceBuilder>
{
    private readonly List<IFileSystemAction> _actions = new();
    private Func<IFileSystemService>? _createServiceFunc;

    public IFileSystemService Build()
    {
        IFileSystemService service;
        if (_createServiceFunc is null)
            throw new InvalidOperationException("No file system was specified to use. Specify the file system using the 'UsingFileSystem' methods.");
        else
            service = _createServiceFunc?.Invoke() ?? throw new InvalidOperationException("The provided file system service is null.");

        foreach (var action in _actions)
            action.Invoke(service);

        return service;
    }

    public FileSystemServiceBuilder UsingFileSystem(IFileSystemService service)
    {
        Guard.NotNull(service);
        _createServiceFunc = () => service;
        return this;
    }

    public FileSystemServiceBuilder UsingFileSystem(Func<IFileSystemService> serviceFactory)
    {
        _createServiceFunc = Guard.NotNull(serviceFactory);
        return this;
    }

    public FileSystemServiceBuilder UsingFileSystem<T>()
        where T : IFileSystemService, new()
    {
        _createServiceFunc = () => new T();
        return this;
    }

    public FileSystemServiceBuilder WithRoot(string rootName)
    {
        EnsureCorrectPathRoot(rootName);
        _actions.Add(new FileSystemRootCreateAction(rootName));
        return this;
    }

    public FileSystemServiceBuilder WithRoot(string rootName, Action<IFileSystemRootBuilder> rootConfiguration)
    {
        Guard.NotNull(rootConfiguration);
        EnsureCorrectPathRoot(rootName);
        var builder = new FileSystemRootBuilder(rootName);
        rootConfiguration.Invoke(builder);
        _actions.Add(builder.Build());
        return this;
    }

    public FileSystemServiceBuilder WithFile(string path)
    {
        EnsureRootedPath(path);
        _actions.Add(new FileCreateAction(path));
        return this;
    }

    public FileSystemServiceBuilder WithFile(string path, Action<IFileBuilder> fileConfiguration)
    {
        Guard.NotNull(fileConfiguration);
        EnsureRootedPath(path);
        var builder = new FileBuilder(path);
        fileConfiguration.Invoke(builder);
        _actions.Add(builder.Build());
        return this;
    }

    public FileSystemServiceBuilder WithoutFile(string path)
    {
        EnsureRootedPath(path);
        _actions.Add(new FileDeleteAction(path));
        return this;
    }

    public FileSystemServiceBuilder WithDirectory(string path)
    {
        EnsureRootedPath(path);
        _actions.Add(new DirectoryCreateAction(path));
        return this;
    }

    public FileSystemServiceBuilder WithDirectory(string path, Action<IDirectoryBuilder> directoryConfiguration)
    {
        Guard.NotNull(directoryConfiguration);
        EnsureRootedPath(path);
        var builder = new DirectoryBuilder(path);
        directoryConfiguration.Invoke(builder);
        _actions.Add(builder.Build());
        return this;
    }

    public FileSystemServiceBuilder WithoutDirectory(string path)
    {
        EnsureRootedPath(path);
        _actions.Add(new DirectoryDeleteAction(path));
        return this;
    }

    private static void EnsureCorrectPathRoot(string rootName)
    {
        if (!Path.IsPathRooted(rootName) || Path.GetPathRoot(rootName) != rootName)
            throw new ArgumentException($"The value \"{rootName}\" is not a valid path root.", nameof(rootName));
    }

    private static void EnsureRootedPath(string path)
    {
        if (!Path.IsPathRooted(path))
            throw new ArgumentException($"The path \"{path}\" is not rooted. Relative paths are not supported.", nameof(path));
    }
}