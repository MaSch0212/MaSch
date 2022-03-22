using MaSch.FileSystem.FileSystemBuilder;

namespace MaSch.FileSystem;

public static class FileSystemBuilderExtensions
{
    public static T UsingPhysicalFileSystem<T>(this T builder)
        where T : IFileSystemServiceBuilder<T>
    {
        return builder.UsingFileSystem<Physical.PhysicalFileSystemService>();
    }

    public static T UsingInMemoryFileSystem<T>(this T builder)
        where T : IFileSystemServiceBuilder<T>
    {
        return builder.UsingFileSystem<InMemory.InMemoryFileSystemService>();
    }
}
