namespace MaSch.FileSystem.FileSystemBuilder;

/// <summary>
/// Represents an action on a <see cref="IFileSystemService"/>.
/// </summary>
public interface IFileSystemAction
{
    /// <summary>
    /// Inokes this action.
    /// </summary>
    /// <param name="service">The <see cref="IFileSystemService"/> on which to invoke the action.</param>
    void Invoke(IFileSystemService service);
}
