#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem;

/// <summary>
/// Exposes methods for creating, moving, and enumerating through directories and subdirectories.
/// </summary>
public interface IDirectoryService
{
    /// <summary>
    /// Gets the instance of <see cref="IFileSystemService"/> this <see cref="IDirectoryService"/> has been created from.
    /// </summary>
    IFileSystemService FileSystem { get; }

    /// <summary>
    /// Creates an instance of <see cref="IDirectoryInfo"/> representing the specified directory.
    /// </summary>
    /// <param name="path">The directory from which to create the <see cref="IDirectoryInfo"/>.</param>
    /// <returns>An instance of type <see cref="IDirectoryInfo"/>.</returns>
    IDirectoryInfo GetInfo(string path);

    /// <summary>
    /// Retrieves the parent directory of the specified path, including both absolute
    /// and relative paths.
    /// </summary>
    /// <param name="path">The path for which to retrieve the parent directory.</param>
    /// <returns>
    /// The parent directory, or null if path is the root directory, including the root
    /// of a UNC server or share name.
    /// </returns>
    IDirectoryInfo? GetParent(string path);

    /// <summary>
    /// Creates all directories and subdirectories in the specified path unless they
    /// already exist.
    /// </summary>
    /// <param name="path">The directory to create.</param>
    /// <returns>
    /// An object that represents the directory at the specified path. This object is
    /// returned regardless of whether a directory at the specified path already exists.
    /// </returns>
    IDirectoryInfo CreateDirectory(string path);

    /// <summary>
    /// Determines whether the given path refers to an existing directory on disk.
    /// </summary>
    /// <param name="path">The path to test.</param>
    /// <returns>
    /// <c>true</c> if path refers to an existing directory; <c>false</c> if the directory does not
    /// exist or an error occurs when trying to determine if the specified directory exists.
    /// </returns>
    bool Exists([NotNullWhen(true)] string? path);

    /// <summary>
    /// Sets the creation date and time for the specified file or directory.
    /// </summary>
    /// <param name="path">The file or directory for which to set the creation date and time information.</param>
    /// <param name="creationTime">The date and time the file or directory was last written to. This value is expressed in local time.</param>
    void SetCreationTime(string path, DateTime creationTime);

    /// <summary>
    /// Sets the creation date and time, in Coordinated Universal Time (UTC) format,
    /// for the specified file or directory.
    /// </summary>
    /// <param name="path">The file or directory for which to set the creation date and time information.</param>
    /// <param name="creationTimeUtc">The date and time the directory or file was created. This value is expressed in local time.</param>
    void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

    /// <summary>
    /// Gets the creation date and time of a directory.
    /// </summary>
    /// <param name="path">The path of the directory.</param>
    /// <returns>
    /// A structure that is set to the creation date and time for the specified directory.
    /// This value is expressed in local time.
    /// </returns>
    DateTime GetCreationTime(string path);

    /// <summary>
    /// Gets the creation date and time, in Coordinated Universal Time (UTC) format, of a directory.
    /// </summary>
    /// <param name="path">The path of the directory.</param>
    /// <returns>
    /// A structure that is set to the creation date and time for the specified directory.
    /// This value is expressed in UTC time.
    /// </returns>
    DateTime GetCreationTimeUtc(string path);

    /// <summary>
    /// Sets the date and time a directory was last written to.
    /// </summary>
    /// <param name="path">The path of the directory.</param>
    /// <param name="lastWriteTime">The date and time the directory was last written to. This value is expressed in local time.</param>
    void SetLastWriteTime(string path, DateTime lastWriteTime);

    /// <summary>
    /// Sets the date and time, in Coordinated Universal Time (UTC) format, that a directory
    /// was last written to.
    /// </summary>
    /// <param name="path">The path of the directory.</param>
    /// <param name="lastWriteTimeUtc">The date and time the directory was last written to. This value is expressed in UTC time.</param>
    void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

    /// <summary>
    /// Returns the date and time the specified file or directory was last written to.
    /// </summary>
    /// <param name="path">The file or directory for which to obtain modification date and time information.</param>
    /// <returns>
    /// A structure that is set to the date and time the specified file or directory
    /// was last written to. This value is expressed in local time.
    /// </returns>
    DateTime GetLastWriteTime(string path);

    /// <summary>
    /// Returns the date and time, in Coordinated Universal Time (UTC) format, that the
    /// specified file or directory was last written to.
    /// </summary>
    /// <param name="path">
    /// The file or directory for which to obtain modification date and time information.</param>
    /// <returns>
    /// A structure that is set to the date and time the specified file or directory
    /// was last written to. This value is expressed in UTC time.
    /// </returns>
    DateTime GetLastWriteTimeUtc(string path);

