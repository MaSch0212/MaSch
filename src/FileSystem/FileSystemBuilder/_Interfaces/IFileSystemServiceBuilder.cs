namespace MaSch.FileSystem.FileSystemBuilder;

/// <summary>
/// Provides methods to built a <see cref="IFileSystemService"/>.
/// </summary>
/// <typeparam name="T">The type of this <see cref="IFileSystemServiceBuilder{T}"/>.</typeparam>
public interface IFileSystemServiceBuilder<T> : IFileSystemContainerBuilder<T>
    where T : IFileSystemServiceBuilder<T>
{
    /// <summary>
    /// Specifies an instance of <see cref="IFileSystemService"/> that should be used when building files and folders.
    /// </summary>
    /// <param name="service">The <see cref="IFileSystemService"/> to use.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemServiceBuilder{T}"/>.</returns>
    T UsingFileSystem(IFileSystemService service);

    /// <summary>
    /// Specifies a factory of <see cref="IFileSystemService"/> that should be used when building files and folders.
    /// </summary>
    /// <param name="serviceFactory">The function.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemServiceBuilder{T}"/>.</returns>
    T UsingFileSystem(Func<IFileSystemService> serviceFactory);

    /// <summary>
    /// Specified the type of <see cref="IFileSystemService"/> to use when building files and folders.
    /// </summary>
    /// <typeparam name="TService">The type of <see cref="IFileSystemService"/>.</typeparam>
    /// <returns>A self-reference to this <see cref="IFileSystemServiceBuilder{T}"/>.</returns>
    T UsingFileSystem<TService>()
        where TService : IFileSystemService, new();

    /// <summary>
    /// Ensures that the specified path root exists. If the selected <see cref="IFileSystemService"/> supports creating path roots, the path root is created; otherwise an exception is raised when the <see cref="IFileSystemService"/> is built.
    /// </summary>
    /// <param name="rootName">The name of the path root.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemServiceBuilder{T}"/>.</returns>
    T WithRoot(string rootName);

    /// <summary>
    /// Ensures that the specified path root exists. If the selected <see cref="IFileSystemService"/> supports creating path roots, the path root is created; otherwise an exception is raised when the <see cref="IFileSystemService"/> is built.
    /// </summary>
    /// <param name="rootName">The name of the path root.</param>
    /// <param name="rootConfiguration">An action that is used to further configure the specified path root.</param>
    /// <returns>A self-reference to this <see cref="IFileSystemServiceBuilder{T}"/>.</returns>
    T WithRoot(string rootName, Action<IFileSystemRootBuilder> rootConfiguration);

    /// <summary>
    /// Builds the <see cref="IFileSystemService"/>.
    /// </summary>
    /// <returns>The built <see cref="IFileSystemService"/>.</returns>
    IFileSystemService Build();
}