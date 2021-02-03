using MaSch.Console.Cli.Help;

namespace MaSch.Console.Cli.Runtime
{
    public class CliApplicationOptions
    {
        public ICliHelpPage HelpPage { get; set; }

        public CliApplicationOptions()
        {
            HelpPage = new CliHelpPage();
        }
    }
}
