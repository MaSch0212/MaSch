namespace MaSch.FileSystem.FileSystemBuilder;

/// <summary>
/// Provides methods to built a file or directory inside a file system.
/// </summary>
/// <typeparam name="T">The type of this <see cref="IFileSystemEntryBuilder{T}"/>.</typeparam>
public interface IFileSystemEntryBuilder<T>
    where T : IFileSystemEntryBuilder<T>
{
    /// <summary>
    /// Sets the creation time the built file/folder should have.
    /// </summary>
    /// <param name="creationTime">The creation time represented as loca time.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemEntryBuilder{T}"/>.</returns>
    T WithCreationTime(DateTime creationTime);

    /// <summary>
    /// Sets the creation time the built file/folder should have.
    /// </summary>
    /// <param name="creationTimeUtc">The creation time represented as UTC time.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemEntryBuilder{T}"/>.</returns>
    T WithCreationTimeUtc(DateTime creationTimeUtc);

    /// <summary>
    /// Sets the creation time the built file/folder should have.
    /// </summary>
    /// <param name="lastAccessTime">The creation time represented as loca time.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemEntryBuilder{T}"/>.</returns>
    T WithLastAccessTime(DateTime lastAccessTime);

    /// <summary>
    /// Sets the last access time the built file/folder should have.
    /// </summary>
    /// <param name="lastAccessTimeUtc">The last access time represented as UTC time.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemEntryBuilder{T}"/>.</returns>
    T WithLastAccessTimeUtc(DateTime lastAccessTimeUtc);

    /// <summary>
    /// Sets the creation time the built file/folder should have.
    /// </summary>
    /// <param name="lastWriteTime">The creation time represented as loca time.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemEntryBuilder{T}"/>.</returns>
    T WithLastWriteTime(DateTime lastWriteTime);

    /// <summary>
    /// Sets the last write time the built file/folder should have.
    /// </summary>
    /// <param name="lastWriteTimeUtc">The last write time represented as UTC time.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemEntryBuilder{T}"/>.</returns>
    T WithLastWriteTimeUtc(DateTime lastWriteTimeUtc);

    /// <summary>
    /// Sets the attributes the built file/folder should have.
    /// </summary>
    /// <param name="attributes">The attributes.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemEntryBuilder{T}"/>.</returns>
    T WithAttributes(FileAttributes attributes);
}
