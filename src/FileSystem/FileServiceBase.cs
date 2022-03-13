using MaSch.Core;
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
using System.Buffers;
#endif

namespace MaSch.FileSystem;

/// <summary>
/// Base class that can be used to more easily implement the <see cref="IFileService"/> class.
/// </summary>
public abstract class FileServiceBase : IFileService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileServiceBase"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system that is used by this <see cref="IFileService"/>.</param>
    protected FileServiceBase(IFileSystemService fileSystem)
    {
        FileSystem = Guard.NotNull(fileSystem);
    }

    /// <inheritdoc/>
    public IFileSystemService FileSystem { get; }

    /// <summary>
    /// Gets the default buffer size to use for file streams.
    /// </summary>
    protected virtual int DefaultBufferSize { get; } = 4096;

    /// <inheritdoc/>
    public abstract IFileInfo GetInfo(string path);

    /// <inheritdoc/>
    public virtual IDirectoryInfo? GetDirectory(string path)
    {
        Guard.NotNullOrEmpty(path);
        string fullPath = Path.GetFullPath(path);
        string? directoryName = Path.GetDirectoryName(fullPath);
        return directoryName == null ? null : FileSystem.GetDirectoryInfo(directoryName);
    }

    /// <inheritdoc/>
    public virtual void AppendAllLines(string path, IEnumerable<string> contents)
    {
        AppendAllLines(path, contents, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
        using var fileStream = Open(path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None);
        using var streamWriter = new StreamWriter(fileStream, encoding);
        foreach (var content in contents)
            streamWriter.WriteLine(content);
    }

    /// <inheritdoc/>
    public virtual async Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        await AppendAllLinesAsync(path, contents, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var streamWriter = new StreamWriter(fileStream);
        foreach (var content in contents)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await streamWriter.WriteLineAsync(content).ConfigureAwait(false);
        }

        cancellationToken.ThrowIfCancellationRequested();
        await streamWriter.FlushAsync().ConfigureAwait(false);
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public virtual async Task AppendAllLinesAsync(string path, IAsyncEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        await AppendAllLinesAsync(path, contents, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task AppendAllLinesAsync(string path, IAsyncEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var streamWriter = new StreamWriter(fileStream);
        await foreach (var content in contents.WithCancellation(cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await streamWriter.WriteLineAsync(content).ConfigureAwait(false);
        }

        cancellationToken.ThrowIfCancellationRequested();
        await streamWriter.FlushAsync().ConfigureAwait(false);
    }
#endif

    /// <inheritdoc/>
    public virtual void AppendAllText(string path, string? contents)
    {
        AppendAllText(path, contents, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual void AppendAllText(string path, string? contents, Encoding encoding)
    {
        using var fileStream = Open(path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None);
        using var streamWriter = new StreamWriter(fileStream, encoding);
        streamWriter.Write(contents);
    }

    /// <inheritdoc/>
    public virtual async Task AppendAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
    {
        await AppendAllTextAsync(path, contents, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task AppendAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(contents))
        {
            Open(path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None).Dispose();
            return;
        }

        using var fileStream = Open(path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var streamWriter = new StreamWriter(fileStream);
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await streamWriter.WriteAsync(contents.AsMemory(), cancellationToken).ConfigureAwait(false);
#else
        await streamWriter.WriteAsync(contents).ConfigureAwait(false);
#endif
        await streamWriter.FlushAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual StreamWriter AppendText(string path)
    {
        var fileStream = Open(path, FileMode.Append, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None);
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        return new StreamWriter(fileStream, leaveOpen: false);
#else
        return new StreamWriter(fileStream, Encoding.Default, 1024, leaveOpen: false);
#endif
    }

    /// <inheritdoc/>
    public virtual void Copy(string sourceFileName, string destFileName)
    {
        Copy(sourceFileName, destFileName, false);
    }

    /// <inheritdoc/>
    public abstract void Copy(string sourceFileName, string destFileName, bool overwrite);

    /// <inheritdoc/>
    public virtual Stream Create(string path)
    {
        return Create(path, DefaultBufferSize, FileOptions.None);
    }

    /// <inheritdoc/>
    public virtual Stream Create(string path, int bufferSize)
    {
        return Create(path, bufferSize, FileOptions.None);
    }

    /// <inheritdoc/>
    public virtual Stream Create(string path, int bufferSize, FileOptions options)
    {
        return Open(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, options);
    }

    /// <inheritdoc/>
    public virtual StreamWriter CreateText(string path)
    {
        var fileStream = Create(path, DefaultBufferSize, FileOptions.None);
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        return new StreamWriter(fileStream, leaveOpen: false);
#else
        return new StreamWriter(fileStream, Encoding.Default, 1024, leaveOpen: false);
#endif
    }

    /// <inheritdoc/>
    public abstract void Delete(string path);

    /// <inheritdoc/>
    public abstract bool Exists([NotNullWhen(true)] string? path);

    /// <inheritdoc/>
    public abstract FileAttributes GetAttributes(string path);

    /// <inheritdoc/>
    public virtual DateTime GetCreationTime(string path)
    {
        return GetCreationTimeUtc(path).ToLocalTime();
    }

    /// <inheritdoc/>
    public abstract DateTime GetCreationTimeUtc(string path);

    /// <inheritdoc/>
    public virtual DateTime GetLastAccessTime(string path)
    {
        return GetLastAccessTimeUtc(path).ToLocalTime();
    }

    /// <inheritdoc/>
    public abstract DateTime GetLastAccessTimeUtc(string path);

    /// <inheritdoc/>
    public virtual DateTime GetLastWriteTime(string path)
    {
        return GetLastWriteTimeUtc(path).ToLocalTime();
    }

    /// <inheritdoc/>
    public abstract DateTime GetLastWriteTimeUtc(string path);

    /// <inheritdoc/>
    public virtual void Move(string sourceFileName, string destFileName)
    {
        Move(sourceFileName, destFileName, false);
    }

    /// <inheritdoc/>
    public abstract void Move(string sourceFileName, string destFileName, bool overwrite);

    /// <inheritdoc/>
    public virtual Stream Open(string path, FileMode mode)
    {
        return Open(path, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.None, DefaultBufferSize, FileOptions.None);
    }

    /// <inheritdoc/>
    public virtual Stream Open(string path, FileMode mode, FileAccess access)
    {
        return Open(path, mode, access, FileShare.None, DefaultBufferSize, FileOptions.None);
    }

    /// <inheritdoc/>
    public virtual Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
    {
        return Open(path, mode, access, share, DefaultBufferSize, FileOptions.None);
    }

    /// <inheritdoc/>
    public abstract Stream Open(string path, FileStreamOptions options);

    /// <inheritdoc/>
    public virtual Stream OpenRead(string path)
    {
        return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None);
    }

    /// <inheritdoc/>
    public virtual StreamReader OpenText(string path)
    {
        var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None);
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        return new StreamReader(fileStream, leaveOpen: false);
#else
        return new StreamReader(fileStream, Encoding.Default, true, 1024, leaveOpen: false);
#endif
    }

    /// <inheritdoc/>
    public virtual Stream OpenWrite(string path)
    {
        return Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, DefaultBufferSize, FileOptions.None);
    }

    /// <inheritdoc/>
    public virtual byte[] ReadAllBytes(string path)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.SequentialScan);
        using var memStream = new MemoryStream(new byte[fileStream.Length]);
        fileStream.CopyTo(memStream);
        return memStream.ToArray();
    }

    /// <inheritdoc/>
    public virtual async Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1, FileOptions.Asynchronous | FileOptions.SequentialScan);
        using var memStream = new MemoryStream(new byte[fileStream.Length]);
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await fileStream.CopyToAsync(memStream, cancellationToken).ConfigureAwait(false);
#else
        await fileStream.CopyToAsync(memStream, 81920, cancellationToken).ConfigureAwait(false);
#endif
        return memStream.ToArray();
    }

    /// <inheritdoc/>
    public virtual string[] ReadAllLines(string path)
    {
        return ReadAllLines(path, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual string[] ReadAllLines(string path, Encoding encoding)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None);
        using var streamReader = new StreamReader(fileStream, encoding);

        var result = new List<string>();
        string? item;
        while ((item = streamReader.ReadLine()) != null)
            result.Add(item);

        return result.ToArray();
    }

    /// <inheritdoc/>
    public virtual async Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default)
    {
        return await ReadAllLinesAsync(path, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous);
        using var streamReader = new StreamReader(fileStream, encoding);

        var result = new List<string>();
        string? item;
        while ((item = await streamReader.ReadLineAsync().ConfigureAwait(false)) != null)
            result.Add(item);

        return result.ToArray();
    }

    /// <inheritdoc/>
    public virtual string ReadAllText(string path)
    {
        return ReadAllText(path, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual string ReadAllText(string path, Encoding encoding)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None);
        using var streamReader = new StreamReader(fileStream, encoding);
        return streamReader.ReadToEnd();
    }

    /// <inheritdoc/>
    public virtual async Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default)
    {
        return await ReadAllTextAsync(path, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous);
        using var streamReader = new StreamReader(fileStream, encoding);
        return await InternalReadAllTextAsync(streamReader, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<string> ReadLines(string path)
    {
        return ReadLines(path, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual IEnumerable<string> ReadLines(string path, Encoding encoding)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.None);
        using var streamReader = new StreamReader(fileStream, encoding);

        string? line;
        while ((line = streamReader.ReadLine()) != null)
            yield return line;
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public virtual IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default)
    {
        return ReadLinesAsync(path, Encoding.Default, cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous);
        using var streamReader = new StreamReader(fileStream, encoding);

        cancellationToken.ThrowIfCancellationRequested();
        string? line;
        while ((line = await streamReader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return line;
        }
    }
#endif

    /// <inheritdoc/>
    public virtual void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName)
    {
        Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
    }

    /// <inheritdoc/>
    public virtual void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors)
    {
        if (destinationBackupFileName != null)
            Move(destinationFileName, destinationBackupFileName);
        else
            Delete(destinationFileName);

        Move(sourceFileName, destinationFileName);
    }

    /// <inheritdoc/>
    public abstract void SetAttributes(string path, FileAttributes fileAttributes);

    /// <inheritdoc/>
    public virtual void SetCreationTime(string path, DateTime creationTime)
    {
        SetCreationTimeUtc(path, creationTime.ToUniversalTime());
    }

    /// <inheritdoc/>
    public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

    /// <inheritdoc/>
    public virtual void SetLastAccessTime(string path, DateTime lastAccessTime)
    {
        SetLastAccessTimeUtc(path, lastAccessTime.ToUniversalTime());
    }

    /// <inheritdoc/>
    public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

    /// <inheritdoc/>
    public virtual void SetLastWriteTime(string path, DateTime lastWriteTime)
    {
        SetLastWriteTimeUtc(path, lastWriteTime.ToUniversalTime());
    }

    /// <inheritdoc/>
    public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

    /// <inheritdoc/>
    public virtual void WriteAllBytes(string path, byte[] bytes)
    {
        using var fileStream = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None);
        fileStream.Write(bytes, 0, bytes.Length);
    }

    /// <inheritdoc/>
    public virtual async Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous);
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await fileStream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
#else
        await fileStream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
