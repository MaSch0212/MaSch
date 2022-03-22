namespace MaSch.FileSystem.FileSystemBuilder;

public interface IFileSystemServiceBuilder<T> : IFileSystemContainerBuilder<T>
    where T : IFileSystemServiceBuilder<T>
{
    T UsingFileSystem(IFileSystemService service);
    T UsingFileSystem(Func<IFileSystemService> serviceFactory);
    T UsingFileSystem<TService>()
        where TService : IFileSystemService, new();

    T WithRoot(string rootName);
    T WithRoot(string rootName, Action<IFileSystemRootBuilder> rootConfiguration);

    IFileSystemService Build();
}