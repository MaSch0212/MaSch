using MaSch.Core;
using MaSch.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly ICliApplicationBase _application;
        private readonly IEnumerable<ICliValidator<object>> _validators;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliArgumentParser"/> class.
        /// </summary>
        /// <param name="application">The application for which this parser is created.</param>
        /// <param name="validators">The validators to execute after parsing.</param>
        /// <param name="serviceProvider">The service provider to use to get options instances.</param>
        public CliArgumentParser(ICliApplicationBase application, IEnumerable<ICliValidator<object>> validators, IServiceProvider serviceProvider)
        {
            _application = Guard.NotNull(application, nameof(application));
            _validators = Guard.NotNull(validators, nameof(validators));
            _serviceProvider = Guard.NotNull(serviceProvider, nameof(serviceProvider));
        }

        /// <inheritdoc/>
        public CliArgumentParserResult Parse(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    var defaultCommand = _application.Commands.DefaultCommand;
                    if (defaultCommand == null)
                        return new(new[] { new CliError(CliErrorType.MissingCommand) });
                    else
                        return new(new(_serviceProvider, defaultCommand), Activator.CreateInstance(defaultCommand.CommandType)!);
                }

                var hasCommand = TryParseCommandInfo(args, null, out var command, out var commandForHelp, out var commandArgIndex);
                if (args.Length > commandArgIndex + 1)
                    HandleSpecialCommands(args.Skip(commandArgIndex + 1).ToArray(), commandForHelp);

                if (!hasCommand || command == null)
                    return new(new[] { new CliError(CliErrorType.UnknownCommand, command) { CommandName = args[commandArgIndex + 1] } });
                if (!command.IsExecutable)
                    return new(new[] { new CliError(CliErrorType.CommandNotExecutable, command) });

                var ctx = new CommandParserContext(args.Skip(commandArgIndex + 1).ToArray(), _serviceProvider, command, new List<CliError>());
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
            IEnumerable<CliError>? validationErrors;
            foreach (var validator in _validators)
            {
                if (!validator.ValidateOptions(ctx.ExecutionContext, ctx.OptionsObj, out validationErrors))
                    ctx.Errors.Add(validationErrors);
            }

            if (ctx.OptionsObj is ICliValidatable validatable && !validatable.ValidateOptions(ctx.ExecutionContext, out validationErrors))
                ctx.Errors.Add(validationErrors);

            if (!ctx.Command.ValidateOptions(ctx.ExecutionContext, ctx.OptionsObj, out validationErrors))
                ctx.Errors.Add(validationErrors);
        }

        private bool TryParseCommandInfo(string[] args, ICliCommandInfo? baseCommand, [NotNullWhen(true)] out ICliCommandInfo? command, out ICliCommandInfo? commandForHelp, out int commandArgIndex)
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
                    ? _application.Commands.GetRootCommands()
                    : command.ChildCommands)
                        .FirstOrDefault(x => x.Aliases.Contains(nextCommandName, StringComparer.OrdinalIgnoreCase));
                if (nextCommand == null)
                    break;

                command = nextCommand;
            }

            if (command == null)
            {
                commandForHelp = command;
                command = _application.Commands.DefaultCommand;
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

        private void HandleSpecialCommands(string[] args, ICliCommandInfo? currentCommand)
        {
            if (IsCommand("version"))
                DetermineCommandAndThrow(CliErrorType.VersionRequested, x => GetProvideVersionCommand(x));
            else if (IsOption("version") && GetProvideVersionOptions(currentCommand) && !OptionExists("version"))
                throw GetException(CliErrorType.VersionRequested, currentCommand, Array.Empty<CliError>());

            if (IsCommand("help"))
                DetermineCommandAndThrow(CliErrorType.HelpRequested, x => GetProvideHelpCommand(x));
            else if (IsOption("help") && GetProvideHelpOptions(currentCommand) && !OptionExists("help"))
                throw GetException(CliErrorType.HelpRequested, currentCommand, Array.Empty<CliError>());

            bool IsCommand(string expectedCommand) => string.Equals(args[0], expectedCommand, StringComparison.OrdinalIgnoreCase);
            bool IsOption(string expectedCommand) => string.Equals(args[0], "--" + expectedCommand, StringComparison.OrdinalIgnoreCase);
            bool OptionExists(string optionName) => currentCommand?.Options.Any(x => x.Aliases.Any(y => string.Equals(y, optionName, StringComparison.OrdinalIgnoreCase))) == true;

            void DetermineCommandAndThrow(CliErrorType errorType, Func<ICliCommandInfo?, bool> shouldThrowFunc)
            {
                ICliCommandInfo? command = currentCommand;
                IEnumerable<CliError> additionalErrors = Array.Empty<CliError>();
                if (args.Length > 1 && !TryParseCommandInfo(args.Skip(1).ToArray(), currentCommand, out _, out command, out var idx))
                    additionalErrors = additionalErrors.Append(new CliError(CliErrorType.UnknownCommand) { CommandName = args[idx + 1] });

                if (shouldThrowFunc(command))
                    throw GetException(errorType, command, additionalErrors);
            }

            CliErrorException GetException(CliErrorType errorType, ICliCommandInfo? command, IEnumerable<CliError> additionalErrors)
                => new(additionalErrors.Prepend(new CliError(errorType, command)));
        }

        private void ParseOptions(CommandParserContext ctx)
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
                    else if (!GetIgnoreAdditionalValues(ctx.Command) && !ctx.Errors.Any(x => x.Type == CliErrorType.UnknownValue))
                    {
                        ctx.Errors.Add(new CliError(CliErrorType.UnknownValue, ctx.Command));
                    }
                }
            }
        }

        private void ParseValue(CommandParserContext ctx, ICliCommandValueInfo value)
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

        private void ParseOption(CommandParserContext ctx)
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
                    else if (!GetIgnoreUnknownOptions(ctx.Command))
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
                        HandleSpecialOptions(a[2..], ctx.Command);
                    if (!GetIgnoreUnknownOptions(ctx.Command))
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

        private void HandleSpecialOptions(string optionName, ICliCommandInfo command)
        {
            if (GetProvideVersionOptions(command) && IsOption("version"))
                Throw(CliErrorType.VersionRequested);
            else if (GetProvideHelpOptions(command) && IsOption("help"))
                Throw(CliErrorType.HelpRequested);

            bool IsOption(string expectedOption) => string.Equals(optionName, expectedOption, StringComparison.OrdinalIgnoreCase);
            void Throw(CliErrorType errorType) => throw new CliErrorException(new[] { new CliError(errorType, command) });
        }

        private bool GetIgnoreUnknownOptions(ICliCommandInfo? command)
            => command == null ? _application.Options.IgnoreUnknownOptions : (command.ParserOptions.IgnoreUnknownOptions ?? GetIgnoreUnknownOptions(command.ParentCommand));
        private bool GetIgnoreAdditionalValues(ICliCommandInfo? command)
            => command == null ? _application.Options.IgnoreAdditionalValues : (command.ParserOptions.IgnoreAdditionalValues ?? GetIgnoreAdditionalValues(command.ParentCommand));
        private bool GetProvideHelpCommand(ICliCommandInfo? command)
            => command == null ? _application.Options.ProvideHelpCommand : (command.ParserOptions.ProvideHelpCommand ?? GetProvideHelpCommand(command.ParentCommand));
        private bool GetProvideVersionCommand(ICliCommandInfo? command)
            => command == null ? _application.Options.ProvideVersionCommand : (command.ParserOptions.ProvideVersionCommand ?? GetProvideVersionCommand(command.ParentCommand));
        private bool GetProvideHelpOptions(ICliCommandInfo? command)
            => command == null ? _application.Options.ProvideHelpOptions : (command.ParserOptions.ProvideHelpOptions ?? GetProvideHelpOptions(command.ParentCommand));
        private bool GetProvideVersionOptions(ICliCommandInfo? command)
            => command == null ? _application.Options.ProvideVersionOptions : (command.ParserOptions.ProvideVersionOptions ?? GetProvideVersionOptions(command.ParentCommand));

        /// <summary>
        /// Exception that occurs when somethings fails during cli argument parsing.
        /// </summary>
        [SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "No need for serialization")]
        public class CliErrorException : Exception
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CliErrorException"/> class.
            /// </summary>
            /// <param name="errors">The errors that lead to this exception.</param>
            public CliErrorException(IEnumerable<CliError> errors)
            {
                Errors = errors;
            }

            /// <summary>
            /// Gets the errors that lead to this exception.
            /// </summary>
            public IEnumerable<CliError> Errors { get; }
        }

        private class CommandParserContext
        {
            public CommandParserContext(IList<string> args, IServiceProvider serviceProvider, ICliCommandInfo command, IList<CliError> errors)
            {
                Args = args;
                Command = command;
                OptionsObj = CreateOptionsWithDefaultValues(serviceProvider, command);
                Errors = errors;
                ExecutionContext = new CliExecutionContext(serviceProvider, command);
            }

            public IList<string> Args { get; }
            public ICliCommandInfo Command { get; }
            public object OptionsObj { get; }
            public IList<CliError> Errors { get; }
            public CliExecutionContext ExecutionContext { get; }

            public int ArgIndex { get; set; }
            public bool IgnoreNextValues { get; set; }
            public int CurrentValueIndex { get; set; }
            public bool AreOptionsEscaped { get; set; }

            private static object CreateOptionsWithDefaultValues(IServiceProvider serviceProvider, ICliCommandInfo command)
            {
                object result = command.OptionsInstance ?? serviceProvider.GetRequiredService(command.CommandType);
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
