using static MaSch.FileSystem.Test.FileSystemAssertions;

namespace MaSch.FileSystem.Test;

/// <summary>
/// Provides methods to assert files in a file system.
/// </summary>
public static class FileAssertions
{
    /// <summary>
    /// Asserts a specified file.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="filePath">The absolute path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    public static void File(this AssertBase assert, IFileSystemService fileSystemService, string filePath, Action<FileAssert> assertions)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(filePath);
        if (!fileSystemService.File.Exists(filePath))
            assert.ThrowAssertError("The file does not exist.", ("ExpectedFilePath", filePath));
        assertions.Invoke(new FileAssert(assert, fileSystemService, filePath));
    }

    /// <summary>
    /// Asserts a specified file.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="filePath">The absolute path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task FileAsync(this AssertBase assert, IFileSystemService fileSystemService, string filePath, Func<FileAssert, Task> assertions)
    {
        await FileAsync(assert, fileSystemService, filePath, (a, c) => assertions(a), CancellationToken.None);
    }

    /// <summary>
    /// Asserts a specified file.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="filePath">The absolute path of the file to assert.</param>
    /// <param name="assertions">An action that is used to assert the file.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task FileAsync(this AssertBase assert, IFileSystemService fileSystemService, string filePath, Func<FileAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(filePath);
        if (!fileSystemService.File.Exists(filePath))
            assert.ThrowAssertError("The file does not exist.", ("ExpectedFilePath", filePath));
        await assertions.Invoke(new FileAssert(assert, fileSystemService, filePath), cancellationToken);
    }

    /// <summary>
    /// Asserts that a specified file exists.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="expectedFilePath">The absolute path to the file that is expected to exist.</param>
    public static void FileExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedFilePath)
    {
        FileExists(assert, fileSystemService, expectedFilePath, null);
    }

    /// <summary>
    /// Asserts that a specified file exists.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="expectedFilePath">The absolute path to the file that is expected to exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expectedFilePath"/> does not exist. The message is shown in test results.</param>
    public static void FileExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedFilePath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(expectedFilePath);
        EnsureRootedPath(expectedFilePath);
        if (!fileSystemService.File.Exists(expectedFilePath))
            assert.ThrowAssertError(message, ("ExpectedFilePath", expectedFilePath));
    }

    /// <summary>
    /// Asserts that a specified file does not exist.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="unexpectedFilePath">The absolute path to the file that is expected to not exist.</param>
    public static void FileDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedFilePath)
    {
        FileDoesNotExist(assert, fileSystemService, unexpectedFilePath, null);
    }

    /// <summary>
    /// Asserts that a specified file does not exist.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="unexpectedFilePath">The absolute path to the file that is expected to not exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="unexpectedFilePath"/> exists. The message is shown in test results.</param>
    public static void FileDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedFilePath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(unexpectedFilePath);
        EnsureRootedPath(unexpectedFilePath);
        if (fileSystemService.File.Exists(unexpectedFilePath))
            assert.ThrowAssertError(message, ("UnexpectedFilePath", unexpectedFilePath));
    }
}
