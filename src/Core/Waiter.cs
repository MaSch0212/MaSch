using MaSch.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Core
{
    /// <summary>
    /// Provides methods for waiting on specific conditions.
    /// </summary>
    public static class Waiter
    {
        private static readonly ILoggingService DefaultLoggingService = new LoggingService(new TraceLoggingProvider());
        /// <summary>
        /// Gets the default options for waiting actions.
        /// </summary>
        public static WaiterOptions DefaultOptions { get; } = new WaiterOptions(null)
        {
            Timeout = TimeSpan.FromSeconds(30),
            ThinkTimeBetweenChecks = TimeSpan.FromMilliseconds(100),
            MinimumCheckCount = 0,
            MaximumCheckCount = -1,
            IgnoreAllExceptions = true,
            IgnoredExceptionTypes = new List<Type>(),
            MaximumIgnoredExceptions = -1,
            OnException = null,
            LogIgnoredExceptions = true,
            IncludeExceptionStackInLog = false,
            ThrowException = true,
            ExceptionMessage = WaiterOptions.DefaultFailMessage,
            LoggingService = DefaultLoggingService,
        };
        /// <summary>
        /// Gets the default options for retry actions.
        /// </summary>
        public static RetryOptions DefaultRetryOptions { get; } = new RetryOptions(DefaultOptions)
        {
            Timeout = null,
            MaximumCheckCount = 5,
            ExceptionMessage = RetryOptions.DefaultFailMessage,
        };

        /// <summary>
        /// Waits until the specified action returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="waitAction">The wait action.</param>
        /// <returns>The result of the <paramref name="waitAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in time; otherwise, <see langword="default"/>.</returns>
        public static T? WaitUntil<T>(Func<T> waitAction) => WaitUntil(x => waitAction(), DefaultOptions);
        /// <summary>
        /// Waits until the specified action returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="waitAction">The wait action.</param>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <returns>The result of the <paramref name="waitAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in time; otherwise, <see langword="default"/>.</returns>
        public static T? WaitUntil<T>(Func<T> waitAction, TimeSpan timeout) => WaitUntil(x => waitAction(), new WaiterOptions { Timeout = timeout });
        /// <summary>
        /// Waits until the specified action returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="waitAction">The wait action.</param>
        /// <param name="options">The wait options to use with the wait operation.</param>
        /// <returns>The result of the <paramref name="waitAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in time; otherwise, <see langword="default"/>.</returns>
        public static T? WaitUntil<T>(Func<T> waitAction, WaiterOptions options) => WaitUntil(x => waitAction(), options);
        /// <summary>
        /// Waits until the specified action returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="waitAction">The wait action.</param>
        /// <returns>The result of the <paramref name="waitAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in time; otherwise, <see langword="default"/>.</returns>
        public static T? WaitUntil<T>(Func<WaitingState, T> waitAction) => WaitUntil(waitAction, DefaultOptions);
        /// <summary>
        /// Waits until the specified action returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="waitAction">The wait action.</param>
        /// <param name="timeout">The timeout for the wait.</param>
        /// <returns>The result of the <paramref name="waitAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in time; otherwise, <see langword="default"/>.</returns>
        public static T? WaitUntil<T>(Func<WaitingState, T> waitAction, TimeSpan timeout) => WaitUntil(waitAction, new WaiterOptions { Timeout = timeout });
        /// <summary>
        /// Waits until the specified action returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="waitAction">The wait action.</param>
        /// <param name="options">The wait options to use with the wait operation.</param>
        /// <returns>The result of the <paramref name="waitAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in time; otherwise, <see langword="default"/>.</returns>
        public static T? WaitUntil<T>(Func<WaitingState, T> waitAction, WaiterOptions options)
        {
            var state = new WaitingState(options);
            T? result = default;

            while (Equals(result, default(T)) &&
                   ((!options.Timeout.HasValue || state.Stopwatch.Elapsed <= options.Timeout) &&
                   (state.CheckCount < options.MaximumCheckCount || options.MaximumCheckCount < 0) ||
                   state.CheckCount < options.MinimumCheckCount) &&
                   !state.IsCanceled &&
                   (state.OccurredExceptionList.Count <= options.MaximumIgnoredExceptions || options.MaximumIgnoredExceptions < 0))
            {
                try
                {
                    result = waitAction(state);
                }
                catch (Exception ex)
                {
                    state.OccurredExceptionList.Add(ex);
                    if (!options.IgnoreAllExceptions && options.IgnoredExceptionTypes?.Contains(ex.GetType()) != true)
                        throw;
                    if (options.LogIgnoredExceptions)
                        options.LoggingService?.LogInformation($"Exception while waiting: {(options.IncludeExceptionStackInLog ? ex.ToString() : ex.Message)}");
                }
                finally { state.CheckCount++; }
            }
            state.Stopwatch.Stop();

            if (Equals(result, default(T)))
            {
                string failReason;
                if (state.IsCanceled)
                    failReason = "Canceled by action";
                else if (state.CheckCount > options.MaximumCheckCount && options.MaximumCheckCount >= 0)
                    failReason = $"Maximum check count of {options.MaximumCheckCount} exceeded";
                else if (state.Stopwatch.Elapsed > options.Timeout)
                    failReason = $"Timeout of {options.Timeout} exceeded";
                else if (state.OccurredExceptionList.Count > options.MaximumIgnoredExceptions && options.MaximumIgnoredExceptions >= 0)
                    failReason = $"Maximum exception count of {options.MaximumIgnoredExceptions} exceeded";
                else
                    failReason = "Unknown fail reason";

                var errorMessage = $"{options.ExceptionMessage ?? WaiterOptions.DefaultFailMessage} (Reason: {failReason})";
                options.LoggingService?.LogWarning(errorMessage);
                if (options.ThrowException)
                    throw new WaiterException(errorMessage);
            }

            return result;
        }

        /// <summary>
        /// Retries the specified action until it returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="retryAction">The retry action.</param>
        /// <returns>The result of the <paramref name="retryAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in the specified execution limit; otherwise, <see langword="default"/>.</returns>
        public static T? Retry<T>(Func<T> retryAction) => WaitUntil(x => retryAction(), DefaultRetryOptions);
        /// <summary>
        /// Retries the specified action until it returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="maxCheckCount">The maximum number of retries.</param>
        /// <returns>The result of the <paramref name="retryAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in the specified execution limit; otherwise, <see langword="default"/>.</returns>
        public static T? Retry<T>(Func<T> retryAction, int maxCheckCount) => WaitUntil(x => retryAction(), new RetryOptions { MaximumCheckCount = maxCheckCount });
        /// <summary>
        /// Retries the specified action until it returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="options">The retry options to use with the retry operation.</param>
        /// <returns>The result of the <paramref name="retryAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in the specified execution limit; otherwise, <see langword="default"/>.</returns>
        public static T? Retry<T>(Func<T> retryAction, RetryOptions options) => WaitUntil(x => retryAction(), options);
        /// <summary>
        /// Retries the specified action until it returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="retryAction">The retry action.</param>
        /// <returns>The result of the <paramref name="retryAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in the specified execution limit; otherwise, <see langword="default"/>.</returns>
        public static T? Retry<T>(Func<WaitingState, T> retryAction) => WaitUntil(retryAction, DefaultRetryOptions);
        /// <summary>
        /// Retries the specified action until it returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="maxCheckCount">The maximum number of retries.</param>
        /// <returns>The result of the <paramref name="retryAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in the specified execution limit; otherwise, <see langword="default"/>.</returns>
        public static T? Retry<T>(Func<WaitingState, T> retryAction, int maxCheckCount) => WaitUntil(retryAction, new RetryOptions { MaximumCheckCount = maxCheckCount });
        /// <summary>
        /// Retries the specified action until it returns a value other than <see langword="default"/> and throws no exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the action result.</typeparam>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="options">The retry options to use with the retry operation.</param>
        /// <returns>The result of the <paramref name="retryAction"/> if it returned a value other than <see langword="default"/> and throws no exceptions in the specified execution limit; otherwise, <see langword="default"/>.</returns>
        public static T? Retry<T>(Func<WaitingState, T> retryAction, RetryOptions options) => WaitUntil(retryAction, options);

        /// <summary>
        /// Retries the specified action until it throws no exceptions.
        /// </summary>
        /// <param name="retryAction">The retry action.</param>
        /// <returns><see langword="true"/> if the <paramref name="retryAction"/> throws no exceptions in the specified execution limit; otherwise, <see langword="false"/>.</returns>
        public static bool Retry(Action retryAction) => WaitUntil(ToWaitAction(retryAction), DefaultRetryOptions);
        /// <summary>
        /// Retries the specified action until it throws no exceptions.
        /// </summary>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="maxCheckCount">The maximum number of retries.</param>
        /// <returns><see langword="true"/> if the <paramref name="retryAction"/> throws no exceptions in the specified execution limit; otherwise, <see langword="false"/>.</returns>
        public static bool Retry(Action retryAction, int maxCheckCount) => WaitUntil(ToWaitAction(retryAction), new RetryOptions { MaximumCheckCount = maxCheckCount });
        /// <summary>
        /// Retries the specified action until it throws no exceptions.
        /// </summary>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="options">The retry options to use with the retry operation.</param>
        /// <returns><see langword="true"/> if the <paramref name="retryAction"/> throws no exceptions in the specified execution limit; otherwise, <see langword="false"/>.</returns>
        public static bool Retry(Action retryAction, RetryOptions options) => WaitUntil(ToWaitAction(retryAction), options);
        /// <summary>
        /// Retries the specified action until it throws no exceptions.
        /// </summary>
        /// <param name="retryAction">The retry action.</param>
        /// <returns><see langword="true"/> if the <paramref name="retryAction"/> throws no exceptions in the specified execution limit; otherwise, <see langword="false"/>.</returns>
        public static bool Retry(Action<WaitingState> retryAction) => WaitUntil(ToWaitAction(retryAction), DefaultRetryOptions);
        /// <summary>
        /// Retries the specified action until it throws no exceptions.
        /// </summary>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="maxCheckCount">The maximum number of retries.</param>
        /// <returns><see langword="true"/> if the <paramref name="retryAction"/> throws no exceptions in the specified execution limit; otherwise, <see langword="false"/>.</returns>
        public static bool Retry(Action<WaitingState> retryAction, int maxCheckCount) => WaitUntil(ToWaitAction(retryAction), new RetryOptions { MaximumCheckCount = maxCheckCount });
        /// <summary>
        /// Retries the specified action until it throws no exceptions.
        /// </summary>
        /// <param name="retryAction">The retry action.</param>
        /// <param name="options">The retry options to use with the retry operation.</param>
        /// <returns><see langword="true"/> if the <paramref name="retryAction"/> throws no exceptions in the specified execution limit; otherwise, <see langword="false"/>.</returns>
        public static bool Retry(Action<WaitingState> retryAction, RetryOptions options) => WaitUntil(ToWaitAction(retryAction), options);

        private static Func<WaitingState, bool> ToWaitAction(Action retryAction)
        {
            return x =>
            {
                retryAction();
                return true;
            };
        }

        private static Func<WaitingState, bool> ToWaitAction(Action<WaitingState> retryAction)
        {
            return x =>
            {
                retryAction(x);
                return true;
            };
        }
    }

    /// <summary>
    /// Provides options for waiting actions of the <see cref="Waiter"/>.
    /// </summary>
    /// <seealso cref="ICloneable" />
    public class WaiterOptions : ICloneable
    {
        internal const string DefaultFailMessage = "The waiting action has ended without result.";
        
        /// <summary>
        /// Gets or sets the timeout of the waiting action.
        /// </summary>
        public TimeSpan? Timeout { get; set; }
        /// <summary>
        /// Gets or sets the time that is slepts between wait condition checks.
        /// </summary>
        public TimeSpan ThinkTimeBetweenChecks { get; set; }        
        /// <summary>
        /// Gets or sets the number of checks that needs to be done at minimum while waiting.
        /// </summary>
        public int MinimumCheckCount { get; set; }        
        /// <summary>
        /// Gets or sets the number of checks that can be done at maximum while waiting. If set to -1 the number of checks is unlimited.
        /// </summary>
        public int MaximumCheckCount { get; set; }        
        /// <summary>
        /// Gets or sets a value indicating wether all exceptions (and not only the ones defined by <see cref="IgnoredExceptionTypes"/>) should be ignored while waiting.
        /// </summary>
        public bool IgnoreAllExceptions { get; set; }
        /// <summary>
        /// Gets or sets a collection of exception types that should be ignored while waiting (only used if <see cref="IgnoreAllExceptions"/> is set to <see langword="false"/>).
        /// </summary>
        public ICollection<Type>? IgnoredExceptionTypes { get; set; }
        /// <summary>
        /// Gets or sets the maximum number of ignored exceptions while waiting. If set to -1 the number fo exceptions is unlimited.
        /// </summary>
        public int MaximumIgnoredExceptions { get; set; }
        /// <summary>
        /// Gets or sets an action that is executed when an exception occures while waiting.
        /// </summary>
        public Action<WaitingState>? OnException { get; set; }
        /// <summary>
        /// Gets or sets a value indicating wether occurring exceptions while waiting shuld be logged usingg the <see cref="LoggingService"/>.
        /// </summary>
        public bool LogIgnoredExceptions { get; set; }
        /// <summary>
        /// Gets or sets a value indicating wether to include the exception stack in the log if <see cref="LogIgnoredExceptions"/> is set to <see langword="true"/>.
        /// </summary>
        public bool IncludeExceptionStackInLog { get; set; }
        /// <summary>
        /// Gets or sets a value indicating wether the waiting operation should throw an exception if it runs into a timeout.
        /// </summary>
        public bool ThrowException { get; set; }
        /// <summary>
        /// Gets or sets the error message that should be logged if the waiting operation runs into a timeout. If <see cref="ThrowException"/> is set to <see langword="true"/> this message is also used for the exception.
        /// </summary>
        public string? ExceptionMessage { get; set; }
        /// <summary>
        /// Gets or sets a <see cref="ILoggingService"/> that is used for logging. If set to <see langword="null"/> logging is disabled.
        /// </summary>
        public ILoggingService? LoggingService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaiterOptions" /> class with default values defined by <see cref="Waiter.DefaultOptions"/>.
        /// </summary>
        public WaiterOptions() : this(Waiter.DefaultOptions) { }
        internal WaiterOptions(WaiterOptions? defaultValues)
        {
            if (defaultValues != null)
            {
                Timeout = defaultValues.Timeout;
                ThinkTimeBetweenChecks = defaultValues.ThinkTimeBetweenChecks;
                MinimumCheckCount = defaultValues.MinimumCheckCount;
                MaximumCheckCount = defaultValues.MaximumCheckCount;
                IgnoreAllExceptions = defaultValues.IgnoreAllExceptions;
                IgnoredExceptionTypes = defaultValues.IgnoredExceptionTypes;
                MaximumIgnoredExceptions = defaultValues.MaximumIgnoredExceptions;
                OnException = defaultValues.OnException;
                LogIgnoredExceptions = defaultValues.LogIgnoredExceptions;
                IncludeExceptionStackInLog = defaultValues.IncludeExceptionStackInLog;
                ThrowException = defaultValues.ThrowException;
                ExceptionMessage = defaultValues.ExceptionMessage;
            }
        }

        object ICloneable.Clone() => MemberwiseClone();
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public WaiterOptions Clone() => (WaiterOptions)MemberwiseClone();
    }

    /// <summary>
    /// Provides options for retry actions of the <see cref="Waiter"/>.
    /// </summary>
    /// <seealso cref="WaiterOptions" />
    public class RetryOptions : WaiterOptions
    {
        internal new const string DefaultFailMessage = "The retry action has ended without result.";

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryOptions" /> class with default values defined by <see cref="Waiter.DefaultRetryOptions"/>.
        /// </summary>
        public RetryOptions() : this(Waiter.DefaultRetryOptions) { }
        internal RetryOptions(WaiterOptions defaultValues) : base(defaultValues)
        {
        }
    }

    /// <summary>
    /// Provides state information about a waiting operation.
    /// </summary>
    public class WaitingState
    {
        internal List<Exception> OccurredExceptionList { get; }
        internal Stopwatch Stopwatch { get; }
        
        /// <summary>
        /// Gets the elapsed time of this waiting operation.
        /// </summary>
        public TimeSpan ElapsedTime => Stopwatch.Elapsed;
        /// <summary>
        /// Gets the number of checks executed by this waiting operation.
        /// </summary>
        public int CheckCount { get; internal set; }
        /// <summary>
        /// Gets a collection of exceptions occurred while executing this waiting operation.
        /// </summary>
        public IReadOnlyList<Exception> OccurredExceptions { get; }
        /// <summary>
        /// Gets the last exception that occurred while executing this waiting operation. If no exception occurred, <see langword="null"/> is returned.
        /// </summary>
        public Exception? LatestException => OccurredExceptions.Count == 0 ? null : OccurredExceptions[OccurredExceptions.Count - 1];
        /// <summary>
        /// Gets a value indicating wether this waiting operation has been canceled.
        /// </summary>
        public bool IsCanceled { get; private set; }
        /// <summary>
        /// Gets the options of this waiting operation.
        /// </summary>
        public WaiterOptions Options { get; }

        internal WaitingState(WaiterOptions options)
        {
            Stopwatch = new Stopwatch();
            OccurredExceptionList = new List<Exception>();
            OccurredExceptions = OccurredExceptionList.AsReadOnly();
            Options = options;

            Stopwatch.Start();
        }

        /// <summary>
        /// Cancels this waiting operation.
        /// </summary>
        public void Cancel() => IsCanceled = true;
    }

    /// <summary>
    /// An exception thown by the <see cref="Waiter"/> if a waiting operation runs into a timeout or retry action runs out of retries.
    /// </summary>
    /// <seealso cref="ApplicationException" />
    public class WaiterException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WaiterException" /> class.
        /// </summary>
        public WaiterException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaiterException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public WaiterException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WaiterException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public WaiterException(string message, Exception innerException) : base(message, innerException) { }
    }
}
