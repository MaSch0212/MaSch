using MaSch.Console.Cli.Help;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Configuration
{
    public interface ICliErrorHandlingCommand
    {
        IEnumerable<CliError> HandleErrors(IEnumerable<CliError> errors);
    }

    public interface ICliErrorHandlingCommand<TCommand>
    {
        IEnumerable<CliError> HandleErrors(IEnumerable<CliError> errors, TCommand parameters);
    }

    public interface ICliCommandExecutor
    {
        int ExecuteCommand();
    }

    public interface ICliCommandExecutor<TCommand>
    {
        int ExecuteCommand(TCommand parameters);
    }

    public interface ICliAsyncCommandExecutor
    {
        Task<int> ExecuteCommandAsync();
    }

    public interface ICliAsyncCommandExecutor<TCommand>
    {
        Task<int> ExecuteCommandAsync(TCommand parameters);
    }
}
