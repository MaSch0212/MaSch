using MaSch.Console.Cli.Configuration;
using MaSch.Core;
using System.Collections.Generic;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    /// <inheritdoc/>
    public class CliCommandOptionInfo : CliCommandMemberInfo, ICliCommandOptionInfo
    {
        /// <inheritdoc/>
        public CliCommandOptionAttribute Attribute { get; }

        /// <inheritdoc/>
        public override object? DefaultValue => Attribute.Default;

        /// <inheritdoc/>
        public override bool IsRequired => Attribute.Required;

        /// <inheritdoc/>
        public override string? HelpText => Attribute.HelpText;

        /// <inheritdoc/>
        public IReadOnlyList<char> ShortAliases => Attribute.ShortAliases;

        /// <inheritdoc/>
        public IReadOnlyList<string> Aliases => Attribute.Aliases;

        /// <inheritdoc/>
        public int HelpOrder => Attribute.HelpOrder;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandOptionInfo"/> class.
        /// </summary>
        /// <param name="command">The command this member belongs to.</param>
        /// <param name="property">The property this member represents.</param>
        /// <param name="attribute">The option code attribute.</param>
        public CliCommandOptionInfo(ICliCommandInfo command, PropertyInfo property, CliCommandOptionAttribute attribute)
            : base(command, property)
        {
            Attribute = Guard.NotNull(attribute, nameof(attribute));
        }
    }
}
