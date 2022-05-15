using MaSch.Core;

namespace MaSch.FileSystem.Physical;

internal class PhysicalFileInfo : PhysicalFileSystemInfo, IFileInfo
{
    public PhysicalFileInfo(IFileSystemService fileSystem, FileInfo wrappedInfo)
        : base(fileSystem, wrappedInfo)
    {
        WrappedInfo = Guard.NotNull(wrappedInfo);
    }

    public long Length => WrappedInfo.Length;

    public string? DirectoryName => WrappedInfo.DirectoryName;

    public IDirectoryInfo? Directory
    {
        get
        {
            var directoryInfo = WrappedInfo.Directory;
            return directoryInfo == null ? null : new PhysicalDirectoryInfo(FileSystem, directoryInfo);
        }
    }

    public bool IsReadOnly
    {
        get => WrappedInfo.IsReadOnly;
        set => WrappedInfo.IsReadOnly = value;
    }

#if NET5_0_OR_GREATER
    protected override FileInfo WrappedInfo { get; }
#else
    protected new FileInfo WrappedInfo { get; }
#endif

    public StreamWriter AppendText()
    {
        return WrappedInfo.AppendText();
    }

    public IFileInfo CopyTo(string destFileName)
    {
        return new PhysicalFileInfo(FileSystem, WrappedInfo.CopyTo(destFileName));
    }

    public IFileInfo CopyTo(string destFileName, bool overwrite)
    {
        return new PhysicalFileInfo(FileSystem, WrappedInfo.CopyTo(destFileName, overwrite));
    }

    public Stream Create()
    {
        return WrappedInfo.Create();
    }

    public StreamWriter CreateText()
    {
        return WrappedInfo.CreateText();
    }

    public void MoveTo(string destFileName)
    {
        WrappedInfo.MoveTo(destFileName);
    }

    public void MoveTo(string destFileName, bool overwrite)
    {
#if NET5_0_OR_GREATER
        WrappedInfo.MoveTo(destFileName, overwrite);
#else
        FileSystem.File.Move(FullName, destFileName, overwrite);
#endif
    }

    public Stream Open(FileMode mode)
    {
        return WrappedInfo.Open(mode);
    }

    public Stream Open(FileMode mode, FileAccess access)
    {
        return WrappedInfo.Open(mode, access);
    }

    public Stream Open(FileMode mode, FileAccess access, FileShare share)
    {
        return WrappedInfo.Open(mode, access, share);
    }

    public Stream Open(FileStreamOptions options)
    {
#if NET6_0_OR_GREATER
        return WrappedInfo.Open(options);
#else
        return new FileStream(WrappedInfo.FullName, options.Mode, options.Access, options.Share, options.BufferSize, options.Options);
#endif
    }

    public Stream OpenRead()
    {
        return WrappedInfo.OpenRead();
    }

    public StreamReader OpenText()
    {
        return WrappedInfo.OpenText();
    }

    public Stream OpenWrite()
    {
        return WrappedInfo.OpenWrite();
    }

    public IFileInfo Replace(string destinationFileName, string? destinationBackupFileName)
    {
        return new PhysicalFileInfo(FileSystem, WrappedInfo.Replace(destinationFileName, destinationBackupFileName));
    }

    public IFileInfo Replace(string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
    {
        return new PhysicalFileInfo(FileSystem, WrappedInfo.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors));
    }
}
