namespace MaSch.FileSystem.FileSystemBuilder;

/// <summary>
/// Provides methods to build a file inside a file system.
/// </summary>
public interface IFileBuilder : IFileSystemEntryBuilder<IFileBuilder>, IFileSystemActionBuilder
{
    /// <summary>
    /// Specifies the content of the file that is built.
    /// </summary>
    /// <param name="bytes">The raw bytes to use a the content of the file.</param>
    /// <returns>A self-reference to this <see cref="IFileBuilder"/>.</returns>
    IFileBuilder WithContent(byte[] bytes);

    /// <summary>
    /// Specifies the content of the file that is built.
    /// </summary>
    /// <param name="text">The text to use a the content of the file.</param>
    /// <returns>A self-reference to this <see cref="IFileBuilder"/>.</returns>
    IFileBuilder WithContent(string text);

    /// <summary>
    /// Specifies the content of the file that is built.
    /// </summary>
    /// <param name="text">The text to use a the content of the file.</param>
    /// <param name="encoding">The encoding to use.</param>
    /// <returns>A self-reference to this <see cref="IFileBuilder"/>.</returns>
    IFileBuilder WithContent(string text, Encoding encoding);
}
