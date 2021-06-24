using MaSch.Console.Cli.Runtime;
using MaSch.Console.Controls;
using MaSch.Console.Controls.Table;
using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaSch.Console.Cli
{
    /// <inheritdoc/>
    public class CliHelpPage : ICliHelpPage
    {
        /// <inheritdoc/>
        public virtual bool Write(ICliApplicationBase application, IEnumerable<CliError>? errors)
        {
            Guard.NotNull(application, nameof(application));

            IList<CliError> errorList = errors?.Where(x => x != null).ToList() ?? new List<CliError>();
            if (errorList.Count == 0)
                errorList.Add(new CliError(CliErrorType.Unknown));

            if (errorList.TryFirst(x => x.Type == CliErrorType.HelpRequested, out var hError))
            {
                var e = errorList.Where(x => x.IsError).Prepend(hError).ToArray();
                WriteHelpPage(application, e);
                return true;
            }
            else if (errorList.TryFirst(x => x.Type == CliErrorType.VersionRequested, out var vError))
            {
                var e = errorList.Where(x => x.IsError).Prepend(vError).ToArray();
                WriteVersionPage(application, e);
                return true;
            }
            else
            {
                WriteHelpPage(application, errorList);
                return false;
            }
        }

        /// <summary>
        /// Writes the version page.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="errors">The errors that occured.</param>
        protected virtual void WriteVersionPage(ICliApplicationBase application, IList<CliError> errors)
        {
            var console = application.Options.ConsoleService;

            WriteCommandNameAndVersion(application, errors[0]);
            WriteCopyright(application, errors[0]);

            if (errors.Any(x => x.IsError))
                console.WriteLine();
            foreach (var error in errors.Where(x => x.IsError))
                WriteErrorMessage(application, error);

            WriteCommandVersions(application, errors[0]);
            console.WriteLine();
        }

        /// <summary>
        /// Writes the help page.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="errors">The errors that occured.</param>
        protected virtual void WriteHelpPage(ICliApplicationBase application, IList<CliError> errors)
        {
            var console = application.Options.ConsoleService;

            WriteCommandNameAndVersion(application, errors[0]);
            WriteCopyright(application, errors[0]);

            if (errors.Any(x => x.IsError))
                console.WriteLine();
            foreach (var error in errors.Where(x => x.IsError))
                WriteErrorMessage(application, error);

            console.WriteLine();
            WriteCommandUsage(application, errors[0]);
            WriteCommandParameters(application, errors[0]);
            WriteCommands(application, errors[0]);
            console.WriteLine();
        }

        /// <summary>
        /// Writes the command name and version.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCommandNameAndVersion(ICliApplicationBase application, CliError error)
        {
            var console = application.Options.ConsoleService;
            var c = error.AffectedCommand;
            console.WriteLine($"{GetDisplayName(c, application)} {GetVersion(c, application)}");
        }

        /// <summary>
        /// Writes the copyright.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCopyright(ICliApplicationBase application, CliError error)
        {
            var console = application.Options.ConsoleService;
            var c = error.AffectedCommand;
            console.WriteLine($"Copyright {(console.IsFancyConsole ? "©" : "(C)")} {GetYear(c, application)} {GetAuthor(c, application)}");
        }

        /// <summary>
        /// Writes an error message.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteErrorMessage(ICliApplicationBase application, CliError error)
        {
            var console = application.Options.ConsoleService;
            var message = error.Type switch
            {
                CliErrorType.UnknownCommand => $"The command \"{error.CommandName}\" is unknown.",
                CliErrorType.UnknownOption => $"The option \"{error.OptionName}\" is unknown.",
                CliErrorType.UnknownValue => $"Too many values given.",
                CliErrorType.MissingCommand => $"No command has been provided.",
                CliErrorType.MissingOption => $"The option {GetOptionName(error.AffectedOption!)} is required.",
                CliErrorType.MissingOptionValue => $"A value needs to be provided for option {GetOptionName(error.AffectedOption!)}.",
                CliErrorType.MissingValue => $"One or more values for this command are missing.",
                CliErrorType.WrongOptionFormat => $"The value for option {GetOptionName(error.AffectedOption!)} has the wrong format.",
                CliErrorType.WrongValueFormat => $"The value {error.AffectedValue!.DisplayName} has the wrong format.",
                CliErrorType.CommandNotExecutable => $"This command cannot be executed by itself. Please choose a child command listed below.",
                CliErrorType.Custom => error.CustomErrorMessage,
                CliErrorType.HelpRequested => null,
                CliErrorType.VersionRequested => null,
                _ => "An unknown error occured.",
            };

            if (message == null)
                return;

            console.WriteLineWithColor(message, ConsoleColor.Red);

            if (error.Exception != null)
            {
                var lines = new TextBlockControl(console)
                {
                    X = 3,
                    Text = error.Exception.Message,
                }.GetTextLines();
                foreach (var line in lines)
                    console.WriteLineWithColor("   " + line, ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Writes the command usage.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCommandUsage(ICliApplicationBase application, CliError error)
        {
            var console = application.Options.ConsoleService;
            var sb = new StringBuilder(application.Options.CliName);

            if (error.AffectedCommand != null)
                AppendCommandName(error.AffectedCommand);

            var childCommands = error.AffectedCommand?.ChildCommands ?? application.Commands.GetRootCommands();
            if (childCommands.Any())
            {
                var childCommandRequired = error.AffectedCommand?.IsExecutable != true;
                sb.Append(' ')
                  .Append(childCommandRequired ? '<' : '[')
                  .Append("command")
                  .Append(childCommandRequired ? '>' : ']');
            }

            if (error.AffectedCommand != null && error.AffectedCommand.Values.Count > 0)
            {
                foreach (var value in error.AffectedCommand.Values.OrderBy(x => x.Order))
                {
                    sb.Append(' ')
                      .Append(value.IsRequired ? '<' : '[')
                      .Append(value.DisplayName);

                    if (typeof(IEnumerable).IsAssignableFrom(value.PropertyType) && value.PropertyType != typeof(string))
                    {
                        sb.Append(" [")
                          .Append(value.DisplayName)
                          .Append("]...");
                    }

                    sb.Append(value.IsRequired ? '>' : ']');
                }
            }

            if (error.AffectedCommand != null && error.AffectedCommand.Options.Any(x => !x.Hidden))
            {
                var hasRequiredOption = error.AffectedCommand.Options.Any(x => x.IsRequired);
                sb.Append(' ')
                  .Append(hasRequiredOption ? '<' : '[')
                  .Append("options")
                  .Append(hasRequiredOption ? '>' : ']');
            }

            var usg = "Usage: ";
            var tb = new TextBlockControl(console)
            {
                Text = sb.ToString(),
                X = usg.Length,
            };
            bool isFirst = true;
            foreach (var line in tb.GetTextLines())
            {
                console.Write(isFirst ? usg : new string(' ', usg.Length));
                console.WriteLine(line);
                isFirst = false;
            }

            void AppendCommandName(ICliCommandInfo cmd)
            {
                if (cmd.ParentCommand != null)
                    AppendCommandName(cmd.ParentCommand);
                if (cmd.ParserOptions.CliName != null)
                    sb.Append($" {cmd.ParserOptions.CliName}");
                else if (cmd.Aliases.Count == 1)
                    sb.Append($" {cmd.Name}");
                else
                    sb.Append($" ({string.Join("|", cmd.Aliases)})");
            }
        }

        /// <summary>
        /// Writes the command parameter list.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCommandParameters(ICliApplicationBase application, CliError error)
        {
            var console = application.Options.ConsoleService;
            if (error.AffectedCommand == null || (error.AffectedCommand.Options.Count == 0 && error.AffectedCommand.Values.Count == 0))
                return;

            var table = new TableControl(console)
            {
                Margin = new(3, 0, 0, 0),
                ShowColumnHeaders = false,
                Columns =
                {
                    new Column
                    {
                        WidthMode = ColumnWidthMode.Auto,
                        MaxWidth = console.BufferSize.Width / 3,
                    },
                    new Column
                    {
                        Width = 1,
                        WidthMode = ColumnWidthMode.Star,
                    },
                },
            };
            table.Columns[0].NonWrappingChars.Add('-');

            var values = (from o in error.AffectedCommand.Values
                          where !o.Hidden
                          orderby o.Order
                          select o).ToArray();
            if (values.Length > 0)
            {
                console.WriteLine();
                console.WriteLine("Values:");
                table.Rows.Set(values.Select(x => new Row
                {
                    Values = new[]
                    {
                        x.DisplayName,
                        (x.IsRequired ? string.Empty : "(Optional) ") + x.HelpText,
                    },
                }));
                table.Render();
            }

            var requiredOptions = OrderOptions(application, error, error.AffectedCommand.Options.Where(x => !x.Hidden && x.IsRequired)).ToArray();
            if (requiredOptions.Length > 0)
            {
                console.WriteLine();
                console.WriteLine("Required options:");
                table.Rows.Set(requiredOptions.Select(x => new Row
                {
                    Values = new[]
                    {
                        GetOptionName(x),
                        x.HelpText ?? string.Empty,
                    },
                }));
                table.Render();
            }

            var optionalOptions = OrderOptions(application, error, error.AffectedCommand.Options.Where(x => !x.Hidden && !x.IsRequired)).ToArray();
            if (optionalOptions.Length > 0)
            {
                console.WriteLine();
                console.WriteLine("Optional options:");
                table.Rows.Set(optionalOptions.Select(x => new Row
                {
                    Values = new[]
                    {
                        GetOptionName(x),
                        x.HelpText ?? string.Empty,
                    },
                }));
                table.Render();
            }
        }

        /// <summary>
        /// Writes the command list.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCommands(ICliApplicationBase application, CliError error)
        {
            var console = application.Options.ConsoleService;
            var commands = (from command in error.AffectedCommand?.ChildCommands ?? application.Commands.GetRootCommands()
                            where !command.Hidden
                            select command).ToArray();
            if (commands.Length == 0)
                return;

            var table = new TableControl(console)
            {
                Margin = new(3, 0, 0, 0),
                ShowColumnHeaders = false,
                Columns =
                {
                    new Column
                    {
                        WidthMode = ColumnWidthMode.Auto,
                        MaxWidth = console.BufferSize.Width / 3,
                    },
                    new Column
                    {
                        Width = 1,
                        WidthMode = ColumnWidthMode.Star,
                    },
                },
            };
            table.Columns[0].NonWrappingChars.Add('-');

            console.WriteLine();
            console.WriteLine("Commands:");
            table.Rows.Add(OrderCommands(application, error, commands).Select(x => new Row
            {
                Values = new[]
                {
                    x.Aliases.Count > 1 ? $"{string.Join(", ", x.Aliases)}" : x.Name,
                    (x.IsDefault ? "(Default) " : string.Empty) + x.HelpText,
                },
            }));

            table.Render();
        }

        /// <summary>
        /// Writes the command version list.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCommandVersions(ICliApplicationBase application, CliError error)
        {
            var console = application.Options.ConsoleService;
            var commands = (from command in error.AffectedCommand?.ChildCommands ?? application.Commands.GetRootCommands()
                            where !command.Hidden && (
                                command.ParserOptions.Author != null ||
                                command.ParserOptions.Name != null ||
                                command.ParserOptions.Version != null ||
                                command.ParserOptions.Year != null)
                            select command).ToArray();
            if (commands.Length == 0)
                return;

            var table = new TableControl(console)
            {
                Margin = new(3, 0, 0, 0),
                ShowColumnHeaders = false,
                Columns =
                {
                    new Column // Command Name
                    {
                        WidthMode = ColumnWidthMode.Auto,
                        MaxWidth = console.BufferSize.Width / 3,
                    },
                    new Column // Command DisplayName
                    {
                        WidthMode = ColumnWidthMode.Auto,
                    },
                    new Column // Command Version
                    {
                        WidthMode = ColumnWidthMode.Auto,
                    },
                    new Column // Command Copyright
                    {
                        WidthMode = ColumnWidthMode.Auto,
                    },
                },
            };
            table.Columns[0].NonWrappingChars.Add('-');

            console.WriteLine();
            console.WriteLine("Commands:");
            table.Rows.Add(OrderCommands(application, error, commands).Select(x => new Row
            {
                Values = new[]
                {
                    x.Aliases.Count > 1 ? $"{string.Join(", ", x.Aliases)}" : x.Name,
                    x.ParserOptions.Name ?? string.Empty,
                    GetVersion(x, application) ?? string.Empty,
                    $"{(console.IsFancyConsole ? "©" : "(C)")} {GetYear(x, application)} {GetAuthor(x, application)}",
                },
            }));

            table.Render();
        }

        /// <summary>
        /// Orders the specified <see cref="ICliCommandInfo"/> objects. Override to change ordering in commands list.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        /// <param name="commands">The commands to order.</param>
        /// <returns>The ordered commands.</returns>
        protected virtual IEnumerable<ICliCommandInfo> OrderCommands(ICliApplicationBase application, CliError error, IEnumerable<ICliCommandInfo> commands)
        {
            return commands.OrderByDescending(x => x.IsDefault).ThenBy(x => x.Order).ThenBy(x => x.Name);
        }

        /// <summary>
        /// Orders the specified <see cref="ICliCommandOptionInfo"/> objects. Override to change ordering in options list.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        /// <param name="options">The options to order.</param>
        /// <returns>The ordered options.</returns>
        protected virtual IEnumerable<ICliCommandOptionInfo> OrderOptions(ICliApplicationBase application, CliError error, IEnumerable<ICliCommandOptionInfo> options)
        {
            return options.OrderBy(x => x.HelpOrder).ThenBy(x => x.ShortAliases.TryFirst(out var s) ? s.ToString() : x.Aliases[0]);
        }

        /// <summary>
        /// Gets the correct display name from either the command, one of its parent commands or the application.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="app">The application.</param>
        /// <returns>The first name found in the hierarchy.</returns>
        protected static string? GetDisplayName(ICliCommandInfo? command, ICliApplicationBase app)
            => command?.ParserOptions.Name ?? (command?.ParentCommand != null ? GetDisplayName(command.ParentCommand, app) : app.Options.Name);

        /// <summary>
        /// Gets the correct version from either the command, one of its parent commands or the application.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="app">The application.</param>
        /// <returns>The first version found in the hierarchy.</returns>
        protected static string? GetVersion(ICliCommandInfo? command, ICliApplicationBase app)
            => command?.ParserOptions.Version ?? (command?.ParentCommand != null ? GetVersion(command.ParentCommand, app) : app.Options.Version);

        /// <summary>
        /// Gets the correct year from either the command, one of its parent commands or the application.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="app">The application.</param>
        /// <returns>The first year found in the hierarchy.</returns>
        protected static string? GetYear(ICliCommandInfo? command, ICliApplicationBase app)
            => command?.ParserOptions.Year ?? (command?.ParentCommand != null ? GetYear(command.ParentCommand, app) : app.Options.Year);

        /// <summary>
        /// Gets the correct author from either the command, one of its parent commands or the application.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="app">The application.</param>
        /// <returns>The first author found in the hierarchy.</returns>
        protected static string? GetAuthor(ICliCommandInfo? command, ICliApplicationBase app)
            => command?.ParserOptions.Author ?? (command?.ParentCommand != null ? GetAuthor(command.ParentCommand, app) : app.Options.Author);

        private static string GetOptionName(ICliCommandOptionInfo option)
            => string.Join(", ", option.ShortAliases.Select(y => $"-{y}").Concat(option.Aliases.Select(y => $"--{y}")));
    }
}
