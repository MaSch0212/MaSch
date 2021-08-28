using MaSch.Core.Collections;
using MaSch.Core.Observable.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Cointains extensions for <see cref="IEnumerable{T}"/> and <see cref="IEnumerable"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region IEnumerable<T> Extensions

        #region Each

        /// <summary>
        /// Executes an action for each item in the <see cref="IEnumerable{T}"/> and returns each item after the action is executed.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>Returns each item after the action has been executed.</returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            foreach (var item in enumerable)
            {
                action(item);
                yield return item;
            }
        }

        /// <summary>
        /// Executes an action for each item in the <see cref="IEnumerable{T}"/> and returns each item after the action is executed.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>Returns each item after the action has been executed.</returns>
        public static IEnumerable<T> Each<T>(this IEnumerable<T> enumerable, Action<T, LoopState> action)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            var state = new LoopState();
            foreach (var item in enumerable)
            {
                action(item, state);
                if (!state.Next())
                    break;
                yield return item;
            }
        }

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/> and returns each item and the exception (if the action failed) after the action is executed.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>Returns each item with the exception that occurred after the action has been executed. If no exception occurred, the exception entry is <c>null</c>.</returns>
        public static IEnumerable<(T Item, Exception? Error)> TryEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            foreach (var item in enumerable)
            {
                Exception? error = null;
                try
                {
                    action(item);
                }
                catch (Exception ex)
                {
                    error = ex;
                }

                yield return (item, error);
            }
        }

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/> and returns each item and the exception (if the action failed) after the action is executed.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>Returns each item with the exception that occurred after the action has been executed. If no exception occurred, the exception entry is <c>null</c>.</returns>
        public static IEnumerable<(T Item, Exception? Error)> TryEach<T>(this IEnumerable<T> enumerable, Action<T, LoopState> action)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            var state = new LoopState();
            foreach (var item in enumerable)
            {
                Exception? error = null;
                try
                {
                    action(item, state);
                }
                catch (Exception ex)
                {
                    error = ex;
                }

                if (!state.Next())
                    break;
                yield return (item, error);
            }
        }

        #endregion

        #region ForEach

        /// <summary>
        /// Executes an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            foreach (var item in enumerable)
                action(item);
        }

        /// <summary>
        /// Executes an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, LoopState> action)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            var state = new LoopState();
            foreach (var item in enumerable)
            {
                action(item, state);
                if (!state.Next())
                    break;
            }
        }

        /// <summary>
        /// Executes an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute. The first parameter is the last element of the loop - for the first item, this parameter is <c>default</c>.</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T?, T, LoopState> action)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            T? last = default;
            var state = new LoopState();
            foreach (var item in enumerable)
            {
                action(last, item, state);
                if (!state.Next())
                    break;
                last = item;
            }
        }

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
            => TryForEach(enumerable, action, true);

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T> action, bool continueOnError)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            bool result = true;
            foreach (var item in enumerable)
            {
                try
                {
                    action(item);
                }
                catch
                {
                    result = false;
                    if (!continueOnError)
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T, LoopState> action)
            => TryForEach(enumerable, action, true);

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T, LoopState> action, bool continueOnError)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            bool result = true;
            var state = new LoopState();
            foreach (var item in enumerable)
            {
                try
                {
                    action(item, state);
                }
                catch
                {
                    result = false;
                    if (!continueOnError)
                        break;
                }

                if (!state.Next())
                    break;
            }

            return result;
        }

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="errors">A collection containing the errors that occurred.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T> action, out ICollection<(T Item, Exception Error)> errors)
            => TryForEach(enumerable, action, out errors, true);

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="errors">A collection containing the errors that occurred.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T> action, out ICollection<(T Item, Exception Error)> errors, bool continueOnError)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            errors = new List<(T, Exception)>();
            foreach (var item in enumerable)
            {
                try
                {
                    action(item);
                }
                catch (Exception ex)
                {
                    errors.Add((item, ex));
                    if (!continueOnError)
                        break;
                }
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="errors">A collection containing the errors that occurred.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T, LoopState> action, out ICollection<(T Item, int Index, Exception Error)> errors)
            => TryForEach(enumerable, action, out errors, true);

        /// <summary>
        /// Tries to execute an action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="errors">A collection containing the errors that occurred.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static bool TryForEach<T>(this IEnumerable<T> enumerable, Action<T, LoopState> action, out ICollection<(T Item, int Index, Exception Error)> errors, bool continueOnError)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            errors = new List<(T, int, Exception)>();
            var state = new LoopState();
            foreach (var item in enumerable)
            {
                try
                {
                    action(item, state);
                }
                catch (Exception ex)
                {
                    errors.Add((item, state.CurrentIndex, ex));
                    if (!continueOnError)
                        break;
                }

                if (!state.Next())
                    break;
            }

            return errors.Count == 0;
        }

        #endregion

        #region ForEachAsync

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
            => await ForEachAsync(enumerable, action, CancellationToken.None);

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            foreach (var item in enumerable)
            {
                await action(item);
                if (token.IsCancellationRequested)
                    break;
            }
        }

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action)
            => await ForEachAsync(enumerable, action, CancellationToken.None);

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            var state = new AsyncLoopState(token);
            foreach (var item in enumerable)
            {
                await action(item, state);
                if (!state.Next())
                    break;
            }
        }

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute. The first parameter is the last element of the loop - for the first item, this parameter is <c>default</c>.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T?, T, AsyncLoopState, Task> action)
            => await ForEachAsync(enumerable, action, CancellationToken.None);

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute. The first parameter is the last element of the loop - for the first item, this parameter is <c>default</c>.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T?, T, AsyncLoopState, Task> action, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            T? last = default;
            var state = new AsyncLoopState(token);
            foreach (var item in enumerable)
            {
                await action(last, item, state);
                if (!state.Next())
                    break;
                last = item;
            }
        }

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
            => await ParallelForEachAsync(enumerable, action, CancellationToken.None);

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));

            var tcs = new TaskCompletionSource<object>();
            using (token.Register(() => tcs.TrySetCanceled(), false))
                await Task.WhenAny(Task.WhenAll(enumerable.Select(action)), tcs.Task);
        }

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of parallel running actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, int maxDegreeOfParallelism)
            => await ParallelForEachAsync(enumerable, action, maxDegreeOfParallelism, CancellationToken.None);

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of parallel running actions.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, int maxDegreeOfParallelism, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));

            var activeTasks = new HashSet<Task>();
            foreach (var item in enumerable)
            {
                activeTasks.Add(action(item));
                if (activeTasks.Count >= maxDegreeOfParallelism)
                {
                    var completed = await Task.WhenAny(activeTasks);
                    activeTasks.Remove(completed);
                    if (token.IsCancellationRequested)
                        break;
                }
            }
        }

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action)
            => await ParallelForEachAsync(enumerable, action, CancellationToken.None);

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));

            var state = new AsyncLoopState(token);
            var activeTasks = new HashSet<Task>(enumerable.Select(x => action(x, state)));
            while (activeTasks.Count > 0)
            {
                var completed = await Task.WhenAny(activeTasks);
                activeTasks.Remove(completed);
                if (!state.Next())
                    break;
            }
        }

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of parallel running actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action, int maxDegreeOfParallelism)
            => await ParallelForEachAsync(enumerable, action, maxDegreeOfParallelism, CancellationToken.None);

        /// <summary>
        /// Executes an async action for each item in the <see cref="IEnumerable{T}"/> in parallel.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="maxDegreeOfParallelism">The maximum number of parallel running actions.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action, int maxDegreeOfParallelism, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));

            var state = new AsyncLoopState(token);
            var activeTasks = new HashSet<Task>();
            foreach (var item in enumerable)
            {
                activeTasks.Add(action(item, state));
                if (activeTasks.Count >= maxDegreeOfParallelism)
                {
                    var completed = await Task.WhenAny(activeTasks);
                    activeTasks.Remove(completed);
                    if (!state.Next())
                        break;
                }
            }
        }

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
            => await TryForEachAsync(enumerable, action, true, CancellationToken.None);

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, CancellationToken token)
            => await TryForEachAsync(enumerable, action, true, token);

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, bool continueOnError)
            => await TryForEachAsync(enumerable, action, continueOnError, CancellationToken.None);

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action, bool continueOnError, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            bool result = true;
            foreach (var item in enumerable)
            {
                try
                {
                    await action(item);
                }
                catch
                {
                    result = false;
                    if (!continueOnError)
                        break;
                }

                if (token.IsCancellationRequested)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action)
            => await TryForEachAsync(enumerable, action, true, CancellationToken.None);

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action, CancellationToken token)
            => await TryForEachAsync(enumerable, action, true, token);

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action, bool continueOnError)
            => await TryForEachAsync(enumerable, action, continueOnError, CancellationToken.None);

        /// <summary>
        /// Tries to execute an async action for each item in the <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of objects in the specified enumerable.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="continueOnError">Determines wether the loop should continue if an error occurres.</param>
        /// <param name="token">A cancellation token to observe while executing the asynchronous actions.</param>
        /// <returns><c>true</c> if no expected occurred; otherwise, <c>false</c>.</returns>
        public static async Task<bool> TryForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, AsyncLoopState, Task> action, bool continueOnError, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            bool result = true;
            var state = new AsyncLoopState(token);
            foreach (var item in enumerable)
            {
                try
                {
                    await action(item, state);
                }
                catch
                {
                    result = false;
                    if (!continueOnError)
                        break;
                }

                if (!state.Next())
                    break;
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="value">The object to locate in the <see cref="IEnumerable{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="IEnumerable{T}"/>, if found; otherwise, –1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            int i = 0;
            foreach (var item in enumerable)
            {
                if (Equals(item, value))
                    return i;
                i++;
            }

            return -1;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="IEnumerable{T}"/> using a specific <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="value">The object to locate in the <see cref="IEnumerable{T}"/>. The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> that should be used for the comparison.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire <see cref="IEnumerable{T}"/>, if found; otherwise, –1.</returns>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, T value, IEqualityComparer<T> comparer)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(comparer, nameof(comparer));
            int i = 0;
            foreach (var item in enumerable)
            {
                if (comparer.Equals(item, value))
                    return i;
                i++;
            }

            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="match"/> is <see langword="null"/>.</exception>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Predicate<T> match)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(match, nameof(match));
            int i = 0;
            foreach (var item in enumerable)
            {
                if (match(item))
                    return i;
                i++;
            }

            return -1;
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to search in.</param>
        /// <param name="match">The <see cref="Func{T1, T2, TResult}"/> delegate that defines the conditions of the element to search for.</param>
        /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="match"/> is <see langword="null"/>.</exception>
        public static int IndexOf<T>(this IEnumerable<T> enumerable, Func<T, int, bool> match)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(match, nameof(match));
            int i = 0;
            foreach (var item in enumerable)
            {
                if (match(item, i))
                    return i;
                i++;
            }

            return -1;
        }

        /// <summary>Determines whether any element of a sequence satisfies a condition.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="expression">A function to test each element for a condition.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>
        ///   <see langword="true" /> if any elements in the source sequence pass the test in the specified predicate; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="expression" /> is <see langword="null" />.</exception>
        public static bool Any<T>(this IEnumerable<T> enumerable, Func<T, int, bool> expression)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(expression, nameof(expression));
            int i = 0;
            return enumerable.Any(x => expression(x, i++));
        }

        /// <summary>
        /// Searches for the first element of a sequence that has the maximum value of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TValue">The type of the value to determine the maximum element.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="func">The function to determine the maximum value with.</param>
        /// <returns>The first element with the maximum value defined by <paramref name="func"/>.</returns>
        public static TSource? FirstMaxElement<TSource, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TValue> func)
            where TValue : IComparable<TValue>
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var first = true;
            var maxO = default(TSource);
            var maxV = default(TValue);
            foreach (var t in enumerable)
            {
                var v = func(t);
                if (first || maxV?.CompareTo(v) > 0)
                {
                    maxV = v;
                    maxO = t;
                }

                first = false;
            }

            return maxO;
        }

        /// <summary>
        /// Searches for the first element of a sequence that has the maximum value of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TValue">The type of the value to determine the maximum element.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="func">The function to determine the maximum value with.</param>
        /// <returns>The index of the first element with the maximum value defined by <paramref name="func"/>.</returns>
        public static int FirstMaxIndex<TSource, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TValue> func)
            where TValue : IComparable<TValue>
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var result = -1;
            var currentIndex = -1;
            var maxV = default(TValue);
            foreach (var t in enumerable)
            {
                currentIndex++;
                var v = func(t);
                if (result < 0 || maxV?.CompareTo(v) > 0)
                {
                    maxV = v;
                    result = currentIndex;
                }
            }

            return result;
        }

        /// <summary>
        /// Searches for the first element of a sequence that has the minimum value of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TValue">The type of the value to determine the minimum element.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="func">The function to determine the minimum value with.</param>
        /// <returns>The first element with the minimum value defined by <paramref name="func"/>.</returns>
        public static TSource? FirstMinElement<TSource, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TValue> func)
            where TValue : IComparable<TValue>
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var first = true;
            var maxO = default(TSource);
            var maxV = default(TValue);
            foreach (var t in enumerable)
            {
                var v = func(t);
                if (first || maxV?.CompareTo(v) < 0)
                {
                    maxV = v;
                    maxO = t;
                }

                first = false;
            }

            return maxO;
        }

        /// <summary>
        /// Searches for the first element of a sequence that has the minimum value of the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TValue">The type of the value to determine the minimum element.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> whose elements to apply the predicate to.</param>
        /// <param name="func">The function to determine the minimum value with.</param>
        /// <returns>The index of the first element with the minimum value defined by <paramref name="func"/>.</returns>
        public static int FirstMinIndex<TSource, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TValue> func)
            where TValue : IComparable<TValue>
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var result = -1;
            var currentIndex = -1;
            var maxV = default(TValue);
            foreach (var t in enumerable)
            {
                currentIndex++;
                var v = func(t);
                if (result < 0 || maxV?.CompareTo(v) < 0)
                {
                    maxV = v;
                    result = currentIndex;
                }
            }

            return result;
        }

