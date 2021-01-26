using System;
using System.Collections.Generic;
using System.Text;

namespace MaSch.Console.Cli.Help
{
    public enum CliErrorType
    {
        VersionRequested,
        HelpRequested,
        MissingRequiredOption,
        UnknownOption,
        UnknownCommand,
        MissingOptionValue,
        BadOptionValue
    }
}
