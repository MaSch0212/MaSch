
namespace MaSch.Core.Logging
{
    /// <summary>
    /// The type of a log entry.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// The debug type - used only for log entries that are important when debugging.
        /// </summary>
        Debug,

        /// <summary>
        /// The information type - used for informational log entries that are not only important while debugging.
        /// </summary>
        Information,

        /// <summary>
        /// The success type - used for log entries that report success.
        /// </summary>
        Success,

        /// <summary>
        /// The warning type - used for log entries that warns about potential errors.
        /// </summary>
        Warning,

        /// <summary>
        /// The error type - used for log entries that report errors.
        /// </summary>
        Error,

        /// <summary>
        /// The fatal error type - used for log entries that report fatal errors.
        /// </summary>
        FatalError
    }
}
