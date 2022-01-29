using MaSch.Core;

namespace MaSch.Console.Cli.Configuration;

/// <summary>
/// Marks a property as a value for the containing command.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class CliCommandValueAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliCommandValueAttribute"/> class.
    /// </summary>
    /// <param name="order">The order of the value. This is used when multiple values are provided for the command in which order they should appear in the command line arguments.</param>
    /// <param name="displayName">The display name of this value. This is used when displaying the value list on the help page.</param>
    public CliCommandValueAttribute(int order, string displayName)
    {
        _ = Guard.NotNullOrEmpty(displayName);

        Order = order;
        DisplayName = displayName;
    }

    /// <summary>
    /// Gets the display name of this value. This is used when displaying the value list on the help page.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Gets the order of the value. This is used when multiple values are provided for the command in which order they should appear in the command line arguments.
    /// </summary>
    public int Order { get; }

    /// <summary>
    /// Gets or sets the default value of this value. Default values can also be achived by setting the desired value in the constructor of the containing class or by initialiting the property directly.
    /// </summary>
    public object? Default { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this value is required.
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets a help text that is displayed in the value list on the help page.
    /// </summary>
    public string? HelpText { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this value should be hidden from the help page.
    /// </summary>
    public bool Hidden { get; set; }
}
