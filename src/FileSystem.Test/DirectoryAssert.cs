namespace MaSch.FileSystem.Test;

public class DirectoryAssert
{
    public DirectoryAssert(AssertBase assert, IFileSystemService fileSystemService, string fullPath)
    {
        Assert = Guard.NotNull(assert);
        FileSystemService = Guard.NotNull(fileSystemService);
        FullPath = fullPath;
    }

    public AssertBase Assert { get; }
    public IFileSystemService FileSystemService { get; }
    public string FullPath { get; }

    public DateTime CreationTime => FileSystemService.Directory.GetCreationTime(FullPath);
    public DateTime CreationTimeUtc => FileSystemService.Directory.GetCreationTimeUtc(FullPath);
    public DateTime LastWriteTime => FileSystemService.Directory.GetLastWriteTime(FullPath);
    public DateTime LastWriteTimeUtc => FileSystemService.Directory.GetLastWriteTimeUtc(FullPath);
    public DateTime LastAccessTime => FileSystemService.Directory.GetLastAccessTime(FullPath);
    public DateTime LastAccessTimeUtc => FileSystemService.Directory.GetLastAccessTimeUtc(FullPath);
    public FileAttributes Attributes => FileSystemService.GetDirectoryInfo(FullPath).Attributes;

    public void Directory(string directoryPath, Action<DirectoryAssert> assertions)
        => Assert.Directory(FileSystemService, GetFullPath(directoryPath), assertions);
    public void DirectoryExists(string expectedDirectoryPath)
        => Assert.DirectoryExists(FileSystemService, GetFullPath(expectedDirectoryPath));
    public void DirectoryExists(string expectedDirectoryPath, string? message)
        => Assert.DirectoryExists(FileSystemService, GetFullPath(expectedDirectoryPath), message);
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath)
        => Assert.DirectoryDoesNotExist(FileSystemService, GetFullPath(unexpectedDirectoryPath));
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath, string? message)
        => Assert.DirectoryDoesNotExist(FileSystemService, GetFullPath(unexpectedDirectoryPath), message);

    public void File(string filePath, Action<FileAssert> assertions)
        => Assert.File(FileSystemService, GetFullPath(filePath), assertions);
    public void FileExists(string expectedFilePath)
        => Assert.FileExists(FileSystemService, GetFullPath(expectedFilePath));
    public void FileExists(string expectedFilePath, string? message)
        => Assert.FileExists(FileSystemService, GetFullPath(expectedFilePath), message);
    public void FileDoesNotExist(string unexpectedFilePath)
        => Assert.FileDoesNotExist(FileSystemService, GetFullPath(unexpectedFilePath));
    public void FileDoesNotExist(string unexpectedFilePath, string? message)
        => Assert.FileDoesNotExist(FileSystemService, GetFullPath(unexpectedFilePath), message);

    public void HasAttributes(FileAttributes expectedAttributes)
    {
        HasAttributes(expectedAttributes, null);
    }

    public void HasAttributes(FileAttributes expectedAttributes, string? message)
    {
        var actualAttributes = FileSystemService.GetDirectoryInfo(FullPath).Attributes;
        if ((actualAttributes & expectedAttributes) != expectedAttributes)
            ThrowAssertError(message, ("ExpectedAttributes", expectedAttributes), ("ActualAttributes", actualAttributes));
    }

    public void DoesNotHaveAttributes(FileAttributes unexpectedAttributes)
    {
        DoesNotHaveAttributes(unexpectedAttributes, null);
    }

    public void DoesNotHaveAttributes(FileAttributes unexpectedAttributes, string? message)
    {
        var actualAttributes = FileSystemService.GetDirectoryInfo(FullPath).Attributes;
        if ((actualAttributes & unexpectedAttributes) != 0)
            ThrowAssertError(message, ("UnexpectedAttributes", unexpectedAttributes), ("ActualAttributes", actualAttributes));
    }

    private string GetFullPath(string path, [CallerArgumentExpression("path")] string paramName = "")
    {
        if (Path.IsPathRooted(path))
            throw new ArgumentException("The path cannot be rooted", paramName);
        return Path.Combine(FullPath, path);
    }

    private void ThrowAssertError(string? message, params (string Name, object? Value)?[]? values)
    {
        var assertName = $"Directory.{new StackFrame(1).GetMethod()?.Name}";
        Assert.ThrowAssertError(assertName, message, values);
    }
}
