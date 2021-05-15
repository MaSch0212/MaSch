using MaSch.Console.Cli.ErrorHandling;
using MaSch.Console.Cli.Runtime;
using System.Collections.Generic;

namespace MaSch.Console.Cli.Help
{
    public interface ICliHelpPage
    {
        void Write(ICliApplicationBase application, IEnumerable<CliError>? errors);
    }
}
