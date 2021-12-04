namespace MaSch.Test.Assertion;

/// <summary>
/// Provides assertion methods for <see cref="string"/> to the <see cref="AssertBase"/> class.
/// </summary>
public partial class AssertBase
{
    /// <summary>
    /// Tests whether the specified string is null or an empty string.
    /// </summary>
    /// <param name="actual">The string expected to be null or an empty string.</param>
    public void IsNullOrEmpty(string? actual)
    {
        IsNullOrEmpty(actual, null);
    }

    /// <summary>
    /// Tests whether the specified string is null or an empty string.
    /// </summary>
    /// <param name="actual">The string expected to be null or an empty string.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not null nor an empty string. The message is shown in test results.</param>
    public void IsNullOrEmpty(string? actual, string? message)
    {
        RunAssertion(actual, message, string.IsNullOrEmpty);
    }

    /// <summary>
    /// Tests whether the specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="actual">The string expected to be null, empty, or consists only of white-space characters.</param>
    public void IsNullOrWhitespace(string? actual)
    {
        IsNullOrWhitespace(actual, null);
    }

    /// <summary>
    /// Tests whether the specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="actual">The string expected to be null, empty, or consists only of white-space characters.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not null, empty, nor consists only of white-space characters. The message is shown in test results.</param>
    public void IsNullOrWhitespace(string? actual, string? message)
    {
        RunAssertion(actual, message, string.IsNullOrWhiteSpace);
    }

    /// <summary>
    /// Tests whether the specified string is not null nor an empty string.
    /// </summary>
    /// <param name="actual">The string expected to be not null nor an empty string.</param>
    public void IsNotNullOrEmpty(string? actual)
    {
        IsNotNullOrEmpty(actual, null);
    }

    /// <summary>
    /// Tests whether the specified string is not null nor an empty string.
    /// </summary>
    /// <param name="actual">The string expected to be not null nor an empty string.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> is null or an empty string. The message is shown in test results.</param>
    public void IsNotNullOrEmpty(string? actual, string? message)
    {
        RunNegatedAssertion(actual, message, string.IsNullOrEmpty);
    }

    /// <summary>
    /// Tests whether the specified string is not null, empty, nor consists only of white-space characters.
    /// </summary>
    /// <param name="actual">The string expected to be not null, empty, nor consists only of white-space characters.</param>
    public void IsNotNullOrWhitespace(string? actual)
    {
        IsNotNullOrWhitespace(actual, null);
    }

    /// <summary>
    /// Tests whether the specified string is not null, empty, nor consists only of white-space characters.
    /// </summary>
    /// <param name="actual">The string expected to be not null, empty, nor consists only of white-space characters.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> is null, empty, or consists only of white-space characters. The message is shown in test results.</param>
    public void IsNotNullOrWhitespace(string? actual, string? message)
    {
        RunNegatedAssertion(actual, message, string.IsNullOrWhiteSpace);
    }

