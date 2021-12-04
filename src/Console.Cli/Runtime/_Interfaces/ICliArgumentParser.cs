namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents a parser that parses command line arguments.
/// </summary>
public interface ICliArgumentParser
{
    /// <summary>
    /// Parses the given command line arguments using the options and comands of the given application.
    /// </summary>
    /// <param name="args">The command line arguments to parse.</param>
    /// <returns>A <see cref="CliArgumentParserResult"/> object containing the result of the parse.</returns>
    CliArgumentParserResult Parse(string[] args);
}
