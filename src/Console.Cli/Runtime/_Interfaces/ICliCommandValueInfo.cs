using MaSch.Console.Cli.Configuration;

namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents an value of a command line interface command.
/// </summary>
public interface ICliCommandValueInfo : ICliCommandMemberInfo
{
    /// <summary>
    /// Gets the value code attribute.
    /// </summary>
    CliCommandValueAttribute Attribute { get; }

    /// <summary>
    /// Gets the display name of this value.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Gets the order of the value.
    /// </summary>
    int Order { get; }
}
