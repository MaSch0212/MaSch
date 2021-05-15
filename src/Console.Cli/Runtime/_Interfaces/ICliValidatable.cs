using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliValidatable
    {
        bool ValidateOptions([MaybeNullWhen(true)] out IEnumerable<CliError> errors);
    }
}
