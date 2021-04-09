using MaSch.Core;
using MaSch.Test.Assertion;
using System;
using System.Collections.Generic;

namespace MaSch.Test
{
    /// <summary>
    /// Provides assertion methods for <see cref="IComparable"/> and <see cref="IComparable{T}"/> objects for the <see cref="AssertBase"/> class.
    /// </summary>
    public static class ComparableAssertions
    {
        /// <summary>
        /// Tests whether the specified actual value is greater than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsGreaterThan<T>(this AssertBase assert, T expected, T actual)
            => IsGreaterThan(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is greater than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not greater than <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsGreaterThan<T>(this AssertBase assert, T expected, T actual, string? message)
            => IsGreaterThan(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is greater than the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsGreaterThan<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer)
            => IsGreaterThan(assert, expected, actual, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is greater than the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not greater than <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsGreaterThan<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            assert.RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) > 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is greater than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsGreaterThanOrEqualTo<T>(this AssertBase assert, T expected, T actual)
            => IsGreaterThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is greater than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not greater than or equal to <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsGreaterThanOrEqualTo<T>(this AssertBase assert, T expected, T actual, string? message)
            => IsGreaterThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is greater than or equal to the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsGreaterThanOrEqualTo<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer)
            => IsGreaterThanOrEqualTo(assert, expected, actual, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is greater than or equal to the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not greater than or equal to <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsGreaterThanOrEqualTo<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            assert.RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) >= 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is smaller than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsSmallerThan<T>(this AssertBase assert, T expected, T actual)
            => IsSmallerThan(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is smaller than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not smaller than <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsSmallerThan<T>(this AssertBase assert, T expected, T actual, string? message)
            => IsSmallerThan(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is smaller than the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsSmallerThan<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer)
            => IsSmallerThan(assert, expected, actual, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is smaller than the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not smaller than <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsSmallerThan<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            assert.RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) < 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is smaller than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsSmallerThanOrEqualTo<T>(this AssertBase assert, T expected, T actual)
            => IsSmallerThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is smaller than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not smaller than or equal to <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsSmallerThanOrEqualTo<T>(this AssertBase assert, T expected, T actual, string? message)
            => IsSmallerThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is smaller than or equal to the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsSmallerThanOrEqualTo<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer)
            => IsSmallerThanOrEqualTo(assert, expected, actual, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is smaller than or equal to the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not smaller than or equal to <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsSmallerThanOrEqualTo<T>(this AssertBase assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            assert.RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) <= 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual)
            where T : IComparable<T>
            => IsBetween(assert, expectedMin, expectedMax, actual, false, false, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="expectedMin"/> and <paramref name="expectedMax"/>.</param>. The message is shown in test results.
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual, string? message)
            where T : IComparable<T>
            => IsBetween(assert, expectedMin, expectedMax, actual, false, false, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual, IComparer<T> comparer)
            where T : IComparable<T>
            => IsBetween(assert, expectedMin, expectedMax, actual, false, false, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="expectedMin"/> and <paramref name="expectedMax"/>.</param>. The message is shown in test results.
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual, IComparer<T> comparer, string? message)
            where T : IComparable<T>
            => IsBetween(assert, expectedMin, expectedMax, actual, false, false, comparer, message);

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMax"/>.</param>
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive)
            where T : IComparable<T>
            => IsBetween(assert, expectedMin, expectedMax, actual, isMinInclusive, isMaxInclusive, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMax"/>.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="expectedMin"/> and <paramref name="expectedMax"/>.</param>. The message is shown in test results.
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, string? message)
            where T : IComparable<T>
            => IsBetween(assert, expectedMin, expectedMax, actual, isMinInclusive, isMaxInclusive, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMax"/>.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, IComparer<T> comparer)
            where T : IComparable<T>
            => IsBetween(assert, expectedMin, expectedMax, actual, isMinInclusive, isMaxInclusive, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is allowed to equal to <paramref name="expectedMax"/>.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="expectedMin"/> and <paramref name="expectedMax"/>.</param>. The message is shown in test results.
        public static void IsBetween<T>(this AssertBase assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, IComparer<T> comparer, string? message)
            where T : IComparable<T>
        {
            var cmin = comparer.Compare(actual, expectedMin);
            var cmax = comparer.Compare(actual, expectedMax);
            if (cmin < 0 || (!isMinInclusive && cmin == 0) || cmax > 0 || (!isMaxInclusive && cmax == 0))
                assert.ThrowAssertError(0, message, ("ExpectedMin", expectedMin), ("ExpectedMax", expectedMax), ("Actual", actual));
        }

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual)
            where T : IComparable<T>
            => IsNotBetween(assert, notExpectedMin, notExpectedMax, actual, false, false, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="notExpectedMin"/> and <paramref name="notExpectedMax"/>.</param>. The message is shown in test results.
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual, string? message)
            where T : IComparable<T>
            => IsNotBetween(assert, notExpectedMin, notExpectedMax, actual, false, false, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual, IComparer<T> comparer)
            where T : IComparable<T>
            => IsNotBetween(assert, notExpectedMin, notExpectedMax, actual, false, false, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="notExpectedMin"/> and <paramref name="notExpectedMax"/>.</param>. The message is shown in test results.
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual, IComparer<T> comparer, string? message)
            where T : IComparable<T>
            => IsNotBetween(assert, notExpectedMin, notExpectedMax, actual, false, false, comparer, message);

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMax"/>.</param>
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual, bool isMinInclusive, bool isMaxInclusive)
            where T : IComparable<T>
            => IsNotBetween(assert, notExpectedMin, notExpectedMax, actual, isMinInclusive, isMaxInclusive, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMax"/>.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="notExpectedMin"/> and <paramref name="notExpectedMax"/>.</param>. The message is shown in test results.
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, string? message)
            where T : IComparable<T>
            => IsNotBetween(assert, notExpectedMin, notExpectedMax, actual, isMinInclusive, isMaxInclusive, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMax"/>.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, IComparer<T> comparer)
            where T : IComparable<T>
            => IsNotBetween(assert, notExpectedMin, notExpectedMax, actual, isMinInclusive, isMaxInclusive, comparer, null);

        /// <summary>
        /// Tests whether the specified actual value is not between the expected minimum and expected maximum values and throws an exception if the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpectedMin">The minimum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="notExpectedMax">The maximum value of the range, <paramref name="actual"/> is not expected to be in..</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="isMinInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMin"/>.</param>
        /// <param name="isMaxInclusive">if set to <c>true</c> <paramref name="actual"/> is not allowed to equal to <paramref name="notExpectedMax"/>.</param>
        /// <param name="comparer">The comparer to use to compare the values.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not between <paramref name="notExpectedMin"/> and <paramref name="notExpectedMax"/>.</param>. The message is shown in test results.
        public static void IsNotBetween<T>(this AssertBase assert, T notExpectedMin, T notExpectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, IComparer<T> comparer, string? message)
            where T : IComparable<T>
        {
            var cmin = comparer.Compare(actual, notExpectedMin);
            var cmax = comparer.Compare(actual, notExpectedMax);
            if ((cmin > 0 || (!isMinInclusive && cmin == 0)) && (cmax < 0 || (!isMaxInclusive && cmax == 0)))
                assert.ThrowAssertError(0, message, ("NotExpectedMin", notExpectedMin), ("NotExpectedMax", notExpectedMax), ("Actual", actual));
        }
    }
}
