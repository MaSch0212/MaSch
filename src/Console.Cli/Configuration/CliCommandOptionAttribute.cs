using MaSch.Core;
using System;
using System.Linq;

namespace MaSch.Console.Cli.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CliCommandOptionAttribute : Attribute
    {
        public char? ShortName => ShortAliases.Length > 0 ? ShortAliases[0] : null;
        public char[] ShortAliases { get; }
        public string Name => Aliases[0];
        public string[] Aliases { get; }
        public object? Default { get; set; }
        public bool Required { get; set; }
        public int HelpOrder { get; set; } = -1;
        public string? HelpText { get; set; }

        private CliCommandOptionAttribute(char[]? shortAliases, string[] aliases)
        {
            Guard.NotNullOrEmpty(aliases, nameof(aliases));

            ShortAliases = shortAliases ?? Array.Empty<char>();
            Aliases = aliases;
        }

        public CliCommandOptionAttribute(string name)
            : this(null, new[] { Guard.NotNullOrEmpty(name, nameof(name)) })
        {
        }

        public CliCommandOptionAttribute(char shortName, string name)
            : this(new[] { shortName }, new[] { Guard.NotNullOrEmpty(name, nameof(name)) })
        {
        }

        public CliCommandOptionAttribute(params object[] names)
            : this(
                  Guard.NotNull(names, nameof(names)).Where(x => x != null).OfType<char>().Distinct().ToArray(),
                  names.Where(x => x != null).OfType<string>().Where(x => !string.IsNullOrEmpty(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray())
        {
        }
    }
}
