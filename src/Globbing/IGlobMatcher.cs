using DotNet.Globbing;

namespace MaSch.Globbing;

/// <summary>
/// Represents a matcher that uses glob patterns as condition.
/// </summary>
public interface IGlobMatcher : IMatcher
{
    /// <summary>
    /// Gets or sets the path root of all matched values.
    /// Glob patterns will be applied for files in that path using the relative path.
    /// </summary>
    string? PathRoot { get; set; }

    /// <summary>
    /// Adds a new glob pattern to this <see cref="IGlobMatcher"/>. Patterns that are prefixed by '!' are added as excludes automatically.
    /// </summary>
    /// <param name="globPattern">The glob pattern to add.</param>
    void Add(string globPattern);

    /// <summary>
    /// Adds a new glob pattern to this <see cref="IGlobMatcher"/> that excludes specific files.
    /// </summary>
    /// <param name="globBuilder">The glob builder function to configure the glob pattern.</param>
    void AddExclude(Action<GlobBuilder> globBuilder);

    /// <summary>
    /// Adds a new glob pattern to this <see cref="IGlobMatcher"/> that excludes specific files.
    /// </summary>
    /// <param name="glob">The glob to add as exclude.</param>
    void AddExclude(Glob glob);

    /// <summary>
    /// Adds a new glob pattern to this <see cref="IGlobMatcher"/> that excludes specific files.
    /// </summary>
    /// <param name="globPattern">The glob pattern to add as exclude.</param>
    void AddExclude(string globPattern);

    /// <summary>
    /// Adds a new glob pattern to this <see cref="IGlobMatcher"/> that includes specific files.
    /// </summary>
    /// <param name="globBuilder">The glob builder function to configure the glob pattern.</param>
    void AddInclude(Action<GlobBuilder> globBuilder);

    /// <summary>
    /// Adds a new glob pattern to this <see cref="IGlobMatcher"/> that includes specific files.
    /// </summary>
    /// <param name="globPattern">The glob pattern to add as include.</param>
    void AddInclude(string globPattern);

    /// <summary>
    /// Adds a new glob pattern to this <see cref="IGlobMatcher"/> that includes specific files.
    /// </summary>
    /// <param name="glob">The glob to add as include.</param>
    void AddInclue(Glob glob);

    /// <summary>
    /// Craetes a <see cref="IMatcher"/> that uses the glob patterns defined by this <see cref="IGlobMatcher"/> but a different <see cref="PathRoot"/>.
    /// </summary>
    /// <param name="pathRoot">The value for the <paramref name="pathRoot"/> of the new matcher.</param>
    /// <returns>Returns a wrapper around this <see cref="IGlobMatcher"/> that uses <paramref name="pathRoot"/> as <see cref="PathRoot"/>.</returns>
    IMatcher ForPathRoot(string pathRoot);
}