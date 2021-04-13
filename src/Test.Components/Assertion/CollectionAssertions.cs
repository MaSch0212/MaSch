using MaSch.Core;
using MaSch.Test.Assertion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Test
{
    /// <summary>
    /// Provides assertion methods for collections to the <see cref="AssertBase"/> class.
    /// </summary>
    public static class CollectionAssertions
    {
        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        public static void Contains<T>(this AssertBase assert, T expected, IEnumerable<T>? actual)
            => Contains(assert, expected, actual, (e, a) => Equals(a, e));

        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="expected"/>. The message is shown in test results.</param>
        public static void Contains<T>(this AssertBase assert, T expected, IEnumerable<T>? actual, string? message)
            => Contains(assert, expected, actual, (e, a) => Equals(a, e), message);

        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="expected"/> value.</param>
        public static void Contains<T>(this AssertBase assert, T expected, IEnumerable<T>? actual, IEqualityComparer<T> comparer)
            => Contains(assert, expected, actual, (e, a) => comparer.Equals(a, e));

        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="expected"/> value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="expected"/>. The message is shown in test results.</param>
        public static void Contains<T>(this AssertBase assert, T expected, IEnumerable<T>? actual, IEqualityComparer<T> comparer, string? message)
            => Contains(assert, expected, actual, (e, a) => comparer.Equals(a, e), message);

        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="expected"/> value.</param>
        public static void Contains(this AssertBase assert, object? expected, IEnumerable? actual, IEqualityComparer comparer)
            => Contains(assert, expected, actual?.OfType<object?>(), (e, a) => comparer.Equals(a, e));

        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="expected"/> value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="expected"/>. The message is shown in test results.</param>
        public static void Contains(this AssertBase assert, object? expected, IEnumerable? actual, IEqualityComparer comparer, string? message)
            => Contains(assert, expected, actual?.OfType<object?>(), (e, a) => comparer.Equals(a, e), message);

        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <typeparam name="TExpected">The type of the expected value.</typeparam>
        /// <typeparam name="TActual">The type of the items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="predicate">The predicate to validate whether an item in <paramref name="actual"/> matches the <paramref name="expected"/> value.</param>
        public static void Contains<TExpected, TActual>(this AssertBase assert, TExpected expected, IEnumerable<TActual>? actual, Func<TExpected, TActual, bool> predicate)
            => Contains(assert, expected, actual, predicate, null);

        /// <summary>
        /// Tests whether the specified actual enumerable contains the expected value and throws an exception if the actual enumerable does not contain the expected value.
        /// </summary>
        /// <typeparam name="TExpected">The type of the expected value.</typeparam>
        /// <typeparam name="TActual">The type of the items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="predicate">The predicate to validate whether an item in <paramref name="actual"/> matches the <paramref name="expected"/> value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="expected"/>. The message is shown in test results.</param>
        public static void Contains<TExpected, TActual>(this AssertBase assert, TExpected expected, IEnumerable<TActual>? actual, Func<TExpected, TActual, bool> predicate, string? message)
        {
            var collection = actual is ICollection<TActual> c ? c : actual?.ToArray();
            if (collection?.Any(x => predicate(expected, x)) != true)
                assert.ThrowAssertError(message, ("Expected", expected), ("Actual", FormatCollection(collection)));
        }

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        public static void DoesNotContain<T>(this AssertBase assert, T notExpected, IEnumerable<T>? actual)
            => DoesNotContain(assert, notExpected, actual, (e, a) => Equals(a, e));

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="notExpected"/>. The message is shown in test results.</param>
        public static void DoesNotContain<T>(this AssertBase assert, T notExpected, IEnumerable<T>? actual, string? message)
            => DoesNotContain(assert, notExpected, actual, (e, a) => Equals(a, e), message);

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="notExpected"/> value.</param>
        public static void DoesNotContain<T>(this AssertBase assert, T notExpected, IEnumerable<T>? actual, IEqualityComparer<T> comparer)
            => DoesNotContain(assert, notExpected, actual, (e, a) => comparer.Equals(a, e));

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <typeparam name="T">The type of the expected value and items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="notExpected"/> value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="notExpected"/>. The message is shown in test results.</param>
        public static void DoesNotContain<T>(this AssertBase assert, T notExpected, IEnumerable<T>? actual, IEqualityComparer<T> comparer, string? message)
            => DoesNotContain(assert, notExpected, actual, (e, a) => comparer.Equals(a, e), message);

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="notExpected"/> value.</param>
        public static void DoesNotContain(this AssertBase assert, object? notExpected, IEnumerable? actual, IEqualityComparer comparer)
            => DoesNotContain(assert, notExpected, actual?.OfType<object?>(), (e, a) => comparer.Equals(a, e));

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="comparer">The comparer that is used to validate whether an item in <paramref name="actual"/> matches the <paramref name="notExpected"/> value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="notExpected"/>. The message is shown in test results.</param>
        public static void DoesNotContain(this AssertBase assert, object? notExpected, IEnumerable? actual, IEqualityComparer comparer, string? message)
            => DoesNotContain(assert, notExpected, actual?.OfType<object?>(), (e, a) => comparer.Equals(a, e), message);

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <typeparam name="TNotExpected">The type of the expected value.</typeparam>
        /// <typeparam name="TActual">The type of the items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="predicate">The predicate to validate whether an item in <paramref name="actual"/> matches the <paramref name="notExpected"/> value.</param>
        public static void DoesNotContain<TNotExpected, TActual>(this AssertBase assert, TNotExpected notExpected, IEnumerable<TActual>? actual, Func<TNotExpected, TActual, bool> predicate)
            => DoesNotContain(assert, notExpected, actual, predicate, null);

        /// <summary>
        /// Tests whether the specified actual enumerable does not contain the expected value and throws an exception if the actual enumerable contains the expected value.
        /// </summary>
        /// <typeparam name="TNotExpected">The type of the expected value.</typeparam>
        /// <typeparam name="TActual">The type of the items in the actual enumerable.</typeparam>
        /// <param name="assert">The assert object.</param>
        /// <param name="notExpected">The unexpected value.</param>
        /// <param name="actual">The actual enumerable.</param>
        /// <param name="predicate">The predicate to validate whether an item in <paramref name="actual"/> matches the <paramref name="notExpected"/> value.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual"/> does not contain <paramref name="notExpected"/>. The message is shown in test results.</param>
        public static void DoesNotContain<TNotExpected, TActual>(this AssertBase assert, TNotExpected notExpected, IEnumerable<TActual>? actual, Func<TNotExpected, TActual, bool> predicate, string? message)
        {
            var collection = actual is ICollection<TActual> c ? c : actual?.ToArray();
            if (collection?.All(x => !predicate(notExpected, x)) != true)
                assert.ThrowAssertError(message, ("NotExpected", notExpected), ("Actual", FormatCollection(collection)));
        }

        /// <summary>
        /// Tests whether all items in the specified collection are non-null and throws an exception if any element is null.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for null elements.</param>
        public static void AllItemsAreNotNull(this AssertBase assert, IEnumerable collection)
            => AllItemsAreNotNull(assert, collection, null);

        /// <summary>
        /// Tests whether all items in the specified collection are non-null and throws an exception if any element is null.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for null elements.</param>
        /// <param name="message"> The message to include in the exception when <paramref name="collection" /> contains a null element. The message is shown in test results.</param>
        public static void AllItemsAreNotNull(this AssertBase assert, IEnumerable collection, string? message)
        {
            Guard.NotNull(collection, nameof(collection));

            var faultyIdx = new List<int>();
            int idx = 0;
            foreach (var item in collection)
            {
                if (item == null)
                    faultyIdx.Add(idx);
                idx++;
            }

            if (faultyIdx.Count > 0)
                assert.ThrowAssertError(message, (faultyIdx.Count > 1 ? "Indices" : "Index", string.Join(", ", faultyIdx)));
        }

        /// <summary>
        /// Tests whether all items in the specified collection are unique or not and throws if any two elements in the collection are equal.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for duplicate elements.</param>
        public static void AllItemsAreUnique<T>(this AssertBase assert, IEnumerable<T> collection)
            => AllItemsAreUnique(assert, collection, EqualityComparer<T>.Default, null);

        /// <summary>
        /// Tests whether all items in the specified collection are unique or not and throws if any two elements in the collection are equal.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for duplicate elements.</param>
        /// <param name="message">The message to include in the exception when <paramref name="collection" /> contains at least one duplicate element. The message is shown in test results.</param>
        public static void AllItemsAreUnique<T>(this AssertBase assert, IEnumerable<T> collection, string? message)
            => AllItemsAreUnique(assert, collection, EqualityComparer<T>.Default, message);

        /// <summary>
        /// Tests whether all items in the specified collection are unique or not and throws if any two elements in the collection are equal.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for duplicate elements.</param>
        /// <param name="comparer">The comparer used to determine if all items are unique.</param>
        public static void AllItemsAreUnique<T>(this AssertBase assert, IEnumerable<T> collection, IEqualityComparer<T> comparer)
            => AllItemsAreUnique(assert, collection, comparer, null);

        /// <summary>
        /// Tests whether all items in the specified collection are unique or not and throws if any two elements in the collection are equal.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for duplicate elements.</param>
        /// <param name="comparer">The comparer used to determine if all items are unique.</param>
        /// <param name="message">The message to include in the exception when <paramref name="collection" /> contains at least one duplicate element. The message is shown in test results.</param>
        public static void AllItemsAreUnique<T>(this AssertBase assert, IEnumerable<T> collection, IEqualityComparer<T> comparer, string? message)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(comparer, nameof(comparer));

            int? nullIdx = null;
            var checkedItems = new Dictionary<int, int>();
            var faultyIdx = new List<(int, int)>();
            int idx = 0;
            foreach (var item in collection)
            {
                if (item == null)
                {
                    if (nullIdx.HasValue)
                        faultyIdx.Add((nullIdx.Value, idx));
                    else
                        nullIdx = idx;
                }
                else
                {
                    var hashCode = comparer.GetHashCode(item);
                    if (checkedItems.TryGetValue(hashCode, out int i))
                        faultyIdx.Add((i, idx));
                    else
                        checkedItems.Add(hashCode, idx);
                }

                idx++;
            }

            if (faultyIdx.Count > 0)
                assert.ThrowAssertError(message, (faultyIdx.Count > 1 ? "Indices" : "Index", string.Join(", ", faultyIdx.Select(x => $"{x.Item1}={x.Item2}"))));
        }

        /// <summary>
        /// Tests whether one collection is a subset of another collection and throws an exception if any element in the subset is not also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected to be a subset of <paramref name="superset" />.</param>
        /// <param name="superset">The collection expected to be a superset of <paramref name="subset" />.</param>
        public static void IsSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset)
            => IsSubsetOf(assert, subset, superset, EqualityComparer<T>.Default, null);

        /// <summary>
        /// Tests whether one collection is a subset of another collection and throws an exception if any element in the subset is not also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected to be a subset of <paramref name="superset" />.</param>
        /// <param name="superset">The collection expected to be a superset of <paramref name="subset" />.</param>
        /// <param name="message"> The message to include in the exception when an element in <paramref name="subset" /> is not found in <paramref name="superset" />. The message is shown in test results.</param>
        public static void IsSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset, string? message)
            => IsSubsetOf(assert, subset, superset, EqualityComparer<T>.Default, message);

        /// <summary>
        /// Tests whether one collection is a subset of another collection and throws an exception if any element in the subset is not also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected to be a subset of <paramref name="superset" />.</param>
        /// <param name="superset">The collection expected to be a superset of <paramref name="subset" />.</param>
        /// <param name="comparer">The comparer used to determine if an elements in the <paramref name="superset"/> matched that of the <paramref name="subset"/>.</param>
        public static void IsSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset, IEqualityComparer<T> comparer)
            => IsSubsetOf(assert, subset, superset, comparer, null);

        /// <summary>
        /// Tests whether one collection is a subset of another collection and throws an exception if any element in the subset is not also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected to be a subset of <paramref name="superset" />.</param>
        /// <param name="superset">The collection expected to be a superset of <paramref name="subset" />.</param>
        /// <param name="comparer">The comparer used to determine if an elements in the <paramref name="superset"/> matched that of the <paramref name="subset"/>.</param>
        /// <param name="message"> The message to include in the exception when an element in <paramref name="subset" /> is not found in <paramref name="superset" />. The message is shown in test results.</param>
        public static void IsSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset, IEqualityComparer<T> comparer, string? message)
        {
            Guard.NotNull(subset, nameof(subset));
            Guard.NotNull(superset, nameof(superset));
            Guard.NotNull(comparer, nameof(comparer));

            var missingItems = subset.Except(superset, comparer).ToArray();
            if (missingItems.Length > 0)
                assert.ThrowAssertError(message, ("MissingItems", FormatCollection(missingItems)));
        }

        /// <summary>
        /// Tests whether one collection is not a subset of another collection and throws an exception if all elements in the subset are also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected not to be a subset of <paramref name="superset" />..</param>
        /// <param name="superset">The collection expected not to be a superset of <paramref name="subset" />.</param>
        public static void IsNotSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset)
            => IsNotSubsetOf(assert, subset, superset, EqualityComparer<T>.Default, null);

        /// <summary>
        /// Tests whether one collection is not a subset of another collection and throws an exception if all elements in the subset are also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected not to be a subset of <paramref name="superset" />..</param>
        /// <param name="superset">The collection expected not to be a superset of <paramref name="subset" />.</param>
        /// <param name="message">The message to include in the exception when every element in <paramref name="subset" /> is also found in <paramref name="superset" />. The message is shown in test results.</param>
        public static void IsNotSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset, string? message)
            => IsNotSubsetOf(assert, subset, superset, EqualityComparer<T>.Default, message);

        /// <summary>
        /// Tests whether one collection is not a subset of another collection and throws an exception if all elements in the subset are also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected not to be a subset of <paramref name="superset" />..</param>
        /// <param name="superset">The collection expected not to be a superset of <paramref name="subset" />.</param>
        /// <param name="comparer">The comparer used to determine if an elements in the <paramref name="superset"/> matched that of the <paramref name="subset"/>.</param>
        public static void IsNotSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset, IEqualityComparer<T> comparer)
            => IsNotSubsetOf(assert, subset, superset, comparer, null);

        /// <summary>
        /// Tests whether one collection is not a subset of another collection and throws an exception if all elements in the subset are also in the superset.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="subset">The collection expected not to be a subset of <paramref name="superset" />..</param>
        /// <param name="superset">The collection expected not to be a superset of <paramref name="subset" />.</param>
        /// <param name="comparer">The comparer used to determine if an elements in the <paramref name="superset"/> matched that of the <paramref name="subset"/>.</param>
        /// <param name="message">The message to include in the exception when every element in <paramref name="subset" /> is also found in <paramref name="superset" />. The message is shown in test results.</param>
        public static void IsNotSubsetOf<T>(this AssertBase assert, IEnumerable<T> subset, IEnumerable<T> superset, IEqualityComparer<T> comparer, string? message)
        {
            Guard.NotNull(subset, nameof(subset));
            Guard.NotNull(superset, nameof(superset));
            Guard.NotNull(comparer, nameof(comparer));

            var missingItems = subset.Except(superset, comparer).ToArray();
            if (missingItems.Length == 0)
                assert.ThrowAssertError(message);
        }

        /// <summary>
        /// Tests whether two collections contain the same elements and throws an exception if either collection contains an element not in the other collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        public static void AreEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual)
            => AreEquivalent(assert, expected, actual, EqualityComparer<T>.Default, null);

        /// <summary>
        /// Tests whether two collections contain the same elements and throws an exception if either collection contains an element not in the other collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when an element was found in one of the collections but not the other. The message is shown in test results.</param>
        public static void AreEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, string? message)
            => AreEquivalent(assert, expected, actual, EqualityComparer<T>.Default, message);

        /// <summary>
        /// Tests whether two collections contain the same elements and throws an exception if either collection contains an element not in the other collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="expected"/>.</param>
        public static void AreEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer)
            => AreEquivalent(assert, expected, actual, comparer, null);

        /// <summary>
        /// Tests whether two collections contain the same elements and throws an exception if either collection contains an element not in the other collection.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="expected"/>.</param>
        /// <param name="message">The message to include in the exception when an element was found in one of the collections but not the other. The message is shown in test results.</param>
        public static void AreEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer, string? message)
        {
            Guard.NotNull(expected, nameof(expected));
            Guard.NotNull(actual, nameof(actual));
            Guard.NotNull(comparer, nameof(comparer));

            var diff = new HashSet<T>(expected, comparer);
            diff.SymmetricExceptWith(actual);
            if (diff.Count > 0)
                assert.ThrowAssertError(message, ("Difference", FormatCollection(diff)));
        }

        /// <summary>
        /// Tests whether two collections contain the different elements and throws an exception if the two collections contain identical elements without regard to order.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects to be different than the actual collection.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        public static void AreNotEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual)
            => AreNotEquivalent(assert, expected, actual, EqualityComparer<T>.Default, null);

        /// <summary>
        /// Tests whether two collections contain the different elements and throws an exception if the two collections contain identical elements without regard to order.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects to be different than the actual collection.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> contains the same elements as <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreNotEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, string? message)
            => AreNotEquivalent(assert, expected, actual, EqualityComparer<T>.Default, message);

        /// <summary>
        /// Tests whether two collections contain the different elements and throws an exception if the two collections contain identical elements without regard to order.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects to be different than the actual collection.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="expected"/>.</param>
        public static void AreNotEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer)
            => AreNotEquivalent(assert, expected, actual, comparer, null);

        /// <summary>
        /// Tests whether two collections contain the different elements and throws an exception if the two collections contain identical elements without regard to order.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This contains the elements the test expects to be different than the actual collection.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="expected"/>.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> contains the same elements as <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreNotEquivalent<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer, string? message)
        {
            Guard.NotNull(expected, nameof(expected));
            Guard.NotNull(actual, nameof(actual));
            Guard.NotNull(comparer, nameof(comparer));

            var diff = new HashSet<T>(expected, comparer);
            diff.SymmetricExceptWith(actual);
            if (diff.Count == 0)
                assert.ThrowAssertError(message);
        }

        /// <summary>
        /// Tests whether all elements in the specified collection are instances of the expected type and throws an exception if the expected type is
        /// not in the inheritance hierarchy of one or more of the elements.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection containing elements the test expects to be of the specified type.</param>
        /// <param name="expectedType">The expected type of each element of <paramref name="collection" />.</param>
        public static void AllItemsAreInstancesOfType(this AssertBase assert, IEnumerable collection, Type expectedType)
            => AllItemsAreInstancesOfType(assert, collection, expectedType, null);

        /// <summary>
        /// Tests whether all elements in the specified collection are instances of the expected type and throws an exception if the expected type is
        /// not in the inheritance hierarchy of one or more of the elements.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection containing elements the test expects to be of the specified type.</param>
        /// <param name="expectedType">The expected type of each element of <paramref name="collection" />.</param>
        /// <param name="message">The message to include in the exception when an element in <paramref name="collection" /> is not an instance of <paramref name="expectedType" />. The message is shown in test results.</param>
        public static void AllItemsAreInstancesOfType(this AssertBase assert, IEnumerable collection, Type expectedType, string? message)
        {
            Guard.NotNull(collection, nameof(collection));
            Guard.NotNull(expectedType, nameof(expectedType));

            var wrong = new List<(int, Type?, object?)>();
            int idx = 0;
            foreach (var item in collection)
            {
                if (!expectedType.IsInstanceOfType(item))
                    wrong.Add((idx, item?.GetType(), item));
                idx++;
            }

            if (wrong.Count > 0)
                assert.ThrowAssertError(message, ("ExpectedType", expectedType), ("WrongItems", FormatCollection(wrong)));
        }

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This is the collection the tests expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        public static void AreEqual<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual)
            => AreEqual(assert, expected, actual, EqualityComparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This is the collection the tests expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreEqual<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, string? message)
            => AreEqual(assert, expected, actual, EqualityComparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This is the collection the tests expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="expected"/>.</param>
        public static void AreEqual<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer)
            => AreEqual(assert, expected, actual, comparer, null);

        /// <summary>
        /// Tests whether the specified collections are equal and throws an exception if the two collections are not equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="expected">The first collection to compare. This is the collection the tests expects.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="expected"/>.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is not equal to <paramref name="expected" />. The message is shown in test results.</param>
        public static void AreEqual<T>(this AssertBase assert, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer, string? message)
        {
            Guard.NotNull(expected, nameof(expected));
            Guard.NotNull(actual, nameof(actual));
            Guard.NotNull(comparer, nameof(comparer));

            var wrong = new List<(int, T?, T?)>();
            using var eenum = expected.GetEnumerator();
            using var aenum = actual.GetEnumerator();
            var idx = 0;
            bool emore = true;
            bool amore = true;
            while ((amore = aenum.MoveNext()) && (emore = eenum.MoveNext()))
            {
                if (!comparer.Equals(aenum.Current, eenum.Current))
                    wrong.Add((idx, aenum.Current, eenum.Current));
                idx++;
            }

            string? p2name = null;
            var p2items = new List<T?>();
            IEnumerator<T>? p2enum;
            if (amore)
            {
                p2name = "UnexpectedItems";
                p2enum = aenum;
            }
            else if (emore)
            {
                p2name = "MissingItems";
                p2enum = eenum;
            }
            else
            {
                p2enum = null;
            }

            if (p2enum != null && p2name != null)
            {
                while (p2enum.MoveNext())
                    p2items.Add(p2enum.Current);
                assert.ThrowAssertError(message, ("WrongItems", FormatCollection(wrong)), (p2name, FormatCollection(p2items)));
            }
            else
            {
                assert.ThrowAssertError(message, ("WrongItems", FormatCollection(wrong)));
            }
        }

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first collection to compare. This is the collection the tests expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        public static void AreNotEqual<T>(this AssertBase assert, IEnumerable<T> notExpected, IEnumerable<T> actual)
            => AreNotEqual(assert, notExpected, actual, EqualityComparer<T>.Default, null);

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first collection to compare. This is the collection the tests expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void AreNotEqual<T>(this AssertBase assert, IEnumerable<T> notExpected, IEnumerable<T> actual, string? message)
            => AreNotEqual(assert, notExpected, actual, EqualityComparer<T>.Default, message);

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first collection to compare. This is the collection the tests expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="notExpected"/>.</param>
        public static void AreNotEqual<T>(this AssertBase assert, IEnumerable<T> notExpected, IEnumerable<T> actual, IEqualityComparer<T> comparer)
            => AreNotEqual(assert, notExpected, actual, comparer, null);

        /// <summary>
        /// Tests whether the specified collections are unequal and throws an exception if the two collections are equal. Equality is defined as having the same
        /// elements in the same order and quantity.
        /// </summary>
        /// <typeparam name="T">The type of items in the collection.</typeparam>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="notExpected">The first collection to compare. This is the collection the tests expects not to match <paramref name="actual" />.</param>
        /// <param name="actual">The second collection to compare. This is the collection produced by the code under test.</param>
        /// <param name="comparer">The comparer used to determine if an elements from <paramref name="actual"/> is equal to an element in <paramref name="notExpected"/>.</param>
        /// <param name="message">The message to include in the exception when <paramref name="actual" /> is equal to <paramref name="notExpected" />. The message is shown in test results.</param>
        public static void AreNotEqual<T>(this AssertBase assert, IEnumerable<T> notExpected, IEnumerable<T> actual, IEqualityComparer<T> comparer, string? message)
        {
            Guard.NotNull(notExpected, nameof(notExpected));
            Guard.NotNull(actual, nameof(actual));
            Guard.NotNull(comparer, nameof(comparer));

            if (notExpected.SequenceEqual(actual, comparer))
                assert.ThrowAssertError(message);
        }

        private static string? FormatCollection<T>(ICollection<T>? collection)
        {
            return collection == null ? null :
                   collection.Count == 0 ? "[]" : $"[{Environment.NewLine}\t{string.Join($",{Environment.NewLine}\t", collection)}{Environment.NewLine}]";
        }
    }
}
