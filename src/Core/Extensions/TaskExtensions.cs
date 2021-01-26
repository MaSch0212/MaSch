using System;
using System.Threading;
using System.Threading.Tasks;

namespace MaSch.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Task"/> anf <see cref="Task{TResult}"/>.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Waits until the task has finished and the given time has passed.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="miliseconds">The minimum time to wait in miliseconds.</param>
        /// <returns>Returns an awaitable task.</returns>
        public static async Task RunAtLeast(this Task task, int miliseconds)
        {
            await Task.WhenAll(Guard.NotNull(task, nameof(task)), Task.Delay(miliseconds));
        }
        /// <summary>
        /// Waits until the task has finished and the given time has passed.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeSpan">The minimum time to wait.</param>
        /// <returns>Returns an awaitable task.</returns>
        public static async Task RunAtLeast(this Task task, TimeSpan timeSpan)
        {
            await Task.WhenAll(Guard.NotNull(task, nameof(task)), Task.Delay(timeSpan));
        }

        /// <summary>
        /// Waits until the task has finished and the given time has passed.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="miliseconds">The minimum time to wait in miliseconds.</param>
        /// <returns>Returns an awaitable task.</returns>
        public static async Task<T?> RunAtLeast<T>(this Task<T> task, int miliseconds)
        {
            _ = Guard.NotNull(task, nameof(task));
            var waitTask = Task.Run(async () =>
            {
                await Task.Delay(miliseconds);
                return default(T);
            });
            return (await Task.WhenAll<T?>(task!, waitTask))[0];
        }
        /// <summary>
        /// Waits until the task has finished and the given time has passed.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        /// <param name="timeSpan">The minimum time to wait.</param>
        /// <returns>Returns an awaitable task.</returns>
        public static async Task<T?> RunAtLeast<T>(this Task<T> task, TimeSpan timeSpan)
        {
            _ = Guard.NotNull(task, nameof(task));
            var waitTask = Task.Run(async () =>
            {
                await Task.Delay(timeSpan);
                return default(T);
            });
            return (await Task.WhenAll<T?>(task!, waitTask))[0];
        }
        
        /// <summary>
        /// Forgets the task, so it runs in the background. Use this function if you do not want to await a task.
        /// </summary>
        /// <param name="task">The task to forget.</param>
        public static async void Forget(this Task task)
        {
            await Guard.NotNull(task, nameof(task));
        }

        /// <summary>
        /// Forgets the task, so it runs in the background. Use this function if you do not want to await a task.
        /// </summary>
        /// <param name="task">The task to forget.</param>
        public static async void Forget<T>(this Task<T> task)
        {
            await Guard.NotNull(task, nameof(task));
        }

        /// <summary>
        /// Catches the <see cref="TaskCanceledException"/> that is thrown for some APIs if the passed <see cref="CancellationToken"/> has been marked as cancelled.
        /// </summary>
        /// <param name="task">The task for which the <see cref="TaskCanceledException"/> should be catched.</param>
        /// <returns>Return true if the task has ben completed without a cancellation; otherwise false.</returns>
        public static async Task<bool> CatchCancellation(this Task task)
        {
            _ = Guard.NotNull(task, nameof(task));
            try
            {
                await task;
                return true;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        /// <summary>
        /// Catches the <see cref="TaskCanceledException"/> that is thrown for some APIs if the passed <see cref="CancellationToken"/> has been marked as cancelled.
        /// </summary>
        /// <typeparam name="T">The result type of the task.</typeparam>
        /// <param name="task">The task for which the <see cref="TaskCanceledException"/> should be catched.</param>
        /// <returns>Return true and the result of the task if the task has ben completed without a cancellation; otherwise false and the default value of <typeparamref name="T"/>.</returns>
        public static async Task<(bool hasCompleted, T? result)> CatchCancellation<T>(this Task<T> task)
        {
            _ = Guard.NotNull(task, nameof(task));
            try
            {
                return (true, await task);
            }
            catch (TaskCanceledException)
            {
                return (false, default);
            }
        }
    }
}
