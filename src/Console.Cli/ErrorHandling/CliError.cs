using System;

namespace MaSch.Console.Cli.ErrorHandling
{
    public class CliError
    {
        public CliErrorType Type { get; set; }
        public CliCommandInfo? AffectedCommand { get; set; }
        public CliCommandOptionInfo? AffectedOption { get; set; }
        public CliCommandValueInfo? AffectedValue { get; set; }

        public string? CommandName { get; set; }
        public string? OptionName { get; set; }
        public Exception? Exception { get; set; }

        public CliError(CliErrorType type)
        {
            Type = type;
        }

        public CliError(CliErrorType type, CliCommandInfo? affectedCommand)
            : this(type)
        {
            AffectedCommand = affectedCommand;
        }

        public CliError(CliErrorType type, CliCommandInfo? affectedCommand, CliCommandOptionInfo? affectedOption)
            : this(type, affectedCommand)
        {
            AffectedOption = affectedOption;
        }

        public CliError(CliErrorType type, CliCommandInfo? affectedCommand, CliCommandValueInfo? affectedValue)
            : this(type, affectedCommand)
        {
            AffectedValue = affectedValue;
        }
    }
}
