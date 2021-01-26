using System.Threading.Tasks;

namespace MaSch.Console.Cli.Internal
{
    internal interface IExecutor
    {
        int Execute(object obj);
        Task<int> ExecuteAsync(object obj);
    }
}
