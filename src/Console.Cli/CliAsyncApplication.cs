using MaSch.Console.Cli.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace MaSch.Console.Cli;

/// <summary>
/// An asynchronous application that is using a command line interface.
/// </summary>
public class CliAsyncApplication : CliApplicationBase, ICliAsyncApplication
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CliAsyncApplication"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to use.</param>
    /// <param name="options">The options to use.</param>
    /// <param name="commandsCollection">The command collection to use.</param>
    protected internal CliAsyncApplication(IServiceProvider serviceProvider, CliApplicationOptions options, ICliCommandInfoCollection commandsCollection)
        : base(serviceProvider, options, commandsCollection)
    {
    }

    /// <inheritdoc/>
    protected override Type ExecutorType { get; } = typeof(ICliAsyncExecutable);

    /// <inheritdoc/>
    protected override Type GenericExecutorType { get; } = typeof(ICliAsyncExecutor<>);

    /// <inheritdoc/>
    public async Task<int> RunAsync(string[] args)
    {
        using var scope = ServiceProvider.CreateScope();
        if (TryParseArguments(scope.ServiceProvider, args, out var context, out var options, out int errorCode))
            return await context.Command.ExecuteAsync(context, options);
        return errorCode;
    }
}
