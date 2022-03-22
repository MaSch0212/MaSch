namespace MaSch.FileSystem.FileSystemBuilder;

public interface IFileSystemEntryBuilder<T>
    where T : IFileSystemEntryBuilder<T>
{
    T WithCreationTime(DateTime creationTime);
    T WithCreationTimeUtc(DateTime creationTimeUtc);
    T WithLastAccessTime(DateTime lastAccessTime);
    T WithLastAccessTimeUtc(DateTime lastAccessTimeUtc);
    T WithLastWriteTime(DateTime lastWriteTime);
    T WithLastWriteTimeUtc(DateTime lastWriteTimeUtc);
    T WithAttributes(FileAttributes attributes);
}
