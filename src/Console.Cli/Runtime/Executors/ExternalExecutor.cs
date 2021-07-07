using MaSch.Console.Cli.Internal;
using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime.Executors
{
    internal static class ExternalExecutor
    {
        public static ICliCommandExecutor GetExecutor(Type executorType, Type commandType, object? executorInstance)
        {
            Guard.NotNull(executorType, nameof(executorType));
            Guard.NotNull(commandType, nameof(commandType));

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

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic counterpart to ExternalExecutor.")]
    internal class ExternalExecutor<T> : ICliCommandExecutor
    {
        private readonly Type _executorType;
        private object? _executorInstance;

        internal object? LastExecutorInstance { get; private set; }

        public ExternalExecutor(Type executorType, object? executorInstance)
        {
            _executorType = Guard.NotNull(executorType, nameof(executorType));
            _executorInstance = Guard.OfType(executorInstance, nameof(executorInstance), true, executorType);

            if (!typeof(ICliExecutor<T>).IsAssignableFrom(executorType) && !typeof(ICliAsyncExecutor<T>).IsAssignableFrom(executorType))
                throw new ArgumentException($"The type {executorType.Name} needs to implement {typeof(ICliExecutor<T>).Name} and/or {typeof(ICliAsyncExecutor<T>).Name} for type {typeof(T).Name}.", nameof(executorType));
        }

        public int Execute(CliExecutionContext context, object obj)
        {
            Guard.NotNull(context, nameof(context));
            Guard.NotNull(obj, nameof(obj));
            var (ee, tObj) = PreExecute(context.ServiceProvider, obj);
            LastExecutorInstance = ee;

            if (ee is ICliExecutor<T> executor)
                return executor.ExecuteCommand(context, tObj);
            else if (ee is ICliAsyncExecutor<T> asyncExecutor)
                return asyncExecutor.ExecuteCommandAsync(context, tObj).GetAwaiter().GetResult();
            else
                throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliExecutor<T>).Name} and/or {typeof(ICliAsyncExecutor<T>).Name} for type {typeof(T).Name}.");
        }

        public async Task<int> ExecuteAsync(CliExecutionContext context, object obj)
        {
            Guard.NotNull(context, nameof(context));
            Guard.NotNull(obj, nameof(obj));
            var (ee, tObj) = PreExecute(context.ServiceProvider, obj);
            LastExecutorInstance = ee;

            if (ee is ICliAsyncExecutor<T> asyncExecutor)
                return await asyncExecutor.ExecuteCommandAsync(context, tObj);
            else if (ee is ICliExecutor<T> executor)
                return executor.ExecuteCommand(context, tObj);
            else
                throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliExecutor<T>).Name} and/or {typeof(ICliAsyncExecutor<T>).Name} for type {typeof(T).Name}.");
        }

        public bool ValidateOptions(CliExecutionContext context, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
        {
            Guard.NotNull(context, nameof(context));
            Guard.OfType(parameters, nameof(parameters), false, typeof(T));
            var ee = PreValidate(context.ServiceProvider);
            LastExecutorInstance = ee;

            var types = (from i in ee.GetType().GetInterfaces()
                         where i.IsGenericType
                         let baseType = i.GetGenericTypeDefinition()
                         where baseType == typeof(ICliValidator<>)
                         select i.GetGenericArguments()[0]).ToArray();
            var pType = parameters.GetType();
            var type = types.Contains(pType)
                ? pType
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

        public (object Executor, T TObj) PreExecute(IServiceProvider serviceProvider, object obj)
        {
            if (obj is not T tObj)
                throw new ArgumentException($"The object needs to be an instance of class {typeof(T).Name}. (Actual: {obj.GetType().Name})", nameof(obj));
            var executor = PreValidate(serviceProvider);
            return (executor, tObj);
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
}
