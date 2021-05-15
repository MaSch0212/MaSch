using MaSch.Console.Cli.Runtime;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Internal
{
    internal interface ICliExecutor : ICliValidator<object>
    {
        int Execute(object obj);
        Task<int> ExecuteAsync(object obj);
    }
}
