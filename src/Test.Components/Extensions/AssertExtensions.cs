using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using MaSch.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.Test.Extensions
{
    /// <summary>
    /// Contains extensions for the <see cref="RunAssertion"/> class.
    /// </summary>
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Assert object is not really needed for these extensions.")]
    public static class AssertExtensions
    {
        /// <summary>
        /// Tests whether an array contains specific elements.
        /// </summary>
        /// <typeparam name="TValue">The type of the expected values.</typeparam>
        /// <typeparam name="TArray">The type of the actual values.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedValues">The expected values.</param>
        /// <param name="actualArray">The actual array.</param>
        /// <param name="itemAssertFunction">The function that compares values between <typeparamref name="TArray"/> and <typeparamref name="TValue"/>.</param>
        public static void AllElements<TValue, TArray>(this Assert assert, TValue[]? expectedValues, IEnumerable<TArray>? actualArray, Action<TValue, TArray> itemAssertFunction)
        {
            Guard.NotNull(itemAssertFunction, nameof(itemAssertFunction));

            if (expectedValues == null)
            {
                Assert.IsNull(actualArray, "The actual enumerable is null.");
                return;
            }

            Assert.IsNotNull(actualArray, "The actual enumerable is not null.");

            if (actualArray is ICollection collection)
                Assert.AreEqual(expectedValues.Length, collection.Count, "The lengths of the arrays do not match.");

            int index = 0;
            foreach (var actual in actualArray!)
            {
                if (index >= expectedValues.Length)
                    Assert.Fail("The actual enumerable has more elements than the expected values array.");

                try
                {
                    itemAssertFunction(expectedValues[index], actual);
                }
                catch (AssertFailedException ex)
                {
                    Assert.Fail($"The assert for item {index} failed with the following message: " + ex.Message);
                }

                index++;
            }

            if (index < expectedValues.Length)
                Assert.Fail("The actual enumerable has less elements than the expected values array.");
        }

        /// <summary>
        /// Tests whether the specified actual value is greater than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsGreaterThan<T>(this Assert assert, T expected, T actual)
            => IsGreaterThan(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is greater than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not greater than <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsGreaterThan<T>(this Assert assert, T expected, T actual, string? message)
            => IsGreaterThan(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is greater than the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsGreaterThan<T>(this Assert assert, T expected, T actual, IComparer<T> comparer)
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
        public static void IsGreaterThan<T>(this Assert assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) > 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is greater than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsGreaterThanOrEqualTo<T>(this Assert assert, T expected, T actual)
            => IsGreaterThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is greater than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not greater than or equal to <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsGreaterThanOrEqualTo<T>(this Assert assert, T expected, T actual, string? message)
            => IsGreaterThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is greater than or equal to the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsGreaterThanOrEqualTo<T>(this Assert assert, T expected, T actual, IComparer<T> comparer)
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
        public static void IsGreaterThanOrEqualTo<T>(this Assert assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) >= 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is smaller than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsSmallerThan<T>(this Assert assert, T expected, T actual)
            => IsSmallerThan(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is smaller than the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not smaller than <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsSmallerThan<T>(this Assert assert, T expected, T actual, string? message)
            => IsSmallerThan(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is smaller than the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsSmallerThan<T>(this Assert assert, T expected, T actual, IComparer<T> comparer)
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
        public static void IsSmallerThan<T>(this Assert assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) < 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is smaller than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsSmallerThanOrEqualTo<T>(this Assert assert, T expected, T actual)
            => IsSmallerThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified actual value is smaller than or equal to the specified expected value and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> is not smaller than or equal to <paramref name="expected"/>.</param>. The message is shown in test results.
        public static void IsSmallerThanOrEqualTo<T>(this Assert assert, T expected, T actual, string? message)
            => IsSmallerThanOrEqualTo(assert, expected, actual, Comparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified actual value is smaller than or equal to the specified expected value using a specified comparer and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual value.</param>
        /// <param name="comparer">The comparer to use to compare the two values.</param>
        public static void IsSmallerThanOrEqualTo<T>(this Assert assert, T expected, T actual, IComparer<T> comparer)
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
        public static void IsSmallerThanOrEqualTo<T>(this Assert assert, T expected, T actual, IComparer<T> comparer, string? message)
        {
            Guard.NotNull(comparer, nameof(comparer));
            RunAssertion(expected, actual, message, (e, a) => comparer.Compare(a, e) <= 0);
        }

        /// <summary>
        /// Tests whether the specified actual value is between the expected minimum and expected maximum values and throws an exception if not the case.
        /// </summary>
        /// <typeparam name="T">The type of values to compare.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expectedMin">The expected minimum value.</param>
        /// <param name="expectedMax">The expected maximum value.</param>
        /// <param name="actual">The actual value.</param>
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual)
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
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual, string? message)
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
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual, IComparer<T> comparer)
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
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual, IComparer<T> comparer, string? message)
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
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive)
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
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, string? message)
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
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, IComparer<T> comparer)
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
        public static void IsBetween<T>(this Assert assert, T expectedMin, T expectedMax, T actual, bool isMinInclusive, bool isMaxInclusive, IComparer<T> comparer, string? message)
            where T : IComparable<T>
        {
            var cmin = comparer.Compare(actual, expectedMin);
            var cmax = comparer.Compare(actual, expectedMax);
            if (cmin < 0 || (!isMinInclusive && cmin == 0) || cmax > 0 || (!isMaxInclusive && cmax == 0))
                ThrowAssertFail(0, message, ("ExpectedMin", expectedMin), ("ExpectedMax", expectedMax), ("Actual", actual));
        }

        private static void RunAssertion<T>(T expected, T actual, string? message, Func<T, T, bool> assertFunction)
        {
            if (!assertFunction(expected, actual))
                ThrowAssertFail(1, message, ("Expected", expected), ("Actual", actual));
        }

        private static void ThrowAssertFail(int skipFrames, string? message, params (string Name, object? Value)[] values)
        {
            var builder = new StringBuilder();
            builder.Append("Assert.That.")
                   .Append(new StackFrame(skipFrames + 1).GetMethod()?.Name)
                   .Append(" failed.");

            foreach (var (name, value) in values)
            {
                builder.Append(' ')
                       .Append(name)
                       .Append(":<")
                       .Append(value ?? "(null)")
                       .Append(">.");
            }

            if (!string.IsNullOrWhiteSpace(message))
                builder.Append(' ').Append(message);

            throw new AssertFailedException(builder.ToString());
        }
    }
}
