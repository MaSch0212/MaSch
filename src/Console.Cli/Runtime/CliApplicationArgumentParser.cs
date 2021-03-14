using MaSch.Console.Cli.ErrorHandling;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Console.Cli.Runtime
{
    public static class CliApplicationArgumentParser
    {
        private static readonly object UnsetValue = new();

        public static (CliCommandInfo? Command, object? Options, CliError? Error) Parse(string[] args, CliCommandInfoCollection availableCommands)
        {
            if (args == null || args.Length == 0)
            {
                if (availableCommands.DefaultCommand == null)
                    return (null, null, new CliError(CliErrorType.UnknownCommand));
                else
                    return (availableCommands.DefaultCommand, Activator.CreateInstance(availableCommands.DefaultCommand.CommandType), null);
            }

            var currentCommand = availableCommands.FirstOrDefault(x => x.Aliases.Contains(args[0], StringComparer.OrdinalIgnoreCase));
            var index = 0;
            if (currentCommand != null)
            {
                for (index = 1; index < args.Length; index++)
                {
                    var next = currentCommand.ChildCommands.FirstOrDefault(x => x.Aliases.Contains(args[index], StringComparer.OrdinalIgnoreCase));
                    if (next != null)
                        currentCommand = next;
                    else
                        break;
                }
            }
            else
            {
                currentCommand = availableCommands.DefaultCommand;
            }

            if (currentCommand == null)
                return (null, null, new CliError(CliErrorType.UnknownCommand));

            var orderedValueInfos = currentCommand.Values.OrderBy(x => x.Order).ToArray();
            var values = new List<ValueValue>();
            var options = new List<OptionValue>();
            OptionValue? currentOption = null;
            bool areOptionsEscaped = false;
            foreach (var a in args.Skip(index))
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
                        var option = currentCommand.Options.FirstOrDefault(x => x.Aliases.Contains(optionName, StringComparer.OrdinalIgnoreCase));
                        if (option == null)
                            return (null, null, new CliError(CliErrorType.UnknownOption, currentCommand) { OptionName = a });
                        currentOption = new OptionValue(option);
                        continue;
                    }
                    else if (a?.StartsWith("-") == true && a.Length > 1)
                    {
                        foreach (var s in a.Skip(1))
                        {
                            if (currentOption != null)
                                options.Add(currentOption);
                            var option = currentCommand.Options.FirstOrDefault(x => x.ShortAliases.Contains(s));
                            if (option == null)
                                return (null, null, new CliError(CliErrorType.UnknownOption, currentCommand) { OptionName = $"-{s}" });
                            currentOption = new OptionValue(option);
                        }

                        continue;
                    }
                    else if (currentOption != null)
                    {
                        try
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
                        }
                        catch (Exception ex)
                        {
                            return (null, null, new CliError(CliErrorType.BadOptionValue, currentCommand, currentOption?.Option) { Exception = ex });
                        }

                        continue;
                    }
                }

                if (values.Count > 0 && values[^1].Value is List<string?> listValue)
                {
                    listValue.Add(a);
                }
                else
                {
                    var valueType = orderedValueInfos[values.Count].PropertyType;
                    if (typeof(IEnumerable).IsAssignableFrom(valueType) && valueType != typeof(string))
                        values.Add(new ValueValue(orderedValueInfos[values.Count], new List<string?> { a }));
                    else
                        values.Add(new ValueValue(orderedValueInfos[values.Count], a));
                }
            }

            if (currentOption != null)
                options.Add(currentOption);

            var optionsObj = Activator.CreateInstance(currentCommand.CommandType)!;
            foreach (var value in currentCommand.Values)
            {
                if (typeof(IEnumerable).IsAssignableFrom(value.PropertyType) && value.PropertyType != typeof(string))
                    value.SetValue(optionsObj, Array.Empty<object?>());
                else
                    value.SetValue(optionsObj, value.DefaultValue ?? value.PropertyType.GetDefault());
            }

            foreach (var option in currentCommand.Options)
            {
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
                value.ValueInfo.SetValue(optionsObj, v);
            }

            foreach (var option in options)
            {
                var v = option.Value == UnsetValue
                    ? typeof(IEnumerable).IsAssignableFrom(option.Option.PropertyType) && option.Option.PropertyType != typeof(string) ? Array.Empty<object?>() : (option.Option.DefaultValue ?? option.Option.PropertyType.GetDefault())
                    : option.Value;
                option.Option.SetValue(optionsObj, v);
            }

            return (currentCommand, optionsObj, null);
        }

        private class OptionValue
        {
            public CliCommandOptionInfo Option { get; set; }
            public object? Value { get; set; } = UnsetValue;

            public OptionValue(CliCommandOptionInfo option)
            {
                Option = option;
            }
        }

        private class ValueValue
        {
            public CliCommandValueInfo ValueInfo { get; set; }
            public object? Value { get; set; }

            public ValueValue(CliCommandValueInfo valueInfo, object? value)
            {
                ValueInfo = valueInfo;
                Value = value;
            }
        }
    }
}
