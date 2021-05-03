using MaSch.Console.Cli.ErrorHandling;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliValidatable
    {
        bool ValidateOptions([MaybeNullWhen(true)] out CliError error);
    }
}
