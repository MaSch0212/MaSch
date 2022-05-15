namespace MaSch.FileSystem;

/// <summary>
/// Base class that can be used to more easily implement the <see cref="IFileSystemService"/> class.
/// </summary>
public abstract class FileSystemServiceBase : IFileSystemService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemServiceBase"/> class.
    /// </summary>
    protected FileSystemServiceBase()
    {
        File = CreateFileService();
        Directory = CreateDirectoryService();
    }

    /// <inheritdoc />
    public virtual IFileService File { get; }

    /// <inheritdoc />
    public virtual IDirectoryService Directory { get; }

    /// <inheritdoc />
    public virtual IDirectoryInfo GetDirectoryInfo(string directoryPath)
    {
        return Directory.GetInfo(directoryPath);
    }

    /// <inheritdoc />
    public virtual IFileInfo GetFileInfo(string filePath)
    {
        return File.GetInfo(filePath);
    }

    /// <summary>
    /// Creates the default file service for this file system.
    /// </summary>
    /// <returns>An instance of the default file service.</returns>
    protected abstract IFileService CreateFileService();

    /// <summary>
    /// Creates the default directory service for this file system.
    /// </summary>
    /// <returns>An instance of the default directory service.</returns>
    protected abstract IDirectoryService CreateDirectoryService();
}
