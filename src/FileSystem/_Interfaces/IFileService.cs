#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem;

/// <summary>Provides methods for the creation, copying, deletion, moving, and opening of a single file, and aids in the creation of <see cref="Stream" /> objects to access files.</summary>
public interface IFileService
{
    /// <summary>
    /// Gets the instance of <see cref="IFileSystemService"/> this <see cref="IFileService"/> has been created from.
    /// </summary>
    IFileSystemService FileSystem { get; }

    /// <summary>
    /// Creates an instance of <see cref="IFileInfo"/> representing the specified file.
    /// </summary>
    /// <param name="path">The file from which to create the <see cref="IFileInfo"/>.</param>
    /// <returns>An instance of type <see cref="IFileInfo"/>.</returns>
    IFileInfo GetInfo(string path);

    /// <summary>
    /// Gets an instance of <see cref="IDirectoryInfo"/> representing the directory the specified file is contained in.
    /// </summary>
    /// <param name="path">The file path.</param>
    /// <returns>An instance of type <see cref="IDirectoryInfo"/>.</returns>
    IDirectoryInfo? GetDirectory(string path);

    /// <summary>Appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    void AppendAllLines(string path, IEnumerable<string> contents);

    /// <summary>Appends lines to a file by using a specified encoding, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

    /// <summary>Asynchronously appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously appends lines to a file by using a specified encoding, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);

    /// <summary>Opens a file, appends the specified string to the file, and then closes the file. If the file does not exist, this method creates a file, writes the specified string to the file, then closes the file.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    void AppendAllText(string path, string? contents);

    /// <summary>Appends the specified string to the file using the specified encoding, creating the file if it does not already exist.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    void AppendAllText(string path, string? contents, Encoding encoding);

    /// <summary>Asynchronously opens a file or creates a file if it does not already exist, appends the specified string to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    Task AppendAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously opens a file or creates the file if it does not already exist, appends the specified string to the file using the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to append the specified string to.</param>
    /// <param name="contents">The string to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    Task AppendAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default);

    /// <summary>Creates a <see cref="T:System.IO.StreamWriter" /> that appends UTF-8 encoded text to an existing file, or to a new file if the specified file does not exist.</summary>
    /// <param name="path">The path to the file to append to.</param>
    /// <returns>A stream writer that appends UTF-8 encoded text to the specified file or to a new file.</returns>
    StreamWriter AppendText(string path);

    /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is not allowed.</summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory or an existing file.</param>
    void Copy(string sourceFileName, string destFileName);

    /// <summary>Copies an existing file to a new file. Overwriting a file of the same name is allowed.</summary>
    /// <param name="sourceFileName">The file to copy.</param>
    /// <param name="destFileName">The name of the destination file. This cannot be a directory.</param>
    /// <param name="overwrite"><see langword="true" /> if the destination file can be overwritten; otherwise, <see langword="false" />.</param>
    void Copy(string sourceFileName, string destFileName, bool overwrite);

    /// <summary>Creates or overwrites a file in the specified path.</summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <returns>A <see cref="Stream" /> that provides read/write access to the file specified in <paramref name="path" />.</returns>
    Stream Create(string path);

    /// <summary>Creates or overwrites a file in the specified path, specifying a buffer size.</summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
    /// <returns>A <see cref="Stream" /> with the specified buffer size that provides read/write access to the file specified in <paramref name="path" />.</returns>
    Stream Create(string path, int bufferSize);

    /// <summary>Creates or overwrites a file in the specified path, specifying a buffer size and options that describe how to create or overwrite the file.</summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <param name="bufferSize">The number of bytes buffered for reads and writes to the file.</param>
    /// <param name="options">One of the <see cref="FileOptions" /> values that describes how to create or overwrite the file.</param>
    /// <returns>A new file with the specified buffer size.</returns>
    Stream Create(string path, int bufferSize, FileOptions options);

    /// <summary>Creates or opens a file for writing UTF-8 encoded text. If the file already exists, its contents are overwritten.</summary>
    /// <param name="path">The file to be opened for writing.</param>
    /// <returns>A <see cref="StreamWriter" /> that writes to the specified file using UTF-8 encoding.</returns>
    StreamWriter CreateText(string path);

