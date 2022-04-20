namespace MaSch.FileSystem;

/// <summary>
/// Provides methods to access a file system.
/// </summary>
public interface IFileSystemService
{
    /// <summary>
    /// Gets an object that is used to interact with files in the file system represented by this <see cref="IFileSystemService"/>.
    /// </summary>
    IFileService File { get; }

    /// <summary>
    /// Gets an object that is used to interact with files in the file system represented by this <see cref="IDirectoryService"/>.
    /// </summary>
    IDirectoryService Directory { get; }

    /// <summary>
    /// Creates an instance of <see cref="IFileInfo"/> representing the specified file.
    /// </summary>
    /// <param name="filePath">The file from which to create the <see cref="IFileInfo"/>.</param>
    /// <returns>An instance of type <see cref="IFileInfo"/>.</returns>
    IFileInfo GetFileInfo(string filePath);

    /// <summary>
    /// Creates an instance of <see cref="IDirectoryInfo"/> representing the specified directory.
    /// </summary>
    /// <param name="directoryPath">The directory from which to create the <see cref="IDirectoryInfo"/>.</param>
    /// <returns>An instance of type <see cref="IDirectoryInfo"/>.</returns>
    IDirectoryInfo GetDirectoryInfo(string directoryPath);
}
