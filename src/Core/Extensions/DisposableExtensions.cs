using System;
using System.Threading.Tasks;

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IDisposable"/>.
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        /// Executes an action on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="action">The action to execute.</param>
        public static void DoAndDispose<TDisposable>(this TDisposable disposable, Action<TDisposable> action)
            where TDisposable : IDisposable
        {
            Guard.NotNull(disposable, nameof(disposable));
            using (disposable)
            {
                Guard.NotNull(action, nameof(action));
                action(disposable);
            }
        }

        /// <summary>
        /// Executes a function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <typeparam name="TOut1">The type of the first <c>out</c> parameter.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="out1">The first <c>out</c> parameter.</param>
        /// <returns>The first element of the function result.</returns>
        public static TResult DoAndDispose<TDisposable, TResult, TOut1>(this TDisposable disposable, Func<TDisposable, (TResult Result, TOut1 Out1)> function, out TOut1 out1)
            where TDisposable : IDisposable
            => disposable.DoAndDispose(function).ExpandOut(out out1);

        /// <summary>
        /// Executes a function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <typeparam name="TOut1">The type of the first <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut2">The type of the second <c>out</c> parameter.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="out1">The first <c>out</c> parameter.</param>
        /// <param name="out2">The second <c>out</c> parameter.</param>
        /// <returns>The first element of the function result.</returns>
        public static TResult DoAndDispose<TDisposable, TResult, TOut1, TOut2>(this TDisposable disposable, Func<TDisposable, (TResult Result, TOut1 Out1, TOut2 Out2)> function, out TOut1 out1, out TOut2 out2)
            where TDisposable : IDisposable
            => disposable.DoAndDispose(function).ExpandOut(out out1, out out2);

        /// <summary>
        /// Executes a function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <typeparam name="TOut1">The type of the first <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut2">The type of the second <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut3">The type of the third <c>out</c> parameter.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="out1">The first <c>out</c> parameter.</param>
        /// <param name="out2">The second <c>out</c> parameter.</param>
        /// <param name="out3">The third <c>out</c> parameter.</param>
        /// <returns>The first element of the function result.</returns>
        public static TResult DoAndDispose<TDisposable, TResult, TOut1, TOut2, TOut3>(this TDisposable disposable, Func<TDisposable, (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3)> function, out TOut1 out1, out TOut2 out2, out TOut3 out3)
            where TDisposable : IDisposable
            => disposable.DoAndDispose(function).ExpandOut(out out1, out out2, out out3);

        /// <summary>
        /// Executes a function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <typeparam name="TOut1">The type of the first <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut2">The type of the second <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut3">The type of the third <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut4">The type of the fourth <c>out</c> parameter.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="out1">The first <c>out</c> parameter.</param>
        /// <param name="out2">The second <c>out</c> parameter.</param>
        /// <param name="out3">The third <c>out</c> parameter.</param>
        /// <param name="out4">The fourth <c>out</c> parameter.</param>
        /// <returns>The first element of the function result.</returns>
        public static TResult DoAndDispose<TDisposable, TResult, TOut1, TOut2, TOut3, TOut4>(this TDisposable disposable, Func<TDisposable, (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3, TOut4 Out4)> function, out TOut1 out1, out TOut2 out2, out TOut3 out3, out TOut4 out4)
            where TDisposable : IDisposable
            => disposable.DoAndDispose(function).ExpandOut(out out1, out out2, out out3, out out4);

        /// <summary>
        /// Executes a function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <typeparam name="TOut1">The type of the first <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut2">The type of the second <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut3">The type of the third <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut4">The type of the fourth <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut5">The type of the fifth <c>out</c> parameter.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="out1">The first <c>out</c> parameter.</param>
        /// <param name="out2">The second <c>out</c> parameter.</param>
        /// <param name="out3">The third <c>out</c> parameter.</param>
        /// <param name="out4">The fourth <c>out</c> parameter.</param>
        /// <param name="out5">The fifth <c>out</c> parameter.</param>
        /// <returns>The first element of the function result.</returns>
        public static TResult DoAndDispose<TDisposable, TResult, TOut1, TOut2, TOut3, TOut4, TOut5>(this TDisposable disposable, Func<TDisposable, (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3, TOut4 Out4, TOut5 Out5)> function, out TOut1 out1, out TOut2 out2, out TOut3 out3, out TOut4 out4, out TOut5 out5)
            where TDisposable : IDisposable
            => disposable.DoAndDispose(function).ExpandOut(out out1, out out2, out out3, out out4, out out5);

        /// <summary>
        /// Executes a function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <typeparam name="TOut1">The type of the first <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut2">The type of the second <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut3">The type of the third <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut4">The type of the fourth <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut5">The type of the fifth <c>out</c> parameter.</typeparam>
        /// <typeparam name="TOut6">The type of the sixth <c>out</c> parameter.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <param name="out1">The first <c>out</c> parameter.</param>
        /// <param name="out2">The second <c>out</c> parameter.</param>
        /// <param name="out3">The third <c>out</c> parameter.</param>
        /// <param name="out4">The fourth <c>out</c> parameter.</param>
        /// <param name="out5">The fifth <c>out</c> parameter.</param>
        /// <param name="out6">The sixth <c>out</c> parameter.</param>
        /// <returns>The first element of the function result.</returns>
        public static TResult DoAndDispose<TDisposable, TResult, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>(this TDisposable disposable, Func<TDisposable, (TResult Result, TOut1 Out1, TOut2 Out2, TOut3 Out3, TOut4 Ou4, TOut5 Ou5, TOut6 Out6)> function, out TOut1 out1, out TOut2 out2, out TOut3 out3, out TOut4 out4, out TOut5 out5, out TOut6 out6)
            where TDisposable : IDisposable
            => disposable.DoAndDispose(function).ExpandOut(out out1, out out2, out out3, out out4, out out5, out out6);

        /// <summary>
        /// Executes a function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <returns>The result of the function.</returns>
        public static TResult DoAndDispose<TDisposable, TResult>(this TDisposable disposable, Func<TDisposable, TResult> function)
            where TDisposable : IDisposable
        {
            Guard.NotNull(disposable, nameof(disposable));
            using (disposable)
            {
                Guard.NotNull(function, nameof(function));
                return function(disposable);
            }
        }

        /// <summary>
        /// Executes an async action on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="action">The action to execute.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task DoAndDisposeAsync<TDisposable>(this TDisposable disposable, Func<TDisposable, Task> action)
            where TDisposable : IDisposable
        {
            Guard.NotNull(disposable, nameof(disposable));
            using (disposable)
            {
                Guard.NotNull(action, nameof(action));
                await action(disposable);
            }
        }

        /// <summary>
        /// Executes an async function on the <see cref="IDisposable"/> as disposes it afterwards.
        /// </summary>
        /// <typeparam name="TDisposable">The type of the disposable.</typeparam>
        /// <typeparam name="TResult">The type of the result of the function.</typeparam>
        /// <param name="disposable">The <see cref="IDisposable"/> to execute the action on.</param>
        /// <param name="function">The function to execute.</param>
        /// <returns>The result of the function.</returns>
        public static async Task<TResult> DoAndDisposeAsync<TDisposable, TResult>(this TDisposable disposable, Func<TDisposable, Task<TResult>> function)
            where TDisposable : IDisposable
        {
            Guard.NotNull(disposable, nameof(disposable));
            using (disposable)
            {
                Guard.NotNull(function, nameof(function));
                return await function(disposable);
            }
        }
    }
}
