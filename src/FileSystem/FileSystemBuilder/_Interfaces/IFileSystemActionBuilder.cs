namespace MaSch.FileSystem.FileSystemBuilder;

/// <summary>
/// Provides methods to build a <see cref="IFileSystemAction"/>.
/// </summary>
public interface IFileSystemActionBuilder
{
    /// <summary>
    /// Builds the <see cref="IFileSystemAction"/>.
    /// </summary>
    /// <returns>The built <see cref="IFileSystemAction"/>.</returns>
    IFileSystemAction Build();
}
