using MaSch.Console.Cli.ErrorHandling;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliValidator<TCommand>
    {
        bool ValidateOptions(TCommand parameters, [MaybeNullWhen(true)] out CliError error);
    }
}
