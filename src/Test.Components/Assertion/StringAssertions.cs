using MaSch.Test.Assertion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#if !NETCOREAPP2_1_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
using MaSch.Core.Extensions;
#endif

namespace MaSch.Test
{
    /// <summary>
    /// Provides assertion methods for <see cref="string"/> to the <see cref="AssertBase"/> class.
    /// </summary>
    public static class StringAssertions
    {
        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and throws an exception if the substring does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        public static void Contains(this AssertBase assert, string expected, string? actual)
            => Contains(assert, expected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and throws an exception if the substring does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void Contains(this AssertBase assert, string expected, string? actual, string? message)
            => Contains(assert, expected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and throws an exception if the substring does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void Contains(this AssertBase assert, string expected, string? actual, StringComparison comparisonType)
            => Contains(assert, expected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string contains the specified substring
        /// and throws an exception if the substring does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void Contains(this AssertBase assert, string expected, string? actual, StringComparison comparisonType, string? message)
            => assert.RunAssertion(expected, actual, message, (e, a) => a?.Contains(e, comparisonType) == true);

        /// <summary>
        /// Tests whether the specified string contains any of the specified substrings
        /// and throws an exception if none of the substrings do occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        public static void ContainsAny(this AssertBase assert, IEnumerable<string> expected, string? actual)
            => ContainsAny(assert, expected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string contains any of the specified substrings
        /// and throws an exception if none of the substrings do occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void ContainsAny(this AssertBase assert, IEnumerable<string> expected, string? actual, string? message)
            => ContainsAny(assert, expected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string contains any of the specified substrings
        /// and throws an exception if none of the substrings do occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void ContainsAny(this AssertBase assert, IEnumerable<string> expected, string? actual, StringComparison comparisonType)
            => ContainsAny(assert, expected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string contains any of the specified substrings
        /// and throws an exception if none of the substrings do occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings of which at least one is expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void ContainsAny(this AssertBase assert, IEnumerable<string> expected, string? actual, StringComparison comparisonType, string? message)
            => assert.RunAssertion(expected, actual, message, (e, a) => e.Any(x => a?.Contains(x, comparisonType) == true));

        /// <summary>
        /// Tests whether the specified string contains all of the specified substrings
        /// and throws an exception if one of the substrings does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        public static void ContainsAll(this AssertBase assert, IEnumerable<string> expected, string? actual)
            => ContainsAll(assert, expected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string contains all of the specified substrings
        /// and throws an exception if one of the substrings does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void ContainsAll(this AssertBase assert, IEnumerable<string> expected, string? actual, string? message)
            => ContainsAll(assert, expected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string contains all of the specified substrings
        /// and throws an exception if one of the substrings does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void ContainsAll(this AssertBase assert, IEnumerable<string> expected, string? actual, StringComparison comparisonType)
            => ContainsAll(assert, expected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string contains all of the specified substrings
        /// and throws an exception if one of the substrings does not occur within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The strings expected to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to contain <paramref name="expected" />.</param>
        /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="expected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void ContainsAll(this AssertBase assert, IEnumerable<string> expected, string? actual, StringComparison comparisonType, string? message)
        {
            var missingStrings = expected.Where(x => actual?.Contains(x, comparisonType) != true).ToArray();
            if (missingStrings.Length > 0)
                assert.ThrowAssertError(message, ("Expected", expected), ("Actual", actual), ("MissingStrings", missingStrings));
        }

        /// <summary>
        /// Tests whether the specified string does not contain the specified substring
        /// and throws an exception if the substring occurs within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
        public static void DoesNotContain(this AssertBase assert, string notExpected, string? actual)
            => DoesNotContain(assert, notExpected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string does not contain the specified substring
        /// and throws an exception if the substring occurs within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="notExpected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void DoesNotContain(this AssertBase assert, string notExpected, string? actual, string? message)
            => DoesNotContain(assert, notExpected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string does not contain the specified substring
        /// and throws an exception if the substring occurs within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void DoesNotContain(this AssertBase assert, string notExpected, string? actual, StringComparison comparisonType)
            => DoesNotContain(assert, notExpected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string does not contain the specified substring
        /// and throws an exception if the substring occurs within the
        /// test string.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected not to occur within <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not contain <paramref name="notExpected" />.</param>
        /// <param name="comparisonType"> The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="notExpected" /> is not in <paramref name="actual" />. The message is shown in test results.</param>
        public static void DoesNotContain(this AssertBase assert, string notExpected, string? actual, StringComparison comparisonType, string? message)
            => assert.RunNegatedAssertion(notExpected, actual, message, (e, a) => a?.Contains(e, comparisonType) != false);

        /// <summary>
        /// Tests whether the specified string begins with the specified substring
        /// and throws an exception if the test string does not start with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
        public static void StartsWith(this AssertBase assert, string expected, string? actual)
            => StartsWith(assert, expected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string begins with the specified substring
        /// and throws an exception if the test string does not start with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="expected" />. The message is shown in test results.</param>
        public static void StartsWith(this AssertBase assert, string expected, string? actual, string? message)
            => StartsWith(assert, expected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string begins with the specified substring
        /// and throws an exception if the test string does not start with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void StartsWith(this AssertBase assert, string expected, string? actual, StringComparison comparisonType)
            => StartsWith(assert, expected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string begins with the specified substring
        /// and throws an exception if the test string does not start with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to begin with <paramref name="expected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="expected" />. The message is shown in test results.</param>
        public static void StartsWith(this AssertBase assert, string expected, string? actual, StringComparison comparisonType, string? message)
            => assert.RunAssertion(expected, actual, message, (e, a) => a?.StartsWith(e, comparisonType) == true);

        /// <summary>
        /// Tests whether the specified string does not begin with the specified substring
        /// and throws an exception if the test string starts with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
        public static void DoesNotStartWith(this AssertBase assert, string notExpected, string? actual)
            => DoesNotStartWith(assert, notExpected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string does not begin with the specified substring
        /// and throws an exception if the test string starts with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void DoesNotStartWith(this AssertBase assert, string notExpected, string? actual, string? message)
            => DoesNotStartWith(assert, notExpected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string does not begin with the specified substring
        /// and throws an exception if the test string starts with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void DoesNotStartWith(this AssertBase assert, string notExpected, string? actual, StringComparison comparisonType)
            => DoesNotStartWith(assert, notExpected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string does not begin with the specified substring
        /// and throws an exception if the test string starts with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a prefix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected not to begin with <paramref name="notExpected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not begin with <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void DoesNotStartWith(this AssertBase assert, string notExpected, string? actual, StringComparison comparisonType, string? message)
            => assert.RunNegatedAssertion(notExpected, actual, message, (e, a) => a?.StartsWith(e, comparisonType) != false);

        /// <summary>
        /// Tests whether the specified string ends with the specified substring
        /// and throws an exception if the test string does not end with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
        public static void EndsWith(this AssertBase assert, string expected, string? actual)
            => EndsWith(assert, expected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string ends with the specified substring
        /// and throws an exception if the test string does not end with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="expected" />. The message is shown in test results.</param>
        public static void EndsWith(this AssertBase assert, string expected, string? actual, string? message)
            => EndsWith(assert, expected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string ends with the specified substring
        /// and throws an exception if the test string does not end with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void EndsWith(this AssertBase assert, string expected, string? actual, StringComparison comparisonType)
            => EndsWith(assert, expected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string ends with the specified substring
        /// and throws an exception if the test string does not end with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The string expected to be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to end with <paramref name="expected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="expected" />. The message is shown in test results.</param>
        public static void EndsWith(this AssertBase assert, string expected, string? actual, StringComparison comparisonType, string? message)
            => assert.RunAssertion(expected, actual, message, (e, a) => a?.EndsWith(e, comparisonType) == true);

        /// <summary>
        /// Tests whether the specified string does not end with the specified substring
        /// and throws an exception if the test string ends with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
        public static void DoesNotEndWith(this AssertBase assert, string notExpected, string? actual)
            => DoesNotEndWith(assert, notExpected, actual, StringComparison.Ordinal, null);

        /// <summary>
        /// Tests whether the specified string does not end with the specified substring
        /// and throws an exception if the test string ends with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void DoesNotEndWith(this AssertBase assert, string notExpected, string? actual, string? message)
            => DoesNotEndWith(assert, notExpected, actual, StringComparison.Ordinal, message);

        /// <summary>
        /// Tests whether the specified string does not end with the specified substring
        /// and throws an exception if the test string ends with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        public static void DoesNotEndWith(this AssertBase assert, string notExpected, string? actual, StringComparison comparisonType)
            => DoesNotEndWith(assert, notExpected, actual, comparisonType, null);

        /// <summary>
        /// Tests whether the specified string does not end with the specified substring
        /// and throws an exception if the test string ends with the
        /// substring.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The string expected to not be a suffix of <paramref name="actual" />.</param>
        /// <param name="actual">The string that is expected to not end with <paramref name="notExpected" />.</param>
        /// <param name="comparisonType">The comparison method to compare strings <paramref name="comparisonType" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not end with <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void DoesNotEndWith(this AssertBase assert, string notExpected, string? actual, StringComparison comparisonType, string? message)
            => assert.RunNegatedAssertion(notExpected, actual, message, (e, a) => a?.EndsWith(e, comparisonType) != false);

        /// <summary>
        /// Tests whether the specified string matches a regular expression and
        /// throws an exception if the string does not match the expression.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The regular expression that <paramref name="actual" /> is expected to match.</param>
        /// <param name="actual">The string that is expected to match <paramref name="expected" />.</param>
        public static void Matches(this AssertBase assert, Regex expected, string? actual)
            => Matches(assert, expected, actual, null);

        /// <summary>
        /// Tests whether the specified string matches a regular expression and
        /// throws an exception if the string does not match the expression.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The regular expression that <paramref name="actual" /> is expected to match.</param>
        /// <param name="actual">The string that is expected to match <paramref name="expected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> does not match <paramref name="expected" />. The message is shown in test results.</param>
        public static void Matches(this AssertBase assert, Regex expected, string? actual, string? message)
            => assert.RunAssertion(expected, actual, message, (e, a) => a != null && e.IsMatch(a));

        /// <summary>
        /// Tests whether the specified string does not match a regular expression
        /// and throws an exception if the string matches the expression.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The regular expression that <paramref name="actual" /> is expected to not match.</param>
        /// <param name="actual">The string that is expected not to match <paramref name="notExpected" />.</param>
        public static void DoesNotMatch(this AssertBase assert, Regex notExpected, string? actual)
            => DoesNotMatch(assert, notExpected, actual, null);

        /// <summary>
        /// Tests whether the specified string does not match a regular expression
        /// and throws an exception if the string matches the expression.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The regular expression that <paramref name="actual" /> is expected to not match.</param>
        /// <param name="actual">The string that is expected not to match <paramref name="notExpected" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> matches <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void DoesNotMatch(this AssertBase assert, Regex notExpected, string? actual, string? message)
            => assert.RunNegatedAssertion(notExpected, actual, message, (e, a) => a != null && e.IsMatch(a));
    }
}
