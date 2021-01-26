﻿using MaSch.Common.Extensions;
using System;
using System.Linq;

namespace MaSch.Console.Cli.Configuration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class CliCommandAttribute : Attribute
    {
        public string Name => Aliases[0];
        public string[] Aliases { get; }
        public bool IsDefault { get; set; }
        public int HelpOrder { get; set; } = -1;
        public string? HelpText { get; set; }
        public Type? ParentCommand { get; set; }
        public bool Executable { get; set; } = true;

        public CliCommandAttribute(string name)
        {
            Aliases = new[] { name };
        }

        public CliCommandAttribute(string name, params string[] aliases)
        {
            Aliases = aliases.Prepend(name).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        }
    }
}
