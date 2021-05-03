using MaSch.Console.Cli.ErrorHandling;

namespace MaSch.Console.Cli.Runtime
{
    public class CliApplicationArgumentParserResult
    {
        public bool Success { get; }
        public CliError? Error { get; }
        public ICliCommandInfo? Command { get; }
        public object? Options { get; }

        internal CliApplicationArgumentParserResult(CliError error)
        {
            Success = false;
            Error = error;
        }

        internal CliApplicationArgumentParserResult(ICliCommandInfo command, object options)
        {
            Success = true;
            Command = command;
            Options = options;
        }
    }
}
