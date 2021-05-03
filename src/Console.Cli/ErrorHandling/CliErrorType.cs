namespace MaSch.Console.Cli.ErrorHandling
{
    public enum CliErrorType
    {
        Custom,
        VersionRequested,
        HelpRequested,
        UnknownCommand,
        UnknownOption,
        UnknownValue,
        MissingCommand,
        MissingOption,
        MissingValue,
        WrongOptionFormat,
        WrongValueFormat,
    }
}
