namespace MaSch.Console.Cli.Configuration;

/// <summary>
/// Adds metadata to a class or interface that has the <see cref="CliCommandAttribute"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class CliMetadataAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the display name that is shown in the help page.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the version of this command that is shown in the help page.
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Gets or sets the author of this command that is shown in the help page.
    /// </summary>
    public string? Author { get; set; }

    /// <summary>
    /// Gets or sets the year in which this command was last changed. This information is shown in the help page.
    /// </summary>
    public string? Year { get; set; }

    /// <summary>
    /// Gets or sets the name of the command that is shown in the usage of the help page.
    /// </summary>
    public string? CliName { get; set; }
}
