namespace MaSch.Console.Cli
{
    /// <summary>
    /// Represents an error type for the <see cref="CliError"/> class.
    /// </summary>
    public enum CliErrorType
    {
        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown,

        /// <summary>
        /// Custom error. More information are provided via the <see cref="CliError.CustomErrorMessage"/> property.
        /// </summary>
        Custom,

        /// <summary>
        /// Version has been requested.
        /// </summary>
        VersionRequested,

        /// <summary>
        /// Help has been requested.
        /// </summary>
        HelpRequested,

        /// <summary>
        /// The given command is unknown. The unknown command name is provided via the <see cref="CliError.CommandName"/> property.
        /// </summary>
        UnknownCommand,

        /// <summary>
        /// The given option is unknwon. The unknown option name is provided via the <see cref="CliError.OptionName"/> property.
        /// </summary>
        UnknownOption,

        /// <summary>
        /// The given value is unknown. In other words: Too many values have been provided.
        /// </summary>
        UnknownValue,

        /// <summary>
        /// No executable command has been provided.
        /// </summary>
        MissingCommand,

        /// <summary>
        /// A required option is missing.
        /// </summary>
        MissingOption,

        /// <summary>
        /// A value is missing for a command.
        /// </summary>
        MissingOptionValue,

        /// <summary>
        /// A required value is missing. In other words: Too few values have been provided.
        /// </summary>
        MissingValue,

        /// <summary>
        /// An option value has the wrong format.
        /// </summary>
        WrongOptionFormat,

        /// <summary>
        /// A value has the wrong format.
        /// </summary>
        WrongValueFormat,

        /// <summary>
        /// The given command is not executable.
        /// </summary>
        CommandNotExecutable,
    }
}
