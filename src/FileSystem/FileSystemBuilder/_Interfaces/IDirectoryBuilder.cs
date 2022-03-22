namespace MaSch.FileSystem.FileSystemBuilder;

public interface IDirectoryBuilder : IFileSystemEntryBuilder<IDirectoryBuilder>, IFileSystemContainerBuilder<IDirectoryBuilder>, IFileSystemActionBuilder
{
}
