using MaSch.Core;

namespace MaSch.FileSystem.Physical;

internal class PhysicalFileSystemInfo : IFileSystemInfo
{
    public PhysicalFileSystemInfo(IFileSystemService fileSystem, FileSystemInfo wrappedInfo)
    {
        FileSystem = Guard.NotNull(fileSystem);
        WrappedInfo = Guard.NotNull(wrappedInfo);
    }

    public virtual IFileSystemService FileSystem { get; }

    public virtual string FullName => WrappedInfo.FullName;

    public virtual string Extension => WrappedInfo.Extension;

    public virtual string Name => WrappedInfo.Name;

    public virtual bool Exists => WrappedInfo.Exists;

    public virtual DateTime CreationTime
    {
        get => WrappedInfo.CreationTime;
        set => WrappedInfo.CreationTime = value;
    }

    public virtual DateTime CreationTimeUtc
    {
        get => WrappedInfo.CreationTimeUtc;
        set => WrappedInfo.CreationTimeUtc = value;
    }

    public virtual DateTime LastAccessTime
    {
        get => WrappedInfo.LastAccessTime;
        set => WrappedInfo.LastAccessTime = value;
    }

    public virtual DateTime LastAccessTimeUtc
    {
        get => WrappedInfo.LastAccessTimeUtc;
        set => WrappedInfo.LastAccessTimeUtc = value;
    }

    public virtual DateTime LastWriteTime
    {
        get => WrappedInfo.LastWriteTime;
        set => WrappedInfo.LastWriteTime = value;
    }

    public virtual DateTime LastWriteTimeUtc
    {
        get => WrappedInfo.LastWriteTimeUtc;
        set => WrappedInfo.LastWriteTimeUtc = value;
    }

    public virtual FileAttributes Attributes
    {
        get => WrappedInfo.Attributes;
        set => WrappedInfo.Attributes = value;
    }

    protected virtual FileSystemInfo WrappedInfo { get; }

    public virtual void Delete()
    {
        WrappedInfo.Delete();
    }

    public virtual void Refresh()
    {
        WrappedInfo.Refresh();
    }
}
