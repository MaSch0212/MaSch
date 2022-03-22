namespace MaSch.FileSystem.FileSystemBuilder;

public interface IFileSystemContainerBuilder<T>
    where T : IFileSystemContainerBuilder<T>
{
    T WithFile(string path);
    T WithFile(string path, Action<IFileBuilder> fileConfiguration);
    T WithoutFile(string path);

    T WithDirectory(string path);
    T WithDirectory(string path, Action<IDirectoryBuilder> directoryConfiguration);
    T WithoutDirectory(string path);
}
