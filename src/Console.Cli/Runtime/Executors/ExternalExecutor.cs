using MaSch.Console.Cli.Internal;
using MaSch.Core;
using MaSch.Core.Extensions;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Console.Cli.Runtime.Executors;

internal static class ExternalExecutor
{
    public static ICliCommandExecutor GetExecutor(Type executorType, Type commandType, object? executorInstance)
    {
        _ = Guard.NotNull(executorType);
        _ = Guard.NotNull(commandType);

        var types = (from i in executorType.GetInterfaces()
                     where i.IsGenericType
                     let baseType = i.GetGenericTypeDefinition()
                     where baseType.In(typeof(ICliExecutor<>), typeof(ICliAsyncExecutor<>))
                     select i.GetGenericArguments()[0]).ToArray();
        var type = types.Contains(commandType)
            ? commandType
            : types.FirstOrDefault(x => x.IsAssignableFrom(commandType));
        if (type == null)
            throw new ArgumentException($"The type {executorType.Name} needs to implement {typeof(ICliExecutor<>).Name} and/or {typeof(ICliAsyncExecutor<>).Name} for type {commandType.Name}.", nameof(executorType));
        return (ICliCommandExecutor)Activator.CreateInstance(typeof(ExternalExecutor<>).MakeGenericType(type), executorType, executorInstance)!;
    }
}

internal class ExternalExecutor<T> : ICliCommandExecutor
{
    private readonly Type _executorType;
    private object? _executorInstance;

    public ExternalExecutor(Type executorType, object? executorInstance)
    {
        _executorType = Guard.NotNull(executorType);
        _executorInstance = Guard.OfType(executorInstance, executorType, allowNull: true);

        if (!typeof(ICliExecutor<T>).IsAssignableFrom(executorType) && !typeof(ICliAsyncExecutor<T>).IsAssignableFrom(executorType))
            throw new ArgumentException($"The type {executorType.Name} needs to implement {typeof(ICliExecutor<T>).Name} and/or {typeof(ICliAsyncExecutor<T>).Name} for type {typeof(T).Name}.", nameof(executorType));
    }

    internal object? LastExecutorInstance { get; private set; }

    public int Execute(CliExecutionContext context, object obj)
    {
        _ = Guard.NotNull(context);
        _ = Guard.NotNull(obj);
        var (executor, castedObject) = PreExecute(context.ServiceProvider, obj);
        LastExecutorInstance = executor;

        if (executor is ICliExecutor<T> syncExecutor)
            return syncExecutor.ExecuteCommand(context, castedObject);
        else if (executor is ICliAsyncExecutor<T> asyncExecutor)
            return asyncExecutor.ExecuteCommandAsync(context, castedObject).GetAwaiter().GetResult();
        else
            throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliExecutor<T>).Name} and/or {typeof(ICliAsyncExecutor<T>).Name} for type {typeof(T).Name}.");
    }

    public async Task<int> ExecuteAsync(CliExecutionContext context, object obj)
    {
        _ = Guard.NotNull(context);
        _ = Guard.NotNull(obj);
        var (executor, castedObject) = PreExecute(context.ServiceProvider, obj);
        LastExecutorInstance = executor;

        if (executor is ICliAsyncExecutor<T> asyncExecutor)
            return await asyncExecutor.ExecuteCommandAsync(context, castedObject);
        else if (executor is ICliExecutor<T> syncExecutor)
            return syncExecutor.ExecuteCommand(context, castedObject);
        else
            throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliExecutor<T>).Name} and/or {typeof(ICliAsyncExecutor<T>).Name} for type {typeof(T).Name}.");
    }

    public bool ValidateOptions(CliExecutionContext context, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
    {
        _ = Guard.NotNull(context);
        _ = Guard.OfType<T>(parameters);
        var ee = PreValidate(context.ServiceProvider);
        LastExecutorInstance = ee;

        var types = (from i in ee.GetType().GetInterfaces()
                     where i.IsGenericType
                     let baseType = i.GetGenericTypeDefinition()
                     where baseType == typeof(ICliValidator<>)
                     select i.GetGenericArguments()[0]).ToArray();
        var parametersType = parameters.GetType();
        var type = types.Contains(parametersType)
            ? parametersType
            : types.FirstOrDefault(x => x.IsInstanceOfType(parameters));
        if (type != null)
        {
            var validateMethod = typeof(ICliValidator<>).MakeGenericType(type).GetMethod(nameof(ICliValidator<object>.ValidateOptions))!;
            var p = new object?[] { context, parameters, null };
            var r = (bool)validateMethod.Invoke(ee, p)!;
            errors = p[2] as IEnumerable<CliError>;
            return r;
        }

        errors = null;
        return true;
    }

    public (object Executor, T CastedObject) PreExecute(IServiceProvider serviceProvider, object obj)
    {
        if (obj is not T castedObject)
            throw new ArgumentException($"The object needs to be an instance of class {typeof(T).Name}. (Actual: {obj.GetType().Name})", nameof(obj));
        var executor = PreValidate(serviceProvider);
        return (executor, castedObject);
    }

    public object PreValidate(IServiceProvider serviceProvider)
    {
        var executor = _executorInstance
            ?? serviceProvider.GetService(_executorType);

        if (executor is null)
            executor = _executorInstance = Activator.CreateInstance(_executorType)!; // CreateInstance only returns null for Nullable<> (see https://github.com/dotnet/coreclr/pull/23774#discussion_r354785674)
        return executor;
    }
}
