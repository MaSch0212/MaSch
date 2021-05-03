using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliAsyncCommandExecutor
    {
        Task<int> ExecuteCommandAsync();
    }

    public interface ICliAsyncCommandExecutor<TCommand>
    {
        Task<int> ExecuteCommandAsync(TCommand parameters);
    }
}
