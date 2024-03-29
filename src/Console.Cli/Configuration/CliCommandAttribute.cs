﻿using MaSch.Core.Extensions;

namespace MaSch.Console.Cli.Configuration;

/// <summary>
/// Marks a class as a command that can be used inside a <see cref="ICliApplicationBase"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class CliCommandAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of this command.</param>
    public CliCommandAttribute(string name)
    {
        Aliases = new[] { name };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of this command.</param>
    /// <param name="aliases">Additional names for this command.</param>
    public CliCommandAttribute(string name, params string[] aliases)
    {
        Aliases = aliases?.Where(x => !string.IsNullOrEmpty(x)).Prepend(name).Distinct(StringComparer.OrdinalIgnoreCase).ToArray() ?? new[] { name };
    }

    /// <summary>
    /// Gets the name of this command.
    /// </summary>
    public string Name => Aliases[0];

    /// <summary>
    /// Gets the aliases of this command.
    /// </summary>
    public string[] Aliases { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this command is the default command in its context.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets a number that is used for sorting when displaying the command list on the help page.
    /// </summary>
    public int HelpOrder { get; set; } = -1;

    /// <summary>
    /// Gets or sets a help text that is displayed in the command list on the help page.
    /// </summary>
    public string? HelpText { get; set; }

    /// <summary>
    /// Gets or sets the parent command. If no parent command is provided the <see cref="ICliApplicationBase"/> that uses this command is the parent.
    /// </summary>
    public Type? ParentCommand { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this command should be hidden from the help page.
    /// </summary>
    public bool Hidden { get; set; }
}
