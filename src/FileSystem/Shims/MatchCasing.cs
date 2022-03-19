#if NETFRAMEWORK || (NETSTANDARD && !NETSTANDARD2_1_OR_GREATER)
namespace System.IO;

internal enum MatchCasing
{
    PlatformDefault,
    CaseSensitive,
    CaseInsensitive
}
#endif