    /// <summary>
    /// Sets the date and time the specified file or directory was last accessed.
    /// </summary>
    /// <param name="path">The file or directory for which to set the access date and time information.</param>
    /// <param name="lastAccessTime">
    /// An object that contains the value to set for the access date and time of path.
    /// This value is expressed in local time.
    /// </param>
    void SetLastAccessTime(string path, DateTime lastAccessTime);

    /// <summary>
    /// Sets the date and time, in Coordinated Universal Time (UTC) format, that the
    /// specified file or directory was last accessed.
    /// </summary>
    /// <param name="path">The file or directory for which to set the access date and time information.</param>
    /// <param name="lastAccessTimeUtc">
    /// An object that contains the value to set for the access date and time of path.
    /// This value is expressed in UTC time.
    /// </param>
    void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

    /// <summary>
    /// Returns the date and time the specified file or directory was last accessed.
    /// </summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <returns>
    /// A structure that is set to the date and time the specified file or directory
    /// was last accessed. This value is expressed in local time.
    /// </returns>
    DateTime GetLastAccessTime(string path);

    /// <summary>
    /// Returns the date and time, in Coordinated Universal Time (UTC) format, that the
    /// specified file or directory was last accessed.
    /// </summary>
    /// <param name="path">The file or directory for which to obtain access date and time information.</param>
    /// <returns>
    /// A structure that is set to the date and time the specified file or directory
    /// was last accessed. This value is expressed in UTC time.
    /// </returns>
    DateTime GetLastAccessTimeUtc(string path);

    /// <summary>
    /// Returns the names of files (including their paths) in the specified directory.
    /// </summary>
    /// <param name="path">
    /// The relative or absolute path to the directory to search. This string is not
    /// case-sensitive.
    /// </param>
    /// <returns>
    /// An array of the full names (including paths) for the files in the specified directory,
    /// or an empty array if no files are found.
    /// </returns>
    string[] GetFiles(string path);

    /// <summary>
    /// Returns the names of files (including their paths) that match the specified search
    /// pattern in the specified directory.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">
    /// The search string to match against the names of files in path. This parameter
    /// can contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <returns>
    /// An array of the full names (including paths) for the files in the specified directory
    /// that match the specified search pattern, or an empty array if no files are found.
    /// </returns>
    string[] GetFiles(string path, string searchPattern);

    /// <summary>
    /// Returns the names of files (including their paths) that match the specified search
    /// pattern in the specified directory, using a value to determine whether to search
    /// subdirectories.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">
    /// The search string to match against the names of files in path. This parameter
    /// can contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="searchOption">
    /// One of the enumeration values that specifies whether the search operation should
    /// include all subdirectories or only the current directory.
    /// </param>
    /// <returns>
    /// An array of the full names (including paths) for the files in the specified directory
    /// that match the specified search pattern and option, or an empty array if no files are found.
    /// </returns>
    string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Returns the names of subdirectories (including their paths) in the specified directory.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <returns>
    /// An array of the full names (including paths) of subdirectories in the specified
    /// path, or an empty array if no directories are found.
    /// </returns>
    string[] GetDirectories(string path);

    /// <summary>
    /// Returns the names of subdirectories (including their paths) that match the specified
    /// search pattern in the specified directory.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">
    /// The search string to match against the names of subdirectories in path. This
    /// parameter can contain a combination of valid literal and wildcard characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <returns>
    /// An array of the full names (including paths) of the subdirectories that match
    /// the search pattern in the specified directory, or an empty array if no directories
    /// are found.
    /// </returns>
    string[] GetDirectories(string path, string searchPattern);

