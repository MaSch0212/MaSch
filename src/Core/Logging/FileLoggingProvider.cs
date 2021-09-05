using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Environment;

namespace MaSch.Core.Logging
{
    /// <summary>
    /// A <see cref="ILoggingProvider"/> that logs to a file.
    /// </summary>
    /// <seealso cref="ILoggingProvider" />
    public class FileLoggingProvider : ILoggingProvider
    {
        private const long FileSizeThreshold = 10L * 1024 * 1024; // 10 MB

        private readonly object _lock = new();
        private readonly string _directoryPath;
        private readonly string _fileName;
        private int _currentFileNumber = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLoggingProvider"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path in which to store the log files.</param>
        /// <param name="fileName">Name without extension of the log files.</param>
        public FileLoggingProvider(string directoryPath, string fileName)
        {
            _directoryPath = directoryPath;
            _fileName = fileName;
        }

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
            lock (_lock)
                File.AppendAllText(GetFile(), $@"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {strLogType}: {actualMessage}{NewLine}");
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

        private string GetFile()
        {
            if (_currentFileNumber < 0)
            {
                if (Directory.Exists(_directoryPath))
                {
                    var files = Directory.GetFiles(_directoryPath, "*", SearchOption.TopDirectoryOnly);
                    _currentFileNumber = (from x in files
                                          let match = Regex.Match(x, $@"\\{Regex.Escape(_fileName)}\.(?<id>[0-9]+)\.log\Z")
                                          where match.Success
                                          select int.Parse(match.Groups["id"].Value)).Max();
                }
                else
                {
                    _ = Directory.CreateDirectory(_directoryPath);
                    _currentFileNumber = 0;
                }
            }

            var path = Path.Combine(_directoryPath, $"{_fileName}.{_currentFileNumber:000}.log");
            while (File.Exists(path) && new FileInfo(path).Length >= FileSizeThreshold)
                path = Path.Combine(_directoryPath, $"{_fileName}.{++_currentFileNumber:000}.log");
            return path;
        }
    }
}
