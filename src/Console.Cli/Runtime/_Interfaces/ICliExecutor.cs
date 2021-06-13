using MaSch.Console.Cli.Runtime;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Internal
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Internal interface")]
    internal interface ICliExecutor : ICliValidator<object>
    {
        int Execute(ICliCommandInfo command, object obj);
        Task<int> ExecuteAsync(ICliCommandInfo command, object obj);
    }
}
