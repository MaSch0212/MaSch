#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_1_OR_GREATER

namespace MaSch.Globbing;

internal static class InternalStringExtensions
{
    public static string Slice(this string str, int start)
    {
        return str.Substring(start);
    }

    public static bool EndsWith(this string str, char endChar)
    {
        return str[str.Length - 1] == endChar;
    }
}

#endif