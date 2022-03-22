namespace MaSch.FileSystem.FileSystemBuilder.Actions;

public class FileCreateAction : FileSystemEntryCreateAction
{
    public FileCreateAction(string fullPath)
        : base(fullPath)
    {
    }

    public byte[]? Content { get; internal set; }

    public override void Invoke(IFileSystemService service)
    {
        var dirPath = Path.GetDirectoryName(FullPath);
        if (dirPath is not null)
            service.Directory.CreateDirectory(dirPath);

        using (var stream = service.File.Create(FullPath))
        {
            if (Content is not null)
                stream.Write(Content, 0, Content.Length);
        }

        base.Invoke(service);
    }

    protected override void SetAttributes(IFileSystemService service, string fullPath, FileAttributes attributes)
        => service.File.SetAttributes(fullPath, attributes);
    protected override void SetCreationTimeUtc(IFileSystemService service, string fullPath, DateTime creationTimeUtc)
        => service.File.SetCreationTimeUtc(fullPath, creationTimeUtc);
    protected override void SetLastAccessTimeUtc(IFileSystemService service, string fullPath, DateTime lastAccessTimeUtc)
        => service.File.SetLastAccessTimeUtc(fullPath, lastAccessTimeUtc);
    protected override void SetLastWriteTimeUtc(IFileSystemService service, string fullPath, DateTime lastWriteTimeUtc)
        => service.File.SetLastWriteTimeUtc(fullPath, lastWriteTimeUtc);
}
