namespace MaSch.FileSystem.FileSystemBuilder.Actions;

public class DirectoryCreateAction : FileSystemEntryCreateAction
{
    public DirectoryCreateAction(string fullPath)
        : base(fullPath)
    {
        SubActionList = new List<IFileSystemAction>();
        SubActions = new ReadOnlyCollection<IFileSystemAction>(SubActionList);
    }

    public IReadOnlyList<IFileSystemAction> SubActions { get; }

    internal List<IFileSystemAction> SubActionList { get; }

    public override void Invoke(IFileSystemService service)
    {
        service.Directory.CreateDirectory(FullPath);

        base.Invoke(service);
    }

    protected override void SetAttributes(IFileSystemService service, string fullPath, FileAttributes attributes)
        => service.GetDirectoryInfo(fullPath).Attributes = attributes;
    protected override void SetCreationTimeUtc(IFileSystemService service, string fullPath, DateTime creationTimeUtc)
        => service.Directory.SetCreationTimeUtc(fullPath, creationTimeUtc);
    protected override void SetLastAccessTimeUtc(IFileSystemService service, string fullPath, DateTime lastAccessTimeUtc)
        => service.Directory.SetLastAccessTimeUtc(fullPath, lastAccessTimeUtc);
    protected override void SetLastWriteTimeUtc(IFileSystemService service, string fullPath, DateTime lastWriteTimeUtc)
        => service.Directory.SetLastWriteTimeUtc(fullPath, lastWriteTimeUtc);
}
