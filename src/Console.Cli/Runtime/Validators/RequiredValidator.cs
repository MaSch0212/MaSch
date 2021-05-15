using MaSch.Console.Cli.ErrorHandling;
using System;
using System.Collections;
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
                var value = optionInfo.GetValue(parameters);
                if (!ValidateValue(optionInfo.PropertyType, value))
                    errorList.Add(new(CliErrorType.MissingOption, command, optionInfo));
            }

            foreach (var valueInfo in command.Values.Where(x => x.IsRequired))
            {
                var value = valueInfo.GetValue(parameters);
                if (!ValidateValue(valueInfo.PropertyType, value))
                    errorList.Add(new(CliErrorType.MissingValue, command, valueInfo));
            }

            errors = errorList;
            return errorList.Count == 0;
        }

        private static bool ValidateValue(Type type, object? value)
        {
            return value is not null &&
                   !(typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string) && value is IEnumerable e && !e.OfType<object>().Any());
        }
    }
}
