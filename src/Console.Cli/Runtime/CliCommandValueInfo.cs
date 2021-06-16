using MaSch.Console.Cli.Configuration;
using MaSch.Core;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    /// <inheritdoc/>
    public class CliCommandValueInfo : CliCommandMemberInfo, ICliCommandValueInfo
    {
        /// <inheritdoc/>
        public CliCommandValueAttribute Attribute { get; }

        /// <inheritdoc/>
        public override object? DefaultValue => Attribute.Default;

        /// <inheritdoc/>
        public override bool IsRequired => Attribute.Required;

        /// <inheritdoc/>
        public override string? HelpText => Attribute.HelpText;

        /// <inheritdoc/>
        public override bool Hidden => Attribute.Hidden;

        /// <inheritdoc/>
        public string DisplayName => Attribute.DisplayName;

        /// <inheritdoc/>
        public int Order => Attribute.Order;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandValueInfo"/> class.
        /// </summary>
        /// <param name="extensionStorage">The extension data storage.</param>
        /// <param name="command">The command this member belongs to.</param>
        /// <param name="property">The property this member represents.</param>
        /// <param name="attribute">The value code attribute.</param>
        public CliCommandValueInfo(ObjectExtensionDataStorage extensionStorage, ICliCommandInfo command, PropertyInfo property, CliCommandValueAttribute attribute)
            : base(extensionStorage, command, property)
        {
            Attribute = Guard.NotNull(attribute, nameof(attribute));
        }
    }
}
