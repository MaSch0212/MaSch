using MaSch.Console.Cli.Runtime.Validators;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Default implementation of the <see cref="ICliArgumentParser"/> interface.
    /// </summary>
    public class CliArgumentParser : ICliArgumentParser
    {
        private readonly List<ICliValidator<object>> _commonValidators = new()
        {
            new RequiredValidator(),
        };

        /// <inheritdoc/>
        public void AddValidator(ICliValidator<object> validator)
        {
            _commonValidators.Add(validator);
        }

        /// <inheritdoc/>
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
                        return new(new(application, defaultCommand), Activator.CreateInstance(defaultCommand.CommandType)!);
                }

                var hasCommand = TryParseCommandInfo(args, application, null, out var command, out var commandForHelp, out var commandArgIndex);
                if (args.Length > commandArgIndex + 1)
                    HandleSpecialCommands(args.Skip(commandArgIndex + 1).ToArray(), application, commandForHelp);

                if (!hasCommand || command == null)
                    return new(new[] { new CliError(CliErrorType.UnknownCommand, command) { CommandName = args[commandArgIndex + 1] } });
                if (!command.IsExecutable)
                    return new(new[] { new CliError(CliErrorType.CommandNotExecutable, command) });

                var ctx = new CommandParserContext(args.Skip(commandArgIndex + 1).ToArray(), application, command, new List<CliError>());
                ParseOptions(ctx);
                ValidateOptions(ctx);

                return ctx.Errors.Count > 0 ? new(ctx.Errors, ctx.ExecutionContext, ctx.OptionsObj) : new(ctx.ExecutionContext, ctx.OptionsObj);
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
                if (!validator.ValidateOptions(ctx.ExecutionContext, ctx.OptionsObj, out vErrors))
                    ctx.Errors.Add(vErrors);
            }

            if (ctx.OptionsObj is ICliValidatable validatable && !validatable.ValidateOptions(ctx.ExecutionContext, out vErrors))
                ctx.Errors.Add(vErrors);

            if (!ctx.Command.ValidateOptions(ctx.ExecutionContext, ctx.OptionsObj, out vErrors))
                ctx.Errors.Add(vErrors);
        }

        private static bool TryParseCommandInfo(string[] args, ICliApplicationBase application, ICliCommandInfo? baseCommand, [NotNullWhen(true)] out ICliCommandInfo? command, out ICliCommandInfo? commandForHelp, out int commandArgIndex)
        {
            command = baseCommand;
            bool nextIsValue = false;
            for (commandArgIndex = 0; commandArgIndex < args.Length; commandArgIndex++)
            {
                var nextCommandName = args[commandArgIndex];
                nextIsValue = !nextCommandName.StartsWith("-");
                if (!nextIsValue)
                    break;

                var nextCommand = (command == null
                    ? application.Commands.GetRootCommands()
                    : command.ChildCommands)
                        .FirstOrDefault(x => x.Aliases.Contains(nextCommandName, StringComparer.OrdinalIgnoreCase));
                if (nextCommand == null)
                    break;

                command = nextCommand;
            }

            if (command == null)
            {
                commandForHelp = command;
                command = application.Commands.DefaultCommand;
            }
            else if (command.IsExecutable)
            {
                commandForHelp = command;
            }
            else
            {
                commandForHelp = command;
                command = command.ChildCommands.FirstOrDefault(x => x.IsDefault) ?? command;
            }

            commandArgIndex--;
            return command != null && (!nextIsValue || commandArgIndex + 1 == args.Length || command.Values.Count > 0);
        }

        private static void HandleSpecialCommands(string[] args, ICliApplicationBase application, ICliCommandInfo? currentCommand)
        {
            if (IsCommand("version"))
                DetermineCommandAndThrow(CliErrorType.VersionRequested, x => GetProvideVersionCommand(application, x));
            else if (IsOption("version") && GetProvideVersionOptions(application, currentCommand) && !OptionExists("version"))
                throw GetException(CliErrorType.VersionRequested, currentCommand, Array.Empty<CliError>());

            if (IsCommand("help"))
                DetermineCommandAndThrow(CliErrorType.HelpRequested, x => GetProvideHelpCommand(application, x));
            else if (IsOption("help") && GetProvideHelpOptions(application, currentCommand) && !OptionExists("help"))
                throw GetException(CliErrorType.HelpRequested, currentCommand, Array.Empty<CliError>());

            bool IsCommand(string expectedCommand) => string.Equals(args[0], expectedCommand, StringComparison.OrdinalIgnoreCase);
            bool IsOption(string expectedCommand) => string.Equals(args[0], "--" + expectedCommand, StringComparison.OrdinalIgnoreCase);
            bool OptionExists(string optionName) => currentCommand?.Options.Any(x => x.Aliases.Any(y => string.Equals(y, optionName, StringComparison.OrdinalIgnoreCase))) == true;

            void DetermineCommandAndThrow(CliErrorType errorType, Func<ICliCommandInfo?, bool> shouldThrowFunc)
            {
                ICliCommandInfo? command = currentCommand;
                IEnumerable<CliError> additionalErrors = Array.Empty<CliError>();
                if (args.Length > 1 && !TryParseCommandInfo(args.Skip(1).ToArray(), application, currentCommand, out _, out command, out var idx))
                    additionalErrors = additionalErrors.Append(new CliError(CliErrorType.UnknownCommand) { CommandName = args[idx + 1] });

                if (shouldThrowFunc(command))
                    throw GetException(errorType, command, additionalErrors);
            }

            CliErrorException GetException(CliErrorType errorType, ICliCommandInfo? command, IEnumerable<CliError> additionalErrors)
                => new(additionalErrors.Prepend(new CliError(errorType, command)));
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
                    else if (!GetIgnoreAdditionalValues(ctx.Application, ctx.Command) && !ctx.Errors.Any(x => x.Type == CliErrorType.UnknownValue))
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
                    else if (!GetIgnoreUnknownOptions(ctx.Application, ctx.Command))
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
                    for (ctx.ArgIndex++; ctx.ArgIndex < ctx.Args.Count && !((ctx.Args[ctx.ArgIndex].StartsWith("-") && !ctx.AreOptionsEscaped && ctx.Args[ctx.ArgIndex] != "--") && ctx.Args[ctx.ArgIndex].Length > 1); ctx.ArgIndex++)
                    {
                        if (ctx.Args[ctx.ArgIndex] == "--")
                        {
                            ctx.AreOptionsEscaped = true;
                            continue;
                        }

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
                        HandleSpecialOptions(ctx.Application, a[2..], ctx.Command);
                    if (!GetIgnoreUnknownOptions(ctx.Application, ctx.Command))
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

        private static void HandleSpecialOptions(ICliApplicationBase application, string optionName, ICliCommandInfo command)
        {
            if (GetProvideVersionOptions(application, command) && IsOption("version"))
                Throw(CliErrorType.VersionRequested);
            else if (GetProvideHelpOptions(application, command) && IsOption("help"))
                Throw(CliErrorType.HelpRequested);

            bool IsOption(string expectedOption) => string.Equals(optionName, expectedOption, StringComparison.OrdinalIgnoreCase);
            void Throw(CliErrorType errorType) => throw new CliErrorException(new[] { new CliError(errorType, command) });
        }

        private static bool GetIgnoreUnknownOptions(ICliApplicationBase application, ICliCommandInfo? command)
            => command == null ? application.Options.IgnoreUnknownOptions : (command.ParserOptions.IgnoreUnknownOptions ?? GetIgnoreUnknownOptions(application, command.ParentCommand));
        private static bool GetIgnoreAdditionalValues(ICliApplicationBase application, ICliCommandInfo? command)
            => command == null ? application.Options.IgnoreAdditionalValues : (command.ParserOptions.IgnoreAdditionalValues ?? GetIgnoreAdditionalValues(application, command.ParentCommand));
        private static bool GetProvideHelpCommand(ICliApplicationBase application, ICliCommandInfo? command)
            => command == null ? application.Options.ProvideHelpCommand : (command.ParserOptions.ProvideHelpCommand ?? GetProvideHelpCommand(application, command.ParentCommand));
        private static bool GetProvideVersionCommand(ICliApplicationBase application, ICliCommandInfo? command)
            => command == null ? application.Options.ProvideVersionCommand : (command.ParserOptions.ProvideVersionCommand ?? GetProvideVersionCommand(application, command.ParentCommand));
        private static bool GetProvideHelpOptions(ICliApplicationBase application, ICliCommandInfo? command)
            => command == null ? application.Options.ProvideHelpOptions : (command.ParserOptions.ProvideHelpOptions ?? GetProvideHelpOptions(application, command.ParentCommand));
        private static bool GetProvideVersionOptions(ICliApplicationBase application, ICliCommandInfo? command)
            => command == null ? application.Options.ProvideVersionOptions : (command.ParserOptions.ProvideVersionOptions ?? GetProvideVersionOptions(application, command.ParentCommand));

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
            public CliExecutionContext ExecutionContext { get; }

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
                ExecutionContext = new CliExecutionContext(application, command);
            }

            private static object CreateOptionsWithDefaultValues(ICliCommandInfo command)
            {
                object result = command.OptionsInstance ?? Activator.CreateInstance(command.CommandType)!;
                foreach (var m in command.Values.Cast<ICliCommandMemberInfo>().Concat(command.Options))
                {
                    if (m.DefaultValue == null && m.GetValue(result) != m.PropertyType.GetDefault())
                        continue;
                    m.SetDefaultValue(result);
                }

                return result;
            }
        }
    }
}
