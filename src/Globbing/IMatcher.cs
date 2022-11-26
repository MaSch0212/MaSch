namespace MaSch.Globbing;

/// <summary>
/// Represents a matcher that determines whether a <see cref="string"/> matches a specific condition.
/// </summary>
public interface IMatcher
{
    /// <summary>
    /// Matches a specified <see cref="string"/> to the condition of this <see cref="IMatcher"/>.
    /// </summary>
    /// <param name="value">The value to match.</param>
    /// <returns><c>true</c> if <paramref name="value"/> matches the condition of this <see cref="IMatcher"/>; otherwise <c>false</c>.</returns>
    bool IsMatch(string value);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <summary>
    /// Matches a specified <see cref="ReadOnlySpan{T}"/> of <see cref="char"/> to the condition of this <see cref="IMatcher"/>.
    /// </summary>
    /// <param name="value">The value to match.</param>
    /// <returns><c>true</c> if <paramref name="value"/> matches the condition of this <see cref="IMatcher"/>; otherwise <c>false</c>.</returns>
    bool IsMatch(ReadOnlySpan<char> value);
#endif
}
