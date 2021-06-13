using MaSch.Console.Cli.Runtime;
using System;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Represents an error that occures while parsing command line arguments.
    /// </summary>
    public class CliError
    {
        /// <summary>
        /// Gets the error type.
        /// </summary>
        public CliErrorType Type { get; }

        /// <summary>
        /// Gets the affected command.
        /// </summary>
        public ICliCommandInfo? AffectedCommand { get; }

        /// <summary>
        /// Gets the affected option.
        /// </summary>
        public ICliCommandOptionInfo? AffectedOption { get; }

        /// <summary>
        /// Gets the affected value.
        /// </summary>
        public ICliCommandValueInfo? AffectedValue { get; }

        /// <summary>
        /// Gets the custom error message.
        /// </summary>
        public string? CustomErrorMessage { get; }

        /// <summary>
        /// Gets the affected command name.
        /// </summary>
        public string? CommandName { get; init; }

        /// <summary>
        /// Gets the affected option name.
        /// </summary>
        public string? OptionName { get; init; }

        /// <summary>
        /// Gets the exception that lead to this <see cref="CliError"/>.
        /// </summary>
        public Exception? Exception { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="type">The error type.</param>
        public CliError(CliErrorType type)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="affectedCommand">The affected command.</param>
        public CliError(CliErrorType type, ICliCommandInfo? affectedCommand)
            : this(type)
        {
            AffectedCommand = affectedCommand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="affectedCommand">The affected command.</param>
        /// <param name="affectedOption">The affected option.</param>
        public CliError(CliErrorType type, ICliCommandInfo? affectedCommand, ICliCommandOptionInfo? affectedOption)
            : this(type, affectedCommand)
        {
            AffectedOption = affectedOption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="type">The error type.</param>
        /// <param name="affectedCommand">The affected command.</param>
        /// <param name="affectedValue">The affected value.</param>
        public CliError(CliErrorType type, ICliCommandInfo? affectedCommand, ICliCommandValueInfo? affectedValue)
            : this(type, affectedCommand)
        {
            AffectedValue = affectedValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        public CliError(string errorMessage)
            : this(CliErrorType.Custom)
        {
            CustomErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        /// <param name="affectedCommand">The affected command.</param>
        public CliError(string errorMessage, ICliCommandInfo? affectedCommand)
            : this(errorMessage)
        {
            AffectedCommand = affectedCommand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        /// <param name="affectedCommand">The affected command.</param>
        /// <param name="affectedOption">The affected option.</param>
        public CliError(string errorMessage, ICliCommandInfo? affectedCommand, ICliCommandOptionInfo? affectedOption)
            : this(errorMessage, affectedCommand)
        {
            AffectedOption = affectedOption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliError"/> class.
        /// </summary>
        /// <param name="errorMessage">The custom error message.</param>
        /// <param name="affectedCommand">The affected command.</param>
        /// <param name="affectedValue">The affected value.</param>
        public CliError(string errorMessage, ICliCommandInfo? affectedCommand, ICliCommandValueInfo? affectedValue)
            : this(errorMessage, affectedCommand)
        {
            AffectedValue = affectedValue;
        }
    }

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
    }
}
