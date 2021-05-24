using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliAsyncCommandExecutor : ICliCommandExecutorBase
    {
        Task<int> ExecuteCommandAsync();
    }

    public interface ICliAsyncCommandExecutor<TCommand> : ICliCommandExecutorBase<TCommand>
    {
        Task<int> ExecuteCommandAsync(TCommand parameters);
    }
}
