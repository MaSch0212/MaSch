using System;
using static System.Environment;

namespace MaSch.Core.Logging
{
    /// <summary>
    /// A <see cref="ILoggingProvider"/> that logs to the console.
    /// </summary>
    /// <seealso cref="ILoggingProvider" />
    public class ConsoleLoggingProvider : ILoggingProvider
    {
        private readonly bool _logExceptionStackTrace;
        private readonly bool _logDebugMessages;

        public ConsoleLoggingProvider() : this(false, false) { }
        public ConsoleLoggingProvider(bool logExceptionStackTrace, bool logDebugMessages)
        {
            _logExceptionStackTrace = logExceptionStackTrace;
            _logDebugMessages = logDebugMessages;
        }

        /// <inheritdoc/>
        public void Log(LogType logType, string? message)
            => Log(logType, message, null);

        /// <inheritdoc/>
        public void Log(LogType logType, string? message, Exception? exception)
        {
            if (logType == LogType.Debug && !_logDebugMessages)
                return;

            var prevC = Console.ForegroundColor;

            Console.ForegroundColor = logType switch
            {
                LogType.Debug => ConsoleColor.DarkGray,
                LogType.Success => ConsoleColor.Green,
                LogType.Warning => ConsoleColor.Yellow,
                LogType.Error or LogType.FatalError => ConsoleColor.Red,
                _ => ConsoleColor.Gray,
            };
            Console.WriteLine(GetErrorText(message, exception));
            Console.ForegroundColor = prevC;
        }

        private string? GetErrorText(string? message, Exception? exception)
        {
            if (exception is AggregateException aggregateException)
            {
                foreach (var ex in aggregateException.InnerExceptions)
                    message += GetExceptionText(ex, _logExceptionStackTrace);
            }
            else if (exception != null)
                message += GetExceptionText(exception, _logExceptionStackTrace);
            return message;

            static string GetExceptionText(Exception ex, bool includeStackTrace) 
                => $"{NewLine}    - {(includeStackTrace ? ex.ToString() : ex.Message).Replace(NewLine, $"{NewLine}      ")}";
        }
    }
}
