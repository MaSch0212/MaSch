using MaSch.FileSystem.InMemory.Models;

#pragma warning disable S4136 // Method overloads should be grouped together

namespace MaSch.FileSystem.InMemory;

internal class InMemoryDirectoryService : DirectoryServiceBase
{
    private readonly InMemoryFileSystemService _fileSystem;

    public InMemoryDirectoryService(InMemoryFileSystemService fileSystem)
        : base(fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public override IDirectoryInfo CreateDirectory(string path)
    {
        if (!_fileSystem.TryGetRootAndSegments(path, out var rootNode, out var segments))
            throw new DirectoryNotFoundException($"Could not find a part of the path '{path}'.");

        ContainerNode currentNode = rootNode;
        bool startedCreating = false;
        foreach (var segment in segments)
        {
            if (startedCreating || !currentNode.Directories.TryGetByName(segment, out var dirNode))
            {
                startedCreating = true;
                dirNode = new DirectoryNode(rootNode, currentNode as DirectoryNode, segment);
                currentNode.Directories.Add(dirNode);
            }

            currentNode = dirNode;
        }

        return new InMemoryDirectoryInfo(_fileSystem, path, currentNode);
    }

    public override void Delete(string path, bool recursive)
    {
        var node = GetDirectoryNode(path);
        if (!recursive && node.ChildNodes.Count > 0)
            throw new IOException($"The directory is not empty. : '{path}'");

        node.Parent.Directories.Remove(node);
    }

    public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
    {
        var (node, regex) = PrepareFileSystemEntryEnumeration(path, searchPattern, MatchCasing.PlatformDefault);
        if (node is null || regex is null)
            return Array.Empty<string>();
        IEnumerable<DirectoryNode> baseList = searchOption switch
        {
            SearchOption.TopDirectoryOnly => node.Directories,
            SearchOption.AllDirectories => node.GetAllSubDirectories(),
            _ => throw new ArgumentOutOfRangeException(nameof(searchOption)),
        };
        return baseList.Where(x => regex.IsMatch(x.Name)).Select(x => x.Path);
    }

    public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
    {
        var (node, regex) = PrepareFileSystemEntryEnumeration(path, searchPattern, MatchCasing.PlatformDefault);
        if (node is null || regex is null)
            return Array.Empty<string>();
        IEnumerable<FileNode> baseList = searchOption switch
        {
            SearchOption.TopDirectoryOnly => node.Files,
            SearchOption.AllDirectories => node.GetAllFiles(),
            _ => throw new ArgumentOutOfRangeException(nameof(searchOption)),
        };
        return baseList.Where(x => regex.IsMatch(x.Name)).Select(x => x.Path);
    }

    public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
    {
        var (node, regex) = PrepareFileSystemEntryEnumeration(path, searchPattern, MatchCasing.PlatformDefault);
        if (node is null || regex is null)
            return Array.Empty<string>();
        IEnumerable<FileSystemNode> baseList = searchOption switch
        {
            SearchOption.TopDirectoryOnly => node.ChildNodes,
            SearchOption.AllDirectories => node.GetAllFileSystemNodes(),
            _ => throw new ArgumentOutOfRangeException(nameof(searchOption)),
        };
        return baseList.Where(x => regex.IsMatch(x.Name)).Select(x => x.Path);
    }

    public override bool Exists([NotNullWhen(true)] string? path)
    {
        return _fileSystem.TryGetNode<DirectoryNode>(path, out _);
    }

    public override DateTime GetCreationTimeUtc(string path)
    {
        return GetDirectoryNode(path).CreationTimeUtc;
    }

    public override string GetCurrentDirectory()
    {
        return Environment.CurrentDirectory;
    }

    public override IDirectoryInfo GetInfo(string path)
    {
        return new InMemoryDirectoryInfo(_fileSystem, path);
    }

    public override DateTime GetLastAccessTimeUtc(string path)
    {
        return GetDirectoryNode(path).LastAccessTimeUtc;
    }

    public override DateTime GetLastWriteTimeUtc(string path)
    {
        return GetDirectoryNode(path).LastWriteTimeUtc;
    }

    public override void Move(string sourceDirName, string destDirName)
    {
        var nodeToMove = GetDirectoryNode(sourceDirName);
        if (_fileSystem.TryGetNode(destDirName, out _))
            throw new IOException($"Cannot create '{destDirName}' because a file or directory with the same name already exists.");
        if (!_fileSystem.TryGetNode<ContainerNode>(Path.GetDirectoryName(destDirName), out var destParentNode))
            throw new DirectoryNotFoundException($"Could not find a part of the path '{Path.GetDirectoryName(destDirName)}'.");

        if (destParentNode != nodeToMove.Parent)
        {
            nodeToMove.Parent.Directories.Remove(nodeToMove);
            destParentNode.Directories.Add(nodeToMove);
            nodeToMove.Root = destParentNode.Root;
            nodeToMove.ParentDirectory = destParentNode as DirectoryNode;
        }

        var destDirNameOnly = Path.GetFileName(destDirName);
        if (nodeToMove.Name != destDirNameOnly)
            nodeToMove.Name = destDirNameOnly;
    }

    public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
    {
        GetDirectoryNode(path).CreationTimeUtc = creationTimeUtc;
    }

    public override void SetCurrentDirectory(string path)
    {
        Environment.CurrentDirectory = path;
    }

    public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
    {
        GetDirectoryNode(path).LastAccessTimeUtc = lastAccessTimeUtc;
    }

    public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
    {
        GetDirectoryNode(path).LastWriteTimeUtc = lastWriteTimeUtc;
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)
    public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
    {
        var (node, regex) = PrepareFileSystemEntryEnumeration(path, searchPattern, enumerationOptions.MatchCasing);
        if (node is null || regex is null)
            return Array.Empty<string>();
        IEnumerable<DirectoryNode> baseList = enumerationOptions.RecurseSubdirectories
#if NET6_0_OR_GREATER
            ? node.GetAllSubDirectories(enumerationOptions.MaxRecursionDepth)
#else
            ? node.GetAllSubDirectories()
#endif
            : node.Directories;
        return baseList.Where(x => regex.IsMatch(x.Name) && !x.Attributes.HasFlag(enumerationOptions.AttributesToSkip)).Select(x => x.Path);
    }

    public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
    {
        var (node, regex) = PrepareFileSystemEntryEnumeration(path, searchPattern, enumerationOptions.MatchCasing);
        if (node is null || regex is null)
            return Array.Empty<string>();
        IEnumerable<FileNode> baseList = enumerationOptions.RecurseSubdirectories
#if NET6_0_OR_GREATER
            ? node.GetAllFiles(enumerationOptions.MaxRecursionDepth)
#else
            ? node.GetAllFiles()
#endif
            : node.Files;
        return baseList.Where(x => regex.IsMatch(x.Name) && !x.Attributes.HasFlag(enumerationOptions.AttributesToSkip)).Select(x => x.Path);
    }

    public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
    {
        var (node, regex) = PrepareFileSystemEntryEnumeration(path, searchPattern, enumerationOptions.MatchCasing);
        if (node is null || regex is null)
            return Array.Empty<string>();
        IEnumerable<FileSystemNode> baseList = enumerationOptions.RecurseSubdirectories
#if NET6_0_OR_GREATER
            ? node.GetAllFileSystemNodes(enumerationOptions.MaxRecursionDepth)
#else
            ? node.GetAllFileSystemNodes()
#endif
            : node.ChildNodes;
        return baseList.Where(x => regex.IsMatch(x.Name) && !x.Attributes.HasFlag(enumerationOptions.AttributesToSkip)).Select(x => x.Path);
    }
#endif

    private DirectoryNode GetDirectoryNode(string? path)
    {
        if (!_fileSystem.TryGetNode<DirectoryNode>(path, out var node))
            throw new DirectoryNotFoundException($"Could not find a part of the path '{path}'.");
        return node;
    }

    private (DirectoryNode? Node, Regex? FileNameRegex) PrepareFileSystemEntryEnumeration(string path, string searchPattern, MatchCasing casing)
    {
        var node = GetDirectoryNode(path);
        var (subDirSegments, pattern) = GetActualSearchPattern(searchPattern);
        if (subDirSegments.Length > 0)
        {
            if (_fileSystem.TryGetSubNode(node, subDirSegments, out var subNode) && subNode is DirectoryNode subDirNode)
                node = subDirNode;
            else
                return (null, null);
        }

        var regexPattern = GetSearchPatternRegex(pattern);
        RegexOptions options = RegexOptions.None;
        if (casing == MatchCasing.CaseInsensitive || (casing == MatchCasing.PlatformDefault && !InMemoryFileSystemService.IsPathCaseSensitive))
            options |= RegexOptions.IgnoreCase;

        return (node, new Regex(regexPattern, options));
    }

    private (string[] SubDirSegments, string SearchPattern) GetActualSearchPattern(string searchPattern)
    {
        var patternSplit = searchPattern.Split(InMemoryFileSystemService.DirectorySeperatorChars, StringSplitOptions.RemoveEmptyEntries);
        if (patternSplit.Length > 1)
        {
            return (patternSplit.Take(patternSplit.Length - 1).ToArray(), patternSplit[^1]);
        }
        else
        {
            return (Array.Empty<string>(), searchPattern);
        }
    }

    private string GetSearchPatternRegex(string searchPattern)
    {
        return $"^{Regex.Escape(searchPattern).Replace("\\?", ".").Replace("\\*", ".*")}$";
    }
}
