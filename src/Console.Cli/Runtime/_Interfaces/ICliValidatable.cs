namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents a validatable command line interface command.
/// </summary>
public interface ICliValidatable
{
    /// <summary>
    /// Validates the current instance of the command.
    /// </summary>
    /// <param name="context">The execution context.</param>
    /// <param name="errors">The errors that the validation detected.</param>
    /// <returns><c>true</c> when validation succeeded; otherwise <c>false</c>.</returns>
    bool ValidateOptions(CliExecutionContext context, [MaybeNullWhen(true)] out IEnumerable<CliError> errors);
}
