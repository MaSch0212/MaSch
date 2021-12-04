using MaSch.Console.Cli.Configuration;
using MaSch.Core;

namespace MaSch.Console.Cli.Runtime;

/// <inheritdoc/>
public class CliCommandOptionInfo : CliCommandMemberInfo, ICliCommandOptionInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandOptionInfo"/> class.
    /// </summary>
    /// <param name="extensionStorage">The extension data storage.</param>
    /// <param name="command">The command this member belongs to.</param>
    /// <param name="property">The property this member represents.</param>
    /// <param name="attribute">The option code attribute.</param>
    public CliCommandOptionInfo(ObjectExtensionDataStorage extensionStorage, ICliCommandInfo command, PropertyInfo property, CliCommandOptionAttribute attribute)
        : base(extensionStorage, command, property)
    {
        Attribute = Guard.NotNull(attribute, nameof(attribute));
    }

    /// <inheritdoc/>
    public CliCommandOptionAttribute Attribute { get; }

    /// <inheritdoc/>
    public override object? DefaultValue => Attribute.Default;

    /// <inheritdoc/>
    public override bool IsRequired => Attribute.Required;

    /// <inheritdoc/>
    public override string? HelpText => Attribute.HelpText;

    /// <inheritdoc/>
    public override bool Hidden => Attribute.Hidden;

    /// <inheritdoc/>
    public IReadOnlyList<char> ShortAliases => Attribute.ShortAliases;

    /// <inheritdoc/>
    public IReadOnlyList<string> Aliases => Attribute.Aliases;

    /// <inheritdoc/>
    public int HelpOrder => Attribute.HelpOrder;
}
