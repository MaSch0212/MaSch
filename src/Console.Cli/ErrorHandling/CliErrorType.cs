namespace MaSch.Console.Cli.ErrorHandling
{
    public enum CliErrorType
    {
        VersionRequested,
        HelpRequested,
        MissingRequiredOption,
        UnknownOption,
        UnknownCommand,
        MissingOptionValue,
        BadOptionValue,
    }
}
