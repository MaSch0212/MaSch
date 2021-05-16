using MaSch.Core;
using MaSch.Test.Assertion;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace MaSch.Test
{
    /// <summary>
    /// Provides assertion methods for common use cases to the <see cref="AssertBase"/> class.
    /// </summary>
    public static class CommonAssertions
    {
        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be true.</param>
        public static void IsTrue(this AssertBase assert, bool condition)
            => IsTrue(assert, condition, null);

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be true.</param>
        public static void IsTrue(this AssertBase assert, bool? condition)
            => IsTrue(assert, condition == true, null);

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be true.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is false. The message is shown in test results.</param>
        public static void IsTrue(this AssertBase assert, bool condition, string? message)
            => assert.RunAssertion(message, () => condition);

        /// <summary>
        /// Tests whether the specified condition is true and throws an exception if the condition is false.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be true.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is false. The message is shown in test results.</param>
        public static void IsTrue(this AssertBase assert, bool? condition, string? message)
            => IsTrue(assert, condition == true, message);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be false.</param>
        public static void IsFalse(this AssertBase assert, bool condition)
            => IsFalse(assert, condition, null);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be false.</param>
        public static void IsFalse(this AssertBase assert, bool? condition)
            => IsFalse(assert, condition != false);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be false.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is true. The message is shown in test results.</param>
        public static void IsFalse(this AssertBase assert, bool condition, string? message)
            => assert.RunAssertion(message, () => !condition);

        /// <summary>
        /// Tests whether the specified condition is false and throws an exception if the condition is true.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="condition">The condition the test expects to be false.</param>
        /// <param name="message">The message to include in the exception when <paramref name="condition" /> is true. The message is shown in test results.</param>
        public static void IsFalse(this AssertBase assert, bool? condition, string? message)
            => IsFalse(assert, condition != false, message);

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects to be null.</param>
        public static void IsNull(this AssertBase assert, object? value)
            => IsNull(assert, value, null);

        /// <summary>
        /// Tests whether the specified object is null and throws an exception if it is not.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects to be null.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is not null. The message is shown in test results.</param>
        public static void IsNull(this AssertBase assert, object? value, string? message)
            => assert.RunAssertion(message, () => value == null);

        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects not to be null.</param>
        public static void IsNotNull(this AssertBase assert, [NotNull] object? value)
            => IsNotNull(assert, value, null);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
        /// <summary>
        /// Tests whether the specified object is non-null and throws an exception if it is null.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects not to be null.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="value" /> is null. The message is shown in test results.</param>
        public static void IsNotNull(this AssertBase assert, [NotNull] object? value, string? message)
            => assert.RunAssertion(message, () => value != null);
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first object to compare. This is the value the test expects.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        public static void AreSame(this AssertBase assert, object? expected, object? actual)
            => AreSame(assert, expected, actual, null);

        /// <summary>
        /// Tests whether the specified objects both refer to the same object and throws an exception if the two inputs do not refer to the same object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first object to compare. This is the value the test expects.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not the same as <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreSame(this AssertBase assert, object? expected, object? actual, string? message)
            => assert.RunAssertion(expected, actual, message, (e, a) => ReferenceEquals(a, e));

        /// <summary>
        /// Tests whether the specified objects refer to different objects and throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        public static void AreNotSame(this AssertBase assert, object? notExpected, object? actual)
            => AreNotSame(assert, notExpected, actual, null);

        /// <summary>
        /// Tests whether the specified objects refer to different objects and throws an exception if the two inputs refer to the same object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is the same as <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void AreNotSame(this AssertBase assert, object? notExpected, object? actual, string? message)
            => assert.RunNegatedAssertion(notExpected, actual, message, (e, a) => ReferenceEquals(a, e));

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        public static void AreEqual<T>(this AssertBase assert, T? expected, T? actual)
            => AreEqual(assert, expected, actual, null);

        /// <summary>
        /// Tests whether the specified values are equal and throws an exception if the two values are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first value to compare. This is the value the tests expects.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreEqual<T>(this AssertBase assert, T? expected, T? actual, string? message)
        {
            if (Equals(actual, expected))
                return;

            var areDifferentTypes = actual != null && expected != null && !actual.GetType().Equals(expected.GetType());

            assert.ThrowAssertError(
                message,
                ("Expected", expected),
                areDifferentTypes ? ("ExpectedType", expected!.GetType().FullName) : null,
                ("Actual", actual),
                areDifferentTypes ? ("ActualType", actual!.GetType().FullName) : null);
        }

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception if the two objects are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first object to compare. This is the object the tests expects.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        public static void AreEqual(this AssertBase assert, object? expected, object? actual)
            => AreEqual<object?>(assert, expected, actual, null);

        /// <summary>
        /// Tests whether the specified objects are equal and throws an exception if the two objects are not equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first object to compare. This is the object the tests expects.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreEqual(this AssertBase assert, object? expected, object? actual, string? message)
            => AreEqual<object?>(assert, expected, actual, message);

        /// <summary>
        /// Tests whether the specified floats are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first float to compare. This is the float the tests expects.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        public static void AreEqual(this AssertBase assert, float expected, float actual, float delta)
            => AreEqual(assert, expected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified floats are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first float to compare. This is the float the tests expects.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />. The message is shown in test results.</param>
        public static void AreEqual(this AssertBase assert, float expected, float actual, float delta, string? message)
        {
            if (float.IsNaN(expected) || float.IsNaN(actual) || float.IsNaN(delta) || Math.Abs(expected - actual) > delta)
                assert.ThrowAssertError(message, ("Expected", expected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified doubles are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first double to compare. This is the double the tests expects.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        public static void AreEqual(this AssertBase assert, double expected, double actual, double delta)
            => AreEqual(assert, expected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified doubles are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first double to compare. This is the double the tests expects.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is different than <paramref name="expected" /> by more than <paramref name="delta" />. The message is shown in test results.</param>
        public static void AreEqual(this AssertBase assert, double expected, double actual, double delta, string? message)
        {
            if (double.IsNaN(expected) || double.IsNaN(actual) || double.IsNaN(delta) || Math.Abs(expected - actual) > delta)
                assert.ThrowAssertError(message, ("Expected", expected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        public static void AreEqual(this AssertBase assert, string? expected, string? actual, bool ignoreCase)
            => AreEqual(assert, expected, actual, ignoreCase, CultureInfo.InvariantCulture, null);

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreEqual(this AssertBase assert, string? expected, string? actual, bool ignoreCase, string? message)
            => AreEqual(assert, expected, actual, ignoreCase, CultureInfo.InvariantCulture, message);

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        [ExcludeFromCodeCoverage]
        public static void AreEqual(this AssertBase assert, string? expected, string? actual, bool ignoreCase, CultureInfo culture)
            => AreEqual(assert, expected, actual, ignoreCase, culture, null);

        /// <summary>
        /// Tests whether the specified strings are equal and throws an exception if they are not equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first string to compare. This is the string the tests expects.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreEqual(this AssertBase assert, string? expected, string? actual, bool ignoreCase, CultureInfo culture, string? message)
            => assert.RunAssertion(expected, actual, message, (e, a) => culture.CompareInfo.Compare(a, e, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) == 0);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        public static void AreNotEqual<T>(this AssertBase assert, T? notExpected, T? actual)
            => AreNotEqual(assert, notExpected, actual, null);

        /// <summary>
        /// Tests whether the specified values are unequal and throws an exception if the two values are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first value to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second value to compare. This is the value produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void AreNotEqual<T>(this AssertBase assert, T? notExpected, T? actual, string? message)
            => assert.RunNegatedAssertion(notExpected, actual, message, (e, a) => Equals(a, e));

        /// <summary>
        /// Tests whether the specified objects are unequal and throws an exception if the two objects are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        public static void AreNotEqual(this AssertBase assert, object? notExpected, object? actual)
            => AreNotEqual<object?>(assert, notExpected, actual, null);

        /// <summary>
        /// Tests whether the specified objects are unequal and throws an exception if the two objects are equal. Different numeric types are treated as unequal even if the logical values are equal. 42L is not equal to 42.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first object to compare. This is the value the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second object to compare. This is the object produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void AreNotEqual(this AssertBase assert, object? notExpected, object? actual, string? message)
            => AreNotEqual<object?>(assert, notExpected, actual, message);

        /// <summary>
        /// Tests whether the specified floats are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first float to compare. This is the float the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        public static void AreNotEqual(this AssertBase assert, float notExpected, float actual, float delta)
            => AreNotEqual(assert, notExpected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified floats are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first float to compare. This is the float the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second float to compare. This is the float produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" /> or different by less than <paramref name="delta" />. The message is shown in test results.</param>
        public static void AreNotEqual(this AssertBase assert, float notExpected, float actual, float delta, string? message)
        {
            if (Math.Abs(notExpected - actual) <= delta)
                assert.ThrowAssertError(message, ("NotExpected", notExpected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified doubles are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first double to compare. This is the double the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        public static void AreNotEqual(this AssertBase assert, double notExpected, double actual, double delta)
            => AreNotEqual(assert, notExpected, actual, delta, null);

        /// <summary>
        /// Tests whether the specified doubles are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first double to compare. This is the double the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second double to compare. This is the double produced by the code under test.</param>
        /// <param name="delta">The required accuracy. An exception will be thrown only if <paramref name="actual" /> is different than <paramref name="notExpected" /> by at most <paramref name="delta" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" /> or different by less than <paramref name="delta" />. The message is shown in test results.</param>
        public static void AreNotEqual(this AssertBase assert, double notExpected, double actual, double delta, string? message)
        {
            if (Math.Abs(notExpected - actual) <= delta)
                assert.ThrowAssertError(message, ("NotExpected", notExpected), ("Actual", actual), ("Delta", delta));
        }

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        public static void AreNotEqual(this AssertBase assert, string? notExpected, string? actual, bool ignoreCase)
            => AreNotEqual(assert, notExpected, actual, ignoreCase, CultureInfo.InvariantCulture, null);

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal. The invariant culture is used for the comparison.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void AreNotEqual(this AssertBase assert, string? notExpected, string? actual, bool ignoreCase, string? message)
            => AreNotEqual(assert, notExpected, actual, ignoreCase, CultureInfo.InvariantCulture, message);

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        [ExcludeFromCodeCoverage]
        public static void AreNotEqual(this AssertBase assert, string? notExpected, string? actual, bool ignoreCase, CultureInfo culture)
            => AreNotEqual(assert, notExpected, actual, ignoreCase, culture, null);

        /// <summary>
        /// Tests whether the specified strings are unequal and throws an exception if they are equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first string to compare. This is the string the test expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second string to compare. This is the string produced by the code under test.</param>
        /// <param name="ignoreCase">A Boolean indicating a case-sensitive or insensitive comparison. (true indicates a case-insensitive comparison.)</param>
        /// <param name="culture">A CultureInfo object that supplies culture-specific comparison information.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void AreNotEqual(this AssertBase assert, string? notExpected, string? actual, bool ignoreCase, CultureInfo culture, string? message)
            => assert.RunNegatedAssertion(notExpected, actual, message, (e, a) => culture.CompareInfo.Compare(a, e, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) == 0);

        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="expectedType">The expected type of <paramref name="value" />.</param>
        public static void IsInstanceOfType(this AssertBase assert, [NotNull] object? value, Type expectedType)
            => IsInstanceOfType(assert, value, expectedType, null);

        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <typeparam name="T">The expected type of <paramref name="value" />.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <returns><paramref name="value"/> cast to <typeparamref name="T"/>.</returns>
        public static T IsInstanceOfType<T>(this AssertBase assert, [NotNull] object? value)
            => IsInstanceOfType<T>(assert, value, null);

#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="expectedType">The expected type of <paramref name="value" />.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is not an instance of <paramref name="expectedType" />. The message is shown in test results.</param>
        public static void IsInstanceOfType(this AssertBase assert, [NotNull] object? value, Type expectedType, string? message)
            => assert.RunAssertion(Guard.NotNull(expectedType, nameof(expectedType)), value?.GetType(), message, (e, a) => e.IsAssignableFrom(a));

        /// <summary>
        /// Tests whether the specified object is an instance of the expected type and throws an exception if the expected type is not in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <typeparam name="T">The expected type of <paramref name="value" />.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects to be of the specified type.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is not an instance of <typeparamref name="T"/>. The message is shown in test results.</param>
        /// <returns><paramref name="value"/> cast to <typeparamref name="T"/>.</returns>
        public static T IsInstanceOfType<T>(this AssertBase assert, [NotNull] object? value, string? message)
        {
            T result = default!;
            if (value is T tValue)
                result = tValue;
            else
                assert.ThrowAssertError(message, ("Expected", typeof(T)), ("Actual", value?.GetType()));

            return result;
        }
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="wrongType">The type that <paramref name="value" /> should not be.</param>
        public static void IsNotInstanceOfType(this AssertBase assert, object? value, Type wrongType)
            => IsNotInstanceOfType(assert, value, wrongType, null);

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <typeparam name="T">The type that <paramref name="value" /> should not be.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        public static void IsNotInstanceOfType<T>(this AssertBase assert, object? value)
            => IsNotInstanceOfType(assert, value, typeof(T), null);

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="wrongType">The type that <paramref name="value" /> should not be.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is an instance of <paramref name="wrongType" />. The message is shown in test results.</param>
        public static void IsNotInstanceOfType(this AssertBase assert, object? value, Type wrongType, string? message)
            => assert.RunNegatedAssertion(Guard.NotNull(wrongType, nameof(wrongType)), value?.GetType(), message, (e, a) => e.IsAssignableFrom(a));

        /// <summary>
        /// Tests whether the specified object is not an instance of the wrong type and throws an exception if the specified type is in the
        /// inheritance hierarchy of the object.
        /// </summary>
        /// <typeparam name="T">The type that <paramref name="value" /> should not be.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="value">The object the test expects not to be of the specified type.</param>
        /// <param name="message">The message to include in the exception when <paramref name="value" /> is an instance of <typeparamref name="T"/>. The message is shown in test results.</param>
        public static void IsNotInstanceOfType<T>(this AssertBase assert, object? value, string? message)
            => IsNotInstanceOfType(assert, value, typeof(T), message);

        /// <summary>
        /// Throws an AssertFailedException.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        public static void Fail(this AssertBase assert)
            => assert.ThrowAssertError(null);

        /// <summary>
        /// Throws an AssertFailedException.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="message">The message to include in the exception. The message is shown in test results.</param>
        public static void Fail(this AssertBase assert, string? message)
            => assert.ThrowAssertError(message);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(this AssertBase assert, Action action)
            where T : Exception
            => ThrowsException<T>(assert, action, null);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when <paramref name="action" /> does not throws exception of type <typeparamref name="T" />.</param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(this AssertBase assert, Action action, string? message)
            where T : Exception
        {
            Guard.NotNull(action, nameof(action));

            Exception? exception = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception == null || !typeof(T).Equals(exception.GetType()))
                assert.ThrowAssertError(message, ("ExpectedException", typeof(T).Name), ("ActualException", exception?.GetType().Name));

            return (exception as T)!;
        }

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(this AssertBase assert, Func<object?> action)
            where T : Exception
            => ThrowsException<T>(assert, () => { action(); }, null);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when <paramref name="action" /> does not throws exception of type <typeparamref name="T" />.</param>
        /// <returns>The exception that was thrown.</returns>
        public static T ThrowsException<T>(this AssertBase assert, Func<object?> action, string? message)
            where T : Exception
            => ThrowsException<T>(assert, () => { action(); }, message);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> executing the delegate.</returns>
        public static async Task<T> ThrowsExceptionAsync<T>(this AssertBase assert, Func<Task> action)
            where T : Exception
            => await ThrowsExceptionAsync<T>(assert, action, null);

        /// <summary>
        /// Tests whether the code specified by delegate <paramref name="action" /> throws exact given exception of type <typeparamref name="T" /> (and not of derived type)
        /// and throws <c>AssertFailedException</c> if code does not throws exception or throws exception of type other than <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Type of exception expected to be thrown.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="action">Delegate to code to be tested and which is expected to throw exception.</param>
        /// <param name="message">The message to include in the exception when <paramref name="action" /> does not throws exception of type <typeparamref name="T" />.</param>
        /// <returns>The <see cref="T:System.Threading.Tasks.Task" /> executing the delegate.</returns>
        public static async Task<T> ThrowsExceptionAsync<T>(this AssertBase assert, Func<Task> action, string? message)
            where T : Exception
        {
            Guard.NotNull(action, nameof(action));

            Exception? exception = null;
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception == null || !typeof(T).Equals(exception.GetType()))
                assert.ThrowAssertError(nameof(ThrowsExceptionAsync), message, ("ExpectedException", typeof(T).Name), ("ActualException", exception?.GetType().Name));

            return (exception as T)!;
        }
    }
}
