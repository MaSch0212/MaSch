using MaSch.Console.Cli.Runtime;
using System;

namespace MaSch.Console.Cli.ErrorHandling
{
    public class CliError
    {
        public CliErrorType Type { get; }
        public ICliCommandInfo? AffectedCommand { get; }
        public ICliCommandOptionInfo? AffectedOption { get; }
        public ICliCommandValueInfo? AffectedValue { get; }
        public string? CustomErrorMessage { get; }

        public string? CommandName { get; init; }
        public string? OptionName { get; init; }
        public Exception? Exception { get; init; }

        public CliError(CliErrorType type)
        {
            Type = type;
        }

        public CliError(CliErrorType type, ICliCommandInfo? affectedCommand)
            : this(type)
        {
            AffectedCommand = affectedCommand;
        }

        public CliError(CliErrorType type, ICliCommandInfo? affectedCommand, ICliCommandOptionInfo? affectedOption)
            : this(type, affectedCommand)
        {
            AffectedOption = affectedOption;
        }

        public CliError(CliErrorType type, ICliCommandInfo? affectedCommand, ICliCommandValueInfo? affectedValue)
            : this(type, affectedCommand)
        {
            AffectedValue = affectedValue;
        }

        public CliError(string errorMessage)
            : this(CliErrorType.Custom)
        {
            CustomErrorMessage = errorMessage;
        }

        public CliError(string errorMessage, ICliCommandInfo? affectedCommand)
            : this(errorMessage)
        {
            AffectedCommand = affectedCommand;
        }

        public CliError(string errorMessage, ICliCommandInfo? affectedCommand, ICliCommandOptionInfo? affectedOption)
            : this(errorMessage, affectedCommand)
        {
            AffectedOption = affectedOption;
        }

        public CliError(string errorMessage, ICliCommandInfo? affectedCommand, ICliCommandValueInfo? affectedValue)
            : this(errorMessage, affectedCommand)
        {
            AffectedValue = affectedValue;
        }
    }

    public enum CliErrorType
    {
        Unknown,
        Custom,
        VersionRequested,
        HelpRequested,
        UnknownCommand,
        UnknownOption,
        UnknownValue,
        MissingCommand,
        MissingOption,
        MissingOptionValue,
        MissingValue,
        WrongOptionFormat,
        WrongValueFormat,
    }
}
