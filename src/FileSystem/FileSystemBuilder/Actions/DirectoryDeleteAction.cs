namespace MaSch.FileSystem.FileSystemBuilder.Actions;

public class DirectoryDeleteAction : IFileSystemAction
{
    public DirectoryDeleteAction(string fullPath)
    {
        FullPath = fullPath;
    }

    public string FullPath { get; }

    public void Invoke(IFileSystemService service)
    {
        if (service.Directory.Exists(FullPath))
            service.Directory.Delete(FullPath, true);
    }
}
