using MaSch.Core;

#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem;

/// <summary>
/// Base class that can be used to more easily implement the <see cref="IDirectoryInfo"/> class.
/// </summary>
public abstract class DirectoryInfoBase : FileSystemInfoBase, IDirectoryInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryInfoBase"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system service, this <see cref="IDirectoryInfo"/> is part of.</param>
    /// <param name="path">The path to the file system entry.</param>
    protected DirectoryInfoBase(IFileSystemService fileSystem, string path)
        : base(fileSystem, path)
    {
    }

    /// <inheritdoc/>
    public virtual IDirectoryInfo? Parent => FileSystem.Directory.GetParent(FullName);

    /// <inheritdoc/>
    public virtual IDirectoryInfo Root => FileSystem.GetDirectoryInfo(FileSystem.Directory.GetDirectoryRoot(FullName));

    /// <inheritdoc/>
    public override bool Exists => FileSystem.Directory.Exists(FullName);

    /// <inheritdoc/>
    public override DateTime CreationTimeUtc
    {
        get => FileSystem.Directory.GetCreationTimeUtc(FullName);
        set => FileSystem.Directory.SetCreationTimeUtc(FullName, value);
    }

    /// <inheritdoc/>
    public override DateTime LastAccessTimeUtc
    {
        get => FileSystem.Directory.GetLastAccessTimeUtc(FullName);
        set => FileSystem.Directory.SetLastAccessTimeUtc(FullName, value);
    }

    /// <inheritdoc/>
    public override DateTime LastWriteTimeUtc
    {
        get => FileSystem.Directory.GetLastWriteTimeUtc(FullName);
        set => FileSystem.Directory.SetLastWriteTimeUtc(FullName, value);
    }

    /// <inheritdoc/>
    public virtual void Create()
    {
        _ = FileSystem.Directory.CreateDirectory(FullName);
    }

    /// <inheritdoc/>
    public virtual IDirectoryInfo CreateSubdirectory(string path)
    {
        Guard.NotNullOrEmpty(path);
        if (Path.IsPathRooted(path))
            throw new ArgumentException("The path cannot be rooted.", nameof(path));
        string fullPath = Path.GetFullPath(Path.Combine(FullName, path));
        return FileSystem.GetDirectoryInfo(fullPath);
    }

    /// <inheritdoc/>
    public override void Delete()
    {
        FileSystem.Directory.Delete(FullName);
    }

    /// <inheritdoc/>
    public virtual void Delete(bool recursive)
    {
        FileSystem.Directory.Delete(FullName, recursive);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IDirectoryInfo> EnumerateDirectories()
    {
        return ToDirectoryInfo(FileSystem.Directory.EnumerateDirectories(FullName));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern)
    {
        return ToDirectoryInfo(FileSystem.Directory.EnumerateDirectories(FullName, searchPattern));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
    {
        return ToDirectoryInfo(FileSystem.Directory.EnumerateDirectories(FullName, searchPattern, searchOption));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileInfo> EnumerateFiles()
    {
        return ToFileInfo(FileSystem.Directory.EnumerateFiles(FullName));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileInfo> EnumerateFiles(string searchPattern)
    {
        return ToFileInfo(FileSystem.Directory.EnumerateFiles(FullName, searchPattern));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
    {
        return ToFileInfo(FileSystem.Directory.EnumerateFiles(FullName, searchPattern, searchOption));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
    {
        return ToFileSystemInfo(FileSystem.Directory.EnumerateFileSystemEntries(FullName));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
    {
        return ToFileSystemInfo(FileSystem.Directory.EnumerateFileSystemEntries(FullName, searchPattern));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
    {
        return ToFileSystemInfo(FileSystem.Directory.EnumerateFileSystemEntries(FullName, searchPattern, searchOption));
    }

    /// <inheritdoc/>
    public virtual IDirectoryInfo[] GetDirectories()
    {
        return ToDirectoryInfo(FileSystem.Directory.GetDirectories(FullName));
    }

    /// <inheritdoc/>
    public virtual IDirectoryInfo[] GetDirectories(string searchPattern)
    {
        return ToDirectoryInfo(FileSystem.Directory.GetDirectories(FullName, searchPattern));
    }

    /// <inheritdoc/>
    public virtual IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
    {
        return ToDirectoryInfo(FileSystem.Directory.GetDirectories(FullName, searchPattern, searchOption));
    }

    /// <inheritdoc/>
    public virtual IFileInfo[] GetFiles()
    {
        return ToFileInfo(FileSystem.Directory.GetFiles(FullName));
    }

    /// <inheritdoc/>
    public virtual IFileInfo[] GetFiles(string searchPattern)
    {
        return ToFileInfo(FileSystem.Directory.GetFiles(FullName, searchPattern));
    }

    /// <inheritdoc/>
    public virtual IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
    {
        return ToFileInfo(FileSystem.Directory.GetFiles(FullName, searchPattern, searchOption));
    }

    /// <inheritdoc/>
    public virtual IFileSystemInfo[] GetFileSystemInfos()
    {
        return ToFileSystemInfo(FileSystem.Directory.GetFileSystemEntries(FullName));
    }

    /// <inheritdoc/>
    public virtual IFileSystemInfo[] GetFileSystemInfos(string searchPattern)
    {
        return ToFileSystemInfo(FileSystem.Directory.GetFileSystemEntries(FullName, searchPattern));
    }

    /// <inheritdoc/>
    public virtual IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
    {
        return ToFileSystemInfo(FileSystem.Directory.GetFileSystemEntries(FullName, searchPattern, searchOption));
    }

    /// <inheritdoc/>
    public virtual void MoveTo(string destDirName)
    {
        FileSystem.Directory.Move(FullName, destDirName);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public virtual IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToDirectoryInfo(FileSystem.Directory.EnumerateDirectories(FullName, searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileInfo(FileSystem.Directory.EnumerateFiles(FullName, searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileSystemInfo(FileSystem.Directory.EnumerateFileSystemEntries(FullName, searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToDirectoryInfo(FileSystem.Directory.GetDirectories(FullName, searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileInfo(FileSystem.Directory.GetFiles(FullName, searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileSystemInfo(FileSystem.Directory.GetFileSystemEntries(FullName, searchPattern, enumerationOptions));
    }
#endif

    private IEnumerable<IDirectoryInfo> ToDirectoryInfo(IEnumerable<string> paths) => paths.Select(x => FileSystem.GetDirectoryInfo(x));
    private IDirectoryInfo[] ToDirectoryInfo(string[] paths) => ToDirectoryInfo((IEnumerable<string>)paths).ToArray();
    private IEnumerable<IFileInfo> ToFileInfo(IEnumerable<string> paths) => paths.Select(x => FileSystem.GetFileInfo(x));
    private IFileInfo[] ToFileInfo(string[] paths) => ToFileInfo((IEnumerable<string>)paths).ToArray();
    private IEnumerable<IFileSystemInfo> ToFileSystemInfo(IEnumerable<string> paths) => paths.Select(x => (IFileSystemInfo)(FileSystem.Directory.Exists(x) ? FileSystem.GetDirectoryInfo(x) : FileSystem.GetFileInfo(x)));
    private IFileSystemInfo[] ToFileSystemInfo(string[] paths) => ToFileSystemInfo((IEnumerable<string>)paths).ToArray();
}
