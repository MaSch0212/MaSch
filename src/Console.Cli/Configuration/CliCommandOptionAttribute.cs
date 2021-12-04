using MaSch.Core;

namespace MaSch.Console.Cli.Configuration;

/// <summary>
/// Marks a property as an option for the containing command.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class CliCommandOptionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandOptionAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of this option.</param>
    public CliCommandOptionAttribute(string name)
        : this(null, new[] { Guard.NotNullOrEmpty(name, nameof(name)) })
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandOptionAttribute"/> class.
    /// </summary>
    /// <param name="shortName">The short name of this option.</param>
    /// <param name="name">The name of this option.</param>
    public CliCommandOptionAttribute(char shortName, string name)
        : this(new[] { shortName }, new[] { Guard.NotNullOrEmpty(name, nameof(name)) })
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandOptionAttribute"/> class.
    /// </summary>
    /// <param name="names">The names of this option. Can be either of type <see cref="string"/> to provide names or <see cref="char"/> to provde short names.</param>
    public CliCommandOptionAttribute(params object[] names)
        : this(
              Guard.NotNull(names, nameof(names)).Where(x => x != null).OfType<char>().Distinct().ToArray(),
              names.Where(x => x != null).OfType<string>().Where(x => !string.IsNullOrEmpty(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray())
    {
    }

    private CliCommandOptionAttribute(char[]? shortAliases, string[] aliases)
    {
        _ = Guard.NotNullOrEmpty(aliases, nameof(aliases));

        ShortAliases = shortAliases ?? Array.Empty<char>();
        Aliases = aliases;
    }

    /// <summary>
    /// Gets the short name of this option.
    /// </summary>
    public char? ShortName => ShortAliases.Length > 0 ? ShortAliases[0] : null;

    /// <summary>
    /// Gets the short aliases of this option.
    /// </summary>
    public char[] ShortAliases { get; }

    /// <summary>
    /// Gets the name of this option.
    /// </summary>
    public string Name => Aliases[0];

    /// <summary>
    /// Gets the aliases of this option.
    /// </summary>
    public string[] Aliases { get; }

    /// <summary>
    /// Gets or sets the default value of this option. Default values can also be achived by setting the desired value in the constructor of the containing class or by initialiting the property directly.
    /// </summary>
    public object? Default { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this option is required.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a number that is used for sorting when displaying the option list on the help page.
    /// </summary>
    public int HelpOrder { get; set; } = -1;

    /// <summary>
    /// Gets or sets a help text that is displayed in the option list on the help page.
    /// </summary>
    public string? HelpText { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this option should be hidden from the help page.
    /// </summary>
    public bool Hidden { get; set; }
}
