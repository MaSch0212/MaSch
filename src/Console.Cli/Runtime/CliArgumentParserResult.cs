namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents the result of an <see cref="ICliArgumentParser"/>.
/// </summary>
public class CliArgumentParserResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliArgumentParserResult"/> class.
    /// </summary>
    /// <param name="errors">The errors that where detected while parsing.</param>
    /// <param name="executionContext">The execution context that can be used to execute the parsed command.</param>
    /// <param name="options">The command instance that has been created during the parse.</param>
    internal protected CliArgumentParserResult(IEnumerable<CliError> errors, CliExecutionContext? executionContext, object? options)
    {
        Success = false;
        Errors = errors;
        ExecutionContext = executionContext;
        Options = options;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliArgumentParserResult"/> class.
    /// </summary>
    /// <param name="errors">The errors that where detected while parsing.</param>
    internal protected CliArgumentParserResult(IEnumerable<CliError> errors)
        : this(errors, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CliArgumentParserResult"/> class.
    /// </summary>
    /// <param name="executionContext">The execution context that can be used to execute the parsed command.</param>
    /// <param name="options">The command instance that has been created during the parse.</param>
    internal protected CliArgumentParserResult(CliExecutionContext executionContext, object options)
    {
        Success = true;
        Errors = Array.Empty<CliError>();
        ExecutionContext = executionContext;
        Options = options;
    }

    /// <summary>
    /// Gets a value indicating whether the parse completed successfully.
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets the errors that where detected while parsing.
    /// </summary>
    public IEnumerable<CliError> Errors { get; }

    /// <summary>
    /// Gets the execution context that can be used to execute the parsed command.
    /// </summary>
    public CliExecutionContext? ExecutionContext { get; }

    /// <summary>
    /// Gets the command instance that has been created during the parse.
    /// </summary>
    public object? Options { get; }
}
