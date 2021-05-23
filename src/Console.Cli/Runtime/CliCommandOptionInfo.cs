using MaSch.Console.Cli.Configuration;
using MaSch.Core;
using System.Collections.Generic;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    public class CliCommandOptionInfo : CliCommandMemberInfo, ICliCommandOptionInfo
    {
        public CliCommandOptionAttribute Attribute { get; }

        public override object? DefaultValue => Attribute.Default;
        public override bool IsRequired => Attribute.Required;
        public override string? HelpText => Attribute.HelpText;
        public IReadOnlyList<char> ShortAliases => Attribute.ShortAliases;
        public IReadOnlyList<string> Aliases => Attribute.Aliases;
        public int HelpOrder => Attribute.HelpOrder;

        public CliCommandOptionInfo(ICliCommandInfo command, PropertyInfo property, CliCommandOptionAttribute attribute)
            : base(command, property)
        {
            Attribute = Guard.NotNull(attribute, nameof(attribute));
        }
    }
}
