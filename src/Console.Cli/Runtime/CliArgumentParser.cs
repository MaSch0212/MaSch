using MaSch.Console.Cli.Runtime.Validators;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Runtime
{
    public class CliArgumentParser : ICliArgumentParser
    {
        private readonly List<ICliValidator<object>> _commonValidators = new()
        {
            new RequiredValidator(),
        };

        public void AddValidator(ICliValidator<object> validator)
        {
            _commonValidators.Add(validator);
        }

        public CliArgumentParserResult Parse(ICliApplicationBase application, string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    var defaultCommand = application.Commands.DefaultCommand;
                    if (defaultCommand == null)
                        return new(new[] { new CliError(CliErrorType.MissingCommand) });
                    else
                        return new(defaultCommand, Activator.CreateInstance(defaultCommand.CommandType)!);
                }

                if (!TryParseCommandInfo(args, application, out var command, out var commandArgIndex))
                {
                    HandleSpecialCommands(args, application);
                    return new(new[] { new CliError(CliErrorType.UnknownCommand) { CommandName = args[commandArgIndex] } });
                }

                var ctx = new CommandParserContext(args.Skip(commandArgIndex + 1).ToArray(), application, command, new List<CliError>());
                ParseOptions(ctx);
                ValidateOptions(ctx);

                return ctx.Errors.Count > 0 ? new(ctx.Errors, command, ctx.OptionsObj) : new(command, ctx.OptionsObj);
            }
            catch (CliErrorException ex)
            {
                return new(ex.Errors);
            }
        }

        private void ValidateOptions(CommandParserContext ctx)
        {
            IEnumerable<CliError>? vErrors;
            foreach (var validator in _commonValidators)
            {
                if (!validator.ValidateOptions(ctx.Command, ctx.OptionsObj, out vErrors))
                    ctx.Errors.Add(vErrors);
            }

            if (ctx.OptionsObj is ICliValidatable validatable && !validatable.ValidateOptions(out vErrors))
                ctx.Errors.Add(vErrors);

            if (!ctx.Command.ValidateOptions(ctx.Command, ctx.OptionsObj, out vErrors))
                ctx.Errors.Add(vErrors);
        }

        private static bool TryParseCommandInfo(string[] args, ICliApplicationBase application, [NotNullWhen(true)] out ICliCommandInfo? command, out int commandArgIndex)
        {
            commandArgIndex = 0;
            command = application.Commands.GetRootCommands().FirstOrDefault(x => x.Aliases.Contains(args[0], StringComparer.OrdinalIgnoreCase));
            if (command != null)
            {
                for (commandArgIndex = 1; commandArgIndex < args.Length; commandArgIndex++)
                {
                    var index = commandArgIndex;
                    var next = command.ChildCommands.FirstOrDefault(x => x.Aliases.Contains(args[index], StringComparer.OrdinalIgnoreCase));
                    if (next == null)
                    {
                        commandArgIndex--;
                        break;
                    }

                    command = next;
                }
            }
            else if (application.Commands.DefaultCommand?.Values.IsNullOrEmpty() == false)
            {
                command = application.Commands.DefaultCommand;
                commandArgIndex = -1;
            }

            return command != null;
        }

        private static void HandleSpecialCommands(string[] args, ICliApplicationBase application)
        {
            if (application.Options.ProvideVersionCommand)
            {
                if (IsCommand("version"))
                    DetermineCommandAndThrow(CliErrorType.VersionRequested);
                else if (IsOption("version"))
                    throw GetException(CliErrorType.VersionRequested, null);
            }

            if (application.Options.ProvideHelpCommand)
            {
                if (IsCommand("help"))
                    DetermineCommandAndThrow(CliErrorType.HelpRequested);
                else if (IsOption("help"))
                    throw GetException(CliErrorType.HelpRequested, null);
            }

            bool IsCommand(string expectedCommand) => string.Equals(args[0], expectedCommand, StringComparison.OrdinalIgnoreCase);
            bool IsOption(string expectedCommand) => string.Equals(args[0], "--" + expectedCommand, StringComparison.OrdinalIgnoreCase);

            void DetermineCommandAndThrow(CliErrorType errorType)
            {
                ICliCommandInfo? command = null;
                if (args.Length > 1)
                    TryParseCommandInfo(args.Skip(1).ToArray(), application, out command, out _);
                throw GetException(errorType, command);
            }

            CliErrorException GetException(CliErrorType errorType, ICliCommandInfo? command) => new(new[] { new CliError(errorType, command) });
        }

        private static void ParseOptions(CommandParserContext ctx)
        {
            for (; ctx.ArgIndex < ctx.Args.Count; ctx.ArgIndex++)
            {
                var a = ctx.Args[ctx.ArgIndex];
                if (!ctx.AreOptionsEscaped && a == "--")
                {
                    ctx.AreOptionsEscaped = true;
                }
                else if (!ctx.AreOptionsEscaped && a.StartsWith("-") && a.Length > 1)
                {
                    ParseOption(ctx);
                }
                else if (!ctx.IgnoreNextValues)
                {
                    if (ctx.CurrentValueIndex < ctx.Command.Values.Count)
                    {
                        ParseValue(ctx, ctx.Command.Values[ctx.CurrentValueIndex]);
                    }
                    else if (!ctx.Application.Options.IgnoreAdditionalValues && !ctx.Errors.Any(x => x.Type == CliErrorType.UnknownValue))
                    {
                        ctx.Errors.Add(new CliError(CliErrorType.UnknownValue, ctx.Command));
                    }
                }
            }
        }

        private static void ParseValue(CommandParserContext ctx, ICliCommandValueInfo value)
        {
            var valueType = value.PropertyType;
            object v;

            if (typeof(IEnumerable).IsAssignableFrom(valueType) && valueType != typeof(string))
            {
                var values = new List<object>();
                for (; ctx.ArgIndex < ctx.Args.Count && (ctx.AreOptionsEscaped || !(ctx.Args[ctx.ArgIndex].StartsWith("-") && ctx.Args[ctx.ArgIndex].Length > 1)); ctx.ArgIndex++)
                    values.Add(ctx.Args[ctx.ArgIndex]);
                ctx.ArgIndex--;
                v = values;
            }
            else
            {
                v = ctx.Args[ctx.ArgIndex];
                ctx.CurrentValueIndex++;
            }

            try
            {
                var cv = value.GetValue(ctx.OptionsObj);
                if (cv is IEnumerable cve && cv is not string && v is IEnumerable ve && v is not string)
                    v = cve.OfType<object>().Concat(ve.OfType<object>());

                value.SetValue(ctx.OptionsObj, v);
            }
            catch (Exception ex)
            {
                ctx.Errors.Add(new CliError(CliErrorType.WrongValueFormat, ctx.Command, value) { Exception = ex });
            }
        }

        private static void ParseOption(CommandParserContext ctx)
        {
            var a = ctx.Args[ctx.ArgIndex];

            // -abc -> ignore any other args; only valid for bool options
            if (!a.StartsWith("--") && a.Length > 2)
            {
                foreach (var s in a.Skip(1))
                {
                    var option = ctx.Command.Options.FirstOrDefault(x => x.ShortAliases.Contains(s));
                    if (option != null)
                        SetBoolOption(option);
                    else if (!ctx.Application.Options.IgnoreUnknownOptions)
                        ctx.Errors.Add(new CliError(CliErrorType.UnknownOption, ctx.Command) { OptionName = $"-{s}" });
                }
            }
            else
            {
                var option = a.StartsWith("--")
                    ? ctx.Command.Options.FirstOrDefault(x => x.Aliases.Contains(a[2..], StringComparer.OrdinalIgnoreCase))
                    : ctx.Command.Options.FirstOrDefault(x => x.ShortAliases.Contains(a[1]));

                if (option != null)
                {
                    object? v = null;

                    var isList = typeof(IEnumerable).IsAssignableFrom(option.PropertyType) && option.PropertyType != typeof(string);
                    for (ctx.ArgIndex++; ctx.ArgIndex < ctx.Args.Count && !(ctx.Args[ctx.ArgIndex].StartsWith("-") && ctx.Args[ctx.ArgIndex].Length > 1); ctx.ArgIndex++)
                    {
                        if (isList)
                        {
                            v ??= new List<object>();
                            ((IList)v).Add(ctx.Args[ctx.ArgIndex]);
                        }
                        else
                        {
                            v = ctx.Args[ctx.ArgIndex++];
                            break;
                        }
                    }

                    ctx.ArgIndex--;
                    if (v == null)
                    {
                        SetBoolOption(option);
                    }
                    else
                    {
                        try
                        {
                            option.SetValue(ctx.OptionsObj, v);
                        }
                        catch (Exception ex)
                        {
                            ctx.Errors.Add(new CliError(CliErrorType.WrongOptionFormat, ctx.Command, option) { Exception = ex });
                        }
                    }
                }
                else
                {
                    if (a.StartsWith("--"))
                        HandleSpecialOptions(ctx.Application.Options, a[2..], ctx.Command);
                    if (!ctx.Application.Options.IgnoreUnknownOptions)
                        ctx.Errors.Add(new CliError(CliErrorType.UnknownOption, ctx.Command) { OptionName = a });
                }

                ctx.IgnoreNextValues = option == null;
            }

            void SetBoolOption(ICliCommandOptionInfo option)
            {
                if (option.PropertyType == typeof(bool) || option.PropertyType == typeof(bool?))
                    option.SetValue(ctx.OptionsObj, true);
                else
                    ctx.Errors.Add(new CliError(CliErrorType.MissingOptionValue, ctx.Command, option));
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

        [SuppressMessage("Critical Code Smell", "S3871:Exception types should be \"public\"", Justification = "This exception is always catched, so no need to make it public.")]
        private class CliErrorException : Exception
        {
            public IEnumerable<CliError> Errors { get; }

            public CliErrorException(IEnumerable<CliError> errors)
            {
                Errors = errors;
            }
        }

        private class CommandParserContext
        {
            public IList<string> Args { get; }
            public ICliCommandInfo Command { get; }
            public object OptionsObj { get; }
            public IList<CliError> Errors { get; }
            public ICliApplicationBase Application { get; }

            public int ArgIndex { get; set; }
            public bool IgnoreNextValues { get; set; }
            public int CurrentValueIndex { get; set; }
            public bool AreOptionsEscaped { get; set; }

            public CommandParserContext(IList<string> args, ICliApplicationBase application, ICliCommandInfo command, IList<CliError> errors)
            {
                Args = args;
                Application = application;
                Command = command;
                OptionsObj = CreateOptionsWithDefaultValues(command);
                Errors = errors;
            }

            private static object CreateOptionsWithDefaultValues(ICliCommandInfo command)
            {
                object result = command.OptionsInstance ?? Activator.CreateInstance(command.CommandType)!;
                foreach (var m in command.Values.Cast<ICliCommandMemberInfo>().Concat(command.Options))
                    m.SetDefaultValue(result);

                return result;
            }
        }
    }
}
