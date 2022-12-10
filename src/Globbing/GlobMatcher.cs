using DotNet.Globbing;

namespace MaSch.Globbing;

/// <summary>
/// Default implementation of the <see cref="IGlobMatcher"/> interface.
/// </summary>
public class GlobMatcher : IGlobMatcher
{
    private static readonly char[] _pathSeparators = new char[2] { '/', '\\' };

    private readonly GlobOptions _options;
    private readonly List<WrappedGlob> _globs = new();
    private readonly object _globsLock = new();

    private string? _pathRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobMatcher"/> class.
    /// </summary>
    public GlobMatcher()
        : this(GlobOptions.Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobMatcher"/> class.
    /// </summary>
    /// <param name="options">The options to use for globs registered with this <see cref="GlobMatcher"/>.</param>
    public GlobMatcher(GlobOptions options)
    {
        _options = options;
    }

    /// <inheritdoc/>
    public virtual string? PathRoot
    {
        get => _pathRoot;
        set => _pathRoot = NormalizePathRoot(value);
    }

    /// <inheritdoc/>
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "False positive")]
    public void Add(string globPattern)
    {
        if (globPattern is null or [])
            return;

        if (globPattern[0] == '!')
            Add(globPattern[1..], false);
        else
            Add(globPattern, true);
    }

    /// <inheritdoc/>
    public void AddInclude(string globPattern)
        => Add(globPattern, true);

    /// <inheritdoc/>
    public void AddInclude(Action<GlobBuilder> globBuilder)
        => Add(globBuilder, true);

    /// <inheritdoc/>
    public void AddInclue(Glob glob)
        => Add(glob, true);

    /// <inheritdoc/>
    public void AddExclude(string globPattern)
        => Add(globPattern, false);

    /// <inheritdoc/>
    public void AddExclude(Action<GlobBuilder> globBuilder)
        => Add(globBuilder, false);

    /// <inheritdoc/>
    public void AddExclude(Glob glob)
        => Add(glob, false);

    /// <inheritdoc/>
    public bool IsMatch(string value)
    {
        return new Matcher(_pathRoot, _options, GetGlobs).IsMatch(value);
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public bool IsMatch(ReadOnlySpan<char> value)
    {
        return new Matcher(_pathRoot, _options, GetGlobs).IsMatch(value);
    }
#endif

    /// <inheritdoc/>
    public IMatcher ForPathRoot(string pathRoot)
    {
        return new Matcher(NormalizePathRoot(pathRoot), _options, GetGlobs);
    }

    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "False positive")]
    private static string? NormalizePathRoot(string? value)
    {
        return value switch
        {
            null or [] => null,
            [.., '/' or '\\'] => value,
            _ => value + '/',
        };
    }

    private WrappedGlob[] GetGlobs()
    {
        lock (_globsLock)
            return _globs.ToArray();
    }

    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "False positive")]
    private void Add(string globPattern, bool isInclude)
    {
        if (globPattern is null or [])
            return;

        if (!_pathSeparators.Any(globPattern.Contains))
            globPattern = "**/" + globPattern;
        else if (_pathSeparators.Any(globPattern.EndsWith))
            globPattern = globPattern.TrimStart(_pathSeparators) + "**/*";
        else
            globPattern = globPattern.TrimStart(_pathSeparators);

        lock (_globsLock)
            _globs.Add(new WrappedGlob(Glob.Parse(globPattern, _options), isInclude));
    }

    private void Add(Action<GlobBuilder> globBuilder, bool isInclude)
    {
        var builder = new GlobBuilder();
        globBuilder(builder);
        if (builder.Tokens.Count <= 0)
            return;

        lock (_globsLock)
            _globs.Add(new WrappedGlob(builder.ToGlob(_options), isInclude));
    }

    private void Add(Glob glob, bool isInclude)
    {
        lock (_globsLock)
            _globs.Add(new WrappedGlob(EnsureOptions(glob), isInclude));
    }

    private Glob EnsureOptions(Glob glob)
    {
        return new Glob(_options, glob.Tokens);
    }

    private readonly struct Matcher : IMatcher
    {
        private readonly string? _pathRoot;
        private readonly GlobOptions _options;
        private readonly Func<WrappedGlob[]> _getGlobs;

        public Matcher(string? pathRoot, GlobOptions options, Func<WrappedGlob[]> getGlobs)
        {
            _pathRoot = pathRoot;
            _options = options;
            _getGlobs = getGlobs;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        public bool IsMatch(string value)
        {
            return IsMatch((ReadOnlySpan<char>)value);
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "False positive")]
        public bool IsMatch(ReadOnlySpan<char> value)
#else
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "False positive")]
        public bool IsMatch(string value)
#endif
        {
            if (value is [])
                return false;

            if (_pathRoot is not null)
            {
                if (!IsSubPath(_pathRoot, value))
                    return false;
                value = value.Slice(_pathRoot.Length);
            }

            if (value[0] is '/' or '\\')
                value = value.Slice(1);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            Span<WrappedGlob> globs = _getGlobs();
#else
            var globs = _getGlobs();
#endif
            bool isMatch = false;
            for (int i = 0; i < globs.Length; i++)
            {
                if (globs[i].IsInclude)
                    isMatch |= globs[i].Glob.IsMatch(value);
                else if (globs[i].Glob.IsMatch(value))
                    isMatch = false;
            }

            return isMatch;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        private static bool IsSubPathCaseSensitive(ReadOnlySpan<char> rootPath, ReadOnlySpan<char> path)
#else
        private static bool IsSubPathCaseSensitive(string rootPath, string path)
#endif
        {
            for (int i = 0; i < rootPath.Length; i++)
            {
                if (rootPath[i] != path[i] && !(rootPath[i] is '/' or '\\' && path[i] is '/' or '\\'))
                    return false;
            }

            return true;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        private static bool IsSubPathCaseInsensitive(ReadOnlySpan<char> rootPath, ReadOnlySpan<char> path)
#else
        private static bool IsSubPathCaseInsensitive(string rootPath, string path)
#endif
        {
            for (int i = 0; i < rootPath.Length; i++)
            {
                if (rootPath[i] != path[i] &&
                    !(char.IsLetter(rootPath[i]) && char.IsLetter(path[i]) && rootPath[i] - path[i] is 32 or -32) &&
                    !(rootPath[i] is '/' or '\\' && path[i] is '/' or '\\'))
                {
                    return false;
                }
            }

            return true;
        }

        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "False positive")]
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        private bool IsSubPath(ReadOnlySpan<char> rootPath, ReadOnlySpan<char> path)
#else
        private bool IsSubPath(string rootPath, string path)
#endif
        {
            if (rootPath is [])
                return true;
            if (path.Length < rootPath.Length)
                return false;

            if (_options.Evaluation.CaseInsensitive)
                return IsSubPathCaseInsensitive(rootPath, path);
            else
                return IsSubPathCaseSensitive(rootPath, path);
        }
    }

    private sealed class WrappedGlob
    {
        public WrappedGlob(Glob glob, bool isInclude)
        {
            Glob = glob;
            IsInclude = isInclude;
        }

        public Glob Glob { get; }
        public bool IsInclude { get; }

        public override string ToString()
        {
            return $"{(IsInclude ? string.Empty : "!")}{Glob}";
        }
    }
}
