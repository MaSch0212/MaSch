namespace MaSch.FileSystem.FileSystemBuilder;

public interface IFileSystemAction
{
    void Invoke(IFileSystemService service);
}
