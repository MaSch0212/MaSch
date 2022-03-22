namespace MaSch.FileSystem.Test;

public static class FileSystemAssertions
{
    public static void FileSystem(this AssertBase assert, IFileSystemService fileSystemService, Action<FileSystemAssert> assertions)
    {
        Guard.NotNull(fileSystemService);
        Guard.NotNull(assertions);
        assertions.Invoke(new FileSystemAssert(assert, fileSystemService));
    }

    internal static void EnsureRootedPath(string path)
    {
        if (!Path.IsPathRooted(path))
            throw new ArgumentException($"The path \"{path}\" is not rooted. Relative paths are not supported.", nameof(path));
    }
}
