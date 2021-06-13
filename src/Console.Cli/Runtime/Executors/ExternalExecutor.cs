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
        public static ICliExecutor GetExecutor(Type executorType, Type commandType, object? executorInstance)
        {
            Guard.NotNull(executorType, nameof(executorType));
            Guard.NotNull(commandType, nameof(commandType));

            var types = (from i in executorType.GetInterfaces()
                         where i.IsGenericType
                         let baseType = i.GetGenericTypeDefinition()
                         where baseType.In(typeof(ICliCommandExecutor<>), typeof(ICliAsyncCommandExecutor<>))
                         select i.GetGenericArguments()[0]).ToArray();
            var type = types.Contains(commandType)
                ? commandType
                : types.FirstOrDefault(x => x.IsAssignableFrom(commandType));
            if (type == null)
                throw new ArgumentException($"The type {executorType.Name} needs to implement {typeof(ICliCommandExecutor<>).Name} and/or {typeof(ICliAsyncCommandExecutor<>).Name} for type {commandType.Name}.", nameof(executorType));
            return (ICliExecutor)Activator.CreateInstance(typeof(ExternalExecutor<>).MakeGenericType(type), executorType, executorInstance)!;
        }

        public static (object Executor, T TObj) PreExecute<T>(Type executorType, object? executorInstance, object obj)
        {
            if (obj is not T tObj)
                throw new ArgumentException($"The object needs to be an instance of class {typeof(T).Name}. (Actual: {obj.GetType().Name})", nameof(obj));
            var executor = PreValidate(executorType, executorInstance);
            return (executor, tObj);
        }

        public static object PreValidate(Type executorType, object? executorInstance)
        {
            var executor = executorInstance
                ?? Activator.CreateInstance(executorType)!; // CreateInstance only returns null for Nullable<> (see https://github.com/dotnet/coreclr/pull/23774#discussion_r354785674)
            return executor;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic counterpart to ExternalExecutor.")]
    internal class ExternalExecutor<T> : ICliExecutor
    {
        private readonly Type _executorType;
        private readonly object? _executorInstance;
        private object? _cachedExecutor;

        internal object? LastExecutorInstance => _cachedExecutor;

        public ExternalExecutor(Type executorType, object? executorInstance)
        {
            _executorType = Guard.NotNull(executorType, nameof(executorType));
            _executorInstance = Guard.OfType(executorInstance, nameof(executorInstance), true, executorType);

            if (!typeof(ICliCommandExecutor<T>).IsAssignableFrom(executorType) && !typeof(ICliAsyncCommandExecutor<T>).IsAssignableFrom(executorType))
                throw new ArgumentException($"The type {executorType.Name} needs to implement {typeof(ICliCommandExecutor<T>).Name} and/or {typeof(ICliAsyncCommandExecutor<T>).Name} for type {typeof(T).Name}.", nameof(executorType));
        }

        public int Execute(ICliCommandInfo command, object obj)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(obj, nameof(obj));
            var (ee, tObj) = ExternalExecutor.PreExecute<T>(_executorType, _executorInstance ?? _cachedExecutor, obj);
            _cachedExecutor = ee;

            if (ee is ICliCommandExecutor<T> executor)
                return executor.ExecuteCommand(command, tObj);
            else if (ee is ICliAsyncCommandExecutor<T> asyncExecutor)
                return asyncExecutor.ExecuteCommandAsync(command, tObj).GetAwaiter().GetResult();
            else
                throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliCommandExecutor<T>).Name} and/or {typeof(ICliAsyncCommandExecutor<T>).Name} for type {typeof(T).Name}.");
        }

        public async Task<int> ExecuteAsync(ICliCommandInfo command, object obj)
        {
            Guard.NotNull(command, nameof(command));
            Guard.NotNull(obj, nameof(obj));
            var (ee, tObj) = ExternalExecutor.PreExecute<T>(_executorType, _executorInstance ?? _cachedExecutor, obj);
            _cachedExecutor = ee;

            if (ee is ICliAsyncCommandExecutor<T> asyncExecutor)
                return await asyncExecutor.ExecuteCommandAsync(command, tObj);
            else if (ee is ICliCommandExecutor<T> executor)
                return executor.ExecuteCommand(command, tObj);
            else
                throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliCommandExecutor<T>).Name} and/or {typeof(ICliAsyncCommandExecutor<T>).Name} for type {typeof(T).Name}.");
        }

        public bool ValidateOptions(ICliCommandInfo command, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
        {
            Guard.NotNull(command, nameof(command));
            Guard.OfType(parameters, nameof(parameters), false, typeof(T));
            var ee = ExternalExecutor.PreValidate(_executorType, _executorInstance ?? _cachedExecutor);
            _cachedExecutor = ee;

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
                var p = new object?[] { command, parameters, null };
                var r = (bool)validateMethod.Invoke(ee, p)!;
                errors = p[2] as IEnumerable<CliError>;
                return r;
            }

            errors = null;
            return true;
        }
    }
}
