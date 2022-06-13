#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem;

/// <summary>
/// Exposes instance methods for creating, moving, and enumerating through directories
/// and subdirectories.
/// </summary>
public interface IDirectoryInfo : IFileSystemInfo
{
    /// <summary>
    /// Gets the parent directory of a specified subdirectory.
    /// </summary>
    IDirectoryInfo? Parent { get; }

    /// <summary>
    /// Gets the root portion of the directory.
    /// </summary>
    IDirectoryInfo Root { get; }

    /// <summary>
    /// Creates a subdirectory or subdirectories on the specified path. The specified
    /// path can be relative to this instance.
    /// </summary>
    /// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name.</param>
    /// <returns>The last directory specified in <paramref name="path"/>.</returns>
    IDirectoryInfo CreateSubdirectory(string path);

    /// <summary>
    /// Creates a directory.
    /// </summary>
    void Create();

    /// <summary>
    /// Returns a file list from the current directory.
    /// </summary>
    /// <returns>An array of type <see cref="IFileInfo"/>.</returns>
    IFileInfo[] GetFiles();

    /// <summary>
    /// Returns a file list from the current directory matching the given search pattern.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of files. This parameter can contain
    /// a combination of valid literal path and wildcard (* and ?) characters, but it
    /// doesn't support regular expressions.
    /// </param>
    /// <returns>An array of type <see cref="IFileInfo"/>.</returns>
    IFileInfo[] GetFiles(string searchPattern);

    /// <summary>
    /// Returns a file list from the current directory matching the given search pattern
    /// and using a value to determine whether to search subdirectories.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of files. This parameter can contain
    /// a combination of valid literal path and wildcard (* and ?) characters, but it
    /// doesn't support regular expressions.
    /// </param>
    /// <param name="searchOption">
    /// One of the enumeration values that specifies whether the search operation should
    /// include only the current directory or all subdirectories.
    /// </param>
    /// <returns>An array of type <see cref="IFileInfo"/>.</returns>
    IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Returns an array of strongly typed <see cref="IFileSystemInfo"/> entries representing
    /// all the files and subdirectories in a directory.
    /// </summary>
    /// <returns>An array of strongly typed <see cref="IFileSystemInfo"/> entries.</returns>
    IFileSystemInfo[] GetFileSystemInfos();

    /// <summary>
    /// Retrieves an array of strongly typed System.IO.FileSystemInfo objects representing
    /// the files and subdirectories that match the specified search criteria.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories and files. This parameter
    /// can contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <returns>An array of strongly typed <see cref="IFileSystemInfo"/> objects matching the search criteria.</returns>
    IFileSystemInfo[] GetFileSystemInfos(string searchPattern);

    /// <summary>
    /// Retrieves an array of System.IO.FileSystemInfo objects that represent the files
    /// and subdirectories matching the specified search criteria.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories and files. This parameter
    /// can contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="searchOption">
    /// One of the enumeration values that specifies whether the search operation should
    /// include only the current directory or all subdirectories. The default value is
    /// <see cref="SearchOption.TopDirectoryOnly"/>.
    /// </param>
    /// <returns>An array of file system entries that match the search criteria.</returns>
    IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Returns the subdirectories of the current directory.
    /// </summary>
    /// <returns>An array of <see cref="IDirectoryInfo"/> objects.</returns>
    IDirectoryInfo[] GetDirectories();

    /// <summary>
    /// Returns an array of directories in the current System.IO.DirectoryInfo matching
    /// the given search criteria.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <returns>An array of type <see cref="IDirectoryInfo"/> matching searchPattern.</returns>
    IDirectoryInfo[] GetDirectories(string searchPattern);

    /// <summary>
    /// Returns an array of directories in the current System.IO.DirectoryInfo matching
    /// the given search criteria and using a value to determine whether to search subdirectories.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="searchOption">
    /// One of the enumeration values that specifies whether the search operation should
    /// include only the current directory or all subdirectories.
    /// </param>
    /// <returns>An array of type <see cref="IDirectoryInfo"/> matching <paramref name="searchPattern"/>.</returns>
    IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Returns an enumerable collection of file information in the current directory.
    /// </summary>
    /// <returns>An enumerable collection of the files in the current directory.</returns>
    IEnumerable<IFileInfo> EnumerateFiles();

    /// <summary>
    /// Returns an enumerable collection of file information that matches a search pattern.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of files. This parameter can contain
    /// a combination of valid literal path and wildcard (* and ?) characters, but it
    /// doesn't support regular expressions.
    /// </param>
    /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/>.</returns>
    IEnumerable<IFileInfo> EnumerateFiles(string searchPattern);

