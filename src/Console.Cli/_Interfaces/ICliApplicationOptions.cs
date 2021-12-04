using MaSch.Console.Cli.Runtime;

namespace MaSch.Console.Cli;

/// <summary>
/// Options for an <see cref="ICliApplicationBase"/>.
/// </summary>
public interface ICliApplicationOptions : ICliParserOptions
{
    /// <summary>
    /// Gets the exit code that is returned when parsing of command line arguments fails.
    /// </summary>
    int ParseErrorExitCode { get; }

    /// <summary>
    /// Gets a value indicating whether unknown options should be ignored while parsing command line arguments.
    /// </summary>
    public new bool IgnoreUnknownOptions { get; }

    /// <summary>
    /// Gets a value indicating whether additional values should be ignored while parsing command line arguments.
    /// </summary>
    public new bool IgnoreAdditionalValues { get; }

    /// <summary>
    /// Gets a value indicating whether the "help" command should be automatically provided.
    /// </summary>
    public new bool ProvideHelpCommand { get; }

    /// <summary>
    /// Gets a value indicating whether the "version" command should be automatically provided.
    /// </summary>
    public new bool ProvideVersionCommand { get; }

    /// <summary>
    /// Gets a value indicating whether the "help" option should be automatically provided to all commands.
    /// </summary>
    public new bool ProvideHelpOptions { get; }

    /// <summary>
    /// Gets a value indicating whether the "version" option should be automatically provided to all commands.
    /// </summary>
    public new bool ProvideVersionOptions { get; }
}
