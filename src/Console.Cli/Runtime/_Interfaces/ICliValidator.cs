using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliValidator<TCommand>
    {
        bool ValidateOptions(ICliCommandInfo command, TCommand parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors);
    }
}
