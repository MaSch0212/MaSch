namespace MaSch.FileSystem;

/// <summary>
/// Base class that can be used to more easily implement the <see cref="IFileInfo"/> class.
/// </summary>
public abstract class FileInfoBase : FileSystemInfoBase, IFileInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileInfoBase"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system service, this <see cref="IFileInfo"/> is part of.</param>
    /// <param name="path">The path to the file system entry.</param>
    protected FileInfoBase(IFileSystemService fileSystem, string path)
        : base(fileSystem, path)
    {
    }

    /// <inheritdoc/>
    public abstract long Length { get; }

    /// <inheritdoc/>
    public virtual string? DirectoryName => Path.GetDirectoryName(FullName);

    /// <inheritdoc/>
    public virtual IDirectoryInfo? Directory => FileSystem.File.GetDirectory(FullName);

    /// <inheritdoc/>
    public abstract bool IsReadOnly { get; }

    /// <inheritdoc/>
    public override bool Exists => FileSystem.File.Exists(FullName);

    /// <inheritdoc/>
    public override DateTime CreationTimeUtc
    {
        get => FileSystem.File.GetCreationTimeUtc(FullName);
        set => FileSystem.File.SetCreationTimeUtc(FullName, value);
    }

    /// <inheritdoc/>
    public override DateTime CreationTime
    {
        get => FileSystem.File.GetCreationTime(FullName);
        set => FileSystem.File.SetCreationTime(FullName, value);
    }

    /// <inheritdoc/>
    public override DateTime LastAccessTimeUtc
    {
        get => FileSystem.File.GetLastAccessTimeUtc(FullName);
        set => FileSystem.File.SetLastAccessTimeUtc(FullName, value);
    }

    /// <inheritdoc/>
    public override DateTime LastAccessTime
    {
        get => FileSystem.File.GetLastAccessTime(FullName);
        set => FileSystem.File.SetLastAccessTime(FullName, value);
    }

    /// <inheritdoc/>
    public override DateTime LastWriteTimeUtc
    {
        get => FileSystem.File.GetLastWriteTimeUtc(FullName);
        set => FileSystem.File.SetLastWriteTimeUtc(FullName, value);
    }

    /// <inheritdoc/>
    public override DateTime LastWriteTime
    {
        get => FileSystem.File.GetLastWriteTime(FullName);
        set => FileSystem.File.SetLastWriteTime(FullName, value);
    }

    /// <inheritdoc/>
    public override FileAttributes Attributes
    {
        get => FileSystem.File.GetAttributes(FullName);
        set => FileSystem.File.SetAttributes(FullName, value);
    }

    /// <inheritdoc/>
    public virtual StreamWriter AppendText()
    {
        return FileSystem.File.AppendText(FullName);
    }

    /// <inheritdoc/>
    public virtual IFileInfo CopyTo(string destFileName)
    {
        FileSystem.File.Copy(FullName, destFileName);
        return FileSystem.File.GetInfo(destFileName);
    }

    /// <inheritdoc/>
    public virtual IFileInfo CopyTo(string destFileName, bool overwrite)
    {
        FileSystem.File.Copy(FullName, destFileName, overwrite);
        return FileSystem.File.GetInfo(destFileName);
    }

    /// <inheritdoc/>
    public virtual Stream Create()
    {
        return FileSystem.File.Create(FullName);
    }

    /// <inheritdoc/>
    public virtual StreamWriter CreateText()
    {
        return FileSystem.File.CreateText(FullName);
    }

    /// <inheritdoc/>
    public virtual void MoveTo(string destFileName)
    {
        FileSystem.File.Move(FullName, destFileName);
    }

    /// <inheritdoc/>
    public virtual void MoveTo(string destFileName, bool overwrite)
    {
        FileSystem.File.Move(FullName, destFileName, overwrite);
    }

    /// <inheritdoc/>
    public virtual Stream Open(FileMode mode)
    {
        return FileSystem.File.Open(FullName, mode);
    }

    /// <inheritdoc/>
    public virtual Stream Open(FileMode mode, FileAccess access)
    {
        return FileSystem.File.Open(FullName, mode, access);
    }

    /// <inheritdoc/>
    public virtual Stream Open(FileMode mode, FileAccess access, FileShare share)
    {
        return FileSystem.File.Open(FullName, mode, access, share);
    }

    /// <inheritdoc/>
    public virtual Stream Open(FileStreamOptions options)
    {
        return FileSystem.File.Open(FullName, options);
    }

    /// <inheritdoc/>
    public virtual Stream OpenRead()
    {
        return FileSystem.File.OpenRead(FullName);
    }

    /// <inheritdoc/>
    public virtual StreamReader OpenText()
    {
        return FileSystem.File.OpenText(FullName);
    }

    /// <inheritdoc/>
    public virtual Stream OpenWrite()
    {
        return FileSystem.File.OpenWrite(FullName);
    }

    /// <inheritdoc/>
    public virtual IFileInfo Replace(string destinationFileName, string? destinationBackupFileName)
    {
        FileSystem.File.Replace(FullName, destinationFileName, destinationBackupFileName);
        return FileSystem.GetFileInfo(destinationFileName);
    }

    /// <inheritdoc/>
    public virtual IFileInfo Replace(string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
    {
        FileSystem.File.Replace(FullName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        return FileSystem.GetFileInfo(destinationFileName);
    }

    /// <inheritdoc/>
    public override void Delete()
    {
        FileSystem.File.Delete(FullName);
    }
}
