using MaSch.Console.Cli.ErrorHandling;
using MaSch.Console.Cli.Runtime;

namespace MaSch.Console.Cli.Help
{
    public interface ICliHelpPage
    {
        void WriteHelpPage(ICliApplicationBase application, CliError error);
    }
}
