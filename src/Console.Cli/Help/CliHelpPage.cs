﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MaSch.Console.Cli.Help
{
    public class CliHelpPage : ICliHelpPage
    {
        public void WriteHelpPage(CliCommandInfo command)
        {
            throw new NotImplementedException();
        }

        public void WriteRootHelpPage(IEnumerable<CliCommandInfo> rootCommands)
        {
            throw new NotImplementedException();
        }
    }
}