    /// <summary>
    /// Tests whether the specified string contains the specified substring
    /// and throws an exception if the substring does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    public void Contains(string expected, string? actual)
    {
        Contains(expected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string contains the specified substring
    /// and throws an exception if the substring does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void Contains(string expected, string? actual, string? message)
    {
        Contains(expected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string contains the specified substring
    /// and throws an exception if the substring does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void Contains(string expected, string? actual, StringComparison comparisonType)
    {
        Contains(expected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string contains the specified substring
    /// and throws an exception if the substring does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void Contains(string expected, string? actual, StringComparison comparisonType, string? message)
    {
        RunAssertion(expected, actual, message, (e, a) => a?.Contains(e, comparisonType) == true);
    }

    /// <summary>
    /// Tests whether the specified string contains any of the specified substrings
    /// and throws an exception if none of the substrings do occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    public void ContainsAny(IEnumerable<string> expected, string? actual)
    {
        ContainsAny(expected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string contains any of the specified substrings
    /// and throws an exception if none of the substrings do occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void ContainsAny(IEnumerable<string> expected, string? actual, string? message)
    {
        ContainsAny(expected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string contains any of the specified substrings
    /// and throws an exception if none of the substrings do occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void ContainsAny(IEnumerable<string> expected, string? actual, StringComparison comparisonType)
    {
        ContainsAny(expected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string contains any of the specified substrings
    /// and throws an exception if none of the substrings do occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void ContainsAny(IEnumerable<string> expected, string? actual, StringComparison comparisonType, string? message)
    {
        RunAssertion(expected, actual, message, (e, a) => e.Any(x => a?.Contains(x, comparisonType) == true));
    }

    /// <summary>
    /// Tests whether the specified string contains all of the specified substrings
    /// and throws an exception if one of the substrings does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    public void ContainsAll(IEnumerable<string> expected, string? actual)
    {
        ContainsAll(expected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string contains all of the specified substrings
    /// and throws an exception if one of the substrings does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void ContainsAll(IEnumerable<string> expected, string? actual, string? message)
    {
        ContainsAll(expected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string contains all of the specified substrings
    /// and throws an exception if one of the substrings does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void ContainsAll(IEnumerable<string> expected, string? actual, StringComparison comparisonType)
    {
        ContainsAll(expected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string contains all of the specified substrings
    /// and throws an exception if one of the substrings does not occur within the
    /// test string.
    /// </summary>
    /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
    /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void ContainsAll(IEnumerable<string> expected, string? actual, StringComparison comparisonType, string? message)
    {
        var missingStrings = expected.Where(x => actual?.Contains(x, comparisonType) != true).ToArray();
        if (missingStrings.Length > 0)
            ThrowAssertError(message, ("Expected", expected), ("Actual", actual), ("MissingStrings", missingStrings));
    }

    /// <summary>
    /// Tests whether the specified string does not contain the specified substring
    /// and throws an exception if the substring occurs within the
    /// test string.
    /// </summary>
    /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
    public void DoesNotContain(string notExpected, string? actual)
    {
        DoesNotContain(notExpected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string does not contain the specified substring
    /// and throws an exception if the substring occurs within the
    /// test string.
    /// </summary>
    /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="notExpected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void DoesNotContain(string notExpected, string? actual, string? message)
    {
        DoesNotContain(notExpected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string does not contain the specified substring
    /// and throws an exception if the substring occurs within the
    /// test string.
    /// </summary>
    /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void DoesNotContain(string notExpected, string? actual, StringComparison comparisonType)
    {
        DoesNotContain(notExpected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string does not contain the specified substring
    /// and throws an exception if the substring occurs within the
    /// test string.
    /// </summary>
    /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
    /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="notExpected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
    public void DoesNotContain(string notExpected, string? actual, StringComparison comparisonType, string? message)
    {
        RunNegatedAssertion(notExpected, actual, message, (e, a) => a?.Contains(e, comparisonType) != false);
    }

    /// <summary>
    /// Tests whether the specified string begins with the specified substring
    /// and throws an exception if the test string does not start with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
    public void StartsWith(string expected, string? actual)
    {
        StartsWith(expected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string begins with the specified substring
    /// and throws an exception if the test string does not start with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="expected" />. The message is shown in test results.</param>
    public void StartsWith(string expected, string? actual, string? message)
    {
        StartsWith(expected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string begins with the specified substring
    /// and throws an exception if the test string does not start with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void StartsWith(string expected, string? actual, StringComparison comparisonType)
    {
        StartsWith(expected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string begins with the specified substring
    /// and throws an exception if the test string does not start with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="expected" />. The message is shown in test results.</param>
    public void StartsWith(string expected, string? actual, StringComparison comparisonType, string? message)
    {
        RunAssertion(expected, actual, message, (e, a) => a?.StartsWith(e, comparisonType) == true);
    }

    /// <summary>
    /// Tests whether the specified string does not begin with the specified substring
    /// and throws an exception if the test string starts with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
    public void DoesNotStartWith(string notExpected, string? actual)
    {
        DoesNotStartWith(notExpected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string does not begin with the specified substring
    /// and throws an exception if the test string starts with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="notExpected" />. The message is shown in test results.</param>
    public void DoesNotStartWith(string notExpected, string? actual, string? message)
    {
        DoesNotStartWith(notExpected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string does not begin with the specified substring
    /// and throws an exception if the test string starts with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void DoesNotStartWith(string notExpected, string? actual, StringComparison comparisonType)
    {
        DoesNotStartWith(notExpected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string does not begin with the specified substring
    /// and throws an exception if the test string starts with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="notExpected" />. The message is shown in test results.</param>
    public void DoesNotStartWith(string notExpected, string? actual, StringComparison comparisonType, string? message)
    {
        RunNegatedAssertion(notExpected, actual, message, (e, a) => a?.StartsWith(e, comparisonType) != false);
    }

    /// <summary>
    /// Tests whether the specified string ends with the specified substring
    /// and throws an exception if the test string does not end with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
    public void EndsWith(string expected, string? actual)
    {
        EndsWith(expected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string ends with the specified substring
    /// and throws an exception if the test string does not end with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="expected" />. The message is shown in test results.</param>
    public void EndsWith(string expected, string? actual, string? message)
    {
        EndsWith(expected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string ends with the specified substring
    /// and throws an exception if the test string does not end with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void EndsWith(string expected, string? actual, StringComparison comparisonType)
    {
        EndsWith(expected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string ends with the specified substring
    /// and throws an exception if the test string does not end with the
    /// substring.
    /// </summary>
    /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="expected" />. The message is shown in test results.</param>
    public void EndsWith(string expected, string? actual, StringComparison comparisonType, string? message)
    {
        RunAssertion(expected, actual, message, (e, a) => a?.EndsWith(e, comparisonType) == true);
    }

    /// <summary>
    /// Tests whether the specified string does not end with the specified substring
    /// and throws an exception if the test string ends with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
    public void DoesNotEndWith(string notExpected, string? actual)
    {
        DoesNotEndWith(notExpected, actual, StringComparison.Ordinal, null);
    }

    /// <summary>
    /// Tests whether the specified string does not end with the specified substring
    /// and throws an exception if the test string ends with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="notExpected" />. The message is shown in test results.</param>
    public void DoesNotEndWith(string notExpected, string? actual, string? message)
    {
        DoesNotEndWith(notExpected, actual, StringComparison.Ordinal, message);
    }

    /// <summary>
    /// Tests whether the specified string does not end with the specified substring
    /// and throws an exception if the test string ends with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    public void DoesNotEndWith(string notExpected, string? actual, StringComparison comparisonType)
    {
        DoesNotEndWith(notExpected, actual, comparisonType, null);
    }

    /// <summary>
    /// Tests whether the specified string does not end with the specified substring
    /// and throws an exception if the test string ends with the
    /// substring.
    /// </summary>
    /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
    /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
    /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="notExpected" />. The message is shown in test results.</param>
    public void DoesNotEndWith(string notExpected, string? actual, StringComparison comparisonType, string? message)
    {
        RunNegatedAssertion(notExpected, actual, message, (e, a) => a?.EndsWith(e, comparisonType) != false);
    }

    /// <summary>
    /// Tests whether the specified string matches a regular expression and
    /// throws an exception if the string does not match the expression.
    /// </summary>
    /// <param name="expected">The regular expression that <paramref name="actual" /> is expected to match.</param>
    /// <param name="actual">The string that is expected to match <paramref name="expected" />.</param>
    public void Matches(Regex expected, string? actual)
    {
        Matches(expected, actual, null);
    }

    /// <summary>
    /// Tests whether the specified string matches a regular expression and
    /// throws an exception if the string does not match the expression.
    /// </summary>
    /// <param name="expected">The regular expression that <paramref name="actual" /> is expected to match.</param>
    /// <param name="actual">The string that is expected to match <paramref name="expected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not match <paramref name="expected" />. The message is shown in test results.</param>
    public void Matches(Regex expected, string? actual, string? message)
    {
        RunAssertion(expected, actual, message, (e, a) => a != null && e.IsMatch(a));
    }

    /// <summary>
    /// Tests whether the specified string does not match a regular expression
    /// and throws an exception if the string matches the expression.
    /// </summary>
    /// <param name="notExpected">The regular expression that <paramref name="actual" /> is expected to not match.</param>
    /// <param name="actual">The string that is expected not to match <paramref name="notExpected" />.</param>
    public void DoesNotMatch(Regex notExpected, string? actual)
    {
        DoesNotMatch(notExpected, actual, null);
    }

    /// <summary>
    /// Tests whether the specified string does not match a regular expression
    /// and throws an exception if the string matches the expression.
    /// </summary>
    /// <param name="notExpected">The regular expression that <paramref name="actual" /> is expected to not match.</param>
    /// <param name="actual">The string that is expected not to match <paramref name="notExpected" />.</param>
    /// <param name="message">The message to include in the exception when <paramref name="actual" /> matches <paramref name="notExpected" />. The message is shown in test results.</param>
    public void DoesNotMatch(Regex notExpected, string? actual, string? message)
    {
        RunNegatedAssertion(notExpected, actual, message, (e, a) => a != null && e.IsMatch(a));
    }
}
