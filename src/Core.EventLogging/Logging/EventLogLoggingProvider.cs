using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MaSch.Core.Logging
{
    /// <summary>
    /// Implementation of the <see cref="ILoggingProvider"/> interface that logs into the windows EventLog.
    /// </summary>
    /// <seealso cref="ILoggingProvider" />
    public class EventLogLoggingProvider : ILoggingProvider
    {
        private readonly EventLog _eventLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLoggingProvider"/> class.
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="logName">Name of the log.</param>
        public EventLogLoggingProvider(string sourceName, string logName)
            : this(CreateEventLog(sourceName, logName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogLoggingProvider"/> class.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        public EventLogLoggingProvider(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        /// <inheritdoc />
        public void Log(LogType logType, string message) => Log(logType, message, null);

        /// <inheritdoc />
        public void Log(LogType logType, string message, Exception exception)
        {
            var type = GetEntryType(logType);
            var msg = GetErrorText(message, exception, type != EventLogEntryType.Information);
            _eventLog.WriteEntry(msg, type);
        }

        private static string GetErrorText(string message, Exception exception, bool writeNoExceptionText)
        {
            if (exception == null)
            {
                if (writeNoExceptionText)
                    message += "\n\nNo Exception Stack provided.";
            }
            else
            {
                message += "\n\nRaised Exception:\n" + exception;
            }

            return message;
        }

        private static EventLog CreateEventLog(string sourceName, string logName)
        {
            var result = new EventLog
            {
                Source = sourceName,
                Log = logName,
            };

            ((ISupportInitialize)result).BeginInit();

            if (!EventLog.SourceExists(result.Source))
            {
                EventLog.CreateEventSource(result.Source, result.Log);
            }

            ((ISupportInitialize)result).EndInit();
            return result;
        }

        private static EventLogEntryType GetEntryType(LogType type)
        {
            return type switch
            {
                LogType.Warning => EventLogEntryType.Warning,
                LogType.Error or LogType.FatalError => EventLogEntryType.Error,
                _ => EventLogEntryType.Information,
            };
        }
    }
}
