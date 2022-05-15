namespace MaSch.FileSystem.Test;

/// <summary>
/// Provides methods to assert a file system.
/// </summary>
public static class FileSystemAssertions
{
    /// <summary>
    /// Assets a specified file system.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="assertions">An action that is used to assert the file system.</param>
    public static void FileSystem(this AssertBase assert, IFileSystemService fileSystemService, Action<FileSystemAssert> assertions)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        assertions.Invoke(new FileSystemAssert(assert, fileSystemService));
    }

    /// <summary>
    /// Assets a specified file system.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="assertions">An action that is used to assert the file system.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task FileSystemAsync(this AssertBase assert, IFileSystemService fileSystemService, Func<FileSystemAssert, Task> assertions)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        await assertions.Invoke(new FileSystemAssert(assert, fileSystemService));
    }

    /// <summary>
    /// Assets a specified file system.
    /// </summary>
    /// <param name="assert">The assert object to use for assertions.</param>
    /// <param name="fileSystemService">The file system to assert.</param>
    /// <param name="assertions">An action that is used to assert the file system.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task FileSystemAsync(this AssertBase assert, IFileSystemService fileSystemService, Func<FileSystemAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        await assertions.Invoke(new FileSystemAssert(assert, fileSystemService), cancellationToken);
    }

    internal static void EnsureRootedPath(string path)
    {
        if (!Path.IsPathRooted(path))
            throw new ArgumentException($"The path \"{path}\" is not rooted. Relative paths are not supported.", nameof(path));
    }
}
