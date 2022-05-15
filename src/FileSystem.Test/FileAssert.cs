namespace MaSch.FileSystem.Test;

/// <summary>
/// Provides methods to assert a file in a specified file system.
/// </summary>
public class FileAssert
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileAssert"/> class.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="fullPath">The path to the file to assert.</param>
    public FileAssert(AssertBase assert, IFileSystemService fileSystemService, string fullPath)
    {
        Assert = Guard.NotNull(assert);
        FileSystemService = Guard.NotNull(fileSystemService);
        FullPath = fullPath;
    }

    /// <summary>
    /// Gets the assert object to use for assertions.
    /// </summary>
    public AssertBase Assert { get; }

    /// <summary>
    /// Gets the file system to assert.
    /// </summary>
    public IFileSystemService FileSystemService { get; }

    /// <summary>
    /// Gets the path to the file to assert.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets the creation time in local time of the current file.
    /// </summary>
    public DateTime CreationTime => FileSystemService.File.GetCreationTime(FullPath);

    /// <summary>
    /// Gets the creation time in UTC time of the current file.
    /// </summary>
    public DateTime CreationTimeUtc => FileSystemService.File.GetCreationTimeUtc(FullPath);

    /// <summary>
    /// Gets the last write time in local time of the current file.
    /// </summary>
    public DateTime LastWriteTime => FileSystemService.File.GetLastWriteTime(FullPath);

    /// <summary>
    /// Gets the last write time in UTC time of the current file.
    /// </summary>
    public DateTime LastWriteTimeUtc => FileSystemService.File.GetLastWriteTimeUtc(FullPath);

    /// <summary>
    /// Gets the last access time in local time of the current file.
    /// </summary>
    public DateTime LastAccessTime => FileSystemService.File.GetLastAccessTime(FullPath);

    /// <summary>
    /// Gets the last access time in UTC time of the current file.
    /// </summary>
    public DateTime LastAccessTimeUtc => FileSystemService.File.GetLastAccessTimeUtc(FullPath);

    /// <summary>
    /// Gets the file attributes of the current file.
    /// </summary>
    public FileAttributes Attributes => FileSystemService.File.GetAttributes(FullPath);

    /// <summary>
    /// Gets the contents of the current file as an array of bytes.
    /// </summary>
    public byte[] Content => FileSystemService.File.ReadAllBytes(FullPath);

    /// <summary>
    /// Asserts that the current file has specified file attributes.
    /// </summary>
    /// <param name="expectedAttributes">The file attributes the file is to be expected to have.</param>
    public void HasAttributes(FileAttributes expectedAttributes)
    {
        HasAttributes(expectedAttributes, null);
    }

    /// <summary>
    /// Asserts that the current file has specified file attributes.
    /// </summary>
    /// <param name="expectedAttributes">The file attributes the file is to be expected to have.</param>
    /// <param name="message">The message to include in the exception when the current file does not have <paramref name="expectedAttributes"/>. The message is shown in test results.</param>
    public void HasAttributes(FileAttributes expectedAttributes, string? message)
    {
        var actualAttributes = FileSystemService.File.GetAttributes(FullPath);
        if ((actualAttributes & expectedAttributes) != expectedAttributes)
            ThrowAssertError(message, ("ExpectedAttributes", expectedAttributes), ("ActualAttributes", actualAttributes));
    }

    /// <summary>
    /// Asserts that the current file does not have specified file attributes.
    /// </summary>
    /// <param name="unexpectedAttributes">The file attributes the file is to be expected to not have.</param>
    public void DoesNotHaveAttributes(FileAttributes unexpectedAttributes)
    {
        DoesNotHaveAttributes(unexpectedAttributes, null);
    }

    /// <summary>
    /// Asserts that the current file does not have specified file attributes.
    /// </summary>
    /// <param name="unexpectedAttributes">The file attributes the file is to be expected to not have.</param>
    /// <param name="message">The message to include in the exception when the current file has <paramref name="unexpectedAttributes"/>. The message is shown in test results.</param>
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
