using MaSch.Core;
using System.IO.Compression;

namespace MaSch.Globbing;

/// <summary>
/// Provides extensions for the <see cref="IGlobMatcher"/> interface.
/// </summary>
public static class GlobMatcherExtensions
{
    /// <summary>
    /// Adds multiple glob patterns to this <see cref="IGlobMatcher"/>. Prefix the pattern with "!" to add it as an excluded pattern.
    /// </summary>
    /// <param name="globMatcher">The glob matcher to add the patterns to.</param>
    /// <param name="globPatterns">The patterns to add.</param>
    public static void AddRange(IGlobMatcher globMatcher, IEnumerable<string> globPatterns)
    {
        Guard.NotNull(globMatcher);
        Guard.NotNull(globPatterns);

        foreach (var pattern in globPatterns)
            globMatcher.Add(pattern);
    }

    /// <summary>
    /// Matches all files in all sub-directories of a specified directory.
    /// </summary>
    /// <param name="matcher">The matcher to use.</param>
    /// <param name="directoryPath">The directory to match files in.</param>
    /// <returns>Returns the file paths to all files that matched the <paramref name="matcher"/> inside <paramref name="directoryPath"/> and all its sub-directories.</returns>
    public static IEnumerable<string> MatchFilesInDirectory(this IGlobMatcher matcher, string directoryPath)
        => MatchFilesInDirectory(matcher, directoryPath, SearchOption.AllDirectories);

    /// <summary>
    /// Matches files of a specified directory.
    /// </summary>
    /// <param name="matcher">The matcher to use.</param>
    /// <param name="directoryPath">The directory to match files in.</param>
    /// <param name="searchOption">The search option to use for file enumeration.</param>
    /// <returns>Returns the file paths to all files that matched the <paramref name="matcher"/> inside <paramref name="directoryPath"/> using the provided <paramref name="searchOption"/>.</returns>
    public static IEnumerable<string> MatchFilesInDirectory(this IGlobMatcher matcher, string directoryPath, SearchOption searchOption)
    {
        Guard.NotNull(matcher);
        Guard.NotNull(directoryPath);

        var fileMatcher = matcher.ForPathRoot(directoryPath);
        return Directory
            .EnumerateFiles(directoryPath, "*", searchOption)
            .Where(fileMatcher.IsMatch);
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <summary>
    /// Matches files of a specified directory.
    /// </summary>
    /// <param name="matcher">The matcher to use.</param>
    /// <param name="directoryPath">The directory to match files in.</param>
    /// <param name="enumerationOptions">The options to use for file enumeration.</param>
    /// <returns>Returns the file paths to all files that matched the <paramref name="matcher"/> inside <paramref name="directoryPath"/> using the provided <paramref name="enumerationOptions"/>.</returns>
    public static IEnumerable<string> MatchFilesInDirectory(this IGlobMatcher matcher, string directoryPath, EnumerationOptions enumerationOptions)
    {
        Guard.NotNull(matcher);
        Guard.NotNull(directoryPath);
        Guard.NotNull(enumerationOptions);

        var fileMatcher = matcher.ForPathRoot(directoryPath);
        return Directory
            .EnumerateFiles(directoryPath, "*", enumerationOptions)
            .Where(fileMatcher.IsMatch);
    }
#endif

    /// <summary>
    /// Matchs all files in a specified <see cref="ZipArchive"/>.
    /// </summary>
    /// <param name="matcher">The matcher to use.</param>
    /// <param name="zipArchive">The zip archive to match files in.</param>
    /// <returns>Returns an enumerator of <see cref="ZipArchiveEntry"/> of all entries that macthes the specified options.</returns>
    public static IEnumerable<ZipArchiveEntry> MatchFilesInZip(this IGlobMatcher matcher, ZipArchive zipArchive)
        => MatchFilesInZip(matcher, zipArchive, null);

    /// <summary>
    /// Matchs all files in a specified <see cref="ZipArchive"/>.
    /// </summary>
    /// <param name="matcher">The matcher to use.</param>
    /// <param name="zipArchive">The zip archive to match files in.</param>
    /// <param name="pathRoot">The path root for the match. Only files prefixed with that path are matched.</param>
    /// <returns>Returns an enumerator of <see cref="ZipArchiveEntry"/> of all entries that macthes the specified options.</returns>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "False positive")]
    public static IEnumerable<ZipArchiveEntry> MatchFilesInZip(this IGlobMatcher matcher, ZipArchive zipArchive, string? pathRoot)
    {
        Guard.NotNull(matcher);
        Guard.NotNull(zipArchive);

        IMatcher actualMatcher = matcher;
        if (pathRoot is not (null or []))
            actualMatcher = matcher.ForPathRoot(pathRoot);

        return zipArchive.Entries
            .Where(x => !IsDirectory(x))
            .Where(x => actualMatcher.IsMatch(x.FullName));

        static bool IsDirectory(ZipArchiveEntry entry)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET472_OR_GREATER
            return ((FileAttributes)entry.ExternalAttributes).HasFlag(FileAttributes.Directory);
#else
            return entry.FullName.EndsWith("/") && entry.Length == 0;
#endif
        }
    }
}