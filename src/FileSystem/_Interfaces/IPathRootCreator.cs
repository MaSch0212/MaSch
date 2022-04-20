namespace MaSch.FileSystem;

/// <summary>
/// Provides methods to create root paths for a file system.
/// </summary>
public interface IPathRootCreator
{
    /// <summary>
    /// Creates a new path root with the specified name.
    /// </summary>
    /// <param name="name">The name of the path root.</param>
    void CreatePathRoot(string name);
}
