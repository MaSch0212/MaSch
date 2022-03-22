namespace MaSch.FileSystem.FileSystemBuilder.Actions;

public class FileDeleteAction : IFileSystemAction
{
    public FileDeleteAction(string fullPath)
    {
        FullPath = fullPath;
    }

    public string FullPath { get; }

    public void Invoke(IFileSystemService service)
    {
        if (service.File.Exists(FullPath))
            service.File.Delete(FullPath);
    }
}