    /// <summary>Deletes the specified file.</summary>
    /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
    void Delete(string path);

    /// <summary>Determines whether the specified file exists.</summary>
    /// <param name="path">The file to check.</param>
    /// <returns><see langword="true" /> if the caller has the required permissions and <paramref name="path" /> contains the name of an existing file; otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <paramref name="path" /> is <see langword="null" />, an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <see langword="false" /> regardless of the existence of <paramref name="path" />.</returns>
    bool Exists([NotNullWhen(true)] string? path);

    /// <summary>Gets the <see cref="FileAttributes" /> of the file on the path.</summary>
    /// <param name="path">The path to the file.</param>
    /// <returns>The <see cref="FileAttributes" /> of the file on the path.</returns>
    FileAttributes GetAttributes(string path);

    /// <summary>Returns the creation date and time of the specified file or directory.</summary>
    /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
    /// <returns>A <see cref="DateTime" /> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.</returns>
    DateTime GetCreationTime(string path);

    /// <summary>Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory.</summary>
    /// <param name="path">The file or directory for which to obtain creation date and time information.</param>
    /// <returns>A <see cref="DateTime" /> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
    DateTime GetCreationTimeUtc(string path);

    /// <summary>Returns the date and time the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <returns>A <see cref="DateTime" /> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
    DateTime GetLastAccessTime(string path);

    /// <summary>Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed.</summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <returns>A <see cref="DateTime" /> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
    DateTime GetLastAccessTimeUtc(string path);

    /// <summary>Returns the date and time the specified file or directory was last written to.</summary>
    /// <param name="path">The file or directory for which to obtain write date and time information.</param>
    /// <returns>A <see cref="DateTime" /> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
    DateTime GetLastWriteTime(string path);

    /// <summary>Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.</summary>
    /// <param name="path">The file or directory for which to obtain write date and time information.</param>
    /// <returns>A <see cref="DateTime" /> structure set to the date and time that the specified file or directory was last written to. This value is expressed in UTC time.</returns>
    DateTime GetLastWriteTimeUtc(string path);

    /// <summary>Moves a specified file to a new location, providing the option to specify a new file name.</summary>
    /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
    /// <param name="destFileName">The new path and name for the file.</param>
    void Move(string sourceFileName, string destFileName);

    /// <summary>Moves a specified file to a new location, providing the options to specify a new file name and to overwrite the destination file if it already exists.</summary>
    /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
    /// <param name="destFileName">The new path and name for the file.</param>
    /// <param name="overwrite"><see langword="true" /> to overwrite the destination file if it already exists; <see langword="false" /> otherwise.</param>
    void Move(string sourceFileName, string destFileName, bool overwrite);

    /// <summary>Opens a <see cref="Stream" /> on the specified path with read/write access with no sharing.</summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <returns>A <see cref="Stream" /> opened in the specified mode and path, with read/write access and not shared.</returns>
    Stream Open(string path, FileMode mode);

    /// <summary>Opens a <see cref="Stream" /> on the specified path, with the specified mode and access with no sharing.</summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="FileAccess" /> value that specifies the operations that can be performed on the file.</param>
    /// <returns>An unshared <see cref="Stream" /> that provides access to the specified file, with the specified mode and access.</returns>
    Stream Open(string path, FileMode mode, FileAccess access);

    /// <summary>Opens a <see cref="Stream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</summary>
    /// <param name="path">The file to open.</param>
    /// <param name="mode">A <see cref="FileMode" /> value that specifies whether a file is created if one does not exist, and determines whether the contents of existing files are retained or overwritten.</param>
    /// <param name="access">A <see cref="FileAccess" /> value that specifies the operations that can be performed on the file.</param>
    /// <param name="share">A <see cref="FileShare" /> value specifying the type of access other threads have to the file.</param>
    /// <returns>A <see cref="Stream" /> on the specified path, having the specified mode with read, write, or read/write access and the specified sharing option.</returns>
    Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

    /// <summary>Initializes a new instance of the <see cref="Stream" /> class with the specified path, creation mode, read/write and sharing permission, the access other FileStreams can have to the same file, the buffer size, additional file options and the allocation size.</summary>
    /// <param name="path">The path of the file to open.</param>
    /// <param name="options">An object that describes optional <see cref="Stream" /> parameters to use.</param>
    /// <returns>A <see cref="Stream" /> instance that wraps the opened file.</returns>
    Stream Open(string path, FileStreamOptions options);

