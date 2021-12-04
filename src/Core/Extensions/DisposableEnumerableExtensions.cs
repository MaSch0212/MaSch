namespace MaSch.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="IDisposableEnumerable{T}"/> and <see cref="IDisposableEnumerable"/>.
/// </summary>
public static class DisposableEnumerableExtensions
{
    #region IDisposableEnumerable<T>

    /// <summary>
    /// Executes an action for each item in the <see cref="IDisposableEnumerable{T}"/> and returns each item after the action is executed.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>Returns each item after the action has been executed.</returns>
    public static IDisposableEnumerable<T> Each<T>(this IDisposableEnumerable<T> enumerable, Action<T> action)
    {
        return Redirect(Guard.NotNull(enumerable, nameof(enumerable)), x => x.Each(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Executes an action for each item in the <see cref="IDisposableEnumerable{T}"/> and returns each item after the action is executed.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>Returns each item after the action has been executed.</returns>
    public static IDisposableEnumerable<T> Each<T>(this IDisposableEnumerable<T> enumerable, Action<T, LoopState> action)
    {
        return Redirect(Guard.NotNull(enumerable, nameof(enumerable)), x => x.Each(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and returns each item and the exception (if the action failed) after the action is executed.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>Returns each item with the exception that occurred after the action has been executed. If no exception occurred, the exception entry is <c>null</c>.</returns>
    public static IDisposableEnumerable<(T Item, Exception? Error)> TryEach<T>(this IDisposableEnumerable<T> enumerable, Action<T> action)
    {
        return Redirect(Guard.NotNull(enumerable, nameof(enumerable)), x => x.TryEach(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and returns each item and the exception (if the action failed) after the action is executed.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>Returns each item with the exception that occurred after the action has been executed. If no exception occurred, the exception entry is <c>null</c>.</returns>
    public static IDisposableEnumerable<(T Item, Exception? Error)> TryEach<T>(this IDisposableEnumerable<T> enumerable, Action<T, LoopState> action)
    {
        return Redirect(Guard.NotNull(enumerable, nameof(enumerable)), x => x.TryEach(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Executes an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    public static void ForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T> action)
    {
        Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.ForEach(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Executes an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    public static void ForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T, LoopState> action)
    {
        Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.ForEach(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Executes an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute. The first parameter is the last element of the loop - for the first item, this parameter is <c>default</c>.</param>
    public static void ForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T?, T, LoopState> action)
    {
        Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.ForEach(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T> action)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.TryForEach(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T> action, bool continueOnError)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.TryForEach(Guard.NotNull(action, nameof(action)), continueOnError));
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T, LoopState> action)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.TryForEach(Guard.NotNull(action, nameof(action))));
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T, LoopState> action, bool continueOnError)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.TryForEach(Guard.NotNull(action, nameof(action)), continueOnError));
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="errors">A collection containing the errors that occurred.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T> action, out ICollection<(T Item, Exception Error)> errors)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => (x.TryForEach(Guard.NotNull(action, nameof(action)), out var err), err), out errors);
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="errors">A collection containing the errors that occurred.</param>
    /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T> action, out ICollection<(T Item, Exception Error)> errors, bool continueOnError)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => (x.TryForEach(Guard.NotNull(action, nameof(action)), out var err, continueOnError), err), out errors);
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="errors">A collection containing the errors that occurred.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T, LoopState> action, out ICollection<(T Item, int Index, Exception Error)> errors)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => (x.TryForEach(Guard.NotNull(action, nameof(action)), out var err), err), out errors);
    }

    /// <summary>
    /// Tries to execute an action for each item in the <see cref="IDisposableEnumerable{T}"/> and disposes it afterwards.
    /// </summary>
    /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="action">The action to execute.</param>
    /// <param name="errors">A collection containing the errors that occurred.</param>
    /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
    /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
    public static bool TryForEachAndDispose<T>(this IDisposableEnumerable<T> enumerable, Action<T, LoopState> action, out ICollection<(T Item, int Index, Exception Error)> errors, bool continueOnError)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => (x.TryForEach(Guard.NotNull(action, nameof(action)), out var err, continueOnError), err), out errors);
    }