#endif
    }

    /// <inheritdoc/>
    public virtual void WriteAllLines(string path, string[] contents)
    {
        WriteAllLines(path, (IEnumerable<string>)contents, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual void WriteAllLines(string path, IEnumerable<string> contents)
    {
        WriteAllLines(path, contents, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual void WriteAllLines(string path, string[] contents, Encoding encoding)
    {
        WriteAllLines(path, (IEnumerable<string>)contents, encoding);
    }

    /// <inheritdoc/>
    public virtual void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
    {
        using var fileStream = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None);
        using var streamWriter = new StreamWriter(fileStream, encoding);

        foreach (var content in contents)
            streamWriter.WriteLine(content);
    }

    /// <inheritdoc/>
    public virtual async Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        await WriteAllLinesAsync(path, contents, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous);
        using var streamWriter = new StreamWriter(fileStream, encoding);

        foreach (var content in contents)
        {
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
            await streamWriter.WriteLineAsync(content.AsMemory(), cancellationToken).ConfigureAwait(false);
#else
            cancellationToken.ThrowIfCancellationRequested();
            await streamWriter.WriteLineAsync(content).ConfigureAwait(false);
#endif
        }
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    /// <inheritdoc/>
    public virtual async Task WriteAllLinesAsync(string path, IAsyncEnumerable<string> contents, CancellationToken cancellationToken = default)
    {
        await WriteAllLinesAsync(path, contents, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task WriteAllLinesAsync(string path, IAsyncEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous);
        using var streamWriter = new StreamWriter(fileStream, encoding);

        await foreach (var content in contents.WithCancellation(cancellationToken))
        {
            await streamWriter.WriteLineAsync(content.AsMemory(), cancellationToken).ConfigureAwait(false);
        }
    }
#endif

    /// <inheritdoc/>
    public virtual void WriteAllText(string path, string? contents)
    {
        WriteAllText(path, contents, Encoding.Default);
    }

    /// <inheritdoc/>
    public virtual void WriteAllText(string path, string? contents, Encoding encoding)
    {
        using var fileStream = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.None);
        using var streamWriter = new StreamWriter(fileStream, encoding);
        streamWriter.Write(contents);
    }

    /// <inheritdoc/>
    public virtual async Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default)
    {
        await WriteAllTextAsync(path, contents, Encoding.Default, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default)
    {
        using var fileStream = Open(path, FileMode.Create, FileAccess.Write, FileShare.Read, DefaultBufferSize, FileOptions.Asynchronous);
        using var streamWriter = new StreamWriter(fileStream, encoding);
#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
        await streamWriter.WriteAsync(contents.AsMemory(), cancellationToken).ConfigureAwait(false);
#else
        await streamWriter.WriteAsync(contents).ConfigureAwait(false);
#endif
    }

    protected abstract Stream Open(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options);

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    private static async Task<string> InternalReadAllTextAsync(StreamReader reader, CancellationToken cancellationToken)
    {
        char[]? buffer = null;
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            buffer = ArrayPool<char>.Shared.Rent(reader.CurrentEncoding.GetMaxCharCount(4096));
            var result = new StringBuilder();
            while (true)
            {
                int readBytesCount = await reader.ReadAsync(new Memory<char>(buffer), cancellationToken).ConfigureAwait(false);
                if (readBytesCount == 0)
                    break;

                result.Append(buffer, 0, readBytesCount);
            }

            return result.ToString();
        }
        finally
        {
            if (buffer != null)
                ArrayPool<char>.Shared.Return(buffer);
        }
    }
#else
    private static async Task<string> InternalReadAllTextAsync(StreamReader reader, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var buffer = new char[reader.CurrentEncoding.GetMaxCharCount(4096)];
        var result = new StringBuilder();
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            int readBytesCount = await reader.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            if (readBytesCount == 0)
                break;

            result.Append(buffer, 0, readBytesCount);
        }

        return result.ToString();
    }
#endif
}
