using MaSch.Console.Cli.ErrorHandling;

namespace MaSch.Console.Cli.Runtime
{
    public class CliApplicationArgumentParserResult
    {
        public bool Success { get; }
        public CliError? Error { get; }
        public CliCommandInfo? Command { get; }
        public object? Options { get; }

        internal CliApplicationArgumentParserResult(CliError error)
        {
            Success = false;
            Error = error;
        }

        internal CliApplicationArgumentParserResult(CliCommandInfo command, object options)
        {
            Success = true;
            Command = command;
            Options = options;
        }
    }
}
