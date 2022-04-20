namespace MaSch.FileSystem.FileSystemBuilder;

/// <summary>
/// Provides methods to build a directory inside a file system.
/// </summary>
public interface IDirectoryBuilder : IFileSystemEntryBuilder<IDirectoryBuilder>, IFileSystemContainerBuilder<IDirectoryBuilder>, IFileSystemActionBuilder
{
}
