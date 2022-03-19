using MaSch.FileSystem.InMemory.Models;
using System.Runtime.InteropServices;

namespace MaSch.FileSystem.InMemory;

public class InMemoryFileSystemService : FileSystemServiceBase
{
    internal static readonly char[] DirectorySeperatorChars = GetDirectorySeperatorChars();
    internal static readonly StringComparison PathComparison = GetPathComparison();
    internal static readonly StringComparer PathComparer = GetPathComparer(PathComparison);
    internal static readonly bool IsPathCaseSensitive = PathComparison == StringComparison.Ordinal;

    internal Dictionary<string, RootNode> Nodes { get; } = new(PathComparer);

    internal bool TryGetNode<T>(string? path, [NotNullWhen(true)] out T? node)
        where T : FileSystemNode
    {
        if (TryGetNode(path, out var fileSystemNode) && fileSystemNode is T castedNode)
        {
            node = castedNode;
            return true;
        }

        node = null;
        return false;
    }

    internal bool TryGetNode(string? path, [NotNullWhen(true)] out FileSystemNode? node)
    {
        if (!TryGetRootAndSegments(path, out var rootNode, out var segments))
        {
            node = null;
            return false;
        }

        return TryGetSubNode(rootNode, segments, out node);
    }

    internal bool TryGetSubNode(ContainerNode parentNode, string subPath, [NotNullWhen(true)] out FileSystemNode? node)
    {
        var segments = subPath.Split(DirectorySeperatorChars, StringSplitOptions.RemoveEmptyEntries);
        return TryGetSubNode(parentNode, segments, out node);
    }

    internal bool TryGetSubNode(ContainerNode parentNode, string[] segments, [NotNullWhen(true)] out FileSystemNode? node)
    {
        ContainerNode currentNode = parentNode;
        for (int i = 0; i < segments.Length; i++)
        {
            var segment = segments[i];

            if (currentNode.Directories.TryGetByName(segment, out var dirNode))
            {
                currentNode = dirNode;
            }
            else if (i == segment.Length - 1 && currentNode.Files.TryGetByName(segment, out var fileNode))
            {
                node = fileNode;
                return true;
            }
            else
            {
                node = null;
                return false;
            }
        }

        node = currentNode;
        return true;
    }

    internal bool TryGetRootAndSegments(string? path, [NotNullWhen(true)] out RootNode? rootNode, [NotNullWhen(true)] out string[]? segments)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            rootNode = null;
            segments = null;
            return false;
        }

        var pathRoot = Path.GetPathRoot(path);
        if (pathRoot is null || !Nodes.TryGetValue(pathRoot, out rootNode))
        {
            rootNode = null;
            segments = null;
            return false;
        }

        segments = path[pathRoot.Length..].Split(DirectorySeperatorChars, StringSplitOptions.RemoveEmptyEntries);
        return true;
    }

    protected override IDirectoryService CreateDirectoryService()
    {
        throw new NotImplementedException();
    }

    protected override IFileService CreateFileService()
    {
        throw new NotImplementedException();
    }

    private static char[] GetDirectorySeperatorChars()
    {
        if (Path.DirectorySeparatorChar == Path.AltDirectorySeparatorChar)
            return new char[] { Path.DirectorySeparatorChar };
        else
            return new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
    }

    private static StringComparison GetPathComparison()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return StringComparison.Ordinal;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return StringComparison.OrdinalIgnoreCase;
        else
            throw new PlatformNotSupportedException();
    }

    private static StringComparer GetPathComparer(StringComparison comparison)
    {
#if NETFRAMEWORK || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
        return comparison switch
        {
            StringComparison.CurrentCulture => StringComparer.CurrentCulture,
            StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
            StringComparison.InvariantCulture => StringComparer.InvariantCulture,
            StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
            StringComparison.Ordinal => StringComparer.Ordinal,
            StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
            _ => throw new ArgumentOutOfRangeException(nameof(comparison)),
        };
#else
        return StringComparer.FromComparison(comparison);
#endif
    }
}
