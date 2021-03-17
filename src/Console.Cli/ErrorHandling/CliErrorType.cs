namespace MaSch.Console.Cli.ErrorHandling
{
    public enum CliErrorType
    {
        VersionRequested,
        HelpRequested,
        UnknownCommand,
        UnknownOption,
        UnknownValue,
        MissingOption,
        MissingValue,
        WrongOptionFormat,
        WrongValueFormat,
    }
}
