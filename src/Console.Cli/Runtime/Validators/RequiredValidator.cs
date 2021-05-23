using MaSch.Core.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Runtime.Validators
{
    internal class RequiredValidator : ICliValidator<object>
    {
        public bool ValidateOptions(ICliCommandInfo command, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
        {
            var errorList = new List<CliError>();

            foreach (var optionInfo in command.Options.Where(x => x.IsRequired))
            {
                if (!optionInfo.HasValue(parameters))
                    errorList.Add(new(CliErrorType.MissingOption, command, optionInfo));
            }

            foreach (var valueInfo in command.Values.Where(x => x.IsRequired))
            {
                if (!valueInfo.HasValue(parameters))
                    errorList.Add(new(CliErrorType.MissingValue, command, valueInfo));
            }

            errors = errorList;
            return errorList.Count == 0;
        }
    }
}
