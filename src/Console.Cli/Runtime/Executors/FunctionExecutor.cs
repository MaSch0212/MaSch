using MaSch.Console.Cli.Internal;
using MaSch.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime.Executors
{
    internal static class FunctionExecutor
    {
        public static ICliExecutor GetExecutor(object executorFunction)
        {
            Guard.NotNull(executorFunction, nameof(executorFunction));
            var type = executorFunction.GetType();
            if (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(Func<,>))
                throw new ArgumentException($"The executor function needs to be of type Func<,>.", nameof(executorFunction));
            var genArgs = type.GetGenericArguments();
            var eType = typeof(FunctionExecutor<>).MakeGenericType(genArgs[0]);
            return genArgs[1] switch
            {
                Type x when x == typeof(int) => (ICliExecutor)Activator.CreateInstance(eType, executorFunction, null)!,
                Type x when x == typeof(Task<int>) => (ICliExecutor)Activator.CreateInstance(eType, null, executorFunction)!,
                _ => throw new ArgumentException($"The executor function needs to either return int or Task<int>."),
            };
        }

        public static T PreExecute<T>(object obj)
        {
            if (obj is not T tObj)
                throw new ArgumentException($"The object needs to be an instance of class {typeof(T).Name}. (Actual: {obj.GetType()})", nameof(obj));
            return tObj;
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic counterpart to FunctionExecutor")]
    internal class FunctionExecutor<T> : ICliExecutor
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

        public int Execute(ICliCommandInfo command, object obj)
        {
            Guard.NotNull(obj, nameof(obj));
            var tObj = FunctionExecutor.PreExecute<T>(obj);
            if (_executorFunc != null)
                return _executorFunc(tObj);
            else if (_asyncExecutorFunc != null)
                return _asyncExecutorFunc(tObj).GetAwaiter().GetResult();
            else
                throw new InvalidOperationException("At least one function needs to be provided.");
        }

        public async Task<int> ExecuteAsync(ICliCommandInfo command, object obj)
        {
            Guard.NotNull(obj, nameof(obj));
            var tObj = FunctionExecutor.PreExecute<T>(obj);
            if (_asyncExecutorFunc != null)
                return await _asyncExecutorFunc(tObj);
            else if (_executorFunc != null)
                return _executorFunc(tObj);
            else
                throw new InvalidOperationException("At least one function needs to be provided.");
        }

        [ExcludeFromCodeCoverage]
        public bool ValidateOptions(ICliCommandInfo command, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
        {
            // Nothing to validate here. The options are already validated in the CliApplicationArgumentParser.
            errors = null;
            return true;
        }
    }
}
