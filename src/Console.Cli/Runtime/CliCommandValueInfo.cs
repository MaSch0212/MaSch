using MaSch.Console.Cli.Configuration;
using MaSch.Core;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    public class CliCommandValueInfo : CliCommandMemberInfo, ICliCommandValueInfo
    {
        public CliCommandValueAttribute Attribute { get; }

        public override object? DefaultValue => Attribute.Default;
        public override bool IsRequired => Attribute.Required;
        public override string? HelpText => Attribute.HelpText;
        public string DisplayName => Attribute.DisplayName;
        public int Order => Attribute.Order;

        public CliCommandValueInfo(ICliCommandInfo command, PropertyInfo property, CliCommandValueAttribute attribute)
            : base(command, property)
        {
            Attribute = Guard.NotNull(attribute, nameof(attribute));
        }
    }
}
