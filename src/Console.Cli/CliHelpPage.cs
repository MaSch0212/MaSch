using MaSch.Console.Cli.Runtime;
using MaSch.Console.Controls;
using MaSch.Console.Controls.Table;
using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaSch.Console.Cli
{
    /// <inheritdoc/>
    public class CliHelpPage : ICliHelpPage
    {
        private readonly IConsoleService _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliHelpPage"/> class.
        /// </summary>
        /// <param name="console">The console that is used for writing.</param>
        public CliHelpPage(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));
        }

        /// <inheritdoc/>
        public virtual bool Write(ICliApplicationBase application, IEnumerable<CliError>? errors)
        {
            Guard.NotNull(application, nameof(application));

            IList<CliError> errorList = errors?.Where(x => x != null).ToList() ?? new List<CliError>();
            if (errorList.Count == 0)
                errorList.Add(new CliError(CliErrorType.Unknown));

            if (errorList.TryFirst(x => x.Type == CliErrorType.HelpRequested, out var hError))
            {
                WriteHelpPage(application, hError);
                return true;
            }
            else if (errorList.TryFirst(x => x.Type == CliErrorType.VersionRequested, out var vError))
            {
                WriteVersionPage(application, vError);
                return true;
            }
            else
            {
                WriteErrorPage(application, errorList);
                return false;
            }
        }

        /// <summary>
        /// Writes the version page.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteVersionPage(ICliApplicationBase application, CliError error)
        {
            WriteCommandNameAndVersion(application, error);
            WriteCopyright(application, error);
        }

        /// <summary>
        /// Writes the help page.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteHelpPage(ICliApplicationBase application, CliError error)
        {
            WriteCommandNameAndVersion(application, error);
            WriteCopyright(application, error);

            _console.WriteLine();
            WriteCommandUsage(application, error);
            WriteCommandParameters(application, error);
            WriteCommands(application, error);
        }

        /// <summary>
        /// Writes the error page.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="errors">The errors that occured.</param>
        protected virtual void WriteErrorPage(ICliApplicationBase application, IList<CliError> errors)
        {
            WriteCommandNameAndVersion(application, errors[0]);
            WriteCopyright(application, errors[0]);

            _console.WriteLine();
            foreach (var error in errors)
                WriteErrorMessage(application, error);

            _console.WriteLine();
            WriteCommandUsage(application, errors[0]);
            WriteCommandParameters(application, errors[0]);
            WriteCommands(application, errors[0]);
        }

        /// <summary>
        /// Writes the command name and version.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCommandNameAndVersion(ICliApplicationBase application, CliError error)
            => _console.WriteLine($"{application.Options.Name} {application.Options.Version}");

        /// <summary>
        /// Writes the copyright.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCopyright(ICliApplicationBase application, CliError error)
            => _console.WriteLine($"Copyright {(_console.IsFancyConsole ? "©" : "(C)")} {application.Options.Year} {application.Options.Author}");

        /// <summary>
        /// Writes an error message.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteErrorMessage(ICliApplicationBase application, CliError error)
        {
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
                CliErrorType.Custom => error.CustomErrorMessage,
                _ => "An unknown error occured.",
            };
            _console.WriteLineWithColor(message, ConsoleColor.Red);

            if (error.Exception != null)
                _console.WriteLineWithColor("    " + string.Join(Environment.NewLine + "    ", error.Exception.Message.Replace("\r", string.Empty).Split('\n')), ConsoleColor.Red);
        }

        /// <summary>
        /// Writes the command usage.
        /// </summary>
        /// <param name="application">The application in which the error(s) occured.</param>
        /// <param name="error">The error.</param>
        protected virtual void WriteCommandUsage(ICliApplicationBase application, CliError error)
        {
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
                      .Append(value.IsRequired ? "<" : "[")
                      .Append(value.DisplayName)
                      .Append(value.IsRequired ? ">" : "]");
                }
            }

            if (error.AffectedCommand != null && error.AffectedCommand.Options.Count > 0)
                sb.Append(" [options]");

            var usg = "Usage: ";
            var tb = new TextBlockControl(_console)
            {
                Text = sb.ToString(),
                X = usg.Length,
            };
            bool isFirst = true;
            foreach (var line in tb.GetTextLines())
            {
                _console.Write(isFirst ? usg : new string(' ', usg.Length));
                _console.WriteLine(line);
                isFirst = false;
            }

            void AppendCommandName(ICliCommandInfo cmd)
            {
                if (cmd.ParentCommand != null)
                    AppendCommandName(cmd.ParentCommand);
                if (cmd.Aliases.Count == 1)
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
            if (error.AffectedCommand == null || (error.AffectedCommand.Options.Count == 0 && error.AffectedCommand.Values.Count == 0))
                return;

            var table = new TableControl(_console)
            {
                ShowColumnHeaders = false,
                Columns =
                {
                    new Column
                    {
                        Width = 0,
                        WidthMode = ColumnWidthMode.Fixed,
                    },
                    new Column
                    {
                        WidthMode = ColumnWidthMode.Auto,
                        MaxWidth = _console.BufferSize.Width / 3,
                    },
                    new Column
                    {
                        Width = 1,
                        WidthMode = ColumnWidthMode.Star,
                    },
                },
            };
            table.Columns[1].NonWrappingChars.Add('-');

            var values = (from o in error.AffectedCommand.Values
                          orderby o.Order
                          select o).ToArray();
            if (values.Length > 0)
            {
                _console.WriteLine();
                _console.WriteLine("Values:");
                table.Rows.Set(values.Select(x => new Row
                {
                    Values = new[]
                    {
                        string.Empty,
                        x.DisplayName,
                        x.HelpText ?? string.Empty,
                    },
                }));
                table.Render();
            }

            var requiredOptions = (from o in error.AffectedCommand.Options
                                   where o.IsRequired
                                   orderby o.HelpOrder, o.ShortAliases.TryFirst(out var s) ? s.ToString() : o.Aliases[0]
                                   select o).ToArray();
            if (requiredOptions.Length > 0)
            {
                _console.WriteLine();
                _console.WriteLine("Required options:");
                table.Rows.Set(requiredOptions.Select(x => new Row
                {
                    Values = new[]
                    {
                        string.Empty,
                        GetOptionName(x),
                        x.HelpText ?? string.Empty,
                    },
                }));
                table.Render();
            }

            var optionalOptions = (from o in error.AffectedCommand.Options
                                   where !o.IsRequired
                                   orderby o.HelpOrder, o.ShortAliases.TryFirst(out var s) ? s.ToString() : o.Aliases[0]
                                   select o).ToArray();
            if (optionalOptions.Length > 0)
            {
                _console.WriteLine();
                _console.WriteLine("Optional options:");
                table.Rows.Set(optionalOptions.Select(x => new Row
                {
                    Values = new[]
                    {
                        string.Empty,
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
            var commands = error.AffectedCommand?.ChildCommands ?? application.Commands.GetRootCommands().ToArray();
            if (commands.Count == 0)
                return;

            var table = new TableControl(_console)
            {
                ShowColumnHeaders = false,
                Columns =
                {
                    new Column
                    {
                        Width = 0,
                        WidthMode = ColumnWidthMode.Fixed,
                    },
                    new Column
                    {
                        WidthMode = ColumnWidthMode.Auto,
                        MaxWidth = _console.BufferSize.Width / 3,
                    },
                    new Column
                    {
                        Width = 1,
                        WidthMode = ColumnWidthMode.Star,
                    },
                },
            };
            table.Columns[1].NonWrappingChars.Add('-');

            _console.WriteLine();
            _console.WriteLine("Commands:");
            table.Rows.Add(commands.OrderByDescending(x => x.IsDefault).ThenBy(x => x.Order).ThenBy(x => x.Name).Select(x => new Row
            {
                Values = new[]
                {
                    string.Empty,
                    x.Aliases.Count > 1 ? $"{string.Join(", ", x.Aliases)}" : x.Name,
                    (x.IsDefault ? "(Default) " : string.Empty) + x.HelpText,
                },
            }));

            table.Render();
        }

        private static string GetOptionName(ICliCommandOptionInfo option)
            => string.Join(", ", option.ShortAliases.Select(y => $"-{y}").Concat(option.Aliases.Select(y => $"--{y}")));
    }
}
