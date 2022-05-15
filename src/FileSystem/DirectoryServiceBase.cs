using MaSch.Core;

#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem;

/// <summary>
/// Base class that can be used to more easily implement the <see cref="IDirectoryService"/> class.
/// </summary>
public abstract class DirectoryServiceBase : IDirectoryService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryServiceBase"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system that is used by this <see cref="IDirectoryService"/>.</param>
    protected DirectoryServiceBase(IFileSystemService fileSystem)
    {
        FileSystem = Guard.NotNull(fileSystem);
    }

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public IFileSystemService FileSystem { get; }

    /// <inheritdoc/>
    public abstract IDirectoryInfo GetInfo(string path);

    /// <inheritdoc/>
    public abstract IDirectoryInfo CreateDirectory(string path);

    /// <inheritdoc/>
    public virtual void Delete(string path)
    {
        Delete(path, false);
    }

    /// <inheritdoc/>
    public abstract void Delete(string path, bool recursive);

    /// <inheritdoc/>
    public virtual IEnumerable<string> EnumerateDirectories(string path)
    {
        return EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
    {
        return EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public abstract IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

    /// <inheritdoc/>
    public virtual IEnumerable<string> EnumerateFiles(string path)
    {
        return EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<string> EnumerateFiles(string path, string searchPattern)
    {
        return EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

    /// <inheritdoc/>
    public virtual IEnumerable<string> EnumerateFileSystemEntries(string path)
    {
        return EnumerateFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
    {
        return EnumerateFileSystemEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public abstract IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

    /// <inheritdoc/>
    public abstract bool Exists([NotNullWhen(true)] string? path);

    /// <inheritdoc/>
    public virtual DateTime GetCreationTime(string path)
    {
        return GetCreationTimeUtc(path).ToLocalTime();
    }

    /// <inheritdoc/>
    public abstract DateTime GetCreationTimeUtc(string path);

    /// <inheritdoc/>
    public abstract string GetCurrentDirectory();

    /// <inheritdoc/>
    public virtual string[] GetDirectories(string path)
    {
        return GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual string[] GetDirectories(string path, string searchPattern)
    {
        return GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
    {
        return EnumerateDirectories(path, searchPattern, searchOption).ToArray();
    }

    /// <inheritdoc/>
    public string GetDirectoryRoot(string path)
    {
        Guard.NotNull(path);
        string fullPath = Path.GetFullPath(path);
        return Path.GetPathRoot(fullPath)!;
    }

    /// <inheritdoc/>
    public virtual string[] GetFiles(string path)
    {
        return GetFiles(path, "*", SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual string[] GetFiles(string path, string searchPattern)
    {
        return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
    {
        return EnumerateFiles(path, searchPattern, searchOption).ToArray();
    }

    /// <inheritdoc/>
    public virtual string[] GetFileSystemEntries(string path)
    {
        return GetFileSystemEntries(path, "*", SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual string[] GetFileSystemEntries(string path, string searchPattern)
    {
        return GetFileSystemEntries(path, searchPattern, SearchOption.TopDirectoryOnly);
    }

    /// <inheritdoc/>
    public virtual string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
    {
        return EnumerateFileSystemEntries(path, searchPattern, searchOption).ToArray();
    }

    /// <inheritdoc/>
    public virtual DateTime GetLastAccessTime(string path)
    {
        return GetLastAccessTimeUtc(path).ToLocalTime();
    }

    /// <inheritdoc/>
    public abstract DateTime GetLastAccessTimeUtc(string path);

    /// <inheritdoc/>
    public virtual DateTime GetLastWriteTime(string path)
    {
        return GetLastWriteTimeUtc(path).ToLocalTime();
    }

    /// <inheritdoc/>
    public abstract DateTime GetLastWriteTimeUtc(string path);

    /// <inheritdoc/>
    public virtual IDirectoryInfo? GetParent(string path)
    {
        Guard.NotNullOrEmpty(path);
        string fullPath = Path.GetFullPath(path);
        string? directoryName = Path.GetDirectoryName(fullPath);
        return directoryName == null ? null : GetInfo(directoryName);
    }

    /// <inheritdoc/>
    public abstract void Move(string sourceDirName, string destDirName);

    /// <inheritdoc/>
    public abstract void SetCreationTime(string path, DateTime creationTime);

    /// <inheritdoc/>
    public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

    /// <inheritdoc/>
    public abstract void SetCurrentDirectory(string path);

    /// <inheritdoc/>
    public abstract void SetLastAccessTime(string path, DateTime lastAccessTime);

    /// <inheritdoc/>
    public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

    /// <inheritdoc/>
    public abstract void SetLastWriteTime(string path, DateTime lastWriteTime);

    /// <inheritdoc/>
    public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public abstract IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <inheritdoc/>
    public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <inheritdoc/>
    public abstract IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <inheritdoc/>
    public virtual string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
    {
        return EnumerateDirectories(path, searchPattern, enumerationOptions).ToArray();
    }

    /// <inheritdoc/>
    public virtual string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
    {
        return EnumerateFiles(path, searchPattern, enumerationOptions).ToArray();
    }

    /// <inheritdoc/>
    public virtual string[] GetFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
    {
        return EnumerateFileSystemEntries(path, searchPattern, enumerationOptions).ToArray();
    }
#endif
}
