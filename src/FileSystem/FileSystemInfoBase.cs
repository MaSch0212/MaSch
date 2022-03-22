using MaSch.Core;

namespace MaSch.FileSystem;

/// <summary>
/// Base class that can be used to more easily implement the <see cref="IFileSystemInfo"/> class.
/// </summary>
public abstract class FileSystemInfoBase : IFileSystemInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemInfoBase"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system service, this <see cref="IFileSystemService"/> is part of.</param>
    /// <param name="path">The path to the file system entry.</param>
    protected FileSystemInfoBase(IFileSystemService fileSystem, string path)
    {
        FileSystem = Guard.NotNull(fileSystem);
        OriginalPath = path;
        FullName = Path.GetFullPath(path);
        Name = Path.GetFileName(path);
    }

    /// <inheritdoc/>
    public virtual IFileSystemService FileSystem { get; }

    /// <inheritdoc/>
    public virtual string FullName { get; }

    /// <inheritdoc/>
    public virtual string Extension => Path.GetExtension(FullName);

    /// <inheritdoc/>
    public virtual string Name { get; }

    /// <inheritdoc/>
    public abstract bool Exists { get; }

    /// <inheritdoc/>
    public abstract DateTime CreationTime { get; set; }

    /// <inheritdoc/>
    public abstract DateTime CreationTimeUtc { get; set; }

    /// <inheritdoc/>
    public abstract DateTime LastAccessTime { get; set; }

    /// <inheritdoc/>
    public abstract DateTime LastAccessTimeUtc { get; set; }

    /// <inheritdoc/>
    public abstract DateTime LastWriteTime { get; set; }

    /// <inheritdoc/>
    public abstract DateTime LastWriteTimeUtc { get; set; }

    /// <inheritdoc/>
    public abstract FileAttributes Attributes { get; set; }

    /// <summary>
    /// Gets the original path this <see cref="IFileSystemService"/> has been initialized with.
    /// </summary>
    protected virtual string OriginalPath { get; }

    /// <inheritdoc/>
    public abstract void Delete();

    /// <inheritdoc/>
    public abstract void Refresh();
}
