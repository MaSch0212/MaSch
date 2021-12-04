using System.Runtime.Versioning;

namespace MaSch.Core.Helper;

/// <summary>
/// Provides helper methods for <see cref="Task"/>s.
/// </summary>
public static class TaskHelper
{
    /// <summary>
    /// Runs the specified action in an STA thread.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>A <see cref="Task"/> wrapping the STA thread.</returns>
    [SupportedOSPlatform("windows")]
    public static Task RunSta(Action action)
    {
        _ = Guard.NotNull(action, nameof(action));
        var tcs = new TaskCompletionSource<bool>();
        var thread = new Thread(() =>
        {
            try
            {
                action();
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }

    /// <summary>
    /// Runs the specified function in an STA thread.
    /// </summary>
    /// <typeparam name="T">The type of the result of the function.</typeparam>
    /// <param name="func">The function to execute.</param>
    /// <returns>A <see cref="Task{TResult}"/> wrapping the STA thread.</returns>
    [SupportedOSPlatform("windows")]
    public static Task<T> RunSta<T>(Func<T> func)
    {
        _ = Guard.NotNull(func, nameof(func));
        var tcs = new TaskCompletionSource<T>();
        var thread = new Thread(() =>
        {
            try
            {
                tcs.SetResult(func());
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        return tcs.Task;
    }
}
