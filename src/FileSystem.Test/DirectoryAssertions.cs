using static MaSch.FileSystem.Test.FileSystemAssertions;

namespace MaSch.FileSystem.Test;

/// <summary>
/// Provides methods to assert directories in a file system.
/// </summary>
public static class DirectoryAssertions
{
    /// <summary>
    /// Asserts a specified directory.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="directoryPath">The path of the directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the sub directory.</param>
    public static void Directory(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, Action<DirectoryAssert> assertions)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(directoryPath);
        if (!fileSystemService.Directory.Exists(directoryPath))
            assert.ThrowAssertError("The directory does not exist.", ("ExpectedDirectoryPath", directoryPath));
        assertions.Invoke(new DirectoryAssert(assert, fileSystemService, directoryPath));
    }

    /// <summary>
    /// Asserts a specified directory.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="directoryPath">The path of the directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the sub directory.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task DirectoryAsync(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, Func<DirectoryAssert, Task> assertions)
    {
        await DirectoryAsync(assert, fileSystemService, directoryPath, (a, c) => assertions(a), CancellationToken.None);
    }

    /// <summary>
    /// Asserts a specified directory.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="directoryPath">The path of the directory to assert.</param>
    /// <param name="assertions">An action that is used to assert the sub directory.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task DirectoryAsync(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, Func<DirectoryAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(directoryPath);
        if (!fileSystemService.Directory.Exists(directoryPath))
            assert.ThrowAssertError("The directory does not exist.", ("ExpectedDirectoryPath", directoryPath));
        await assertions.Invoke(new DirectoryAssert(assert, fileSystemService, directoryPath), cancellationToken);
    }

    /// <summary>
    /// Asserts that a specified directory exists.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="expectedDirectoryPath">The path to the directory that is expected to exist.</param>
    public static void DirectoryExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedDirectoryPath)
    {
        DirectoryExists(assert, fileSystemService, expectedDirectoryPath, null);
    }

    /// <summary>
    /// Asserts that a specified sub directory exists.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="expectedDirectoryPath">The path to the directory that is expected to exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expectedDirectoryPath"/> does not exist. The message is shown in test results.</param>
    public static void DirectoryExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedDirectoryPath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(expectedDirectoryPath);
        EnsureRootedPath(expectedDirectoryPath);
        if (!fileSystemService.Directory.Exists(expectedDirectoryPath))
            assert.ThrowAssertError(message, ("ExpectedDirectoryPath", expectedDirectoryPath));
    }

    /// <summary>
    /// Asserts that a specified sub directory does not exist.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="unexpectedDirectoryPath">The path to the directory that is expected to not exist.</param>
    public static void DirectoryDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedDirectoryPath)
    {
        DirectoryDoesNotExist(assert, fileSystemService, unexpectedDirectoryPath, null);
    }

    /// <summary>
    /// Asserts that a specified sub directory does not exist.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="unexpectedDirectoryPath">The path to the directory that is expected to not exist.</param>
    /// <param name="message">The message to include in the exception when <paramref name="unexpectedDirectoryPath"/> exists. The message is shown in test results.</param>
    public static void DirectoryDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedDirectoryPath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(unexpectedDirectoryPath);
        EnsureRootedPath(unexpectedDirectoryPath);
        if (fileSystemService.Directory.Exists(unexpectedDirectoryPath))
            assert.ThrowAssertError(message, ("UnexpectedDirectoryPath", unexpectedDirectoryPath));
    }

    /// <summary>
    /// Asserts that a specified directory has specified file attributes.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="directoryPath">The path to the directory to assert.</param>
    /// <param name="expectedAttributes">The file attributes the directory is to be expected to have.</param>
    public static void DirectoryHasAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes expectedAttributes)
    {
        DirectoryHasAttributes(assert, fileSystemService, directoryPath, expectedAttributes, null);
    }

    /// <summary>
    /// Asserts that a specified directory has specified file attributes.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="directoryPath">The path to the directory to assert.</param>
    /// <param name="expectedAttributes">The file attributes the directory is to be expected to have.</param>
    /// <param name="message">The message to include in the exception when the directory does not have <paramref name="expectedAttributes"/>. The message is shown in test results.</param>
    public static void DirectoryHasAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes expectedAttributes, string? message)
    {
        Guard.NotNull(fileSystemService);
        EnsureRootedPath(directoryPath);
        var actualAttributes = fileSystemService.GetDirectoryInfo(directoryPath).Attributes;
        if ((actualAttributes & expectedAttributes) != expectedAttributes)
            assert.ThrowAssertError(message, ("ExpectedAttributes", expectedAttributes), ("ActualAttributes", actualAttributes));
    }

    /// <summary>
    /// Asserts that a specified directory does not have specified file attributes.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="directoryPath">The path to the directory to assert.</param>
    /// <param name="unexpectedAttributes">The file attributes the directory is to be expected to not have.</param>
    public static void DirectoryDoesNotHaveAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes unexpectedAttributes)
    {
        DirectoryDoesNotHaveAttributes(assert, fileSystemService, directoryPath, unexpectedAttributes, null);
    }

    /// <summary>
    /// Asserts that a specified directory does not have specified file attributes.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="directoryPath">The path to the directory to assert.</param>
    /// <param name="unexpectedAttributes">The file attributes the directory is to be expected to not have.</param>
    /// <param name="message">The message to include in the exception when the directory has <paramref name="unexpectedAttributes"/>. The message is shown in test results.</param>
    public static void DirectoryDoesNotHaveAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes unexpectedAttributes, string? message)
    {
        Guard.NotNull(fileSystemService);
        EnsureRootedPath(directoryPath);
        var actualAttributes = fileSystemService.GetDirectoryInfo(directoryPath).Attributes;
        if ((actualAttributes & unexpectedAttributes) != 0)
            assert.ThrowAssertError(message, ("UnexpectedAttributes", unexpectedAttributes), ("ActualAttributes", actualAttributes));
    }
}
