#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem;

public interface IDirectoryInfo : IFileSystemInfo
{
    IDirectoryInfo? Parent { get; }
    IDirectoryInfo Root { get; }

    IDirectoryInfo CreateSubdirectory(string path);
    void Create();
    IFileInfo[] GetFiles();
    IFileInfo[] GetFiles(string searchPattern);
    IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption);
    IFileSystemInfo[] GetFileSystemInfos();
    IFileSystemInfo[] GetFileSystemInfos(string searchPattern);
    IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption);
    IDirectoryInfo[] GetDirectories();
    IDirectoryInfo[] GetDirectories(string searchPattern);
    IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption);
    IEnumerable<IFileInfo> EnumerateFiles();
    IEnumerable<IFileInfo> EnumerateFiles(string searchPattern);
    IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption);
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern);
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption);
    IEnumerable<IDirectoryInfo> EnumerateDirectories();
    IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern);
    IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption);
    void MoveTo(string destDirName);
    void Delete(bool recursive);

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions);
    IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions);
    IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions);
    IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions);
    IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions);
    IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions);
#endif
}
