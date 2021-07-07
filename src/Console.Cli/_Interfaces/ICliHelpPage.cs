using System.Collections.Generic;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Represents a help page handler that is used by an application to display help information.
    /// </summary>
    public interface ICliHelpPage
    {
        /// <summary>
        /// Writes the appropriate help page.
        /// </summary>
        /// <param name="errors">The errors that occured.</param>
        /// <returns><c>true</c> when the help page or version information has been requested; otherwise <c>false</c>.</returns>
        bool Write(IEnumerable<CliError>? errors);
    }
}
