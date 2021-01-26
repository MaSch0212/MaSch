using System;
using System.Collections.Generic;
using System.Text;

namespace MaSch.Console.Cli.Help
{
    public class CliError
    {
        public CliErrorType Type { get; set; }
        public CliCommandInfo? AffectedCommand { get; set; }
        public CliCommandOptionInfo? AffectedOption { get; set; }
    }
}
