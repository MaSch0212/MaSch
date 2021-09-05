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
        /// Gets a value indicating whether this is an actual error.
        /// </summary>
        public bool IsError => Type != CliErrorType.VersionRequested && Type != CliErrorType.HelpRequested;
    }
}