    /// <summary>Opens an existing file for reading.</summary>
    /// <param name="path">The file to be opened for reading.</param>
    /// <returns>A read-only <see cref="Stream" /> on the specified path.</returns>
    Stream OpenRead(string path);

    /// <summary>Opens an existing UTF-8 encoded text file for reading.</summary>
    /// <param name="path">The file to be opened for reading.</param>
    /// <returns>A <see cref="StreamReader" /> on the specified path.</returns>
    StreamReader OpenText(string path);

    /// <summary>Opens an existing file or creates a new file for writing.</summary>
    /// <param name="path">The file to be opened for writing.</param>
    /// <returns>An unshared <see cref="Stream" /> object on the specified path with <see cref="FileAccess.Write" /> access.</returns>
    Stream OpenWrite(string path);

    /// <summary>Opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A byte array containing the contents of the file.</returns>
    byte[] ReadAllBytes(string path);

    /// <summary>Asynchronously opens a binary file, reads the contents of the file into a byte array, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the byte array containing the contents of the file.</returns>
    Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>Opens a text file, reads all lines of the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A string array containing all lines of the file.</returns>
    string[] ReadAllLines(string path);

    /// <summary>Opens a file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <returns>A string array containing all lines of the file.</returns>
    string[] ReadAllLines(string path, Encoding encoding);

    /// <summary>Asynchronously opens a text file, reads all lines of the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string array containing all lines of the file.</returns>
    Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously opens a text file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string array containing all lines of the file.</returns>
    Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

    /// <summary>Opens a text file, reads all the text in the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A string containing all the text in the file.</returns>
    string ReadAllText(string path);

    /// <summary>Opens a file, reads all text in the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <returns>A string containing all text in the file.</returns>
    string ReadAllText(string path, Encoding encoding);

    /// <summary>Asynchronously opens a text file, reads all the text in the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string containing all text in the file.</returns>
    Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously opens a text file, reads all text in the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string containing all text in the file.</returns>
    Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

    /// <summary>Reads the lines of a file.</summary>
    /// <param name="path">The file to read.</param>
    /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
    IEnumerable<string> ReadLines(string path);

    /// <summary>Read the lines of a file that has a specified encoding.</summary>
    /// <param name="path">The file to read.</param>
    /// <param name="encoding">The encoding that is applied to the contents of the file.</param>
    /// <returns>All the lines of the file, or the lines that are the result of a query.</returns>
    IEnumerable<string> ReadLines(string path, Encoding encoding);

