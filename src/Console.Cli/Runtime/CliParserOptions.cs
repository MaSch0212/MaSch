namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Options for the <see cref="ICliArgumentParser"/>.
/// </summary>
public class CliParserOptions : ICliParserOptions
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    public virtual string? Version { get; set; }

    /// <summary>
    /// Gets or sets the year in which the version has been released.
    /// </summary>
    public virtual string? Year { get; set; }

    /// <summary>
    /// Gets or sets the author.
    /// </summary>
    public virtual string? Author { get; set; }

    /// <summary>
    /// Gets or sets the name of the command or executable file.
    /// </summary>
    public virtual string? CliName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether unknown options should be ignored while parsing command line arguments.
    /// </summary>
    public virtual bool? IgnoreUnknownOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether additional values should be ignored while parsing command line arguments.
    /// </summary>
    public virtual bool? IgnoreAdditionalValues { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the "help" command should be automatically provided.
    /// </summary>
    public virtual bool? ProvideHelpCommand { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the "version" command should be automatically provided.
    /// </summary>
    public virtual bool? ProvideVersionCommand { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the "help" option should be automatically provided to all commands.
    /// </summary>
    public virtual bool? ProvideHelpOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the "version" option should be automatically provided to all commands.
    /// </summary>
    public virtual bool? ProvideVersionOptions { get; set; }
}
