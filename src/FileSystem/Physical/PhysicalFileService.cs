namespace MaSch.FileSystem.Physical;

internal class PhysicalFileService : FileServiceBase
{
    public PhysicalFileService(IFileSystemService fileSystem)
        : base(fileSystem)
    {
    }

    /// <inheritdoc/>
    public override void AppendAllLines(string path, IEnumerable<string> contents)
    {
        File.AppendAllLines(path, contents);
    }

    /// <inheritdoc/>
    public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
        File.AppendAllLines(path, contents, encoding);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        return File.AppendAllLinesAsync(path, contents, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.AppendAllLinesAsync(path, contents, encoding, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    public override void AppendAllText(string path, string? contents)
    {
        File.AppendAllText(path, contents);
    }

    /// <inheritdoc/>
    public override void AppendAllText(string path, string? contents, Encoding encoding)
    {
        File.AppendAllText(path, contents, encoding);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task AppendAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
    {
        return File.AppendAllTextAsync(path, contents, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task AppendAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.AppendAllTextAsync(path, contents, encoding, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    public override StreamWriter AppendText(string path)
    {
        return File.AppendText(path);
    }

    /// <inheritdoc/>
    public override void Copy(string sourceFileName, string destFileName, bool overwrite)
    {
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    /// <inheritdoc/>
    public override void Copy(string sourceFileName, string destFileName)
    {
        File.Copy(sourceFileName, destFileName);
    }

    /// <inheritdoc/>
    public override Stream Create(string path)
    {
        return File.Create(path);
    }

    /// <inheritdoc/>
    public override Stream Create(string path, int bufferSize)
    {
        return File.Create(path, bufferSize);
    }

    /// <inheritdoc/>
    public override Stream Create(string path, int bufferSize, FileOptions options)
    {
        return File.Create(path, bufferSize, options);
    }

    /// <inheritdoc/>
    public override StreamWriter CreateText(string path)
    {
        return File.CreateText(path);
    }

    /// <inheritdoc/>
    public override void Delete(string path)
    {
        File.Delete(path);
    }

    /// <inheritdoc/>
    public override bool Exists([NotNullWhen(true)] string? path)
    {
        return File.Exists(path);
    }

    /// <inheritdoc/>
    public override FileAttributes GetAttributes(string path)
    {
        return File.GetAttributes(path);
    }

    /// <inheritdoc/>
    public override DateTime GetCreationTime(string path)
    {
        return File.GetCreationTime(path);
    }

    /// <inheritdoc/>
    public override DateTime GetCreationTimeUtc(string path)
    {
        return File.GetCreationTimeUtc(path);
    }

    public override IFileInfo GetInfo(string path)
    {
        return new PhysicalFileInfo(FileSystem, new FileInfo(path));
    }

    /// <inheritdoc/>
    public override DateTime GetLastAccessTime(string path)
    {
        return File.GetLastAccessTime(path);
    }

    /// <inheritdoc/>
    public override DateTime GetLastAccessTimeUtc(string path)
    {
        return File.GetLastAccessTimeUtc(path);
    }

    /// <inheritdoc/>
    public override DateTime GetLastWriteTime(string path)
    {
        return File.GetLastWriteTime(path);
    }

    /// <inheritdoc/>
    public override DateTime GetLastWriteTimeUtc(string path)
    {
        return File.GetLastWriteTimeUtc(path);
    }

    /// <inheritdoc/>
    public override void Move(string sourceFileName, string destFileName, bool overwrite)
    {
#if NET5_0_OR_GREATER
        File.Move(sourceFileName, destFileName, overwrite);
#else
        if (overwrite && File.Exists(destFileName))
            File.Delete(destFileName);
        File.Move(sourceFileName, destFileName);
#endif
    }

    /// <inheritdoc/>
    public override void Move(string sourceFileName, string destFileName)
    {
        base.Move(sourceFileName, destFileName);
    }

    /// <inheritdoc/>
    public override Stream Open(string path, FileStreamOptions options)
    {
#if NET6_0_OR_GREATER
        return File.Open(path, options);
#else
        return new FileStream(path, options.Mode, options.Access, options.Share, options.BufferSize, options.Options);
#endif
    }

    /// <inheritdoc/>
    public override Stream Open(string path, FileMode mode)
    {
        return File.Open(path, mode);
    }

    /// <inheritdoc/>
    public override Stream Open(string path, FileMode mode, FileAccess access)
    {
        return File.Open(path, mode, access);
    }

    /// <inheritdoc/>
    public override Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
    {
        return File.Open(path, mode, access, share);
    }

    /// <inheritdoc/>
    public override Stream OpenRead(string path)
    {
        return File.OpenRead(path);
    }

    /// <inheritdoc/>
    public override StreamReader OpenText(string path)
    {
        return File.OpenText(path);
    }

    /// <inheritdoc/>
    public override Stream OpenWrite(string path)
    {
        return File.OpenWrite(path);
    }

    /// <inheritdoc/>
    public override byte[] ReadAllBytes(string path)
    {
        return File.ReadAllBytes(path);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
    {
        return File.ReadAllBytesAsync(path, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    public override string[] ReadAllLines(string path)
    {
        return File.ReadAllLines(path);
    }

    /// <inheritdoc/>
    public override string[] ReadAllLines(string path, Encoding encoding)
    {
        return File.ReadAllLines(path, encoding);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
    {
        return File.ReadAllLinesAsync(path, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.ReadAllLinesAsync(path, encoding, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    public override string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    /// <inheritdoc/>
    public override string ReadAllText(string path, Encoding encoding)
    {
        return File.ReadAllText(path, encoding);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
    {
        return File.ReadAllTextAsync(path, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.ReadAllTextAsync(path, encoding, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    public override IEnumerable<string> ReadLines(string path)
    {
        return File.ReadLines(path);
    }

    /// <inheritdoc/>
    public override IEnumerable<string> ReadLines(string path, Encoding encoding)
    {
        return File.ReadLines(path, encoding);
    }

    /// <inheritdoc/>
    public override void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName)
    {
        File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
    }

    /// <inheritdoc/>
    public override void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
    {
        File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
    }

    /// <inheritdoc/>
    public override void SetAttributes(string path, FileAttributes fileAttributes)
    {
        File.SetAttributes(path, fileAttributes);
    }

    /// <inheritdoc/>
    public override void SetCreationTime(string path, DateTime creationTime)
    {
        File.SetCreationTime(path, creationTime);
    }

    /// <inheritdoc/>
    public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
    {
        File.SetCreationTimeUtc(path, creationTimeUtc);
    }

    /// <inheritdoc/>
    public override void SetLastAccessTime(string path, DateTime lastAccessTime)
    {
        File.SetLastAccessTime(path, lastAccessTime);
    }

    /// <inheritdoc/>
    public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
    {
        File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
    }

    /// <inheritdoc/>
    public override void SetLastWriteTime(string path, DateTime lastWriteTime)
    {
        File.SetLastWriteTime(path, lastWriteTime);
    }

    /// <inheritdoc/>
    public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
    {
        File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
    }

    /// <inheritdoc/>
    public override void WriteAllBytes(string path, byte[] bytes)
    {
        File.WriteAllBytes(path, bytes);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
    {
        return File.WriteAllBytesAsync(path, bytes, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    public override void WriteAllLines(string path, string[] contents)
    {
        File.WriteAllLines(path, contents);
    }

    /// <inheritdoc/>
    public override void WriteAllLines(string path, IEnumerable<string> contents)
    {
        File.WriteAllLines(path, contents);
    }

    /// <inheritdoc/>
    public override void WriteAllLines(string path, string[] contents, Encoding encoding)
    {
        File.WriteAllLines(path, contents, encoding);
    }

    /// <inheritdoc/>
    public override void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
        File.WriteAllLines(path, contents, encoding);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        return File.WriteAllLinesAsync(path, contents, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.WriteAllLinesAsync(path, contents, encoding, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    public override void WriteAllText(string path, string? contents)
    {
        File.WriteAllText(path, contents);
    }

    /// <inheritdoc/>
    public override void WriteAllText(string path, string? contents, Encoding encoding)
    {
        File.WriteAllText(path, contents, encoding);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public override Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(path, contents, cancellationToken);
    }

    /// <inheritdoc/>
    public override Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        return File.WriteAllTextAsync(path, contents, encoding, cancellationToken);
    }
#endif

    /// <inheritdoc/>
    protected override Stream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
    {
        return new FileStream(path, mode, access, share, bufferSize, options);
    }
}
