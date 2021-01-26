using MaSch.Console.Cli.Help;
using System;
using System.Collections.Generic;
using System.Text;

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
