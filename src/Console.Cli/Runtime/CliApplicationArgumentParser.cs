using MaSch.Console.Cli.ErrorHandling;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Runtime
{
    public static class CliApplicationArgumentParser
    {
        private static readonly object UnsetValue = new();

        public static CliApplicationArgumentParserResult Parse(string[] args, CliApplicationOptions appOptions, CliCommandInfoCollection availableCommands)
        {
            if (args == null || args.Length == 0)
            {
                if (availableCommands.DefaultCommand == null)
                    return new(new CliError(CliErrorType.MissingCommand));
                else
                    return new(availableCommands.DefaultCommand, Activator.CreateInstance(availableCommands.DefaultCommand.CommandType)!);
            }

            if (!TryParseCommandInfo(args, availableCommands, out var command, out var commandArgIndex))
                return new(new CliError(CliErrorType.UnknownCommand) { CommandName = args[commandArgIndex] });

            if (!TryParseOptionsAndValues(args.Skip(commandArgIndex), appOptions, command, out var ovpError, out var values, out var options))
                return new(ovpError);

            if (!TryCreateOptions(command, values, options, out var optError, out var optionsObj))
                return new(optError);

            if (optionsObj is ICliValidatable validatable && !validatable.ValidateOptions(out var vError))
                return new(vError);

            if (!command.ValidateOptions(optionsObj, out vError))
                return new(vError);

            return new(command, optionsObj);
        }

        private static bool TryParseCommandInfo(string[] args, CliCommandInfoCollection availableCommands, [NotNullWhen(true)] out ICliCommandInfo? command, out int commandArgIndex)
        {
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
            else
            {
                commandArgIndex = 0;
                command = availableCommands.DefaultCommand;
            }

            return command != null;
        }

        private static bool TryParseOptionsAndValues(
            IEnumerable<string> args,
            CliApplicationOptions appOptions,
            ICliCommandInfo command,
            [NotNullWhen(false)] out CliError? error,
            out IList<ValueValue> values,
            out IList<OptionValue> options)
        {
            var orderedValueInfos = command.Values.OrderBy(x => x.Order).ToArray();
            values = new List<ValueValue>();
            options = new List<OptionValue>();
            OptionValue? currentOption = null;
            bool areOptionsEscaped = false;
            foreach (var a in args)
            {
                if (!areOptionsEscaped)
                {
                    if (a == "--")
                    {
                        if (currentOption != null)
                        {
                            options.Add(currentOption);
                            currentOption = null;
                        }

                        areOptionsEscaped = true;
                        continue;
                    }
                    else if (a?.StartsWith("--") == true)
                    {
                        if (currentOption != null)
                            options.Add(currentOption);

                        var optionName = a[2..];
                        var option = command.Options.FirstOrDefault(x => x.Aliases.Contains(optionName, StringComparer.OrdinalIgnoreCase));
                        if (option == null)
                        {
                            if (!appOptions.IgnoreUnknownOptions)
                            {
                                error = new CliError(CliErrorType.UnknownOption, command) { OptionName = a };
                                return false;
                            }
                        }
                        else
                        {
                            currentOption = new OptionValue(option);
                        }

                        continue;
                    }
                    else if (a?.StartsWith("-") == true && a.Length > 1)
                    {
                        foreach (var s in a.Skip(1))
                        {
                            if (currentOption != null)
                                options.Add(currentOption);
                            var option = command.Options.FirstOrDefault(x => x.ShortAliases.Contains(s));
                            if (option == null)
                            {
                                if (!appOptions.IgnoreUnknownOptions)
                                {
                                    error = new CliError(CliErrorType.UnknownOption, command) { OptionName = $"-{s}" };
                                    return false;
                                }
                            }
                            else
                            {
                                currentOption = new OptionValue(option);
                            }
                        }

                        continue;
                    }
                    else if (currentOption != null)
                    {
                        if (ReferenceEquals(currentOption.Value, UnsetValue))
                        {
                            if (typeof(IEnumerable).IsAssignableFrom(currentOption.Option.PropertyType) && currentOption.Option.PropertyType != typeof(string))
                            {
                                currentOption.Value = new List<string?> { a };
                            }
                            else
                            {
                                currentOption.Value = a;
                                options.Add(currentOption);
                                currentOption = null;
                            }
                        }
                        else
                        {
                            var list = (List<string?>)currentOption.Value!;
                            list.Add(a);
                        }

                        continue;
                    }
                }

                if (values.Count > 0 && values[^1].Value is List<string?> listValue)
                {
                    listValue.Add(a);
                }
                else if (values.Count < orderedValueInfos.Length)
                {
                    var valueType = orderedValueInfos[values.Count].PropertyType;
                    if (typeof(IEnumerable).IsAssignableFrom(valueType) && valueType != typeof(string))
                        values.Add(new ValueValue(orderedValueInfos[values.Count], new List<string?> { a }));
                    else
                        values.Add(new ValueValue(orderedValueInfos[values.Count], a));
                }
                else if (!appOptions.IgnoreAdditionalValues)
                {
                    error = new CliError(CliErrorType.UnknownValue, command);
                    return false;
                }
            }

            if (currentOption != null)
                options.Add(currentOption);

            error = null;
            return true;
        }

        private static bool TryCreateOptions(ICliCommandInfo command, IList<ValueValue> values, IList<OptionValue> options, [NotNullWhen(false)] out CliError? error, out object optionsObj)
        {
            optionsObj = command.OptionsInstance ?? Activator.CreateInstance(command.CommandType)!;
            foreach (var value in command.Values.Except(values.Select(x => x.ValueInfo)))
            {
                if (value.IsRequired)
                {
                    error = new CliError(CliErrorType.MissingValue, command, value);
                    return false;
                }

                if (typeof(IEnumerable).IsAssignableFrom(value.PropertyType) && value.PropertyType != typeof(string))
                    value.SetValue(optionsObj, Array.Empty<object?>());
                else
                    value.SetValue(optionsObj, value.DefaultValue ?? value.PropertyType.GetDefault());
            }

            foreach (var option in command.Options.Except(options.Select(x => x.Option)))
            {
                if (option.IsRequired)
                {
                    error = new CliError(CliErrorType.MissingOption, command, option);
                    return false;
                }

                if (typeof(IEnumerable).IsAssignableFrom(option.PropertyType) && option.PropertyType != typeof(string))
                    option.SetValue(optionsObj, Array.Empty<object?>());
                else
                    option.SetValue(optionsObj, option.DefaultValue ?? option.PropertyType.GetDefault());
            }

            foreach (var value in values)
            {
                var v = value.Value == UnsetValue
                    ? typeof(IEnumerable).IsAssignableFrom(value.ValueInfo.PropertyType) && value.ValueInfo.PropertyType != typeof(string) ? Array.Empty<object?>() : (value.ValueInfo.DefaultValue ?? value.ValueInfo.PropertyType.GetDefault())
                    : value.Value;
                try
                {
                    value.ValueInfo.SetValue(optionsObj, v);
                }
                catch (Exception ex)
                {
                    error = new CliError(CliErrorType.WrongValueFormat, command, value.ValueInfo) { Exception = ex };
                    return false;
                }
            }

            foreach (var option in options)
            {
                var v = option.Value == UnsetValue
                    ? typeof(IEnumerable).IsAssignableFrom(option.Option.PropertyType) && option.Option.PropertyType != typeof(string) ? Array.Empty<object?>() : (option.Option.DefaultValue ?? option.Option.PropertyType.GetDefault())
                    : option.Value;
                try
                {
                    option.Option.SetValue(optionsObj, v);
                }
                catch (Exception ex)
                {
                    error = new CliError(CliErrorType.WrongOptionFormat, command, option.Option) { Exception = ex };
                    return false;
                }
            }

            error = null;
            return true;
        }

        private class OptionValue
        {
            public ICliCommandOptionInfo Option { get; set; }
            public object? Value { get; set; } = UnsetValue;

            public OptionValue(ICliCommandOptionInfo option)
            {
                Option = option;
            }
        }

        private class ValueValue
        {
            public ICliCommandValueInfo ValueInfo { get; set; }
            public object? Value { get; set; }

            public ValueValue(ICliCommandValueInfo valueInfo, object? value)
            {
                ValueInfo = valueInfo;
                Value = value;
            }
        }
    }
}
