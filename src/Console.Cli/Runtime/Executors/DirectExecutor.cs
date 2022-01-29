using MaSch.Console.Cli.Internal;
using MaSch.Core;

namespace MaSch.Console.Cli.Runtime.Executors;

internal class DirectExecutor : ICliCommandExecutor
{
    private readonly Type _commandType;

    public DirectExecutor(Type commandType)
    {
        _commandType = Guard.NotNull(commandType);

        if (!IsExecutable(commandType))
            throw new ArgumentException($"The type {commandType.Name} needs to implement {typeof(ICliExecutable).Name} and/or {typeof(ICliAsyncExecutable).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.", nameof(commandType));
    }

    public static bool IsExecutable(Type commandType)
    {
        return typeof(ICliExecutable).IsAssignableFrom(commandType) || typeof(ICliAsyncExecutable).IsAssignableFrom(commandType);
    }

    public int Execute(CliExecutionContext context, object obj)
    {
        _ = Guard.NotNull(context);
        _ = Guard.OfType(obj, _commandType);

        if (obj is ICliExecutable executor)
            return executor.ExecuteCommand(context);
        else if (obj is ICliAsyncExecutable asyncExecutor)
            return asyncExecutor.ExecuteCommandAsync(context).GetAwaiter().GetResult();
        else
            throw new InvalidOperationException($"The type {obj.GetType().Name} needs to implement {typeof(ICliExecutable).Name} and/or {typeof(ICliAsyncExecutable).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.");
    }

    public async Task<int> ExecuteAsync(CliExecutionContext context, object obj)
    {
        _ = Guard.NotNull(context);
        _ = Guard.OfType(obj, _commandType);

        if (obj is ICliAsyncExecutable asyncExecutor)
            return await asyncExecutor.ExecuteCommandAsync(context);
        else if (obj is ICliExecutable executor)
            return executor.ExecuteCommand(context);
        else
            throw new InvalidOperationException($"The type {obj.GetType().Name} needs to implement {typeof(ICliExecutable).Name} and/or {typeof(ICliAsyncExecutable).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.");
    }

    [ExcludeFromCodeCoverage]
    public bool ValidateOptions(CliExecutionContext context, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
    {
        // Nothing to validate here. The options are already validated in the CliApplicationArgumentParser.
        errors = null;
        return true;
    }
}
