namespace MaSch.FileSystem;

public interface IFileInfo : IFileSystemInfo
{
    long Length { get; }
    string? DirectoryName { get; }
    IDirectoryInfo? Directory { get; }
    bool IsReadOnly { get; }

    StreamReader OpenText();
    StreamWriter CreateText();
    StreamWriter AppendText();
    IFileInfo CopyTo(string destFileName);
    IFileInfo CopyTo(string destFileName, bool overwrite);
    Stream Create();
    Stream Open(FileMode mode);
    Stream Open(FileMode mode, FileAccess access);
    Stream Open(FileMode mode, FileAccess access, FileShare share);
    Stream OpenRead();
    Stream OpenWrite();
    void MoveTo(string destFileName);
    void MoveTo(string destFileName, bool overwrite);
    IFileInfo Replace(string destinationFileName, string? destinationBackupFileName);
    IFileInfo Replace(string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors);
    Stream Open(FileStreamOptions options);
}
