using System;
using System.Diagnostics;
using static System.Environment;

namespace MaSch.Core.Logging
{
    /// <summary>
    /// A <see cref="ILoggingProvider"/> that logs to the <see cref="Trace"/>.
    /// </summary>
    /// <seealso cref="ILoggingProvider" />
    public class TraceLoggingProvider : ILoggingProvider
    {
        /// <inheritdoc/>
        public void Log(LogType logType, string? message)
        {
            Log(logType, message, null);
        }

        /// <inheritdoc/>
        public void Log(LogType logType, string? message, Exception? exception)
        {
            var strLogType = GetTypeKey(logType);

            var strException = string.Empty;
            if (exception != null)
                strException = $"{NewLine}\tException: {exception.ToString().Replace(NewLine, $"{NewLine}\t")}";

            var actualMessage = message?.Replace(NewLine, $"{NewLine}\t") + strException;

            Trace.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {strLogType}: {actualMessage}{NewLine}");
        }

        private static string GetTypeKey(LogType type)
        {
            return type switch
            {
                LogType.Debug => "DEBG",
                LogType.Information => "INFO",
                LogType.Success => "SUCC",
                LogType.Warning => "WARN",
                LogType.Error => "EROR",
                LogType.FatalError => "FATL",
                _ => "????",
            };
        }
    }
}
