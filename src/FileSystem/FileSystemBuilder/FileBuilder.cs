using MaSch.Core;
using MaSch.FileSystem.FileSystemBuilder.Actions;

namespace MaSch.FileSystem.FileSystemBuilder;

public class FileBuilder : IFileBuilder
{
    private readonly FileCreateAction _action;

    public FileBuilder(string fullPath)
    {
        _action = new FileCreateAction(fullPath);
    }

    public IFileSystemAction Build()
    {
        return _action;
    }

    public IFileBuilder WithAttributes(FileAttributes attributes)
    {
        Guard.NotUndefinedFlagInEnumValue(attributes);
        _action.Attributes = attributes;
        return this;
    }

    public IFileBuilder WithContent(byte[] bytes)
    {
        Guard.NotNull(bytes);
        _action.Content = bytes;
        return this;
    }

    public IFileBuilder WithContent(string text)
    {
        Guard.NotNull(text);
        _action.Content = Encoding.Default.GetBytes(text);
        return this;
    }

    public IFileBuilder WithContent(string text, Encoding encoding)
    {
        Guard.NotNull(text);
        Guard.NotNull(encoding);
        _action.Content = encoding.GetBytes(text);
        return this;
    }

    public IFileBuilder WithCreationTime(DateTime creationTime)
    {
        _action.CreationTimeUtc = creationTime.ToUniversalTime(DateTimeKind.Local);
        return this;
    }

    public IFileBuilder WithCreationTimeUtc(DateTime creationTimeUtc)
    {
        _action.CreationTimeUtc = creationTimeUtc.ToUniversalTime(DateTimeKind.Utc);
        return this;
    }

    public IFileBuilder WithLastAccessTime(DateTime lastAccessTime)
    {
        _action.LastAccessTimeUtc = lastAccessTime.ToUniversalTime(DateTimeKind.Local);
        return this;
    }

    public IFileBuilder WithLastAccessTimeUtc(DateTime lastAccessTimeUtc)
    {
        _action.LastAccessTimeUtc = lastAccessTimeUtc.ToUniversalTime(DateTimeKind.Utc);
        return this;
    }

    public IFileBuilder WithLastWriteTime(DateTime lastWriteTime)
    {
        _action.LastWriteTimeUtc = lastWriteTime.ToUniversalTime(DateTimeKind.Local);
        return this;
    }

    public IFileBuilder WithLastWriteTimeUtc(DateTime lastWriteTimeUtc)
    {
        _action.LastWriteTimeUtc = lastWriteTimeUtc.ToUniversalTime(DateTimeKind.Utc);
        return this;
    }
}
