using System.Collections.Generic;

namespace MaSch.Console.Cli
{
    public interface ICliHelpPage
    {
        bool Write(ICliApplicationBase application, IEnumerable<CliError>? errors);
    }
}
