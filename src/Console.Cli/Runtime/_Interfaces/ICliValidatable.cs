using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a validatable command line interface command.
    /// </summary>
    public interface ICliValidatable
    {
        /// <summary>
        /// Validates the current instance of the command.
        /// </summary>
        /// <param name="errors">The errors that the validation detected.</param>
        /// <returns><c>true</c> when validation succeeded; otherwise <c>false</c>.</returns>
        bool ValidateOptions([MaybeNullWhen(true)] out IEnumerable<CliError> errors);
    }
}
