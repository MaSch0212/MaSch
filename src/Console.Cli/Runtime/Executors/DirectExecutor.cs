﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaSch.Console.Cli.Internal;
using MaSch.Core;

namespace MaSch.Console.Cli.Runtime.Executors
{
    internal class DirectExecutor : ICliExecutor
    {
        public DirectExecutor(Type commandType)
        {
            Guard.NotNull(commandType, nameof(commandType));

            if (!typeof(ICliCommandExecutor).IsAssignableFrom(commandType) && !typeof(ICliAsyncCommandExecutor).IsAssignableFrom(commandType))
                throw new ArgumentException($"The type {commandType.Name} needs to implement {typeof(ICliCommandExecutor).Name} and/or {typeof(ICliAsyncCommandExecutor).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.", nameof(commandType));
        }

        public int Execute(object obj)
        {
            if (obj is ICliCommandExecutor executor)
                return executor.ExecuteCommand();
            else if (obj is ICliAsyncCommandExecutor asyncExecutor)
                return asyncExecutor.ExecuteCommandAsync().GetAwaiter().GetResult();
            else
                throw new InvalidOperationException($"The type {obj.GetType().Name} needs to implement {typeof(ICliCommandExecutor).Name} and/or {typeof(ICliAsyncCommandExecutor).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.");
        }

        public async Task<int> ExecuteAsync(object obj)
        {
            if (obj is ICliAsyncCommandExecutor asyncExecutor)
                return await asyncExecutor.ExecuteCommandAsync();
            else if (obj is ICliCommandExecutor executor)
                return executor.ExecuteCommand();
            else
                throw new InvalidOperationException($"The type {obj.GetType().Name} needs to implement {typeof(ICliCommandExecutor).Name} and/or {typeof(ICliAsyncCommandExecutor).Name}. If this command should not be executable, set the Executable Property on the CliCommandAttribute to false.");
        }

        public bool ValidateOptions(ICliCommandInfo command, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
        {
            // Nothing to validate here. The options are already validated in the CliApplicationArgumentParser.
            errors = null;
            return true;
        }
    }
}
