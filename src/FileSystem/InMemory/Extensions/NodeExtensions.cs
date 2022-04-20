using MaSch.FileSystem.InMemory.Models;

namespace MaSch.FileSystem.InMemory;

internal static class NodeExtensions
{
    internal static bool TryGetByName<TNode>(this IEnumerable<TNode> enumerable, string name, [NotNullWhen(true)] out TNode? node)
        where TNode : FileSystemNode
    {
        var comparer = InMemoryFileSystemService.PathComparer;
        return enumerable.TryFirst<TNode>(x => comparer.Equals(x.Name, name), out node);
    }
}
