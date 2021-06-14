using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents a validator for the specified command types.
    /// </summary>
    /// <typeparam name="TCommand">The commands that can be validated using this validator.</typeparam>
    public interface ICliValidator<TCommand>
    {
        /// <summary>
        /// Validates the given instance of the command.
        /// </summary>
        /// <param name="context">The execution context.</param>
        /// <param name="parameters">The command instance to validate.</param>
        /// <param name="errors">The errors that the validation detected.</param>
        /// <returns><c>true</c> when validation succeeded; otherwise <c>false</c>.</returns>
        bool ValidateOptions(CliExecutionContext context, TCommand parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors);
    }
}
