using MaSch.FileSystem.FileSystemBuilder;

namespace MaSch.FileSystem;

/// <summary>
/// Provides extension methods for the <see cref="IFileSystemServiceBuilder{T}"/> interface.
/// </summary>
public static class FileSystemBuilderExtensions
{
    /// <summary>
    /// Specifies that the physical file system should be used when building files and folders.
    /// </summary>
    /// <typeparam name="T">The type of builder to extend.</typeparam>
    /// <param name="builder">The builder to extend.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemServiceBuilder{T}"/>.</returns>
    public static T UsingPhysicalFileSystem<T>(this T builder)
        where T : IFileSystemServiceBuilder<T>
    {
        return builder.UsingFileSystem<Physical.PhysicalFileSystemService>();
    }

    /// <summary>
    /// Specifies that a new virtual in-memory file system should be used when building files and folders.
    /// </summary>
    /// <typeparam name="T">The type of builder to extend.</typeparam>
    /// <param name="builder">The builder to extend.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemServiceBuilder{T}"/>.</returns>
    public static T UsingInMemoryFileSystem<T>(this T builder)
        where T : IFileSystemServiceBuilder<T>
    {
        return builder.UsingFileSystem<InMemory.InMemoryFileSystemService>();
    }
}
