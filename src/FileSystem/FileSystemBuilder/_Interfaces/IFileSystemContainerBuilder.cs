namespace MaSch.FileSystem.FileSystemBuilder;

/// <summary>
/// Provides methods to build a container inside a file system.
/// </summary>
/// <typeparam name="T">The type of this <see cref="IFileSystemContainerBuilder{T}"/>.</typeparam>
public interface IFileSystemContainerBuilder<T>
    where T : IFileSystemContainerBuilder<T>
{
    /// <summary>
    /// Ensures that the specified file exists in the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>. If the file does not already exists, an empty file is created.
    /// </summary>
    /// <param name="path">The file path relative to the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemContainerBuilder{T}"/>.</returns>
    T WithFile(string path);

    /// <summary>
    /// Ensures that the specified file exists in the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.
    /// </summary>
    /// <param name="path">The file path relative to the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.</param>
    /// <param name="fileConfiguration">An action that is used to further configure the file.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemContainerBuilder{T}"/>.</returns>
    T WithFile(string path, Action<IFileBuilder> fileConfiguration);

    /// <summary>
    /// Ensures that the specified file does not exist in the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.
    /// </summary>
    /// <param name="path">The file path relative to the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemContainerBuilder{T}"/>.</returns>
    T WithoutFile(string path);

    /// <summary>
    /// Ensures that the specified directory exists in the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.
    /// </summary>
    /// <param name="path">The directory path relative to the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemContainerBuilder{T}"/>.</returns>
    T WithDirectory(string path);

    /// <summary>
    /// Ensures that the specified directory exists in the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.
    /// </summary>
    /// <param name="path">The directory path relative to the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.</param>
    /// <param name="directoryConfiguration">An action that is used to further configure the directory.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemContainerBuilder{T}"/>.</returns>
    T WithDirectory(string path, Action<IDirectoryBuilder> directoryConfiguration);

    /// <summary>
    /// Ensures that the specified directory does not exist in the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.
    /// </summary>
    /// <param name="path">The directory path relative to the container that is built by this <see cref="IFileSystemContainerBuilder{T}"/>.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemContainerBuilder{T}"/>.</returns>
    T WithoutDirectory(string path);
}
