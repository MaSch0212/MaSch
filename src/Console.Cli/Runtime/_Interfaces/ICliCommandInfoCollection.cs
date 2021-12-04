namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents a collection of command line interface commands.
/// </summary>
public interface ICliCommandInfoCollection : ICollection<ICliCommandInfo>
{
    /// <summary>
    /// Gets the default command of this collection.
    /// </summary>
    ICliCommandInfo? DefaultCommand { get; }

    /// <summary>
    /// Gets all root commands of this collection.
    /// </summary>
    /// <returns>A list containing the root commands fo this collection.</returns>
    IEnumerable<ICliCommandInfo> GetRootCommands();

    /// <summary>
    /// Creates a read-only representation of this collection.
    /// </summary>
    /// <returns>Read-only representation of this collection.</returns>
    IReadOnlyCliCommandInfoCollection AsReadOnly();
}

/// <summary>
/// Represents a read-only collection of command line interface commands.
/// </summary>
public interface IReadOnlyCliCommandInfoCollection : IReadOnlyCollection<ICliCommandInfo>
{
    /// <summary>
    /// Gets the default command of this collection.
    /// </summary>
    ICliCommandInfo? DefaultCommand { get; }

    /// <summary>
    /// Gets all root commands of this collection.
    /// </summary>
    /// <returns>A list containing the root commands fo this collection.</returns>
    IEnumerable<ICliCommandInfo> GetRootCommands();
}
