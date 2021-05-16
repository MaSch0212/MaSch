using MaSch.Console.Cli.Runtime.Validators;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Runtime
{
    public static class CliArgumentParser
    {
        private static readonly List<ICliValidator<object>> _commonValidators = new()
        {
            new RequiredValidator(),
        };

        public static void AddValidator(ICliValidator<object> validator)
        {
            _commonValidators.Add(validator);
        }

        public static CliArgumentParserResult Parse(string[] args, CliApplicationOptions appOptions, ICliCommandInfoCollection availableCommands)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    if (availableCommands.DefaultCommand == null)
                        return new(new[] { new CliError(CliErrorType.MissingCommand) });
                    else
                        return new(availableCommands.DefaultCommand, Activator.CreateInstance(availableCommands.DefaultCommand.CommandType)!);
                }

                if (!TryParseCommandInfo(args, availableCommands, out var command, out var commandArgIndex))
                {
                    HandleSpecialCommands(args, appOptions, availableCommands);
                    return new(new[] { new CliError(CliErrorType.UnknownCommand) { CommandName = args[commandArgIndex] } });
                }

                var errors = new List<CliError>();
                ParseOptions(args.Skip(commandArgIndex + 1).ToArray(), appOptions, command, errors, out var optionsObj);
                ValidateOptions(command, optionsObj, errors);

                return errors.Count > 0 ? new(errors, command, optionsObj) : new(command, optionsObj);
            }
            catch (CliErrorException ex)
            {
                return new(ex.Errors);
            }
        }

        private static bool TryParseCommandInfo(string[] args, ICliCommandInfoCollection availableCommands, [NotNullWhen(true)] out ICliCommandInfo? command, out int commandArgIndex)
        {
            commandArgIndex = 0;
            command = availableCommands.FirstOrDefault(x => x.Aliases.Contains(args[0], StringComparer.OrdinalIgnoreCase));
            if (command != null)
            {
                for (commandArgIndex = 1; commandArgIndex < args.Length; commandArgIndex++)
                {
                    var index = commandArgIndex;
                    var next = command.ChildCommands.FirstOrDefault(x => x.Aliases.Contains(args[index], StringComparer.OrdinalIgnoreCase));
                    if (next != null)
                        command = next;
                    else
                        break;
                }
            }
            else if (availableCommands.DefaultCommand?.Values.IsNullOrEmpty() == false)
            {
                command = availableCommands.DefaultCommand;
            }

            return command != null;
        }

        private static void HandleSpecialCommands(string[] args, CliApplicationOptions appOptions, ICliCommandInfoCollection availableCommands)
        {
            if (appOptions.ProvideVersionCommand)
            {
                if (IsCommand("version"))
                    DetermineCommandAndThrow(CliErrorType.VersionRequested);
                else if (IsOption("version"))
                    Throw(CliErrorType.VersionRequested, null);
            }

            if (appOptions.ProvideHelpCommand)
            {
                if (IsCommand("help"))
                    DetermineCommandAndThrow(CliErrorType.HelpRequested);
                else if (IsOption("help"))
                    Throw(CliErrorType.HelpRequested, null);
            }

            bool IsCommand(string expectedCommand) => string.Equals(args[0], expectedCommand, StringComparison.OrdinalIgnoreCase);
            bool IsOption(string expectedCommand) => string.Equals(args[0], "--" + expectedCommand, StringComparison.OrdinalIgnoreCase);

            void DetermineCommandAndThrow(CliErrorType errorType)
            {
                ICliCommandInfo? command = null;
                if (args.Length > 1)
                    TryParseCommandInfo(args.Skip(1).ToArray(), availableCommands, out command, out _);
                Throw(errorType, command);
            }

            void Throw(CliErrorType errorType, ICliCommandInfo? command) => throw new CliErrorException(new[] { new CliError(errorType, command) });
        }

        private static void ParseOptions(IList<string> args, CliApplicationOptions appOptions, ICliCommandInfo command, IList<CliError> errors, out object optionsObj)
        {
            optionsObj = CreateOptionsWithDefaultValues(command);

            bool areOptionsEscaped = false;
            int currentValueIndex = 0;
            for (int i = 0; i < args.Count; i++)
            {
                var a = args[i];
                if (!areOptionsEscaped && a == "--")
                {
                    areOptionsEscaped = true;
                }
                else if (!areOptionsEscaped && a.StartsWith("-") && a.Length > 1)
                {
                    ParseOption(args, ref i, appOptions, command, optionsObj, errors);
                }
                else if (currentValueIndex < command.Values.Count)
                {
                    ParseValue(args, ref i, command.Values[currentValueIndex], optionsObj, areOptionsEscaped, errors, out var continueToNextValue);
                    if (continueToNextValue)
                        currentValueIndex++;
                }
                else if (!appOptions.IgnoreAdditionalValues && !errors.Any(x => x.Type == CliErrorType.UnknownValue))
                {
                    errors.Add(new CliError(CliErrorType.UnknownValue, command));
                }
            }
        }

        private static object CreateOptionsWithDefaultValues(ICliCommandInfo command)
        {
            object result = command.OptionsInstance ?? Activator.CreateInstance(command.CommandType)!;
            foreach (var m in command.Values.Cast<ICliCommandMemberInfo>().Concat(command.Options))
            {
                if (typeof(IEnumerable).IsAssignableFrom(m.PropertyType) && m.PropertyType != typeof(string))
                    m.SetValue(result, Array.Empty<object?>());
                else
                    m.SetValue(result, m.DefaultValue ?? m.PropertyType.GetDefault());
            }

            return result;
        }

        private static void ParseValue(IList<string> args, ref int i, ICliCommandValueInfo value, object optionsObj, bool areOptionsEscaped, IList<CliError> errors, out bool continueToNextValue)
        {
            var valueType = value.PropertyType;
            object v;

            if (typeof(IEnumerable).IsAssignableFrom(valueType) && valueType != typeof(string))
            {
                var values = new List<object>();
                for (; i < args.Count && (areOptionsEscaped || !(args[i].StartsWith("-") && args[i].Length > 1)); i++)
                    values.Add(args[i]);
                i--;
                v = values;
                continueToNextValue = false;
            }
            else
            {
                v = args[i];
                continueToNextValue = true;
            }

            try
            {
                var cv = value.GetValue(optionsObj);
                if (cv is IEnumerable cve && cv is not string && v is IEnumerable ve && v is not string)
                    v = cve.OfType<object>().Concat(ve);

                value.SetValue(optionsObj, v);
            }
            catch (Exception ex)
            {
                errors.Add(new CliError(CliErrorType.WrongValueFormat, value.Command, value) { Exception = ex });
            }
        }

        private static void ParseOption(IList<string> args, ref int i, CliApplicationOptions appOptions, ICliCommandInfo command, object optionsObj, IList<CliError> errors)
        {
            var a = args[i];

            // -abc -> ignore any other args; only valid for bool options
            if (!a.StartsWith("--") && a.Length > 2)
            {
                foreach (var s in a.Skip(1))
                {
                    var option = command.Options.FirstOrDefault(x => x.ShortAliases.Contains(s));
                    if (option != null)
                        SetBoolOption(option);
                    else if (!appOptions.IgnoreUnknownOptions)
                        errors.Add(new CliError(CliErrorType.UnknownOption, command) { OptionName = $"-{s}" });
                }
            }
            else
            {
                var option = a.StartsWith("--")
                    ? command.Options.FirstOrDefault(x => x.Aliases.Contains(a[2..], StringComparer.OrdinalIgnoreCase))
                    : command.Options.FirstOrDefault(x => x.ShortAliases.Contains(a[1]));

                if (option != null)
                {
                    object? v = null;

                    var isList = typeof(IEnumerable).IsAssignableFrom(option.PropertyType) && option.PropertyType != typeof(string);
                    for (i++; i < args.Count && !(args[i].StartsWith("-") && args[i].Length > 1); i++)
                    {
                        if (isList)
                        {
                            v ??= new List<object>();
                            ((IList)v).Add(args[i]);
                        }
                        else
                        {
                            v = args[i++];
                            break;
                        }
                    }

                    i--;
                    if (v == null)
                    {
                        SetBoolOption(option);
                    }
                    else
                    {
                        try
                        {
                            option.SetValue(optionsObj, v);
                        }
                        catch (Exception ex)
                        {
                            errors.Add(new CliError(CliErrorType.WrongOptionFormat, command, option) { Exception = ex });
                        }
                    }
                }
                else
                {
                    if (a.StartsWith("--"))
                        HandleSpecialOptions(appOptions, a[2..], command);
                    if (!appOptions.IgnoreUnknownOptions)
                        errors.Add(new CliError(CliErrorType.UnknownOption, command) { OptionName = a });
                }
            }

            void SetBoolOption(ICliCommandOptionInfo option)
            {
                if (option.PropertyType == typeof(bool) || option.PropertyType == typeof(bool?))
                    option.SetValue(optionsObj, true);
                else
                    errors.Add(new CliError(CliErrorType.MissingOptionValue, command, option));
            }
        }

        private static void HandleSpecialOptions(CliApplicationOptions appOptions, string optionName, ICliCommandInfo command)
        {
            if (appOptions.ProvideVersionOptions && IsOption("version"))
                Throw(CliErrorType.VersionRequested);
            else if (appOptions.ProvideHelpOptions && IsOption("help"))
                Throw(CliErrorType.HelpRequested);

            bool IsOption(string expectedOption) => string.Equals(optionName, expectedOption, StringComparison.OrdinalIgnoreCase);
            void Throw(CliErrorType errorType) => throw new CliErrorException(new[] { new CliError(errorType, command) });
        }

        private static void ValidateOptions(ICliCommandInfo command, object optionsObj, IList<CliError> errors)
        {
            IEnumerable<CliError>? vErrors;
            foreach (var validator in _commonValidators)
            {
                if (!validator.ValidateOptions(command, optionsObj, out vErrors))
                    errors.Add(vErrors);
            }

            if (optionsObj is ICliValidatable validatable && !validatable.ValidateOptions(out vErrors))
                errors.Add(vErrors);

            if (!command.ValidateOptions(command, optionsObj, out vErrors))
                errors.Add(vErrors);
        }

        [SuppressMessage("Critical Code Smell", "S3871:Exception types should be \"public\"", Justification = "This exception is always catched, so no need to make it public.")]
        private class CliErrorException : Exception
        {
            public IEnumerable<CliError> Errors { get; }

            public CliErrorException(IEnumerable<CliError> errors)
            {
                Errors = errors;
            }
        }
    }
}
