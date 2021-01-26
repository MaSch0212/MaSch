using MaSch.Core;
using MaSch.Core.Extensions;
using MaSch.Console.Cli.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Internal
{
    internal static class ExternalExecutor
    {
        public static IExecutor GetExecutor(Type executorType, Type commandType)
        {
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
            return (IExecutor)Activator.CreateInstance(typeof(ExternalExecutor<>).MakeGenericType(type), executorType)!;
        }

        public static (object executor, T tObj) PreExecute<T>(Type executorType, object obj)
        {
            if (!(obj is T tObj))
                throw new ArgumentException($"The object needs to be an instance of class {typeof(T).Name}. (Actual: {obj?.GetType().Name ?? "(null)"})", nameof(obj));
            var executor = Activator.CreateInstance(executorType)
                ?? throw new ArgumentException($"And instance of type {executorType.Name} could not be created. Please make sure the class has an empty constructor.", nameof(executorType));
            return (executor, tObj);
        }
    }

    internal class ExternalExecutor<T> : IExecutor
    {
        private readonly Type _executorType;

        public ExternalExecutor(Type executorType)
        {
            _executorType = Guard.NotNull(executorType, nameof(executorType));

            if (!typeof(ICliCommandExecutor<T>).IsAssignableFrom(executorType) && !typeof(ICliAsyncCommandExecutor<T>).IsAssignableFrom(executorType))
                throw new ArgumentException($"The type {executorType.Name} needs to implement {typeof(ICliCommandExecutor<T>).Name} and/or {typeof(ICliAsyncCommandExecutor<T>).Name} for type {typeof(T).Name}.", nameof(executorType));
        }

        public int Execute(object obj)
        {
            var (ee, tObj) = ExternalExecutor.PreExecute<T>(_executorType, obj);
            if (ee is ICliCommandExecutor<T> executor)
                return executor.ExecuteCommand(tObj);
            else if (ee is ICliAsyncCommandExecutor<T> asyncExecutor)
                return asyncExecutor.ExecuteCommandAsync(tObj).GetAwaiter().GetResult();
            else
                throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliCommandExecutor<T>).Name} and/or {typeof(ICliAsyncCommandExecutor<T>).Name} for type {typeof(T).Name}.");
        }

        public async Task<int> ExecuteAsync(object obj)
        {
            var (ee, tObj) = ExternalExecutor.PreExecute<T>(_executorType, obj);
            if (ee is ICliAsyncCommandExecutor<T> asyncExecutor)
                return await asyncExecutor.ExecuteCommandAsync(tObj);
            else if (ee is ICliCommandExecutor<T> executor)
                return executor.ExecuteCommand(tObj);
            else
                throw new InvalidOperationException($"The type {_executorType.Name} needs to implement {typeof(ICliCommandExecutor<T>).Name} and/or {typeof(ICliAsyncCommandExecutor<T>).Name} for type {typeof(T).Name}.");
        }
    }
}
