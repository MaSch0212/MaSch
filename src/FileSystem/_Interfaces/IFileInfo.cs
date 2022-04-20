namespace MaSch.FileSystem;

/// <summary>Provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of <see cref="Stream" /> objects.</summary>
public interface IFileInfo : IFileSystemInfo
{
    /// <summary>Gets the size, in bytes, of the current file.</summary>
    /// <returns>The size of the current file in bytes.</returns>
    long Length { get; }

    /// <summary>Gets a string representing the directory's full path.</summary>
    /// <returns>A string representing the directory's full path.</returns>
    string? DirectoryName { get; }

    /// <summary>Gets an instance of the parent directory.</summary>
    /// <returns>A <see cref="IDirectoryInfo" /> object representing the parent directory of this file.</returns>
    IDirectoryInfo? Directory { get; }

    /// <summary>Gets or sets a value indicating whether the current file is read only.</summary>
    /// <returns><see langword="true" /> if the current file is read only; otherwise, <see langword="false" />.</returns>
    bool IsReadOnly { get; set; }

    /// <summary>Creates a <see cref="StreamReader" /> with UTF8 encoding that reads from an existing text file.</summary>
    /// <returns>A new <see cref="StreamReader" /> with UTF8 encoding.</returns>
    StreamReader OpenText();

    /// <summary>Creates a <see cref="StreamWriter" /> that writes a new text file.</summary>
    /// <returns>A new <see cref="StreamWriter" />.</returns>
    StreamWriter CreateText();

    /// <summary>Creates a <see cref="StreamWriter" /> that appends text to the file represented by this instance of the <see cref="IFileInfo" />.</summary>
    /// <returns>A new <see cref="StreamWriter" />.</returns>
    StreamWriter AppendText();

    /// <summary>Copies an existing file to a new file, disallowing the overwriting of an existing file.</summary>
    /// <param name="destFileName">The name of the new file to copy to.</param>
    /// <returns>A new file with a fully qualified path.</returns>
    IFileInfo CopyTo(string destFileName);

    /// <summary>Copies an existing file to a new file, allowing the overwriting of an existing file.</summary>
    /// <param name="destFileName">The name of the new file to copy to.</param>
    /// <param name="overwrite"><see langword="true" /> to allow an existing file to be overwritten; otherwise, <see langword="false" />.</param>
    /// <returns>A new file, or an overwrite of an existing file if <paramref name="overwrite" /> is <see langword="true" />. If the file exists and <paramref name="overwrite" /> is <see langword="false" />, an <see cref="IOException" /> is thrown.</returns>
    IFileInfo CopyTo(string destFileName, bool overwrite);

    /// <summary>Creates a file.</summary>
    /// <returns>A new file.</returns>
    Stream Create();

    /// <summary>Opens a file in the specified mode.</summary>
    /// <param name="mode">A <see cref="FileMode" /> constant specifying the mode (for example, <see cref="FileMode.Open" /> or <see cref="FileMode.Append" />) in which to open the file.</param>
    /// <returns>A file opened in the specified mode, with read/write access and unshared.</returns>
    Stream Open(FileMode mode);

    /// <summary>Opens a file in the specified mode with read, write, or read/write access.</summary>
    /// <param name="mode">A <see cref="FileMode" /> constant specifying the mode (for example, <see cref="FileMode.Open" /> or <see cref="FileMode.Append" />) in which to open the file.</param>
    /// <param name="access">A <see cref="FileAccess" /> constant specifying whether to open the file with <see cref="FileAccess.Read" />, <see cref="FileAccess.Write" />, or <see ref="FileAccess.ReadWrite" /> file access.</param>
    /// <returns>A <see cref="Stream" /> object opened in the specified mode and access, and unshared.</returns>
    Stream Open(FileMode mode, FileAccess access);

    /// <summary>Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.</summary>
    /// <param name="mode">A <see cref="FileMode" /> constant specifying the mode (for example, <see cref="FileMode.Open" /> or <see cref="FileMode.Append" />) in which to open the file.</param>
    /// <param name="access">A <see cref="FileAccess" /> constant specifying whether to open the file with <see cref="FileAccess.Read" />, <see cref="FileAccess.Write" />, or <see ref="FileAccess.ReadWrite" /> file access.</param>
    /// <param name="share">A <see cref="FileShare" /> constant specifying the type of access other <see cref="Stream" /> objects have to this file.</param>
    /// <returns>A <see cref="Stream" /> object opened with the specified mode, access, and sharing options.</returns>
    Stream Open(FileMode mode, FileAccess access, FileShare share);

    /// <summary>Initializes a new instance of the <see cref="Stream" /> class with the specified creation mode, read/write and sharing permission, the access other FileStreams can have to the same file, the buffer size, additional file options and the allocation size.</summary>
    /// <param name="options">An object that describes optional <see cref="Stream" /> parameters to use.</param>
    /// <returns>A <see cref="Stream" /> that wraps the opened file.</returns>
    Stream Open(FileStreamOptions options);

    /// <summary>Creates a read-only <see cref="Stream" />.</summary>
    /// <returns>A new read-only <see cref="Stream" /> object.</returns>
    Stream OpenRead();

    /// <summary>Creates a write-only <see cref="Stream" />.</summary>
    /// <returns>A write-only unshared <see cref="Stream" /> object for a new or existing file.</returns>
    Stream OpenWrite();

    /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
    /// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
    void MoveTo(string destFileName);

    /// <summary>Moves a specified file to a new location, providing the options to specify a new file name and to overwrite the destination file if it already exists.</summary>
    /// <param name="destFileName">The path to move the file to, which can specify a different file name.</param>
    /// <param name="overwrite"><see langword="true" /> to overwrite the destination file if it already exists; <see langword="false" /> otherwise.</param>
    void MoveTo(string destFileName, bool overwrite);

    /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="IFileInfo" /> object, deleting the original file, and creating a backup of the replaced file.</summary>
    /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
    /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName" /> parameter.</param>
    /// <returns>A <see cref="IFileInfo" /> object that encapsulates information about the file described by the <paramref name="destinationFileName" /> parameter.</returns>
    IFileInfo Replace(string destinationFileName, string? destinationBackupFileName);

    /// <summary>Replaces the contents of a specified file with the file described by the current <see cref="IFileInfo" /> object, deleting the original file, and creating a backup of the replaced file.  Also specifies whether to ignore merge errors.</summary>
    /// <param name="destinationFileName">The name of a file to replace with the current file.</param>
    /// <param name="destinationBackupFileName">The name of a file with which to create a backup of the file described by the <paramref name="destinationFileName" /> parameter.</param>
    /// <param name="ignoreMetadataErrors"><see langword="true" /> to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise <see langword="false" />.</param>
    /// <returns>A <see cref="IFileInfo" /> object that encapsulates information about the file described by the <paramref name="destinationFileName" /> parameter.</returns>
    IFileInfo Replace(string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors);
}
