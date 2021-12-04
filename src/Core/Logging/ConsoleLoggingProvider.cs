using static System.Environment;

namespace MaSch.Core.Logging;

/// <summary>
/// A <see cref="ILoggingProvider"/> that logs to the console.
/// </summary>
/// <seealso cref="ILoggingProvider" />
public class ConsoleLoggingProvider : ILoggingProvider
{
    private readonly bool _logExceptionStackTrace;
    private readonly bool _logDebugMessages;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLoggingProvider"/> class.
    /// </summary>
    public ConsoleLoggingProvider()
        : this(false, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleLoggingProvider"/> class.
    /// </summary>
    /// <param name="logExceptionStackTrace">if set to <c>true</c> the stack trace of exception is logged.</param>
    /// <param name="logDebugMessages">if set to <c>true</c> debug messages are logged.</param>
    public ConsoleLoggingProvider(bool logExceptionStackTrace, bool logDebugMessages)
    {
        _logExceptionStackTrace = logExceptionStackTrace;
        _logDebugMessages = logDebugMessages;
    }

    /// <inheritdoc/>
    public void Log(LogType logType, string? message)
    {
        Log(logType, message, null);
    }

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
        var result = new StringBuilder(message);

        if (exception is AggregateException aggregateException)
        {
            foreach (var ex in aggregateException.InnerExceptions)
                _ = result.Append(GetExceptionText(ex, _logExceptionStackTrace));
        }
        else if (exception != null)
        {
            _ = result.Append(GetExceptionText(exception, _logExceptionStackTrace));
        }

        return result.ToString();

        static string GetExceptionText(Exception ex, bool includeStackTrace)
            => $"{NewLine}    - {(includeStackTrace ? ex.ToString() : ex.Message).Replace(NewLine, $"{NewLine}      ")}";
    }
}
