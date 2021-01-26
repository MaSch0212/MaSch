using System.Collections.Generic;

namespace MaSch.Console.Cli.Help
{
    public interface ICliHelpPage
    {
        void WriteRootHelpPage(IEnumerable<CliCommandInfo> rootCommands);
        void WriteHelpPage(CliCommandInfo command);
    }
}
