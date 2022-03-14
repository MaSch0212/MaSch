#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem;

public interface IFileService
{
    IFileSystemService FileSystem { get; }

    IFileInfo GetInfo(string path);
    IDirectoryInfo? GetDirectory(string path);

    void AppendAllLines(string path, IEnumerable<string> contents);
    void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);
    Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);
    Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);

    void AppendAllText(string path, string? contents);
    void AppendAllText(string path, string? contents, Encoding encoding);
    Task AppendAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default);
    Task AppendAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default);

    StreamWriter AppendText(string path);

    void Copy(string sourceFileName, string destFileName);
    void Copy(string sourceFileName, string destFileName, bool overwrite);

    Stream Create(string path);
    Stream Create(string path, int bufferSize);
    Stream Create(string path, int bufferSize, FileOptions options);
    StreamWriter CreateText(string path);

    void Delete(string path);

    bool Exists([NotNullWhen(true)] string? path);

    FileAttributes GetAttributes(string path);
    DateTime GetCreationTime(string path);
    DateTime GetCreationTimeUtc(string path);
    DateTime GetLastAccessTime(string path);
    DateTime GetLastAccessTimeUtc(string path);
    DateTime GetLastWriteTime(string path);
    DateTime GetLastWriteTimeUtc(string path);

    void Move(string sourceFileName, string destFileName);
    void Move(string sourceFileName, string destFileName, bool overwrite);

    Stream Open(string path, FileMode mode);
    Stream Open(string path, FileMode mode, FileAccess access);
    Stream Open(string path, FileMode mode, FileAccess access, FileShare share);
    Stream Open(string path, FileStreamOptions options);
    Stream OpenRead(string path);
    StreamReader OpenText(string path);
    Stream OpenWrite(string path);

    byte[] ReadAllBytes(string path);
    Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default);

    string[] ReadAllLines(string path);
    string[] ReadAllLines(string path, Encoding encoding);
    Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default);
    Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

    string ReadAllText(string path);
    string ReadAllText(string path, Encoding encoding);
    Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);
    Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

    IEnumerable<string> ReadLines(string path);
    IEnumerable<string> ReadLines(string path, Encoding encoding);

    void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName);
    void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors);

    void SetAttributes(string path, FileAttributes fileAttributes);
    void SetCreationTime(string path, DateTime creationTime);
    void SetCreationTimeUtc(string path, DateTime creationTimeUtc);
    void SetLastAccessTime(string path, DateTime lastAccessTime);
    void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);
    void SetLastWriteTime(string path, DateTime lastWriteTime);
    void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

    void WriteAllBytes(string path, byte[] bytes);
    Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default);

    void WriteAllLines(string path, string[] contents);
    void WriteAllLines(string path, IEnumerable<string> contents);
    void WriteAllLines(string path, string[] contents, Encoding encoding);
    void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);
    Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);
    Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);

    void WriteAllText(string path, string? contents);
    void WriteAllText(string path, string? contents, Encoding encoding);
    Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default);
    Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default);

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    Task AppendAllLinesAsync(string path, IAsyncEnumerable<string> contents, CancellationToken cancellationToken = default);
    Task AppendAllLinesAsync(string path, IAsyncEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);
    IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default);
    IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);
    Task WriteAllLinesAsync(string path, IAsyncEnumerable<string> contents, CancellationToken cancellationToken = default);
    Task WriteAllLinesAsync(string path, IAsyncEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);
#endif
}