    /// <summary>Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file.</summary>
    /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName" />.</param>
    /// <param name="destinationFileName">The name of the file being replaced.</param>
    /// <param name="destinationBackupFileName">The name of the backup file.</param>
    void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName);

    /// <summary>Replaces the contents of a specified file with the contents of another file, deleting the original file, and creating a backup of the replaced file and optionally ignores merge errors.</summary>
    /// <param name="sourceFileName">The name of a file that replaces the file specified by <paramref name="destinationFileName" />.</param>
    /// <param name="destinationFileName">The name of the file being replaced.</param>
    /// <param name="destinationBackupFileName">The name of the backup file.</param>
    /// <param name="ignoreMetadataErrors"><see langword="true" /> to ignore merge errors (such as attributes and access control lists (ACLs)) from the replaced file to the replacement file; otherwise, <see langword="false" />.</param>
    void Replace(string sourceFileName, string destinationFileName, string? destinationBackupFileName, bool ignoreMetadataErrors);

    /// <summary>Sets the specified <see cref="FileAttributes" /> of the file on the specified path.</summary>
    /// <param name="path">The path to the file.</param>
    /// <param name="fileAttributes">A bitwise combination of the enumeration values.</param>
    void SetAttributes(string path, FileAttributes fileAttributes);

    /// <summary>Sets the date and time the file was created.</summary>
    /// <param name="path">The file for which to set the creation date and time information.</param>
    /// <param name="creationTime">A <see cref="DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in local time.</param>
    void SetCreationTime(string path, DateTime creationTime);

    /// <summary>Sets the date and time, in coordinated universal time (UTC), that the file was created.</summary>
    /// <param name="path">The file for which to set the creation date and time information.</param>
    /// <param name="creationTimeUtc">A <see cref="DateTime" /> containing the value to set for the creation date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
    void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

    /// <summary>Sets the date and time the specified file was last accessed.</summary>
    /// <param name="path">The file for which to set the access date and time information.</param>
    /// <param name="lastAccessTime">A <see cref="DateTime" /> containing the value to set for the last access date and time of <paramref name="path" />. This value is expressed in local time.</param>
    void SetLastAccessTime(string path, DateTime lastAccessTime);

    /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last accessed.</summary>
    /// <param name="path">The file for which to set the access date and time information.</param>
    /// <param name="lastAccessTimeUtc">A <see cref="DateTime" /> containing the value to set for the last access date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
    void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

    /// <summary>Sets the date and time that the specified file was last written to.</summary>
    /// <param name="path">The file for which to set the date and time information.</param>
    /// <param name="lastWriteTime">A <see cref="DateTime" /> containing the value to set for the last write date and time of <paramref name="path" />. This value is expressed in local time.</param>
    void SetLastWriteTime(string path, DateTime lastWriteTime);

    /// <summary>Sets the date and time, in coordinated universal time (UTC), that the specified file was last written to.</summary>
    /// <param name="path">The file for which to set the date and time information.</param>
    /// <param name="lastWriteTimeUtc">A <see cref="DateTime" /> containing the value to set for the last write date and time of <paramref name="path" />. This value is expressed in UTC time.</param>
    void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

    /// <summary>Creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    void WriteAllBytes(string path, byte[] bytes);

    /// <summary>Asynchronously creates a new file, writes the specified byte array to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken = default);

    /// <summary>Creates a new file, write the specified string array to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string array to write to the file.</param>
    void WriteAllLines(string path, string[] contents);

    /// <summary>Creates a new file, writes a collection of strings to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    void WriteAllLines(string path, IEnumerable<string> contents);

    /// <summary>Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string array to write to the file.</param>
    /// <param name="encoding">An <see cref="Encoding" /> object that represents the character encoding applied to the string array.</param>
    void WriteAllLines(string path, string[] contents, Encoding encoding);

    /// <summary>Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

    /// <summary>Asynchronously creates a new file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously creates a new file, write the specified lines to the file by using the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);

    /// <summary>Creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    void WriteAllText(string path, string? contents);

    /// <summary>Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <param name="encoding">The encoding to apply to the string.</param>
    void WriteAllText(string path, string? contents, Encoding encoding);

    /// <summary>Asynchronously creates a new file, writes the specified string to the file, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteAllTextAsync(string path, string? contents, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <param name="encoding">The encoding to apply to the string.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteAllTextAsync(string path, string? contents, Encoding encoding, CancellationToken cancellationToken = default);

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)

    /// <summary>Asynchronously appends lines to a file, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    Task AppendAllLinesAsync(string path, IAsyncEnumerable<string> contents, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously appends lines to a file by using a specified encoding, and then closes the file. If the specified file does not exist, this method creates a file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to append the lines to. The file is created if it doesn't already exist.</param>
    /// <param name="contents">The lines to append to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous append operation.</returns>
    Task AppendAllLinesAsync(string path, IAsyncEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously opens a text file, reads all lines of the file, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string array containing all lines of the file.</returns>
    IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously opens a text file, reads all lines of the file with the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to open for reading.</param>
    /// <param name="encoding">The encoding applied to the contents of the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous read operation, which wraps the string array containing all lines of the file.</returns>
    IAsyncEnumerable<string> ReadLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously creates a new file, writes the specified lines to the file, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteAllLinesAsync(string path, IAsyncEnumerable<string> contents, CancellationToken cancellationToken = default);

    /// <summary>Asynchronously creates a new file, write the specified lines to the file by using the specified encoding, and then closes the file.</summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="contents">The lines to write to the file.</param>
    /// <param name="encoding">The character encoding to use.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteAllLinesAsync(string path, IAsyncEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken = default);

#endif
}