#if NETFRAMEWORK
        /// <summary>
        /// Returns a specified number of continuous elements from the end of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The list.</param>
        /// <param name="count">The numer of elements to return.</param>
        /// <returns>A specified number of continuous elements from the end of a sequence.</returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> enumerable, int count)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var buffer = new LinkedList<T>();
            int c = 0;
            foreach (var item in enumerable)
            {
                buffer.AddLast(item);
                if (++c > count)
                    buffer.RemoveFirst();
            }

            return buffer;
        }
#endif

        /// <summary>
        /// Returns a specified number of continuous elements from the end of a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The list.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <param name="skip">The number of elements to skip from the end.</param>
        /// <returns>A specified number of continuous elements from the end of a sequence.</returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> enumerable, int count, int skip)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var buffer = new LinkedList<T>();
            int c = 0;
            foreach (var item in enumerable)
            {
                buffer.AddLast(item);
                if (++c > count + skip)
                    buffer.RemoveFirst();
            }

            return buffer.Take(count);
        }

        /// <summary>Produces the set difference of two sequences by using the default equality comparer to compare values.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{TSource}" /> whose elements that are not <paramref name="itemToExclude" /> will be returned.</param>
        /// <param name="itemToExclude">An instance of <typeparamref name="TSource"/> which also occur in the first sequence will cause it to be removed from the returned sequence.</param>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> enumerable, TSource itemToExclude)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            return enumerable.Except(new[] { itemToExclude });
        }

        /// <summary>Produces the set difference of two sequences by using the specified <see cref="IEqualityComparer{TSource}" /> to compare values.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{TSource}" /> whose elements that are not <paramref name="itemToExclude" /> will be returned.</param>
        /// <param name="itemToExclude">An instance of <typeparamref name="TSource"/> which also occur in the first sequence will cause it to be removed from the returned sequence.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{TSource}" /> to compare values.</param>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> enumerable, TSource itemToExclude, IEqualityComparer<TSource> comparer)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            return enumerable.Except(new[] { itemToExclude }, comparer);
        }

        /// <summary>Filters a sequence of values based on a predicate.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{TSource}" /> to filter.</param>
        /// <param name="func">A function to test each element for a condition. The first parameter is the last element of the loop - for the first item, this parameter is <c>default</c>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>An <see cref="IEnumerable{TSource}" /> that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="func" /> is <see langword="null" />.</exception>
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> enumerable, Func<TSource?, TSource, bool> func)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            TSource? last = default;
            foreach (TSource item in enumerable)
            {
                if (func(last, item))
                    yield return item;
                last = item;
            }
        }

        /// <summary>
        /// Filters the enumerable for items which a specified exception is thrown on a specified action.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="matchedExceptions">The matched exceptions.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements for which the action threw an exception.</returns>
        public static IEnumerable<T> WhereException<T>(this IEnumerable<T> enumerable, Action<T> action, params Type[] matchedExceptions)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            Guard.NotNull(matchedExceptions, nameof(matchedExceptions));
            foreach (T item in enumerable)
            {
                bool matched;
                try
                {
                    action(item);
                    matched = false;
                }
                catch (Exception ex)
                {
                    matched = matchedExceptions.Any(x => x.IsInstanceOfType(ex));
                }

                if (matched)
                    yield return item;
            }
        }

        /// <summary>
        /// Filters the enumerable for items which no exception is thrown on a specified action.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="expectedExceptions">
        /// The excpeption types that are expected.
        /// If an exception is thrown that is not part of this list, it is rethrown.
        /// If no type is given all exception types are seen as expected.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains elements for which the action did not throw an exception.</returns>
        public static IEnumerable<T> WhereNoException<T>(this IEnumerable<T> enumerable, Action<T> action, params Type[] expectedExceptions)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(action, nameof(action));
            Guard.NotNull(expectedExceptions, nameof(expectedExceptions));
            foreach (T item in enumerable)
            {
                bool matched;
                try
                {
                    action(item);
                    matched = true;
                }
                catch (Exception ex)
                {
                    if (expectedExceptions.Length > 0 && !expectedExceptions.Any(x => x.IsInstanceOfType(ex)))
                        throw;
                    matched = false;
                }

                if (matched)
                    yield return item;
            }
        }

        /// <summary>
        /// Concatenates two sequences.
        /// </summary>
        /// <typeparam name="T">The type of the enumerable items.</typeparam>
        /// <param name="first">The enumerable.</param>
        /// <param name="second">The items to add.</param>
        /// <returns>An <see cref="IEnumerable"/> that contains the concatenated elements of the two input sequences.</returns>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, params T[] second)
            => first.Concat((IEnumerable<T>)second);

        /// <summary>Creates a <see cref="List{T}" /> asynchronously from an <see cref="IEnumerable{T}" />.</summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to create a <see cref="List{T}" /> from.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>A <see cref="List{T}" /> that contains elements from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> enumerable)
            => await ToListAsync(enumerable, CancellationToken.None);

        /// <summary>Creates a <see cref="List{T}" /> asynchronously from an <see cref="IEnumerable{T}" />.</summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to create a <see cref="List{T}" /> from.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="List{T}" />.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>A <see cref="List{T}" /> that contains elements from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static async Task<List<T>> ToListAsync<T>(this IEnumerable<T> enumerable, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            return await Task.Run(() => ToList(enumerable, token), token);
        }

        /// <summary>Creates a <see cref="List{T}" /> from an <see cref="IEnumerable{T}" />.</summary>
        /// <param name="enumerable">The <see cref="IEnumerable{T}" /> to create a <see cref="List{T}" /> from.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="List{T}" />.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>A <see cref="List{T}" /> that contains elements from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static List<T> ToList<T>(this IEnumerable<T> enumerable, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var result = new List<T>();
            if (!token.IsCancellationRequested)
            {
                foreach (var item in enumerable)
                {
                    result.Add(item);
                    if (token.IsCancellationRequested)
                        break;
                }
            }

            return result;
        }

        /// <summary>Creates an array asynchronously from a <see cref="IEnumerable{T}" />.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create an array from.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>An array that contains the elements from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<T> enumerable)
            => await ToArrayAsync(enumerable, CancellationToken.None);

        /// <summary>Creates an array asynchronously from a <see cref="IEnumerable{T}" />.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create an array from.</param>
        /// <param name="token">A cancellation token to observe while creating the array.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>An array that contains the elements from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static async Task<T[]> ToArrayAsync<T>(this IEnumerable<T> enumerable, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            return await Task.Run(() => ToArray(enumerable, token), token);
        }

        /// <summary>Creates an array from a <see cref="IEnumerable{T}" />.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create an array from.</param>
        /// <param name="token">A cancellation token to observe while creating the array.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <returns>An array that contains the elements from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> is <see langword="null" />.</exception>
        public static T[] ToArray<T>(this IEnumerable<T> enumerable, CancellationToken token)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var result = new ResizableArray<T>();
            if (!token.IsCancellationRequested)
            {
                foreach (var item in enumerable)
                {
                    result.Add(item);
                    if (token.IsCancellationRequested)
                        break;
                }
            }

            result.ShrinkArray();
            return result.InternalArray;
        }

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to a specified key selector function.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains keys and values.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, x => x, null, CancellationToken.None));

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to a specified key selector function.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains keys and values.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, CancellationToken token)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, x => x, null, token));

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to a specified key selector function and key comparer.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}" /> to compare keys.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by <paramref name="keySelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains keys and values.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> keyComparer)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, x => x, keyComparer, CancellationToken.None));

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to a specified key selector function and key comparer.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}" /> to compare keys.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by <paramref name="keySelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains keys and values.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> keyComparer, CancellationToken token)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, x => x, keyComparer, token));

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to specified key selector and element selector functions.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by <paramref name="valueSelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue" /> selected from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> or <paramref name="valueSelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, valueSelector, null, CancellationToken.None));

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to specified key selector and element selector functions.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by <paramref name="valueSelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue" /> selected from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> or <paramref name="valueSelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, CancellationToken token)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, valueSelector, null, token));

        /// <summary>Creates a<see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to a specified key selector function, a comparer, and an element selector function.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}" /> to compare keys.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by <paramref name="valueSelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue" /> selected from the input sequence.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> or <paramref name="valueSelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> keyComparer)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, valueSelector, keyComparer, CancellationToken.None));

        /// <summary>Creates a<see cref="Dictionary{TKey, TValue}"/> asynchronously from an <see cref="IEnumerable{T}" /> according to a specified key selector function, a comparer, and an element selector function.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}" /> to compare keys.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by <paramref name="valueSelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue" /> selected from the input sequence.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> or <paramref name="valueSelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TSource, TKey, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> keyComparer, CancellationToken token)
            where TKey : notnull
            => await Task.Run(() => ToDictionaryImpl(enumerable, keySelector, valueSelector, keyComparer, token));

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}" /> according to a specified key selector function.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains keys and values.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, CancellationToken token)
            where TKey : notnull
            => ToDictionaryImpl(enumerable, keySelector, x => x, null, token);

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}" /> according to a specified key selector function and key comparer.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}" /> to compare keys.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by <paramref name="keySelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains keys and values.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> keyComparer, CancellationToken token)
            where TKey : notnull
            => ToDictionaryImpl(enumerable, keySelector, x => x, keyComparer, token);

        /// <summary>Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}" /> according to specified key selector and element selector functions.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by <paramref name="valueSelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue" /> selected from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> or <paramref name="valueSelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, CancellationToken token)
            where TKey : notnull
            => ToDictionaryImpl(enumerable, keySelector, valueSelector, null, token);

        /// <summary>Creates a<see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}" /> according to a specified key selector function, a comparer, and an element selector function.</summary>
        /// <param name="enumerable">An <see cref="IEnumerable{T}" /> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="keyComparer">An <see cref="IEqualityComparer{T}" /> to compare keys.</param>
        /// <param name="token">A cancellation token to observe while creating the <see cref="Dictionary{TKey, TValue}"/>.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector" />.</typeparam>
        /// <typeparam name="TValue">The type of the value returned by <paramref name="valueSelector" />.</typeparam>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue" /> selected from the input sequence.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="enumerable" /> or <paramref name="keySelector" /> or <paramref name="valueSelector" /> is <see langword="null" />.
        /// -or-
        /// <paramref name="keySelector" /> produces a key that is <see langword="null" />.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="keySelector" /> produces duplicate keys for two elements.</exception>
        public static Dictionary<TKey, TValue> ToDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey> keyComparer, CancellationToken token)
            where TKey : notnull
            => ToDictionaryImpl(enumerable, keySelector, valueSelector, keyComparer, token);

        /// <summary>
        /// Tries to find the first element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="value">When this method returns, contains the first value , if an element was found; otherwise, the default value for <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if an element was found in the <see cref="IEnumerable{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryFirst<T>(this IEnumerable<T> enumerable, [MaybeNullWhen(false)] out T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            using var enumerator = enumerable.GetEnumerator();
            var result = enumerator.MoveNext();
            value = result ? enumerator.Current : default;
            return result;
        }

        /// <summary>
        /// Tries to find the first element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="value">When this method returns, contains the first value that matched the <paramref name="predicate"/>, if an element was found; otherwise, the default value for <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if an element that matches <paramref name="predicate"/> was found in the <see cref="IEnumerable{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, [MaybeNullWhen(false)] out T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            using var enumerator = enumerable.Where(predicate).GetEnumerator();
            var result = enumerator.MoveNext();
            value = result ? enumerator.Current : default;
            return result;
        }

        /// <summary>
        /// Tries to find a single element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="value">When this method returns, contains the single value, if exactly one element was found; otherwise, the default value for <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if exactly one element was found in the <see cref="IEnumerable{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TrySingle<T>(this IEnumerable<T> enumerable, [MaybeNullWhen(false)] out T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            using var enumerator = enumerable.GetEnumerator();
            var result = enumerator.MoveNext();
            value = result ? enumerator.Current : default;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one matching element");
            return result;
        }

        /// <summary>
        /// Tries to find a single element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="value">When this method returns, contains the single value that matched the <paramref name="predicate"/>, if exactly one element was found; otherwise, the default value for <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if exactly one element matches <paramref name="predicate"/> was found in the <see cref="IEnumerable{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TrySingle<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, [MaybeNullWhen(false)] out T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            using var enumerator = enumerable.Where(predicate).GetEnumerator();
            var result = enumerator.MoveNext();
            value = result ? enumerator.Current : default;
            if (enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains more than one matching element");
            return result;
        }

        /// <summary>
        /// Tries to find the last element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="value">When this method returns, contains the last value, if an element was found; otherwise, the default value for <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if an element was found in the <see cref="IEnumerable{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryLast<T>(this IEnumerable<T> enumerable, [MaybeNullWhen(false)] out T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            bool result = false;
            value = default;
            foreach (var item in enumerable)
            {
                value = item;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Tries to find the last element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="value">When this method returns, contains the last value that matched the <paramref name="predicate"/>, if an element was found; otherwise, the default value for <typeparamref name="T"/>.</param>
        /// <returns><see langword="true"/> if an element that matches <paramref name="predicate"/> was found in the <see cref="IEnumerable{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryLast<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, [MaybeNullWhen(false)] out T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            bool result = false;
            value = default;
            foreach (var item in enumerable.Where(predicate))
            {
                value = item;
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Tries to get the element at a specific index of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="index">The index from which to retrieve the element.</param>
        /// <param name="value">If an element exists at the speicifed index, contains the value at that index; otherwise, the default value for <typeparamref name="T"/>.</param>
        /// <returns><c>true</c> if an element exists at the specified <paramref name="index"/> in the <paramref name="enumerable"/>.</returns>
        public static bool TryElementAt<T>(this IEnumerable<T> enumerable, int index, [MaybeNullWhen(false)] out T value)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            return TryFirst(enumerable.Skip(index), out value);
        }

        /// <summary>
        /// Adds the previous entry of each element to the resulting <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the previous entry for each element in <paramref name="enumerable"/>.</returns>
        public static IEnumerable<(T? Previous, T Current)> WithPrevious<T>(this IEnumerable<T> enumerable)
        {
            T? previous = default;
            foreach (var item in enumerable)
            {
                yield return (previous, item);
                previous = item;
            }
        }

        /// <summary>
        /// Adds the next entry of each element to the resulting <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the next entry for each element in <paramref name="enumerable"/>.</returns>
        public static IEnumerable<(T Current, T? Next)> WithNext<T>(this IEnumerable<T> enumerable)
        {
            T? current = default;
            bool hasCurrent = false;
            foreach (var item in enumerable)
            {
                if (hasCurrent)
                    yield return (current!, item);
                current = item;
                hasCurrent = true;
            }

            if (hasCurrent)
                yield return (current!, default);
        }

        /// <summary>
        /// Adds the previous and next entry of each element to the resulting <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the previous and next entry for each element in <paramref name="enumerable"/>.</returns>
        public static IEnumerable<(T? Previous, T Current, T? Next)> WithPreviousAndNext<T>(this IEnumerable<T> enumerable)
        {
            T? previous = default;
            T? current = default;
            bool hasCurrent = false;
            foreach (var item in enumerable)
            {
                if (hasCurrent)
                {
                    yield return (previous, current!, item);
                    previous = current;
                }

                current = item;
                hasCurrent = true;
            }

            if (hasCurrent)
                yield return (previous, current!, default);
        }

        /// <summary>
        /// Adds the index of each element to the resulting <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the index for each element in <paramref name="enumerable"/>.</returns>
        public static IEnumerable<(int Index, T Item)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            int i = 0;
            foreach (var item in enumerable)
                yield return (i++, item);
        }

        /// <summary>
        /// Links the specified enumerable.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="aggregateFunction">The aggregate function.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> which contains the result of the <paramref name="aggregateFunction"/> of each element in the <paramref name="enumerable"/>.</returns>
        public static IEnumerable<TResult> Link<TItem, TResult>(this IEnumerable<TItem> enumerable, TResult? seed, Func<TResult?, TItem, TResult> aggregateFunction)
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            Guard.NotNull(aggregateFunction, nameof(aggregateFunction));

            var previousResult = seed;
            foreach (var item in enumerable)
            {
                previousResult = aggregateFunction(previousResult, item);
                yield return previousResult;
            }
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable{T}"/> to an <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An <see cref="ObservableCollection{T}"/> containing all elements of the <paramref name="enumerable"/>.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
            => new(enumerable);

        /// <summary>
        /// Converts the <see cref="IEnumerable{T}"/> to an <see cref="FullyObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable" />.</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>An <see cref="FullyObservableCollection{T}"/> containing all elements of the <paramref name="enumerable"/>.</returns>
        public static FullyObservableCollection<T> ToFullyObservableCollection<T>(this IEnumerable<T> enumerable)
            where T : INotifyPropertyChanged
            => new(enumerable);

        /// <summary>
        /// Filters out null values from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <param name="enumerable">The enumerable to filter.</param>
        /// <returns>Returns a new <see cref="IEnumerable{T}"/> that has all non-null values from the <paramref name="enumerable"/>.</returns>
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
                    where T : class
        {
            foreach (var element in enumerable)
            {
                if (element is not null)
                    yield return element;
            }
        }

        #endregion

        #region IEnumerable extensions

        /// <summary>
        /// Determines if the <see cref="IEnumerable"/> is null or empty.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> to check.</param>
        /// <returns>Return true if the <see cref="IEnumerable"/> is null or empty; otherwise false.</returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this IEnumerable? enumerable)
        {
            if (enumerable == null)
                return true;
            if (enumerable is ICollection collection)
                return collection.Count == 0;
            var enumerator = enumerable.GetEnumerator();
            try
            {
                return !enumerator.MoveNext();
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// Converts the <see cref="IEnumerable"/> to an generic <see cref="IEnumerable{T}"/> class of type <see cref="object"/>.
        /// </summary>
        /// <param name="enumerable">The <see cref="IEnumerable"/> to convert.</param>
        /// <returns>Returns a <see cref="IEnumerable{T}"/> that represents the <see cref="IEnumerable"/>.</returns>
        public static IEnumerable<object?> ToGeneric(this IEnumerable enumerable)
            => enumerable.OfType<object?>();

        #endregion

        private static Dictionary<TKey, TValue> ToDictionaryImpl<TSource, TKey, TValue>(IEnumerable<TSource> enumerable, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector, IEqualityComparer<TKey>? keyComparer, CancellationToken token)
            where TKey : notnull
        {
            Guard.NotNull(enumerable, nameof(enumerable));
            var result = new Dictionary<TKey, TValue>(keyComparer);
            if (!token.IsCancellationRequested)
            {
                foreach (var item in enumerable)
                {
                    result.Add(keySelector(item), valueSelector(item));
                    if (token.IsCancellationRequested)
                        break;
                }
            }

            return result;
        }
    }

    /// <summary>
    /// Contains information about a synchronous loop.
    /// </summary>
    public class LoopState
    {
        private bool _shouldBreak;

        /// <summary>
        /// Gets the current index of the loop.
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// Breaks the executing of this loop.
        /// </summary>
        public void Break()
        {
            _shouldBreak = true;
        }

        /// <summary>
        /// Sets this loop state to the next iteration.
        /// </summary>
        /// <returns><see langword="true"/> if the loop should continue; otherwise <see langword="false"/>.</returns>
        protected internal virtual bool Next()
        {
            CurrentIndex++;
            return !_shouldBreak;
        }
    }

    /// <summary>
    /// Contains information about a asynchronous loop.
    /// </summary>
    public class AsyncLoopState : LoopState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLoopState" /> class.
        /// </summary>
        /// <param name="token">A cancellation token that is observed while executing the loop.</param>
        public AsyncLoopState(CancellationToken token)
        {
            CancellationToken = token;
        }

        /// <summary>
        /// Gets a cancellation token that is observed while executing the loop.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Gets a value indicating whether the loop should the cancelled or not.
        /// </summary>
        public bool IsCancellationRequested => CancellationToken.IsCancellationRequested;

        /// <inheritdoc/>
        protected internal override bool Next()
        {
            return base.Next() && !IsCancellationRequested;
        }
    }
}