    #region Linq wrappers

    /// <summary>
    /// Creates an array from a <see cref="IDisposableEnumerable{T}"/> and disposes it.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="enumerable">An <see cref="IDisposableEnumerable{T}"/> to create an array from.</param>
    /// <returns>An array that contains the elements from the input sequence.</returns>
    public static T[] ToArrayAndDispose<T>(this IDisposableEnumerable<T> enumerable)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.ToArray());
    }

    /// <summary>
    /// Creates a <see cref="List{T}"/> from an <see cref="IDisposableEnumerable{T}"/> and disposes it.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="enumerable">The <see cref="IDisposableEnumerable{T}"/> to create a <see cref="List{T}"/> from.</param>
    /// <returns>A <see cref="List{T}"/> that contains elements from the input sequence.</returns>
    public static List<T> ToListAndDispose<T>(this IDisposableEnumerable<T> enumerable)
    {
        return Guard.NotNull(enumerable, nameof(enumerable)).DoAndDispose(x => x.ToList());
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey, TSource}"/> from an <see cref="IDisposableEnumerable{TSource}"/> according to a specified key selector function and disposes it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> to create a <see cref="Dictionary{TKey, TSource}"/> from.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <returns>A <see cref="Dictionary{TKey, TSource}"/> that contains keys and values.</returns>
    public static Dictionary<TKey, TSource> ToDictionaryAndDispose<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        where TKey : notnull
    {
        return Guard.NotNull(source, nameof(source)).DoAndDispose(x => x.ToDictionary(Guard.NotNull(keySelector, nameof(keySelector))));
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey, TSource}"/> from an <see cref="IDisposableEnumerable{TSource}"/> according to a specified key selector function and key comparer and disposes it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> to create a <see cref="Dictionary{TKey, TSource}"/> from.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
    /// <returns>A <see cref="Dictionary{TKey, TSource}"/> that contains keys and values.</returns>
    public static Dictionary<TKey, TSource> ToDictionaryAndDispose<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        return Guard.NotNull(source, nameof(source)).DoAndDispose(x => x.ToDictionary(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey, TElement}"/> from an <see cref="IDisposableEnumerable{TSource}"/> according to specified key selector and element selector functions and disposes it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
    /// <typeparam name="TElement">The type of the value returned by elementSelector.</typeparam>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> to create a <see cref="Dictionary{TKey, TElement}"/> from.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
    /// <returns>A <see cref="Dictionary{TKey, TElement}"/> that contains values of type <typeparamref name="TElement"/> selected from the input sequence.</returns>
    public static Dictionary<TKey, TElement> ToDictionaryAndDispose<TSource, TKey, TElement>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        where TKey : notnull
    {
        return Guard.NotNull(source, nameof(source)).DoAndDispose(x => x.ToDictionary(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(elementSelector, nameof(elementSelector))));
    }

    /// <summary>
    /// Creates a <see cref="Dictionary{TKey, TElement}"/> from an <see cref="IDisposableEnumerable{TSource}"/> according to a specified key selector function, a comparer, and an element selector function and disposes it.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
    /// <typeparam name="TElement">The type of the value returned by elementSelector.</typeparam>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> to create a <see cref="Dictionary{TKey, TElement}"/> from.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
    /// <returns>A <see cref="Dictionary{TKey, TElement}"/> that contains values of type <typeparamref name="TElement"/> selected from the input sequence.</returns>
    public static Dictionary<TKey, TElement> ToDictionaryAndDispose<TSource, TKey, TElement>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        where TKey : notnull
    {
        return Guard.NotNull(source, nameof(source)).DoAndDispose(x => x.ToDictionary(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(elementSelector, nameof(elementSelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Returns distinct elements from a sequence by using the default equality comparer to compare values.</summary>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}"/> that contains distinct elements from the source sequence.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> is <see langword="null" />.
    /// </exception>
    public static IDisposableEnumerable<TSource> Distinct<TSource>(this IDisposableEnumerable<TSource> source)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Distinct());
    }

    /// <summary>Returns distinct elements from a sequence by using a specified <see cref="IEqualityComparer{TSource}"/> to compare values.</summary>
    /// <param name="source">The sequence to remove duplicate elements from.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}"/> that contains distinct elements from the source sequence.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> is <see langword="null" />.
    /// </exception>
    public static IDisposableEnumerable<TSource> Distinct<TSource>(this IDisposableEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Distinct(Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Produces the set difference of two sequences by using the default equality comparer to compare values.</summary>
    /// <param name="first">An <see cref="IDisposableEnumerable{TSource}"/> whose elements that are not also in <paramref name="second" /> will be returned.</param>
    /// <param name="second">An <see cref="IEnumerable{TSource}"/> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.
    /// </exception>
    public static IDisposableEnumerable<TSource> Except<TSource>(this IDisposableEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        return Redirect(Guard.NotNull(first, nameof(first)), x => x.Except(Guard.NotNull(second, nameof(second))));
    }

    /// <summary>Produces the set difference of two sequences by using the specified <see cref="IEqualityComparer{TSource}"/> to compare values.</summary>
    /// <param name="first">An <see cref="IDisposableEnumerable{TSource}"/> whose elements that are not also in <paramref name="second" /> will be returned.</param>
    /// <param name="second">An <see cref="IEnumerable{TSource}"/> whose elements that also occur in the first sequence will cause those elements to be removed from the returned sequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.
    /// </exception>
    public static IDisposableEnumerable<TSource> Except<TSource>(this IDisposableEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
        return Redirect(Guard.NotNull(first, nameof(first)), x => x.Except(Guard.NotNull(second, nameof(second)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. Key values are compared by using a specified comparer, and the elements of each group are projected by using a specified function.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in an <see cref="IGrouping{TKey, TElement}"/>.</param>
    /// <param name="resultSelector">A function to create a result value from each group.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys with.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <typeparam name="TElement">The type of the elements in each <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
    /// <returns>A collection of elements of type <typeparamref name="TResult" /> where each element represents a projection over a group and its key.</returns>
    public static IDisposableEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(elementSelector, nameof(elementSelector)), Guard.NotNull(resultSelector, nameof(resultSelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The elements of each group are projected by using a specified function.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in an <see cref="IGrouping{TKey, TElement}"/>.</param>
    /// <param name="resultSelector">A function to create a result value from each group.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <typeparam name="TElement">The type of the elements in each <see cref="IGrouping{TKey, TElement}"/>.</typeparam>
    /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
    /// <returns>A collection of elements of type <typeparamref name="TResult" /> where each element represents a projection over a group and its key.</returns>
    public static IDisposableEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(elementSelector, nameof(elementSelector)), Guard.NotNull(resultSelector, nameof(resultSelector))));
    }

    /// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key. The keys are compared by using a specified comparer.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="resultSelector">A function to create a result value from each group.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys with.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
    /// <returns>A collection of elements of type <typeparamref name="TResult" /> where each element represents a projection over a group and its key.</returns>
    public static IDisposableEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(resultSelector, nameof(resultSelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Groups the elements of a sequence according to a specified key selector function and creates a result value from each group and its key.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="resultSelector">A function to create a result value from each group.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <typeparam name="TResult">The type of the result value returned by <paramref name="resultSelector" />.</typeparam>
    /// <returns>A collection of elements of type <typeparamref name="TResult" /> where each element represents a projection over a group and its key.</returns>
    public static IDisposableEnumerable<TResult> GroupBy<TSource, TKey, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(resultSelector, nameof(resultSelector))));
    }

    /// <summary>Groups the elements of a sequence according to a specified key selector function and compares the keys by using a specified comparer.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <c>IDisposableEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt;</c> in C# or <c>IDisposableEnumerable(Of IGrouping(Of TKey, TSource))</c> in Visual Basic where each <see cref="IGrouping{TKey,TSource}" /> object contains a collection of objects and a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.
    /// </exception>
    public static IDisposableEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Groups the elements of a sequence according to a specified key selector function and projects the elements for each group by using a specified function.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in the <see cref="IGrouping{TKey,TElement}" />.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the <see cref="IGrouping{TKey,TElement}" />.</typeparam>
    /// <returns>An <c>IDisposableEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt;</c> in C# or <c>IDisposableEnumerable(Of IGrouping(Of TKey, TElement))</c> in Visual Basic where each <see cref="IGrouping{TKey,TElement}" /> object contains a collection of objects of type <typeparamref name="TElement" /> and a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(elementSelector, nameof(elementSelector))));
    }

    /// <summary>Groups the elements of a sequence according to a specified key selector function.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <c>IDisposableEnumerable&lt;IGrouping&lt;TKey, TSource&gt;&gt;</c> in C# or <c>IDisposableEnumerable(Of IGrouping(Of TKey, TSource))</c> in Visual Basic where each <see cref="IGrouping{TKey,TSource}" /> object contains a sequence of objects and a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.
    /// </exception>
    public static IDisposableEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector))));
    }

    /// <summary>Groups the elements of a sequence according to a key selector function. The keys are compared by using a comparer and each group's elements are projected by using a specified function.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}"/> whose elements to group.</param>
    /// <param name="keySelector">A function to extract the key for each element.</param>
    /// <param name="elementSelector">A function to map each source element to an element in an <see cref="IGrouping{TKey,TElement}" />.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to compare keys.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <typeparam name="TElement">The type of the elements in the <see cref="IGrouping{TKey,TElement}" />.</typeparam>
    /// <returns>An <c>IDisposableEnumerable&lt;IGrouping&lt;TKey, TElement&gt;&gt;</c> in C# or <c>IDisposableEnumerable(Of IGrouping(Of TKey, TElement))</c> in Visual Basic where each <see cref="IGrouping{TKey,TElement}" /> object contains a collection of objects of type <typeparamref name="TElement" /> and a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> or <paramref name="elementSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.GroupBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(elementSelector, nameof(elementSelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Correlates the elements of two sequences based on key equality and groups the results. A specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> is used to compare keys.</summary>
    /// <param name="outer">The first sequence to join.</param>
    /// <param name="inner">The sequence to join to the first sequence.</param>
    /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TKey}"/> to hash and compare keys.</param>
    /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
    /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}"/> that contains elements of type <typeparamref name="TResult" /> that are obtained by performing a grouped join on two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IDisposableEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(outer, nameof(outer)), x => x.GroupJoin(Guard.NotNull(inner, nameof(inner)), Guard.NotNull(outerKeySelector, nameof(outerKeySelector)), Guard.NotNull(innerKeySelector, nameof(innerKeySelector)), Guard.NotNull(resultSelector, nameof(resultSelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Correlates the elements of two sequences based on equality of keys and groups the results. The default equality comparer is used to compare keys.</summary>
    /// <param name="outer">The first sequence to join.</param>
    /// <param name="inner">The sequence to join to the first sequence.</param>
    /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
    /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
    /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}"/> that contains elements of type <typeparamref name="TResult" /> that are obtained by performing a grouped join on two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IDisposableEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
    {
        return Redirect(Guard.NotNull(outer, nameof(outer)), x => x.GroupJoin(Guard.NotNull(inner, nameof(inner)), Guard.NotNull(outerKeySelector, nameof(outerKeySelector)), Guard.NotNull(innerKeySelector, nameof(innerKeySelector)), Guard.NotNull(resultSelector, nameof(resultSelector))));
    }

    /// <summary>Produces the set intersection of two sequences by using the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare values.</summary>
    /// <param name="first">An <see cref="IDisposableEnumerable{TSource}"/> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
    /// <param name="second">An <see cref="IEnumerable{TSource}"/> whose distinct elements that also appear in the first sequence will be returned.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TSource}"/> to compare values.</param>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> Intersect<TSource>(this IDisposableEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
    {
        return Redirect(Guard.NotNull(first, nameof(first)), x => x.Intersect(Guard.NotNull(second, nameof(second)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Produces the set intersection of two sequences by using the default equality comparer to compare values.</summary>
    /// <param name="first">An <see cref="IDisposableEnumerable{TSource}"/> whose distinct elements that also appear in <paramref name="second" /> will be returned.</param>
    /// <param name="second">An <see cref="IEnumerable{TSource}"/> whose distinct elements that also appear in the first sequence will be returned.</param>
    /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
    /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="first" /> or <paramref name="second" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> Intersect<TSource>(this IDisposableEnumerable<TSource> first, IEnumerable<TSource> second)
    {
        return Redirect(Guard.NotNull(first, nameof(first)), x => x.Intersect(Guard.NotNull(second, nameof(second))));
    }

    /// <summary>Correlates the elements of two sequences based on matching keys. The default equality comparer is used to compare keys.</summary>
    /// <param name="outer">The first sequence to join.</param>
    /// <param name="inner">The sequence to join to the first sequence.</param>
    /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
    /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
    /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> that has elements of type <typeparamref name="TResult" /> that are obtained by performing an inner join on two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IDisposableEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
        return Redirect(Guard.NotNull(outer, nameof(outer)), x => x.Join(Guard.NotNull(inner, nameof(inner)), Guard.NotNull(outerKeySelector, nameof(outerKeySelector)), Guard.NotNull(innerKeySelector, nameof(innerKeySelector)), Guard.NotNull(resultSelector, nameof(resultSelector))));
    }

    /// <summary>Correlates the elements of two sequences based on matching keys. A specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> is used to compare keys.</summary>
    /// <param name="outer">The first sequence to join.</param>
    /// <param name="inner">The sequence to join to the first sequence.</param>
    /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
    /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
    /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
    /// <param name="comparer">An <see cref="IEqualityComparer{TSource}"/> to hash and compare keys.</param>
    /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
    /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
    /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
    /// <typeparam name="TResult">The type of the result elements.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> that has elements of type <typeparamref name="TResult" /> that are obtained by performing an inner join on two sequences.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="outer" /> or <paramref name="inner" /> or <paramref name="outerKeySelector" /> or <paramref name="innerKeySelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(this IDisposableEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(outer, nameof(outer)), x => x.Join(Guard.NotNull(inner, nameof(inner)), Guard.NotNull(outerKeySelector, nameof(outerKeySelector)), Guard.NotNull(innerKeySelector, nameof(innerKeySelector)), Guard.NotNull(resultSelector, nameof(resultSelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Sorts the elements of a sequence in ascending order by using a specified comparer.</summary>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{TKey}" /> to compare keys.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted according to a key.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> OrderBy<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.OrderBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Sorts the elements of a sequence in ascending order according to a key.</summary>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted according to a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> OrderBy<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.OrderBy(Guard.NotNull(keySelector, nameof(keySelector))));
    }

    /// <summary>Sorts the elements of a sequence in descending order according to a key.</summary>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted in descending order according to a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> OrderByDescending<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.OrderByDescending(Guard.NotNull(keySelector, nameof(keySelector))));
    }

    /// <summary>Sorts the elements of a sequence in descending order by using a specified comparer.</summary>
    /// <param name="source">A sequence of values to order.</param>
    /// <param name="keySelector">A function to extract a key from an element.</param>
    /// <param name="comparer">An <see cref="IComparer{TKey}" /> to compare keys.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted in descending order according to a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> OrderByDescending<TSource, TKey>(this IDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.OrderByDescending(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Inverts the order of the elements in a sequence.</summary>
    /// <param name="source">A sequence of values to reverse.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> Reverse<TSource>(this IDisposableEnumerable<TSource> source)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Reverse());
    }

    /// <summary>Projects each element of a sequence into a new form.</summary>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> whose elements are the result of invoking the transform function on each element of <paramref name="source" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> Select<TSource, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Select(Guard.NotNull(selector, nameof(selector))));
    }

    /// <summary>Projects each element of a sequence into a new form by incorporating the element's index.</summary>
    /// <param name="source">A sequence of values to invoke a transform function on.</param>
    /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by <paramref name="selector" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> whose elements are the result of invoking the transform function on each element of <paramref name="source" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> Select<TSource, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, int, TResult> selector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Select(Guard.NotNull(selector, nameof(selector))));
    }

    /// <summary>Projects each element of a sequence to an <see cref="IDisposableEnumerable{TResult}" /> and flattens the resulting sequences into one sequence.</summary>
    /// <param name="source">A sequence of values to project.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> SelectMany<TSource, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.SelectMany(Guard.NotNull(selector, nameof(selector))));
    }

    /// <summary>Projects each element of a sequence to an <see cref="IDisposableEnumerable{TResult}" />, and flattens the resulting sequences into one sequence. The index of each source element is used in the projected form of that element.</summary>
    /// <param name="source">A sequence of values to project.</param>
    /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TResult">The type of the elements of the sequence returned by <paramref name="selector" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> whose elements are the result of invoking the one-to-many transform function on each element of an input sequence.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="selector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> SelectMany<TSource, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.SelectMany(Guard.NotNull(selector, nameof(selector))));
    }

    /// <summary>Projects each element of a sequence to an <see cref="IDisposableEnumerable{TResult}" />, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein.</summary>
    /// <param name="source">A sequence of values to project.</param>
    /// <param name="collectionSelector">A transform function to apply to each element of the input sequence.</param>
    /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector" />.</typeparam>
    /// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector" /> on each element of <paramref name="source" /> and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.SelectMany(Guard.NotNull(collectionSelector, nameof(collectionSelector)), Guard.NotNull(resultSelector, nameof(resultSelector))));
    }

    /// <summary>Projects each element of a sequence to an <see cref="IDisposableEnumerable{TResult}" />, flattens the resulting sequences into one sequence, and invokes a result selector function on each element therein. The index of each source element is used in the intermediate projected form of that element.</summary>
    /// <param name="source">A sequence of values to project.</param>
    /// <param name="collectionSelector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
    /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector" />.</typeparam>
    /// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TResult}" /> whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector" /> on each element of <paramref name="source" /> and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IDisposableEnumerable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.SelectMany(Guard.NotNull(collectionSelector, nameof(collectionSelector)), Guard.NotNull(resultSelector, nameof(resultSelector))));
    }

    /// <summary>Bypasses a specified number of elements in a sequence and then returns the remaining elements.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}" /> to return elements from.</param>
    /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}" /> that contains the elements that occur after the specified index in the input sequence.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> Skip<TSource>(this IDisposableEnumerable<TSource> source, int count)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Skip(count));
    }

