namespace MaSch.FileSystem.FileSystemBuilder.Actions;

public abstract class FileSystemEntryCreateAction : IFileSystemAction
{
    protected FileSystemEntryCreateAction(string fullPath)
    {
        FullPath = fullPath;
    }

    public string FullPath { get; }
    public FileAttributes? Attributes { get; internal set; }
    public DateTime? CreationTimeUtc { get; internal set; }
    public DateTime? LastAccessTimeUtc { get; internal set; }
    public DateTime? LastWriteTimeUtc { get; internal set; }

    public virtual void Invoke(IFileSystemService service)
    {
        if (Attributes.HasValue)
            SetAttributes(service, FullPath, Attributes.Value);
        if (CreationTimeUtc.HasValue)
            SetCreationTimeUtc(service, FullPath, CreationTimeUtc.Value);
        if (LastAccessTimeUtc.HasValue)
            SetLastAccessTimeUtc(service, FullPath, LastAccessTimeUtc.Value);
        if (LastWriteTimeUtc.HasValue)
            SetLastWriteTimeUtc(service, FullPath, LastWriteTimeUtc.Value);
    }

    protected abstract void SetAttributes(IFileSystemService service, string fullPath, FileAttributes attributes);
    protected abstract void SetCreationTimeUtc(IFileSystemService service, string fullPath, DateTime creationTimeUtc);
    protected abstract void SetLastAccessTimeUtc(IFileSystemService service, string fullPath, DateTime lastAccessTimeUtc);
    protected abstract void SetLastWriteTimeUtc(IFileSystemService service, string fullPath, DateTime lastWriteTimeUtc);
}
