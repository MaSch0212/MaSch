namespace MaSch.FileSystem.Test;

/// <summary>
/// Provides methods to assert a specified file system.
/// </summary>
public class FileSystemAssert
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemAssert"/> class.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    public FileSystemAssert(AssertBase assert, IFileSystemService fileSystemService)
    {
        Assert = Guard.NotNull(assert);
        FileSystemService = Guard.NotNull(fileSystemService);
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
    /// Asserts a specified directory.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the directory.</param>
    public void Directory(string directoryPath, Action<DirectoryAssert> assertions)
        => Assert.Directory(FileSystemService, directoryPath, assertions);

    /// <summary>
    /// Asserts a specified directory.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the directory.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DirectoryAsync(string directoryPath, Func<DirectoryAssert, Task> assertions)
        => await Assert.DirectoryAsync(FileSystemService, directoryPath, assertions);

    /// <summary>
    /// Asserts a specified directory.
    /// </summary>
    /// <param name="directoryPath">The path of the directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the directory.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task DirectoryAsync(string directoryPath, Func<DirectoryAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
        => await Assert.DirectoryAsync(FileSystemService, directoryPath, assertions, cancellationToken);

    /// <summary>
    /// Asserts that a specified directory exists.
    /// </summary>
    /// <param name="expectedDirectoryPath">The path to the directory that is expected to exist.</param>
    public void DirectoryExists(string expectedDirectoryPath)
        => Assert.DirectoryExists(FileSystemService, expectedDirectoryPath);

    /// <summary>
    /// Asserts that a specified directory exists.
    /// </summary>
    /// <param name="expectedDirectoryPath">The path to the directory that is expected to exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expectedDirectoryPath"/> does not exist. The message is shown in test results.</param>
    public void DirectoryExists(string expectedDirectoryPath, string? message)
        => Assert.DirectoryExists(FileSystemService, expectedDirectoryPath, message);

    /// <summary>
    /// Asserts that a specified directory does not exist.
    /// </summary>
    /// <param name="unexpectedDirectoryPath">The path to the directory that is expected to not exist.</param>
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath)
        => Assert.DirectoryDoesNotExist(FileSystemService, unexpectedDirectoryPath);

    /// <summary>
    /// Asserts that a specified directory does not exist.
    /// </summary>
    /// <param name="unexpectedDirectoryPath">The path to the directory that is expected to not exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="unexpectedDirectoryPath"/> exists. The message is shown in test results.</param>
    public void DirectoryDoesNotExist(string unexpectedDirectoryPath, string? message)
        => Assert.DirectoryDoesNotExist(FileSystemService, unexpectedDirectoryPath, message);

    /// <summary>
    /// Asserts a specified file.
    /// </summary>
    /// <param name="filePath">The absolute path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    public void File(string filePath, Action<FileAssert> assertions)
        => Assert.File(FileSystemService, filePath, assertions);

    /// <summary>
    /// Asserts a specified file.
    /// </summary>
    /// <param name="filePath">The absolute path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task FileAsync(string filePath, Func<FileAssert, Task> assertions)
        => await Assert.FileAsync(FileSystemService, filePath, assertions);

    /// <summary>
    /// Asserts a specified file.
    /// </summary>
    /// <param name="filePath">The absolute path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task FileAsync(string filePath, Func<FileAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
        => await Assert.FileAsync(FileSystemService, filePath, assertions, cancellationToken);

    /// <summary>
    /// Asserts that a specified file exists.
    /// </summary>
    /// <param name="expectedFilePath">The absolute path to the file that is expected to exist.</param>
    public void FileExists(string expectedFilePath)
        => Assert.FileExists(FileSystemService, expectedFilePath);

    /// <summary>
    /// Asserts that a specified file exists.
    /// </summary>
    /// <param name="expectedFilePath">The absolute path to the file that is expected to exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expectedFilePath"/> does not exist. The message is shown in test results.</param>
    public void FileExists(string expectedFilePath, string? message)
        => Assert.FileExists(FileSystemService, expectedFilePath, message);

    /// <summary>
    /// Asserts that a specified file does not exist.
    /// </summary>
    /// <param name="unexpectedFilePath">The absolute path to the file that is expected to not exist.</param>
    public void FileDoesNotExist(string unexpectedFilePath)
        => Assert.FileDoesNotExist(FileSystemService, unexpectedFilePath);

    /// <summary>
    /// Asserts that a specified file does not exist.
    /// </summary>
    /// <param name="unexpectedFilePath">The absolute path to the file that is expected to not exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="unexpectedFilePath"/> exists. The message is shown in test results.</param>
    public void FileDoesNotExist(string unexpectedFilePath, string? message)
        => Assert.FileDoesNotExist(FileSystemService, unexpectedFilePath, message);
}
