using MaSch.Console.Cli.Internal;
using MaSch.Core;

namespace MaSch.Console.Cli.Runtime.Executors;

internal static class FunctionExecutor
{
    public static ICliCommandExecutor GetExecutor(object executorFunction)
    {
        _ = Guard.NotNull(executorFunction);
        var type = executorFunction.GetType();
        if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Func<,,>))
            throw new ArgumentException($"The executor function needs to be of type Func<,,>.", nameof(executorFunction));
        var genArgs = type.GetGenericArguments();
        if (!typeof(CliExecutionContext).IsAssignableFrom(genArgs[0]))
            throw new ArgumentException($"The first function arguments need to be of type CliExecutionContext.");
        var executorType = typeof(FunctionExecutor<>).MakeGenericType(genArgs[1]);
        return genArgs[2] switch
        {
            Type x when x == typeof(int) => (ICliCommandExecutor)Activator.CreateInstance(executorType, executorFunction, null)!,
            Type x when x == typeof(Task<int>) => (ICliCommandExecutor)Activator.CreateInstance(executorType, null, executorFunction)!,
            _ => throw new ArgumentException($"The executor function needs to either return int or Task<int>."),
        };
    }

    public static T PreExecute<T>(object obj)
    {
        if (obj is not T castedObject)
            throw new ArgumentException($"The object needs to be an instance of class {typeof(T).Name}. (Actual: {obj.GetType()})", nameof(obj));
        return castedObject;
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic counterpart to FunctionExecutor")]
internal class FunctionExecutor<T> : ICliCommandExecutor
{
    private readonly Func<CliExecutionContext, T, int>? _executorFunc;
    private readonly Func<CliExecutionContext, T, Task<int>>? _asyncExecutorFunc;

    public FunctionExecutor(Func<CliExecutionContext, T, int>? executorFunc, Func<CliExecutionContext, T, Task<int>>? asyncExecutorFunc)
    {
        if (executorFunc == null && asyncExecutorFunc == null)
            throw new ArgumentException("At least one function needs to be provided.");
        _executorFunc = executorFunc;
        _asyncExecutorFunc = asyncExecutorFunc;
    }

    public int Execute(CliExecutionContext context, object obj)
    {
        _ = Guard.NotNull(context);
        _ = Guard.NotNull(obj);
        var castedObject = FunctionExecutor.PreExecute<T>(obj);
        if (_executorFunc != null)
            return _executorFunc(context, castedObject);
        else if (_asyncExecutorFunc != null)
            return _asyncExecutorFunc(context, castedObject).GetAwaiter().GetResult();
        else
            throw new InvalidOperationException("At least one function needs to be provided.");
    }

    public async Task<int> ExecuteAsync(CliExecutionContext context, object obj)
    {
        _ = Guard.NotNull(context);
        _ = Guard.NotNull(obj);
        var castedObject = FunctionExecutor.PreExecute<T>(obj);
        if (_asyncExecutorFunc != null)
            return await _asyncExecutorFunc(context, castedObject);
        else if (_executorFunc != null)
            return _executorFunc(context, castedObject);
        else
            throw new InvalidOperationException("At least one function needs to be provided.");
    }

    [ExcludeFromCodeCoverage]
    public bool ValidateOptions(CliExecutionContext context, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
    {
        // Nothing to validate here. The options are already validated in the CliApplicationArgumentParser.
        errors = null;
        return true;
    }
}
