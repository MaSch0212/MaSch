using System;

namespace MaSch.Core.Logging
{
    /// <summary>
    /// Describes a provider of logging functionality.
    /// </summary>
    public interface ILoggingProvider
    {
        /// <summary>
        /// Logs the specified message with the specified log type.
        /// </summary>
        /// <param name="logType">Type of the log entry.</param>
        /// <param name="message">The message to log.</param>
        void Log(LogType logType, string? message);

        /// <summary>
        /// Logs an exception with the specified message and the specified log type.
        /// </summary>
        /// <param name="logType">Type of the log entry.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        void Log(LogType logType, string? message, Exception? exception);
    }
}
