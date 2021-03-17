using MaSch.Console.Cli.Help;

namespace MaSch.Console.Cli.Runtime
{
    public sealed class CliApplicationOptions
    {
        public ICliHelpPage HelpPage { get; init; }
        public bool IgnoreUnknownOptions { get; init; }
        public bool IgnoreAdditionalValues { get; init; }
        public bool ProvideHelpCommand { get; init; }
        public bool ProvideVersionCommand { get; init; }
        public bool ProvideHelpOptions { get; init; }
        public bool ProvideVersionOptions { get; init; }

        public CliApplicationOptions()
        {
            HelpPage = new CliHelpPage();
        }
    }
}
