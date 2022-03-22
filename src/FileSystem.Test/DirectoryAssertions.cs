using static MaSch.FileSystem.Test.FileSystemAssertions;

namespace MaSch.FileSystem.Test;

public static class DirectoryAssertions
{
    public static void Directory(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, Action<DirectoryAssert> assertions)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(directoryPath);
        if (!fileSystemService.Directory.Exists(directoryPath))
            assert.ThrowAssertError("The directory does not exist.", ("ExpectedDirectoryPath", directoryPath));
        assertions.Invoke(new DirectoryAssert(assert, fileSystemService, directoryPath));
    }

    public static async Task DirectoryAsync(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, Func<DirectoryAssert, Task> assertions)
    {
        await DirectoryAsync(assert, fileSystemService, directoryPath, (a, c) => assertions(a), CancellationToken.None);
    }

    public static async Task DirectoryAsync(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, Func<DirectoryAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(directoryPath);
        if (!fileSystemService.Directory.Exists(directoryPath))
            assert.ThrowAssertError("The directory does not exist.", ("ExpectedDirectoryPath", directoryPath));
        await assertions.Invoke(new DirectoryAssert(assert, fileSystemService, directoryPath), cancellationToken);
    }

    public static void DirectoryExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedDirectoryPath)
    {
        DirectoryExists(assert, fileSystemService, expectedDirectoryPath, null);
    }

    public static void DirectoryExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedDirectoryPath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(expectedDirectoryPath);
        EnsureRootedPath(expectedDirectoryPath);
        if (!fileSystemService.Directory.Exists(expectedDirectoryPath))
            assert.ThrowAssertError(message, ("ExpectedDirectoryPath", expectedDirectoryPath));
    }

    public static void DirectoryDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedDirectoryPath)
    {
        DirectoryDoesNotExist(assert, fileSystemService, unexpectedDirectoryPath, null);
    }

    public static void DirectoryDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedDirectoryPath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(unexpectedDirectoryPath);
        EnsureRootedPath(unexpectedDirectoryPath);
        if (fileSystemService.Directory.Exists(unexpectedDirectoryPath))
            assert.ThrowAssertError(message, ("UnexpectedDirectoryPath", unexpectedDirectoryPath));
    }

    public static void DirectoryHasAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes expectedAttributes)
    {
        DirectoryHasAttributes(assert, fileSystemService, directoryPath, expectedAttributes, null);
    }

    public static void DirectoryHasAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes expectedAttributes, string? message)
    {
        Guard.NotNull(fileSystemService);
        EnsureRootedPath(directoryPath);
        var actualAttributes = fileSystemService.GetDirectoryInfo(directoryPath).Attributes;
        if ((actualAttributes & expectedAttributes) != expectedAttributes)
            assert.ThrowAssertError(message, ("ExpectedAttributes", expectedAttributes), ("ActualAttributes", actualAttributes));
    }

    public static void DirectoryDoesNotHaveAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes unexpectedAttributes)
    {
        DirectoryDoesNotHaveAttributes(assert, fileSystemService, directoryPath, unexpectedAttributes, null);
    }

    public static void DirectoryDoesNotHaveAttributes(this AssertBase assert, IFileSystemService fileSystemService, string directoryPath, FileAttributes unexpectedAttributes, string? message)
    {
        Guard.NotNull(fileSystemService);
        EnsureRootedPath(directoryPath);
        var actualAttributes = fileSystemService.GetDirectoryInfo(directoryPath).Attributes;
        if ((actualAttributes & unexpectedAttributes) != 0)
            assert.ThrowAssertError(message, ("UnexpectedAttributes", unexpectedAttributes), ("ActualAttributes", actualAttributes));
    }
}