#if !NETFRAMEWORK
    /// <summary>
    /// Returns a new enumerable collection that contains the elements from <paramref name="source"/> with the last <paramref name="count"/> elements of the source collection omitted.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the enumerable collection.</typeparam>
    /// <param name="source">An enumerable collection instance.</param>
    /// <param name="count">The number of elements to omit from the end of the collection.</param>
    /// <returns>A new enumerable collection that contains the elements from <paramref name="source"/> minus <paramref name="count"/> elements from the end of the collection.</returns>
    public static IDisposableEnumerable<TSource> SkipLast<TSource>(this IDisposableEnumerable<TSource> source, int count)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.SkipLast(count));
    }
#endif

    /// <summary>Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}" /> to return elements from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> SkipWhile<TSource>(this IDisposableEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.SkipWhile(Guard.NotNull(predicate, nameof(predicate))));
    }

    /// <summary>Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements. The element's index is used in the logic of the predicate function.</summary>
    /// <param name="source">An <see cref="IDisposableEnumerable{TSource}" /> to return elements from.</param>
    /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}" /> that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> SkipWhile<TSource>(this IDisposableEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.SkipWhile(Guard.NotNull(predicate, nameof(predicate))));
    }

    /// <summary>Returns a specified number of contiguous elements from the start of a sequence.</summary>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="count">The number of elements to return.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}" /> that contains the specified number of elements from the start of the input sequence.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> Take<TSource>(this IDisposableEnumerable<TSource> source, int count)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Take(count));
    }

