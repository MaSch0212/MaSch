using static MaSch.FileSystem.Test.FileSystemAssertions;

namespace MaSch.FileSystem.Test;

public static class FileAssertions
{
    public static void File(this AssertBase assert, IFileSystemService fileSystemService, string filePath, Action<FileAssert> assertions)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(filePath);
        if (!fileSystemService.File.Exists(filePath))
            assert.ThrowAssertError("The file does not exist.", ("ExpectedFilePath", filePath));
        assertions.Invoke(new FileAssert(assert, fileSystemService, filePath));
    }

    public static async Task FileAsync(this AssertBase assert, IFileSystemService fileSystemService, string filePath, Func<FileAssert, Task> assertions)
    {
        await FileAsync(assert, fileSystemService, filePath, (a, c) => assertions(a), CancellationToken.None);
    }

    public static async Task FileAsync(this AssertBase assert, IFileSystemService fileSystemService, string filePath, Func<FileAssert, CancellationToken, Task> assertions, CancellationToken cancellationToken)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        EnsureRootedPath(filePath);
        if (!fileSystemService.File.Exists(filePath))
            assert.ThrowAssertError("The file does not exist.", ("ExpectedFilePath", filePath));
        await assertions.Invoke(new FileAssert(assert, fileSystemService, filePath), cancellationToken);
    }

    public static void FileExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedFilePath)
    {
        FileExists(assert, fileSystemService, expectedFilePath, null);
    }

    public static void FileExists(this AssertBase assert, IFileSystemService fileSystemService, string expectedFilePath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(expectedFilePath);
        EnsureRootedPath(expectedFilePath);
        if (!fileSystemService.File.Exists(expectedFilePath))
            assert.ThrowAssertError(message, ("ExpectedFilePath", expectedFilePath));
    }

    public static void FileDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedFilePath)
    {
        FileDoesNotExist(assert, fileSystemService, unexpectedFilePath, null);
    }

    public static void FileDoesNotExist(this AssertBase assert, IFileSystemService fileSystemService, string unexpectedFilePath, string? message)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(unexpectedFilePath);
        EnsureRootedPath(unexpectedFilePath);
        if (fileSystemService.File.Exists(unexpectedFilePath))
            assert.ThrowAssertError(message, ("UnexpectedFilePath", unexpectedFilePath));
    }
}
