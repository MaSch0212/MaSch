using MaSch.Core.Extensions;

namespace MaSch.Console.Cli.Runtime.Validators;

internal class RequiredValidator : ICliValidator<object>
{
    public bool ValidateOptions(CliExecutionContext context, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
    {
        var errorList = new List<CliError>();

        foreach (var optionInfo in context.Command.Options.Where(x => x.IsRequired))
        {
            if (!optionInfo.HasValue(parameters))
                errorList.Add(new(CliErrorType.MissingOption, context.Command, optionInfo));
        }

        foreach (var valueInfo in context.Command.Values.Where(x => x.IsRequired))
        {
            if (!valueInfo.HasValue(parameters))
                errorList.Add(new(CliErrorType.MissingValue, context.Command, valueInfo));
        }

        errors = errorList;
        return errorList.Count == 0;
    }
}