#if !NETFRAMEWORK
    /// <summary>
    /// Returns a new enumerable collection that contains the last <paramref name="count"/> elements from <paramref name="source"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the enumerable collection.</typeparam>
    /// <param name="source">An enumerable collection instance.</param>
    /// <param name="count">The number of elements to take from the end of the collection.</param>
    /// <returns>A new enumerable collection that contains the last <paramref name="count"/> elements from <paramref name="source"/>.</returns>
    public static IDisposableEnumerable<TSource> TakeLast<TSource>(this IDisposableEnumerable<TSource> source, int count)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.TakeLast(count));
    }
#endif

    /// <summary>Returns elements from a sequence as long as a specified condition is true.</summary>
    /// <param name="source">A sequence to return elements from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}" /> that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> TakeWhile<TSource>(this IDisposableEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.TakeWhile(Guard.NotNull(predicate, nameof(predicate))));
    }

    /// <summary>Returns elements from a sequence as long as a specified condition is true. The element's index is used in the logic of the predicate function.</summary>
    /// <param name="source">The sequence to return elements from.</param>
    /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IDisposableEnumerable{TSource}" /> that contains elements from the input sequence that occur before the element at which the test no longer passes.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> TakeWhile<TSource>(this IDisposableEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.TakeWhile(Guard.NotNull(predicate, nameof(predicate))));
    }

    /// <summary>Performs a subsequent ordering of the elements in a sequence in ascending order by using a specified comparer.</summary>
    /// <param name="source">An <see cref="IOrderedDisposableEnumerable{TSource}" /> that contains elements to sort.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="comparer">An <see cref="IComparer{TKey}" /> to compare keys.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted according to a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.ThenBy(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Performs a subsequent ordering of the elements in a sequence in ascending order according to a key.</summary>
    /// <param name="source">An <see cref="IOrderedDisposableEnumerable{TSource}" /> that contains elements to sort.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted according to a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.ThenBy(Guard.NotNull(keySelector, nameof(keySelector))));
    }

    /// <summary>Performs a subsequent ordering of the elements in a sequence in descending order, according to a key.</summary>
    /// <param name="source">An <see cref="IOrderedDisposableEnumerable{TSource}" /> that contains elements to sort.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted in descending order according to a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.ThenByDescending(Guard.NotNull(keySelector, nameof(keySelector))));
    }

    /// <summary>Performs a subsequent ordering of the elements in a sequence in descending order by using a specified comparer.</summary>
    /// <param name="source">An <see cref="IOrderedDisposableEnumerable{TSource}" /> that contains elements to sort.</param>
    /// <param name="keySelector">A function to extract a key from each element.</param>
    /// <param name="comparer">An <see cref="IComparer{TKey}" /> to compare keys.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> whose elements are sorted in descending order according to a key.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="keySelector" /> is <see langword="null" />.</exception>
    public static IOrderedDisposableEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedDisposableEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.ThenByDescending(Guard.NotNull(keySelector, nameof(keySelector)), Guard.NotNull(comparer, nameof(comparer))));
    }

    /// <summary>Filters a sequence of values based on a predicate. Each element's index is used in the logic of the predicate function.</summary>
    /// <param name="source">An <see cref="IOrderedDisposableEnumerable{TSource}" /> to filter.</param>
    /// <param name="predicate">A function to test each source element for a condition; the second parameter of the function represents the index of the source element.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> that contains elements from the input sequence that satisfy the condition.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> Where<TSource>(this IDisposableEnumerable<TSource> source, Func<TSource, int, bool> predicate)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Where(Guard.NotNull(predicate, nameof(predicate))));
    }

    /// <summary>Filters a sequence of values based on a predicate.</summary>
    /// <param name="source">An <see cref="IOrderedDisposableEnumerable{TSource}" /> to filter.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <returns>An <see cref="IOrderedDisposableEnumerable{TSource}" /> that contains elements from the input sequence that satisfy the condition.</returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="source" /> or <paramref name="predicate" /> is <see langword="null" />.</exception>
    public static IDisposableEnumerable<TSource> Where<TSource>(this IDisposableEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Where(Guard.NotNull(predicate, nameof(predicate))));
    }

    #endregion

    #endregion

    #region IDisposableEnumerable

    /// <summary>
    /// Filters the elements of an <see cref="IDisposableEnumerable"/> based on a specified type.
    /// </summary>
    /// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
    /// <param name="source">The <see cref="IDisposableEnumerable"/> whose elements to filter.</param>
    /// <returns>An <see cref="IDisposableEnumerable{T}"/> that contains elements from the input sequence of type <typeparamref name="TResult"/>.</returns>
    public static IDisposableEnumerable<TResult> OfType<TResult>(this IDisposableEnumerable source)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.OfType<TResult>());
    }

    /// <summary>
    /// Casts the elements of an <see cref="IDisposableEnumerable"/> to the specified type.
    /// </summary>
    /// <typeparam name="TResult">The type to cast the elements of source to.</typeparam>
    /// <param name="source">The <see cref="IDisposableEnumerable"/> that contains the elements to be cast to type <typeparamref name="TResult"/>.</param>
    /// <returns>An <see cref="IDisposableEnumerable{T}"/> that contains each element of the source sequence cast to the specified type.</returns>
    public static IDisposableEnumerable<TResult> Cast<TResult>(this IDisposableEnumerable source)
    {
        return Redirect(Guard.NotNull(source, nameof(source)), x => x.Cast<TResult>());
    }

    /// <summary>
    /// Converts the <see cref="IDisposableEnumerable"/> to an generic <see cref="IDisposableEnumerable{T}"/> class of type <see cref="object"/>.
    /// </summary>
    /// <param name="enumerable">The <see cref="IDisposableEnumerable"/> to convert.</param>
    /// <returns>Returns a <see cref="IDisposableEnumerable{T}"/> that represents the <see cref="IDisposableEnumerable"/>.</returns>
    public static IDisposableEnumerable<object?> ToGeneric(this IDisposableEnumerable enumerable)
    {
        return Redirect(Guard.NotNull(enumerable, nameof(enumerable)), x => x.ToGeneric());
    }

    #endregion

    private static IDisposableEnumerable<TTarget> Redirect<TSource, TTarget>(IDisposableEnumerable<TSource> enumerable, Func<IEnumerable<TSource>, IEnumerable<TTarget>> function)
    {
        return new DelegateDisposableEnumerable<TTarget>(function(enumerable), enumerable.Dispose);
    }

    private static IOrderedDisposableEnumerable<TTarget> Redirect<TSource, TTarget>(IDisposableEnumerable<TSource> enumerable, Func<IEnumerable<TSource>, IOrderedEnumerable<TTarget>> function)
    {
        return new DelegateOrderedDisposableEnumerable<TTarget>(function(enumerable), enumerable.Dispose);
    }

    private static IOrderedDisposableEnumerable<TTarget> Redirect<TSource, TTarget>(IOrderedDisposableEnumerable<TSource> enumerable, Func<IOrderedEnumerable<TSource>, IOrderedEnumerable<TTarget>> function)
    {
        return new DelegateOrderedDisposableEnumerable<TTarget>(function(enumerable), enumerable.Dispose);
    }

    private static IDisposableEnumerable<T> Redirect<T>(IDisposableEnumerable enumerable, Func<IEnumerable, IEnumerable<T>> function)
    {
        return new DelegateDisposableEnumerable<T>(function(enumerable), enumerable.Dispose);
    }
}