    /// <summary>
    /// Returns an enumerable collection of file information that matches a specified
    /// search pattern and search subdirectory option.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of files. This parameter can contain
    /// a combination of valid literal path and wildcard (* and ?) characters, but it
    /// doesn't support regular expressions.
    /// </param>
    /// <param name="searchOption">
    /// One of the enumeration values that specifies whether the search operation should
    /// include only the current directory or all subdirectories. The default value is
    /// <see cref="SearchOption.TopDirectoryOnly"/>.
    /// </param>
    /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
    IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Returns an enumerable collection of file system information in the current directory.
    /// </summary>
    /// <returns>An enumerable collection of file system information in the current directory.</returns>
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();

    /// <summary>
    /// Returns an enumerable collection of file system information that matches a specified
    /// search pattern.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/>.</returns>
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern);

    /// <summary>
    /// Returns an enumerable collection of file system information that matches a specified
    /// search pattern and search subdirectory option.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="searchOption">
    /// One of the enumeration values that specifies whether the search operation should
    /// include only the current directory or all subdirectories. The default value is
    /// <see cref="SearchOption.TopDirectoryOnly"/>.
    /// </param>
    /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Returns an enumerable collection of directory information in the current directory.
    /// </summary>
    /// <returns>An enumerable collection of directories in the current directory.</returns>
    IEnumerable<IDirectoryInfo> EnumerateDirectories();

    /// <summary>
    /// Returns an enumerable collection of directory information that matches a specified
    /// search pattern.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/>.</returns>
    IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern);

    /// <summary>
    /// Returns an enumerable collection of directory information that matches a specified
    /// search pattern and search subdirectory option.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="searchOption">
    /// One of the enumeration values that specifies whether the search operation should
    /// include only the current directory or all subdirectories. The default value is
    /// <see cref="SearchOption.TopDirectoryOnly"/>.
    /// </param>
    /// <returns>An enumerable collection of directories that matches searchPattern and searchOption.</returns>
    IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption);

    /// <summary>
    /// Moves a <see cref="IDirectoryInfo"/> instance and its contents to a new path.
    /// </summary>
    /// <param name="destDirName">
    /// The name and path to which to move this directory. The destination cannot be
    /// another disk volume or a directory with the identical name. It can be an existing
    /// directory to which you want to add this directory as a subdirectory.
    /// </param>
    void MoveTo(string destDirName);

    /// <summary>
    /// Deletes this instance of a <see cref="IDirectoryInfo"/>, specifying whether to delete
    /// subdirectories and files.
    /// </summary>
    /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
    void Delete(bool recursive);

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)

    /// <summary>
    /// Returns a file list from the current directory matching the specified search
    /// pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of files. This parameter can contain
    /// a combination of valid literal path and wildcard (* and ?) characters, but it
    /// doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An array of strongly typed <see cref="IFileInfo"/> objects that match <paramref name="searchPattern"/> and <paramref name="enumerationOptions"/>.</returns>
    IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>
    /// Retrieves an array of strongly typed System.IO.FileSystemInfo objects representing
    /// the files and subdirectories that match the specified search pattern and enumeration
    /// options.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories and files. This parameter
    /// can contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An array of strongly typed <see cref="IFileSystemInfo"/> objects matching <paramref name="searchPattern"/> and <paramref name="enumerationOptions"/>.</returns>
    IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>
    /// Returns an array of directories in the current System.IO.DirectoryInfo matching
    /// the specified search pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An array of type <see cref="IDirectoryInfo"/> matching searchPattern and enumerationOptions.</returns>
    IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>
    /// Returns an enumerable collection of file information that matches the specified
    /// search pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of files. This parameter can contain
    /// a combination of valid literal path and wildcard (* and ?) characters, but it
    /// doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="enumerationOptions"/>.</returns>
    IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>
    /// Returns an enumerable collection of file system information that matches the
    /// specified search pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/> and <paramref name="enumerationOptions"/>.</returns>
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions);

    /// <summary>
    /// Returns an enumerable collection of directory information that matches the specified
    /// search pattern and enumeration options.
    /// </summary>
    /// <param name="searchPattern">
    /// The search string to match against the names of directories. This parameter can
    /// contain a combination of valid literal path and wildcard (* and ?) characters,
    /// but it doesn't support regular expressions.
    /// </param>
    /// <param name="enumerationOptions">An object that describes the search and enumeration configuration to use.</param>
    /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="enumerationOptions"/>.</returns>
    IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions);

#endif
}
