namespace MaSch.FileSystem;

public interface IFileSystemInfo
{
    IFileSystemService FileSystem { get; }

    string FullName { get; }
    string Extension { get; }
    string Name { get; }
    bool Exists { get; }
    DateTime CreationTime { get; set; }
    DateTime CreationTimeUtc { get; set; }
    DateTime LastAccessTime { get; set; }
    DateTime LastAccessTimeUtc { get; set; }
    DateTime LastWriteTime { get; set; }
    DateTime LastWriteTimeUtc { get; set; }
    FileAttributes Attributes { get; set; }

    void Delete();
    void Refresh();
}
