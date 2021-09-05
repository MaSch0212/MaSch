using System;

namespace MaSch.Core.Logging
{
    /// <summary>
    /// Provides extension methods for <see cref="ILoggingProvider"/>.
    /// </summary>
    public static class LoggingProviderExtensions
    {
        /// <summary>
        /// Logs a specified message as debug type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        public static void LogDebug(this ILoggingProvider loggingProvider, string? message)
        {
            loggingProvider.Log(LogType.Debug, message);
        }

        /// <summary>
        /// Logs an exception with a specified message as debug type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The excepotion to log.</param>
        public static void LogDebug(this ILoggingProvider loggingProvider, string? message, Exception? exception)
        {
            loggingProvider.Log(LogType.Debug, message, exception);
        }

        /// <summary>
        /// Logs a specified message as information type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        public static void LogInformation(this ILoggingProvider loggingProvider, string? message)
        {
            loggingProvider.Log(LogType.Information, message);
        }

        /// <summary>
        /// Logs a specified message as success type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        public static void LogSuccess(this ILoggingProvider loggingProvider, string? message)
        {
            loggingProvider.Log(LogType.Success, message);
        }

        /// <summary>
        /// Logs a specified message as warning type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        public static void LogWarning(this ILoggingProvider loggingProvider, string? message)
        {
            loggingProvider.Log(LogType.Warning, message);
        }

        /// <summary>
        /// Logs an exception with a specified message as warning type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The excepotion to log.</param>
        public static void LogWarning(this ILoggingProvider loggingProvider, string? message, Exception? exception)
        {
            loggingProvider.Log(LogType.Warning, message, exception);
        }

        /// <summary>
        /// Logs a specified message as error type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        public static void LogError(this ILoggingProvider loggingProvider, string? message)
        {
            loggingProvider.Log(LogType.Error, message);
        }

        /// <summary>
        /// Logs an exception with a specified message as error type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The excepotion to log.</param>
        public static void LogError(this ILoggingProvider loggingProvider, string? message, Exception? exception)
        {
            loggingProvider.Log(LogType.Error, message, exception);
        }

        /// <summary>
        /// Logs a specified message as fatal error type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        public static void LogFatalError(this ILoggingProvider loggingProvider, string? message)
        {
            loggingProvider.Log(LogType.FatalError, message);
        }

        /// <summary>
        /// Logs an exception with a specified message as fatal error type.
        /// </summary>
        /// <param name="loggingProvider">The logging provider to use.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The excepotion to log.</param>
        public static void LogFatalError(this ILoggingProvider loggingProvider, string? message, Exception? exception)
        {
            loggingProvider.Log(LogType.FatalError, message, exception);
        }
    }
}
