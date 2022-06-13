using MaSch.Core;

#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem.Physical;

internal class PhysicalDirectoryInfo : PhysicalFileSystemInfo, IDirectoryInfo
{
    public PhysicalDirectoryInfo(IFileSystemService fileSystem, DirectoryInfo wrappedInfo)
        : base(fileSystem, wrappedInfo)
    {
        WrappedInfo = Guard.NotNull(wrappedInfo);
    }

    public IDirectoryInfo? Parent
    {
        get
        {
            var directoryInfo = WrappedInfo.Parent;
            return directoryInfo == null ? null : new PhysicalDirectoryInfo(FileSystem, directoryInfo);
        }
    }

    public IDirectoryInfo Root => new PhysicalDirectoryInfo(FileSystem, WrappedInfo.Root);

#if NET5_0_OR_GREATER
    protected override DirectoryInfo WrappedInfo { get; }
#else
    protected new DirectoryInfo WrappedInfo { get; }
#endif

    public void Create()
    {
        WrappedInfo.Create();
    }

    public IDirectoryInfo CreateSubdirectory(string path)
    {
        return new PhysicalDirectoryInfo(FileSystem, WrappedInfo.CreateSubdirectory(path));
    }

    public void Delete(bool recursive)
    {
        WrappedInfo.Delete(recursive);
    }

    public IEnumerable<IDirectoryInfo> EnumerateDirectories()
    {
        return ToDirectoryInfo(WrappedInfo.EnumerateDirectories());
    }

    public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern)
    {
        return ToDirectoryInfo(WrappedInfo.EnumerateDirectories(searchPattern));
    }

    public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
    {
        return ToDirectoryInfo(WrappedInfo.EnumerateDirectories(searchPattern, searchOption));
    }

    public IEnumerable<IFileInfo> EnumerateFiles()
    {
        return ToFileInfo(WrappedInfo.EnumerateFiles());
    }

    public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern)
    {
        return ToFileInfo(WrappedInfo.EnumerateFiles(searchPattern));
    }

    public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
    {
        return ToFileInfo(WrappedInfo.EnumerateFiles(searchPattern, searchOption));
    }

    public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
    {
        return ToFileSystemInfo(WrappedInfo.EnumerateFileSystemInfos());
    }

    public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
    {
        return ToFileSystemInfo(WrappedInfo.EnumerateFileSystemInfos(searchPattern));
    }

    public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
    {
        return ToFileSystemInfo(WrappedInfo.EnumerateFileSystemInfos(searchPattern, searchOption));
    }

    public IDirectoryInfo[] GetDirectories()
    {
        return ToDirectoryInfo(WrappedInfo.GetDirectories());
    }

    public IDirectoryInfo[] GetDirectories(string searchPattern)
    {
        return ToDirectoryInfo(WrappedInfo.GetDirectories(searchPattern));
    }

    public IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
    {
        return ToDirectoryInfo(WrappedInfo.GetDirectories(searchPattern, searchOption));
    }

    public IFileInfo[] GetFiles()
    {
        return ToFileInfo(WrappedInfo.GetFiles());
    }

    public IFileInfo[] GetFiles(string searchPattern)
    {
        return ToFileInfo(WrappedInfo.GetFiles(searchPattern));
    }

    public IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
    {
        return ToFileInfo(WrappedInfo.GetFiles(searchPattern, searchOption));
    }

    public IFileSystemInfo[] GetFileSystemInfos()
    {
        return ToFileSystemInfo(WrappedInfo.GetFileSystemInfos());
    }

    public IFileSystemInfo[] GetFileSystemInfos(string searchPattern)
    {
        return ToFileSystemInfo(WrappedInfo.GetFileSystemInfos(searchPattern));
    }

    public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
    {
        return ToFileSystemInfo(WrappedInfo.GetFileSystemInfos(searchPattern, searchOption));
    }

    public void MoveTo(string destDirName)
    {
        WrappedInfo.MoveTo(destDirName);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public virtual IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToDirectoryInfo(WrappedInfo.EnumerateDirectories(searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileInfo(WrappedInfo.EnumerateFiles(searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileSystemInfo(WrappedInfo.EnumerateFileSystemInfos(searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToDirectoryInfo(WrappedInfo.GetDirectories(searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileInfo(WrappedInfo.GetFiles(searchPattern, enumerationOptions));
    }

    /// <inheritdoc/>
    public virtual IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
    {
        return ToFileSystemInfo(WrappedInfo.GetFileSystemInfos(searchPattern, enumerationOptions));
    }
#endif

    private IEnumerable<IDirectoryInfo> ToDirectoryInfo(IEnumerable<DirectoryInfo> paths) => paths.Select(x => new PhysicalDirectoryInfo(FileSystem, x));
    private IDirectoryInfo[] ToDirectoryInfo(DirectoryInfo[] paths) => ToDirectoryInfo((IEnumerable<DirectoryInfo>)paths).ToArray();
    private IEnumerable<IFileInfo> ToFileInfo(IEnumerable<FileInfo> paths) => paths.Select(x => new PhysicalFileInfo(FileSystem, x));
    private IFileInfo[] ToFileInfo(FileInfo[] paths) => ToFileInfo((IEnumerable<FileInfo>)paths).ToArray();
    [ExcludeFromCodeCoverage]
    private IEnumerable<IFileSystemInfo> ToFileSystemInfo(IEnumerable<FileSystemInfo> paths)
        => paths.Select<FileSystemInfo, IFileSystemInfo>(x => x switch
        {
            FileInfo fileInfo => new PhysicalFileInfo(FileSystem, fileInfo),
            DirectoryInfo directoryInfo => new PhysicalDirectoryInfo(FileSystem, directoryInfo),
            _ => throw new InvalidOperationException(),
        });
    private IFileSystemInfo[] ToFileSystemInfo(FileSystemInfo[] paths) => ToFileSystemInfo((IEnumerable<FileSystemInfo>)paths).ToArray();
}
