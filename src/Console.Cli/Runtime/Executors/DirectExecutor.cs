using MaSch.Console.Cli.Internal;
using MaSch.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime.Executors
{
    internal class DirectExecutor : ICliExecutor
    {
        private readonly Type _commandType;

        public DirectExecutor(Type commandType)
        {
            _commandType = Guard.NotNull(commandType, nameof(commandType));

            if (!typeof(ICliCommandExecutor).IsAssignableFrom(commandType) && !typeof(ICliAsyncCommandExecutor).IsAssignableFrom(commandType))
                throw new ArgumentException($"The type {commandType.Name} needs to implement {typeof(ICliCommandExecutor).Name} and/or {typeof(ICliAsyncCommandExecutor).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.", nameof(commandType));
        }

        public int Execute(CliExecutionContext context, object obj)
        {
            Guard.NotNull(context, nameof(context));
            Guard.OfType(obj, nameof(obj), false, _commandType);

            if (obj is ICliCommandExecutor executor)
                return executor.ExecuteCommand(context);
            else if (obj is ICliAsyncCommandExecutor asyncExecutor)
                return asyncExecutor.ExecuteCommandAsync(context).GetAwaiter().GetResult();
            else
                throw new InvalidOperationException($"The type {obj.GetType().Name} needs to implement {typeof(ICliCommandExecutor).Name} and/or {typeof(ICliAsyncCommandExecutor).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.");
        }

        public async Task<int> ExecuteAsync(CliExecutionContext context, object obj)
        {
            Guard.NotNull(context, nameof(context));
            Guard.OfType(obj, nameof(obj), false, _commandType);

            if (obj is ICliAsyncCommandExecutor asyncExecutor)
                return await asyncExecutor.ExecuteCommandAsync(context);
            else if (obj is ICliCommandExecutor executor)
                return executor.ExecuteCommand(context);
            else
                throw new InvalidOperationException($"The type {obj.GetType().Name} needs to implement {typeof(ICliCommandExecutor).Name} and/or {typeof(ICliAsyncCommandExecutor).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.");
        }

        [ExcludeFromCodeCoverage]
        public bool ValidateOptions(CliExecutionContext context, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
        {
            // Nothing to validate here. The options are already validated in the CliApplicationArgumentParser.
            errors = null;
            return true;
        }
    }
}
