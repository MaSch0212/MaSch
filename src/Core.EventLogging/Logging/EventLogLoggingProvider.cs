using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MaSch.Common.Logging
{
    public class EventLogLoggingProvider : ILoggingProvider
    {
        private readonly EventLog _eventLog;

        public EventLogLoggingProvider(string sourceName, string logName) 
            : this(CreateEventLog(sourceName, logName)) { }
        public EventLogLoggingProvider(EventLog eventLog)
        {
            _eventLog = eventLog;
        }

        public void Log(LogType logType, string message) => Log(logType, message, null);
        public void Log(LogType logType, string message, Exception exception)
        {
            var type = GetEntryType(logType);
            var msg = GetErrorText(message, exception, type != EventLogEntryType.Information);
            _eventLog.WriteEntry(msg, type);
        }

        private string GetErrorText(string message, Exception exception, bool writeNoExceptionText)
        {
            if (exception == null)
            {
                if (writeNoExceptionText)
                    message += "\n\nNo Exception Stack provided.";
            }
            else
                message += "\n\nRaised Exception:\n" + exception;
            return message;
        }

        private static EventLog CreateEventLog(string sourceName, string logName)
        {
            var result = new EventLog
            {
                Source = sourceName,
                Log = logName
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
            switch (type)
            {
                case LogType.Warning:
                    return EventLogEntryType.Warning;
                case LogType.Error:
                case LogType.FatalError:
                    return EventLogEntryType.Error;
                case LogType.Debug:
                case LogType.Information:
                case LogType.Success:
                default:
                    return EventLogEntryType.Information;
            }
        }
    }
}
