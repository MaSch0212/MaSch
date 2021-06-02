using MaSch.Console.Cli.Runtime;
using System.Collections.Generic;

namespace MaSch.Console.Cli
{
    public interface ICliHelpPage
    {
        void Write(ICliApplicationBase application, IEnumerable<CliError>? errors);
    }
}