    /// <summary>Returns the names of the subdirectories (including their paths) that match the specified search pattern in the specified directory, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
    /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
    string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);

    /// <summary>Returns the names of all files and subdirectories in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <returns>An array of the names of files and subdirectories in the specified directory, or an empty array if no files or subdirectories are found.</returns>
    string[] GetFileSystemEntries(string path);

    /// <summary>Returns an array of file names and directory names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of file and directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <returns>An array of file names and directory names that match the specified search criteria, or an empty array if no files or directories are found.</returns>
    string[] GetFileSystemEntries(string path, string searchPattern);

    /// <summary>Returns an array of all the file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files and directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <returns>An array of file the file names and directory names that match the specified search criteria, or an empty array if no files or directories are found.</returns>
    string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

    /// <summary>Returns an enumerable collection of directory full names in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" />.</returns>
    IEnumerable<string> EnumerateDirectories(string path);

    /// <summary>Returns an enumerable collection of directory full names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" /> and that match the specified search pattern.</returns>
    IEnumerable<string> EnumerateDirectories(string path, string searchPattern);

    /// <summary>Returns an enumerable collection of directory full names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" /> and that match the specified search pattern and search option.</returns>
    IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

    /// <summary>Returns an enumerable collection of full file names in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" />.</returns>
    IEnumerable<string> EnumerateFiles(string path);

    /// <summary>Returns an enumerable collection of full file names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" /> and that match the specified search pattern.</returns>
    IEnumerable<string> EnumerateFiles(string path, string searchPattern);

    /// <summary>Returns an enumerable collection of full file names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" /> and that match the specified search pattern and search option.</returns>
    IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

    /// <summary>Returns an enumerable collection of file names and directory names in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" />.</returns>
    IEnumerable<string> EnumerateFileSystemEntries(string path);

    /// <summary>Returns an enumerable collection of file names and directory names that match a search pattern in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of file-system entries in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" /> and that match the specified search pattern.</returns>
    IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

    /// <summary>Returns an enumerable collection of file names and directory names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against file-system entries in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values  that specifies whether the search operation should include only the current directory or should include all subdirectories. The default value is <see cref="F:System.IO.SearchOption.TopDirectoryOnly" />.</param>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" /> and that match the specified search pattern and option.</returns>
    IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Returns the volume information, root information, or both for the specified path.
    /// </summary>
    /// <param name="path">The path of a file or directory.</param>
    /// <returns>
    /// A string that contains the volume information, root information, or both for
    /// the specified path.
    /// </returns>
    string GetDirectoryRoot(string path);

    /// <summary>
    /// Gets the current working directory of the application.
    /// </summary>
    /// <returns>
    /// A string that contains the absolute path of the current working directory, and
    /// does not end with a backslash (\).
    /// </returns>
    string GetCurrentDirectory();

    /// <summary>
    /// Sets the application's current working directory to the specified directory.
    /// </summary>
    /// <param name="path">The path to which the current working directory is set.</param>
    void SetCurrentDirectory(string path);

    /// <summary>
    /// Moves a file or a directory and its contents to a new location.
    /// </summary>
    /// <param name="sourceDirName">The path of the file or directory to move.</param>
    /// <param name="destDirName">
    /// The path to the new location for <paramref name="sourceDirName"/>. If sourceDirName is a file, then
    /// <paramref name="destDirName"/> must also be a file name.
    /// </param>
    void Move(string sourceDirName, string destDirName);

    /// <summary>
    /// Deletes an empty directory from a specified path.
    /// </summary>
    /// <param name="path">The name of the empty directory to remove. This directory must be writable and empty.</param>
    void Delete(string path);

    /// <summary>
    /// Deletes the specified directory and, if indicated, any subdirectories and files in the directory.
    /// </summary>
    /// <param name="path">The name of the directory to remove.</param>
    /// <param name="recursive"><c>true</c> to remove directories, subdirectories, and files in path; otherwise, <c>false</c>.</param>
    void Delete(string path, bool recursive);

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)

    /// <summary>
    /// Returns the names of files (including their paths) that match the specified search
    /// pattern and enumeration options in the specified directory.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">
    /// The search string to match against the names of subdirectories in path. This
    /// parameter can contain a combination of valid literal and wildcard characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>
    /// An array of the full names (including paths) for the files in the specified directory
    /// that match the specified search pattern and enumeration options, or an empty
    /// array if no files are found.
    /// </returns>
    string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>
    /// Returns the names of subdirectories (including their paths) that match the specified
    /// search pattern and enumeration options in the specified directory.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">
    /// The search string to match against the names of subdirectories in path. This
    /// parameter can contain a combination of valid literal and wildcard characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>
    /// An array of the full names (including paths) of the subdirectories that match
    /// the search pattern and enumeration options in the specified directory, or an
    /// empty array if no directories are found.
    /// </returns>
    string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>Returns an array of file names and directory names that match a search pattern and enumeration options in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An array of file names and directory names that match the specified search pattern and enumeration options, or an empty array if no files or directories are found.</returns>
    string[] GetFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>Returns an enumerable collection of the directory full names that match a search pattern in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of directories in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path" /> and that match the specified search pattern and enumeration options.</returns>
    IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>Returns an enumerable collection of full file names that match a search pattern and enumeration options in a specified path, and optionally searches subdirectories.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path" />.  This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path" /> and that match the specified search pattern and enumeration options.</returns>
    IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>Returns an enumerable collection of file names and directory names that match a search pattern and enumeration options in a specified path.</summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path" />. This parameter can contain a combination of valid literal and wildcard characters, but it doesn't support regular expressions.</param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An enumerable collection of file-system entries in the directory specified by <paramref name="path" />, that match the specified search pattern and the specified enumeration options.</returns>
    IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions);

#endif
}
