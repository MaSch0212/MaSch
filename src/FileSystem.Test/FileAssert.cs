namespace MaSch.FileSystem.Test;

public class FileAssert
{
    public FileAssert(AssertBase assert, IFileSystemService fileSystemService, string fullPath)
    {
        Assert = Guard.NotNull(assert);
        FileSystemService = Guard.NotNull(fileSystemService);
        FullPath = fullPath;
    }

    public AssertBase Assert { get; }
    public IFileSystemService FileSystemService { get; }
    public string FullPath { get; }

    public DateTime CreationTime => FileSystemService.File.GetCreationTime(FullPath);
    public DateTime CreationTimeUtc => FileSystemService.File.GetCreationTimeUtc(FullPath);
    public DateTime LastWriteTime => FileSystemService.File.GetLastWriteTime(FullPath);
    public DateTime LastWriteTimeUtc => FileSystemService.File.GetLastWriteTimeUtc(FullPath);
    public DateTime LastAccessTime => FileSystemService.File.GetLastAccessTime(FullPath);
    public DateTime LastAccessTimeUtc => FileSystemService.File.GetLastAccessTimeUtc(FullPath);
    public FileAttributes Attributes => FileSystemService.File.GetAttributes(FullPath);
    public byte[] Content => FileSystemService.File.ReadAllBytes(FullPath);

    public void HasAttributes(FileAttributes expectedAttributes)
    {
        HasAttributes(expectedAttributes, null);
    }

    public void HasAttributes(FileAttributes expectedAttributes, string? message)
    {
        var actualAttributes = FileSystemService.File.GetAttributes(FullPath);
        if ((actualAttributes & expectedAttributes) != expectedAttributes)
            ThrowAssertError(message, ("ExpectedAttributes", expectedAttributes), ("ActualAttributes", actualAttributes));
    }

    public void DoesNotHaveAttributes(FileAttributes unexpectedAttributes)
    {
        DoesNotHaveAttributes(unexpectedAttributes, null);
    }

    public void DoesNotHaveAttributes(FileAttributes unexpectedAttributes, string? message)
    {
        var actualAttributes = FileSystemService.File.GetAttributes(FullPath);
        if ((actualAttributes & unexpectedAttributes) != 0)
            ThrowAssertError(message, ("UnexpectedAttributes", unexpectedAttributes), ("ActualAttributes", actualAttributes));
    }

    private void ThrowAssertError(string? message, params (string Name, object? Value)?[]? values)
    {
        var assertName = $"File.{new StackFrame(1).GetMethod()?.Name}";
        Assert.ThrowAssertError(assertName, message, values);
    }
}
