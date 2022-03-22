namespace MaSch.FileSystem.Test;

public class FileSystemAssert
{
    public FileSystemAssert(AssertBase assert, IFileSystemService fileSystemService)
    {
        Assert = Guard.NotNull(assert);
        FileSystemService = Guard.NotNull(fileSystemService);
    }

    public AssertBase Assert { get; }
    public IFileSystemService FileSystemService { get; }

    public void Directory(string filePath, Action<DirectoryAssert> assertions)
        => Assert.Directory(FileSystemService, filePath, assertions);
    public void DirectoryExists(string expectedDirectoryPath)
        => Assert.DirectoryExists(FileSystemService, expectedDirectoryPath);
    public void DirectoryExists(string expectedDirectoryPath, string? message)
        => Assert.DirectoryExists(FileSystemService, expectedDirectoryPath, message);
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath)
        => Assert.DirectoryDoesNotExist(FileSystemService, unexpectedDirectoryPath);
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath, string? message)
        => Assert.DirectoryDoesNotExist(FileSystemService, unexpectedDirectoryPath, message);

    public void File(string filePath, Action<FileAssert> assertions)
        => Assert.File(FileSystemService, filePath, assertions);
    public void FileExists(string expectedFilePath)
        => Assert.FileExists(FileSystemService, expectedFilePath);
    public void FileExists(string expectedFilePath, string? message)
        => Assert.FileExists(FileSystemService, expectedFilePath, message);
    public void FileDoesNotExist(string unexpectedFilePath)
        => Assert.FileDoesNotExist(FileSystemService, unexpectedFilePath);
    public void FileDoesNotExist(string unexpectedFilePath, string? message)
        => Assert.FileDoesNotExist(FileSystemService, unexpectedFilePath, message);
}
