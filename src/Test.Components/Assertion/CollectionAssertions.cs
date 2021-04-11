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

#pragma warning disable SA1005 // Single line comments should begin with single space
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
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for duplicate elements.</param>
        public static void AllItemsAreUnique(this AssertBase assert, IEnumerable collection)
            => AllItemsAreUnique(assert, collection, null);

        /// <summary>
        /// Tests whether all items in the specified collection are unique or not and throws if any two elements in the collection are equal.
        /// </summary>
        /// <param name="assert">The assert object to test with.</param>
        /// <param name="collection">The collection in which to search for duplicate elements.</param>
        /// <param name="message">The message to include in the exception when <paramref name="collection" /> contains at least one duplicate element. The message is shown in test results.</param>
        public static void AllItemsAreUnique(this AssertBase assert, IEnumerable collection, string? message)
        {
            int? nullIdx = null;
            var checkedItems = new Dictionary<object, int>();
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
                    if (checkedItems.TryGetValue(item, out int i))
                        faultyIdx.Add((i, idx));
                    else
                        checkedItems.Add(item, idx);
                }

                idx++;
            }

            if (faultyIdx.Count > 0)
                assert.ThrowAssertError(message, (faultyIdx.Count > 1 ? "Indices" : "Index", string.Join(", ", faultyIdx.Select(x => $"{x.Item1}={x.Item2}"))));
        }

        ///// <summary>
        ///// Tests whether one collection is a subset of another collection and
        ///// throws an exception if any element in the subset is not also in the
        ///// superset.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="subset">
        ///// The collection expected to be a subset of <paramref name="superset" />.
        ///// </param><param name="superset">
        ///// The collection expected to be a superset of <paramref name="subset" />.</param>
        //public static void IsSubsetOf(this AssertBase assert, ICollection subset, ICollection superset)
        //    => assert.CatchAssertException(() => CollectionAssert.IsSubsetOf(subset, superset));

        ///// <summary>
        ///// Tests whether one collection is a subset of another collection and
        ///// throws an exception if any element in the subset is not also in the
        ///// superset.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="subset">
        ///// The collection expected to be a subset of <paramref name="superset" />.
        ///// </param><param name="superset">
        ///// The collection expected to be a superset of <paramref name="subset" />.</param><param name="message">
        ///// The message to include in the exception when an element in
        ///// <paramref name="subset" /> is not found in <paramref name="superset" />.
        ///// The message is shown in test results.
        ///// </param>
        //public static void IsSubsetOf(this AssertBase assert, ICollection subset, ICollection superset, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.IsSubsetOf(subset, superset, message));

        ///// <summary>
        ///// Tests whether one collection is not a subset of another collection and
        ///// throws an exception if all elements in the subset are also in the
        ///// superset.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="subset">
        ///// The collection expected not to be a subset of <paramref name="superset" />.
        ///// </param><param name="superset">
        ///// The collection expected not to be a superset of <paramref name="subset" />.</param>
        //public static void IsNotSubsetOf(this AssertBase assert, ICollection subset, ICollection superset)
        //    => assert.CatchAssertException(() => CollectionAssert.IsNotSubsetOf(subset, superset));

        ///// <summary>
        ///// Tests whether one collection is not a subset of another collection and
        ///// throws an exception if all elements in the subset are also in the
        ///// superset.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="subset">
        ///// The collection expected not to be a subset of <paramref name="superset" />.
        ///// </param><param name="superset">
        ///// The collection expected not to be a superset of <paramref name="subset" />.</param><param name="message">
        ///// The message to include in the exception when every element in
        ///// <paramref name="subset" /> is also found in <paramref name="superset" />.
        ///// The message is shown in test results.
        ///// </param>
        //public static void IsNotSubsetOf(this AssertBase assert, ICollection subset, ICollection superset, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.IsNotSubsetOf(subset, superset, message));

        ///// <summary>
        ///// Tests whether two collections contain the same elements and throws an
        ///// exception if either collection contains an element not in the other
        ///// collection.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This contains the elements the test
        ///// expects.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by
        ///// the code under test.
        ///// </param>
        //public static void AreEquivalent(this AssertBase assert, ICollection expected, ICollection actual)
        //    => assert.CatchAssertException(() => CollectionAssert.AreEquivalent(expected, actual));

        ///// <summary>
        ///// Tests whether two collections contain the same elements and throws an
        ///// exception if either collection contains an element not in the other
        ///// collection.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This contains the elements the test
        ///// expects.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by
        ///// the code under test.
        ///// </param><param name="message">
        ///// The message to include in the exception when an element was found
        ///// in one of the collections but not the other. The message is shown
        ///// in test results.
        ///// </param>
        //public static void AreEquivalent(this AssertBase assert, ICollection expected, ICollection actual, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.AreEquivalent(expected, actual, message));

        ///// <summary>
        ///// Tests whether two collections contain the different elements and throws an
        ///// exception if the two collections contain identical elements without regard
        ///// to order.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This contains the elements the test
        ///// expects to be different than the actual collection.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by
        ///// the code under test.
        ///// </param>
        //public static void AreNotEquivalent(this AssertBase assert, ICollection expected, ICollection actual)
        //    => assert.CatchAssertException(() => CollectionAssert.AreNotEquivalent(expected, actual));

        ///// <summary>
        ///// Tests whether two collections contain the different elements and throws an
        ///// exception if the two collections contain identical elements without regard
        ///// to order.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This contains the elements the test
        ///// expects to be different than the actual collection.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by
        ///// the code under test.
        ///// </param><param name="message">
        ///// The message to include in the exception when <paramref name="actual" />
        ///// contains the same elements as <paramref name="expected" />. The message
        ///// is shown in test results.
        ///// </param>
        //public static void AreNotEquivalent(this AssertBase assert, ICollection expected, ICollection actual, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.AreNotEquivalent(expected, actual, message));

        ///// <summary>
        ///// Tests whether all elements in the specified collection are instances
        ///// of the expected type and throws an exception if the expected type is
        ///// not in the inheritance hierarchy of one or more of the elements.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="collection">
        ///// The collection containing elements the test expects to be of the
        ///// specified type.
        ///// </param><param name="expectedType">
        ///// The expected type of each element of <paramref name="collection" />.
        ///// </param>
        //public static void AllItemsAreInstancesOfType(this AssertBase assert, ICollection collection, Type expectedType)
        //    => assert.CatchAssertException(() => CollectionAssert.AllItemsAreInstancesOfType(collection, expectedType));

        ///// <summary>
        ///// Tests whether all elements in the specified collection are instances
        ///// of the expected type and throws an exception if the expected type is
        ///// not in the inheritance hierarchy of one or more of the elements.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="collection">
        ///// The collection containing elements the test expects to be of the
        ///// specified type.
        ///// </param><param name="expectedType">
        ///// The expected type of each element of <paramref name="collection" />.
        ///// </param><param name="message">
        ///// The message to include in the exception when an element in
        ///// <paramref name="collection" /> is not an instance of
        ///// <paramref name="expectedType" />. The message is shown in test results.
        ///// </param>
        //public static void AllItemsAreInstancesOfType(this AssertBase assert, ICollection collection, Type expectedType, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.AllItemsAreInstancesOfType(collection, expectedType, message));

        ///// <summary>
        ///// Tests whether the specified collections are equal and throws an exception
        ///// if the two collections are not equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Whether two elements are the same
        ///// is checked using <see cref="M:System.Object.Equals(System.Object,System.Object)" /> method.
        ///// Different references to the same value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This is the collection the tests expects.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param>
        //public static void AreEqual(this AssertBase assert, ICollection expected, ICollection actual)
        //    => assert.CatchAssertException(() => CollectionAssert.AreEqual(expected, actual));

        ///// <summary>
        ///// Tests whether the specified collections are equal and throws an exception
        ///// if the two collections are not equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Whether two elements are the same
        ///// is checked using <see cref="M:System.Object.Equals(System.Object,System.Object)" /> method.
        ///// Different references to the same value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This is the collection the tests expects.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param><param name="message">
        ///// The message to include in the exception when <paramref name="actual" />
        ///// is not equal to <paramref name="expected" />. The message is shown in
        ///// test results.
        ///// </param>
        //public static void AreEqual(this AssertBase assert, ICollection expected, ICollection actual, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.AreEqual(expected, actual, message));

        ///// <summary>
        ///// Tests whether the specified collections are unequal and throws an exception
        ///// if the two collections are equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Whether two elements are the same
        ///// is checked using <see cref="M:System.Object.Equals(System.Object,System.Object)" /> method.
        ///// Different references to the same value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="notExpected">
        ///// The first collection to compare. This is the collection the tests expects
        ///// not to match <paramref name="actual" />.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param>
        //public static void AreNotEqual(this AssertBase assert, ICollection notExpected, ICollection actual)
        //    => assert.CatchAssertException(() => CollectionAssert.AreNotEqual(notExpected, actual));

        ///// <summary>
        ///// Tests whether the specified collections are unequal and throws an exception
        ///// if the two collections are equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Whether two elements are the same
        ///// is checked using <see cref="M:System.Object.Equals(System.Object,System.Object)" /> method.
        ///// Different references to the same value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="notExpected">
        ///// The first collection to compare. This is the collection the tests expects
        ///// not to match <paramref name="actual" />.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param><param name="message">
        ///// The message to include in the exception when <paramref name="actual" />
        ///// is equal to <paramref name="notExpected" />. The message is shown in
        ///// test results.
        ///// </param>
        //public static void AreNotEqual(this AssertBase assert, ICollection notExpected, ICollection actual, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.AreNotEqual(notExpected, actual, message));

        ///// <summary>
        ///// Tests whether the specified collections are equal and throws an exception
        ///// if the two collections are not equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Different references to the same
        ///// value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This is the collection the tests expects.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param><param name="comparer">
        ///// The compare implementation to use when comparing elements of the collection.
        ///// </param>
        //public static void AreEqual(this AssertBase assert, ICollection expected, ICollection actual, IComparer comparer)
        //    => assert.CatchAssertException(() => CollectionAssert.AreEqual(expected, actual, comparer));

        ///// <summary>
        ///// Tests whether the specified collections are equal and throws an exception
        ///// if the two collections are not equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Different references to the same
        ///// value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="expected">
        ///// The first collection to compare. This is the collection the tests expects.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param><param name="comparer">
        ///// The compare implementation to use when comparing elements of the collection.
        ///// </param><param name="message">
        ///// The message to include in the exception when <paramref name="actual" />
        ///// is not equal to <paramref name="expected" />. The message is shown in
        ///// test results.
        ///// </param>
        //public static void AreEqual(this AssertBase assert, ICollection expected, ICollection actual, IComparer comparer, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.AreEqual(expected, actual, comparer, message));

        ///// <summary>
        ///// Tests whether the specified collections are unequal and throws an exception
        ///// if the two collections are equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Different references to the same
        ///// value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="notExpected">
        ///// The first collection to compare. This is the collection the tests expects
        ///// not to match <paramref name="actual" />.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param><param name="comparer">
        ///// The compare implementation to use when comparing elements of the collection.
        ///// </param>
        //public static void AreNotEqual(this AssertBase assert, ICollection notExpected, ICollection actual, IComparer comparer)
        //    => assert.CatchAssertException(() => CollectionAssert.AreNotEqual(notExpected, actual, comparer));

        ///// <summary>
        ///// Tests whether the specified collections are unequal and throws an exception
        ///// if the two collections are equal. Equality is defined as having the same
        ///// elements in the same order and quantity. Different references to the same
        ///// value are considered equal.
        ///// </summary><param name="assert">The assert object to test with.
        ///// </param><param name="notExpected">
        ///// The first collection to compare. This is the collection the tests expects
        ///// not to match <paramref name="actual" />.
        ///// </param><param name="actual">
        ///// The second collection to compare. This is the collection produced by the
        ///// code under test.
        ///// </param><param name="comparer">
        ///// The compare implementation to use when comparing elements of the collection.
        ///// </param><param name="message">
        ///// The message to include in the exception when <paramref name="actual" />
        ///// is equal to <paramref name="notExpected" />. The message is shown in
        ///// test results.
        ///// </param>
        //public static void AreNotEqual(this AssertBase assert, ICollection notExpected, ICollection actual, IComparer comparer, string? message)
        //    => assert.CatchAssertException(() => CollectionAssert.AreNotEqual(notExpected, actual, comparer, message));

        private static string? FormatCollection<T>(ICollection<T>? collection)
        {
            return collection == null ? null :
                   collection.Count == 0 ? "[]" : $"[{Environment.NewLine}\t{string.Join($",{Environment.NewLine}\t", collection)}{Environment.NewLine}]";
        }
    }
}
