namespace MaSch.FileSystem.Test;

/// <summary>
/// Provides methods to assert (sub-)directories in a specified file system.
/// </summary>
public class DirectoryAssert
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DirectoryAssert"/> class.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="fullPath">The path to the directory to assert.</param>
    public DirectoryAssert(AssertBase assert, IFileSystemService fileSystemService, string fullPath)
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
    /// Gets the path to the directory to assert.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets the creation time in local time of the current directory.
    /// </summary>
    public DateTime CreationTime => FileSystemService.Directory.GetCreationTime(FullPath);

    /// <summary>
    /// Gets the creation time in UTC time of the current directory.
    /// </summary>
    public DateTime CreationTimeUtc => FileSystemService.Directory.GetCreationTimeUtc(FullPath);

    /// <summary>
    /// Gets the last write time in local time of the current directory.
    /// </summary>
    public DateTime LastWriteTime => FileSystemService.Directory.GetLastWriteTime(FullPath);

    /// <summary>
    /// Gets the last write time in UTC time of the current directory.
    /// </summary>
    public DateTime LastWriteTimeUtc => FileSystemService.Directory.GetLastWriteTimeUtc(FullPath);

    /// <summary>
    /// Gets the last access time in local time of the current directory.
    /// </summary>
    public DateTime LastAccessTime => FileSystemService.Directory.GetLastAccessTime(FullPath);

    /// <summary>
    /// Gets the last access time in UTC time of the current directory.
    /// </summary>
    public DateTime LastAccessTimeUtc => FileSystemService.Directory.GetLastAccessTimeUtc(FullPath);

    /// <summary>
    /// Gets the file attributes of the current directory.
    /// </summary>
    public FileAttributes Attributes => FileSystemService.GetDirectoryInfo(FullPath).Attributes;

    /// <summary>
    /// Asserts a specified sub directory.
    /// </summary>
    /// <param name="directoryPath">The path of the sub directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the sub directory.</param>
    public void Directory(string directoryPath, Action<DirectoryAssert> assertions)
        => Assert.Directory(FileSystemService, GetFullPath(directoryPath), assertions);

    /// <summary>
    /// Asserts a specified sub directory.
    /// </summary>
    /// <param name="directoryPath">The path of the sub directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the sub directory.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DirectoryAsync(string directoryPath, Func<DirectoryAssert, Task> assertions)
        => await Assert.DirectoryAsync(FileSystemService, GetFullPath(directoryPath), assertions);

    /// <summary>
    /// Asserts a specified sub directory.
    /// </summary>
    /// <param name="directoryPath">The path of the sub directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the sub directory.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DirectoryAsync(string directoryPath, Func<DirectoryAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
        => await Assert.DirectoryAsync(FileSystemService, GetFullPath(directoryPath), assertions, cancellationToken);

    /// <summary>
    /// Asserts that a specified sub directory exists.
    /// </summary>
    /// <param name="expectedDirectoryPath">The path to the sub directory that is expected to exist.</param>
    public void DirectoryExists(string expectedDirectoryPath)
        => Assert.DirectoryExists(FileSystemService, GetFullPath(expectedDirectoryPath));

    /// <summary>
    /// Asserts that a specified sub directory exists.
    /// </summary>
    /// <param name="expectedDirectoryPath">The path to the sub directory that is expected to exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expectedDirectoryPath"/> does not exist. The message is shown in test results.</param>
    public void DirectoryExists(string expectedDirectoryPath, string? message)
        => Assert.DirectoryExists(FileSystemService, GetFullPath(expectedDirectoryPath), message);

    /// <summary>
    /// Asserts that a specified sub directory does not exist.
    /// </summary>
    /// <param name="unexpectedDirectoryPath">The path to the sub directory that is expected to not exist.</param>
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath)
        => Assert.DirectoryDoesNotExist(FileSystemService, GetFullPath(unexpectedDirectoryPath));

    /// <summary>
    /// Asserts that a specified sub directory does not exist.
    /// </summary>
    /// <param name="unexpectedDirectoryPath">The path to the sub directory that is expected to not exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="unexpectedDirectoryPath"/> exists. The message is shown in test results.</param>
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath, string? message)
        => Assert.DirectoryDoesNotExist(FileSystemService, GetFullPath(unexpectedDirectoryPath), message);

    /// <summary>
    /// Asserts a specified file in the current directory.
    /// </summary>
    /// <param name="filePath">The relative path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    public void File(string filePath, Action<FileAssert> assertions)
        => Assert.File(FileSystemService, GetFullPath(filePath), assertions);

    /// <summary>
    /// Asserts a specified file in the current directory.
    /// </summary>
    /// <param name="filePath">The relative path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task FileAsync(string filePath, Func<FileAssert, Task> assertions)
        => await Assert.FileAsync(FileSystemService, GetFullPath(filePath), assertions);

    /// <summary>
    /// Asserts a specified file in the current directory.
    /// </summary>
    /// <param name="filePath">The relative path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task FileAsync(string filePath, Func<FileAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
        => await Assert.FileAsync(FileSystemService, GetFullPath(filePath), assertions, cancellationToken);

    /// <summary>
    /// Asserts that a specified file exists in the current directory.
    /// </summary>
    /// <param name="expectedFilePath">The relative path to the file that is expected to exist.</param>
    public void FileExists(string expectedFilePath)
        => Assert.FileExists(FileSystemService, GetFullPath(expectedFilePath));

    /// <summary>
    /// Asserts that a specified file exists in the current directory.
    /// </summary>
    /// <param name="expectedFilePath">The relative path to the file that is expected to exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expectedFilePath"/> does not exist. The message is shown in test results.</param>
    public void FileExists(string expectedFilePath, string? message)
        => Assert.FileExists(FileSystemService, GetFullPath(expectedFilePath), message);

    /// <summary>
    /// Asserts that a specified file does not exist in the current directory.
    /// </summary>
    /// <param name="unexpectedFilePath">The relative path to the file that is expected to not exist.</param>
    public void FileDoesNotExist(string unexpectedFilePath)
        => Assert.FileDoesNotExist(FileSystemService, GetFullPath(unexpectedFilePath));

    /// <summary>
    /// Asserts that a specified file does not exist in the current directory.
    /// </summary>
    /// <param name="unexpectedFilePath">The relative path to the file that is expected to not exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="unexpectedFilePath"/> exists. The message is shown in test results.</param>
    public void FileDoesNotExist(string unexpectedFilePath, string? message)
        => Assert.FileDoesNotExist(FileSystemService, GetFullPath(unexpectedFilePath), message);

    /// <summary>
    /// Asserts that the current directory has specified file attributes.
    /// </summary>
    /// <param name="expectedAttributes">The file attributes the directory is to be expected to have.</param>
    public void HasAttributes(FileAttributes expectedAttributes)
    {
        HasAttributes(expectedAttributes, null);
    }

    /// <summary>
    /// Asserts that the current directory has specified file attributes.
    /// </summary>
    /// <param name="expectedAttributes">The file attributes the directory is to be expected to have.</param>
    /// <param name="message">The message to include in the exception when the current directory does not have <paramref name="expectedAttributes"/>. The message is shown in test results.</param>
    public void HasAttributes(FileAttributes expectedAttributes, string? message)
    {
        var actualAttributes = FileSystemService.GetDirectoryInfo(FullPath).Attributes;
        if ((actualAttributes & expectedAttributes) != expectedAttributes)
            ThrowAssertError(message, ("ExpectedAttributes", expectedAttributes), ("ActualAttributes", actualAttributes));
    }

    /// <summary>
    /// Asserts that the current directory does not have specified file attributes.
    /// </summary>
    /// <param name="unexpectedAttributes">The file attributes the directory is to be expected to not have.</param>
    public void DoesNotHaveAttributes(FileAttributes unexpectedAttributes)
    {
        DoesNotHaveAttributes(unexpectedAttributes, null);
    }

    /// <summary>
    /// Asserts that the current directory does not have specified file attributes.
    /// </summary>
    /// <param name="unexpectedAttributes">The file attributes the directory is to be expected to not have.</param>
    /// <param name="message">The message to include in the exception when the current directory has <paramref name="unexpectedAttributes"/>. The message is shown in test results.</param>
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
