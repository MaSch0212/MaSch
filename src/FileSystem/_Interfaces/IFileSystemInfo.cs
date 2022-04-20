namespace MaSch.FileSystem;

/// <summary>Provides the base interface for both <see cref="IFileInfo" /> and <see cref="IDirectoryInfo" /> objects.</summary>
public interface IFileSystemInfo
{
    /// <summary>
    /// Gets the instance of <see cref="IFileSystemService"/> this <see cref="IFileSystemInfo"/> has been created from.
    /// </summary>
    IFileSystemService FileSystem { get; }

    /// <summary>Gets the full path of the directory or file.</summary>
    /// <returns>A string containing the full path.</returns>
    string FullName { get; }

    /// <summary>Gets the extension part of the file name, including the leading dot <c>.</c> even if it is the entire file name, or an empty string if no extension is present.</summary>
    /// <returns>A string containing the <see cref="IFileSystemInfo" /> extension.</returns>
    string Extension { get; }

    /// <summary>For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the <see langword="Name" /> property gets the name of the directory.</summary>
    /// <returns>A string that is the name of the parent directory, the name of the last directory in the hierarchy, or the name of a file, including the file name extension.</returns>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1623:Property summary documentation should match accessors", Justification = "Copied documentation from .NET")]
    string Name { get; }

    /// <summary>Gets a value indicating whether the file or directory exists.</summary>
    /// <returns><see langword="true" /> if the file or directory exists; otherwise, <see langword="false" />.</returns>
    bool Exists { get; }

    /// <summary>Gets or sets the creation time of the current file or directory.</summary>
    /// <returns>The creation date and time of the current <see cref="IFileSystemInfo" /> object.</returns>
    DateTime CreationTime { get; set; }

    /// <summary>Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.</summary>
    /// <returns>The creation date and time in UTC format of the current <see cref="IFileSystemInfo" /> object.</returns>
    DateTime CreationTimeUtc { get; set; }

    /// <summary>Gets or sets the time the current file or directory was last accessed.</summary>
    /// <returns>The time that the current file or directory was last accessed.</returns>
    DateTime LastAccessTime { get; set; }

    /// <summary>Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.</summary>
    /// <returns>The UTC time that the current file or directory was last accessed.</returns>
    DateTime LastAccessTimeUtc { get; set; }

    /// <summary>Gets or sets the time when the current file or directory was last written to.</summary>
    /// <returns>The time the current file was last written.</returns>
    DateTime LastWriteTime { get; set; }

    /// <summary>Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.</summary>
    /// <returns>The UTC time when the current file was last written to.</returns>
    DateTime LastWriteTimeUtc { get; set; }

    /// <summary>Gets or sets the attributes for the current file or directory.</summary>
    /// <returns><see cref="T:System.IO.FileAttributes" /> of the current <see cref="IFileSystemInfo" />.</returns>
    FileAttributes Attributes { get; set; }

    /// <summary>Deletes a file or directory.</summary>
    void Delete();

    /// <summary>Refreshes the state of the object.</summary>
    void Refresh();
}
