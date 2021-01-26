using System;
using System.Linq;

namespace MaSch.Console.Cli.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CliCommandOptionAttribute : Attribute
    {
        public char? ShortName => ShortAliases.Length > 0 ? ShortAliases[0] : (char?)null;
        public char[] ShortAliases { get; }
        public string? Name => Aliases.Length > 0 ? Aliases[0] : null;
        public string[] Aliases { get; }
        public object? Default { get; set; }
        public bool Required { get; set; }
        public int HelpOrder { get; set; } = -1;
        public string? HelpText { get; set; }

        private CliCommandOptionAttribute(char[]? shortAliases, string[]? aliases)
        {
            ShortAliases = shortAliases ?? new char[0];
            Aliases = aliases ?? new string[0];
        }

        public CliCommandOptionAttribute(string name)
            : this(null, new[] { name })
        {
        }

        public CliCommandOptionAttribute(char shortName, string name)
            : this(new[] { shortName }, new[] { name })
        {
        }

        public CliCommandOptionAttribute(params object[] names)
            : this(names.Where(x => x != null).OfType<char>().ToArray(), names.Where(x => x != null).OfType<string>().Distinct(StringComparer.OrdinalIgnoreCase).ToArray())
        {
        }
    }
}
