namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Default implementation for the <see cref="ICliOptionsProvider"/> interface.
/// </summary>
public class CliOptionsProvider : ICliOptionsProvider, ICliOptionsProvider.ISettable
{
    /// <inheritdoc/>
    public object? Options { get; private set; }

    /// <inheritdoc/>
    public TOptions GetOptions<TOptions>()
        where TOptions : class
    {
        if (Options is null)
            throw new InvalidOperationException("There are currently no options available in this provider. Make sure to inject this provider only on services that are added as scoped services.");

        return (TOptions)Options;
    }

    /// <inheritdoc/>
    public void SetOptions(object? options)
    {
        Options = options;
    }
}
