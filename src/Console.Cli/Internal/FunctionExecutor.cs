using MaSch.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Internal
{
    internal static class FunctionExecutor
    {
        public static IExecutor GetExecutor(object executorFunction)
        {
            Guard.NotNull(executorFunction, nameof(executorFunction));
            var type = executorFunction.GetType();
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Func<,>))
                throw new ArgumentException($"The executor function needs to be of type Func<,>.", nameof(executorFunction));
            var genArgs = type.GetGenericArguments();
            var eType = typeof(FunctionExecutor).MakeGenericType(genArgs[0]);
            return genArgs[1] switch
            {
                Type x when x == typeof(int) => (IExecutor)Activator.CreateInstance(eType, executorFunction, null)!,
                Type x when x == typeof(Task<int>) => (IExecutor)Activator.CreateInstance(eType, null, executorFunction)!,
                _ => throw new ArgumentException($"The executor function needs to either return int or Task<int>.")
            };
        }

        public static T PreExecute<T>(object obj)
        {
            if (!(obj is T tObj))
                throw new ArgumentException($"The object needs to be an instance of class {typeof(T).Name}. (Actual: {obj?.GetType().Name ?? "(null)"})", nameof(obj));
            return tObj;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic counterpart to FunctionExecutor")]
    internal class FunctionExecutor<T> : IExecutor
    {
        private readonly Func<T, int>? _executorFunc;
        private readonly Func<T, Task<int>>? _asyncExecutorFunc;

        public FunctionExecutor(Func<T, int>? executorFunc, Func<T, Task<int>>? asyncExecutorFunc)
        {
            if (executorFunc == null && asyncExecutorFunc == null)
                throw new ArgumentException("At least one function needs to be provided.");
            _executorFunc = executorFunc;
            _asyncExecutorFunc = asyncExecutorFunc;
        }

        public int Execute(object obj)
        {
            var tObj = FunctionExecutor.PreExecute<T>(obj);
            if (_executorFunc != null)
                return _executorFunc(tObj);
            else if (_asyncExecutorFunc != null)
                return _asyncExecutorFunc(tObj).GetAwaiter().GetResult();
            else
                throw new ArgumentException("At least one function needs to be provided.");
        }

        public async Task<int> ExecuteAsync(object obj)
        {
            var tObj = FunctionExecutor.PreExecute<T>(obj);
            if (_asyncExecutorFunc != null)
                return await _asyncExecutorFunc(tObj);
            else if (_executorFunc != null)
                return _executorFunc(tObj);
            else
                throw new ArgumentException("At least one function needs to be provided.");
        }
    }
}
