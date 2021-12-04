using MaSch.Console.Cli.Configuration;

namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents an options of a command line interface command.
/// </summary>
public interface ICliCommandOptionInfo : ICliCommandMemberInfo
{
    /// <summary>
    /// Gets the option code attribute.
    /// </summary>
    CliCommandOptionAttribute Attribute { get; }

    /// <summary>
    /// Gets the short aliases of this option.
    /// </summary>
    IReadOnlyList<char> ShortAliases { get; }

    /// <summary>
    /// Gets the aliases of this option.
    /// </summary>
    IReadOnlyList<string> Aliases { get; }

    /// <summary>
    /// Gets a number that is used for sorting when displaying the command list on the help page.
    /// </summary>
    int HelpOrder { get; }
}
