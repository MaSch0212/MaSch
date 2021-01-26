using MaSch.Core;
using MaSch.Console.Cli.Configuration;
using System;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Internal
{
    internal class DirectExecutor : IExecutor
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
    }
}
