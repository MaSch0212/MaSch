﻿using MaSch.Console.Cli.Help;
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
    public class CliHelpPage : ICliHelpPage
    {
        private readonly IConsoleService _console;

        public CliHelpPage(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));
        }

        public virtual void Write(ICliApplicationBase application, IEnumerable<CliError>? errors)
        {
            IList<CliError> errorList = errors?.ToList() ?? new List<CliError>();
            if (errorList.Count == 0)
                errorList.Add(new CliError(CliErrorType.Unknown));

            if (errorList.TryFirst(x => x.Type == CliErrorType.VersionRequested, out var vError))
                WriteVersionPage(application, vError);
            else if (errorList.TryFirst(x => x.Type == CliErrorType.HelpRequested, out var hError))
                WriteHelpPage(application, hError);
            else
                WriteErrorPage(application, errorList);
        }

        protected virtual void WriteVersionPage(ICliApplicationBase application, CliError error)
        {
            WriteCommandNameAndVersion(application, error);
            WriteCopyright(application, error);
        }

        protected virtual void WriteHelpPage(ICliApplicationBase application, CliError error)
        {
            WriteCommandNameAndVersion(application, error);
            WriteCopyright(application, error);

            _console.WriteLine();
            WriteCommandUsage(application, error);
            WriteCommandParameters(application, error);
            WriteCommands(application, error);
        }

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

        protected virtual void WriteCommandNameAndVersion(ICliApplicationBase application, CliError error)
            => _console.WriteLine($"{application.Options.Name} {application.Options.Version}");

        protected virtual void WriteCopyright(ICliApplicationBase application, CliError error)
            => _console.WriteLine($"Copyright {(_console.IsFancyConsole ? "©" : "(C)")} {application.Options.Year} {application.Options.Author}");

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
            _console.WriteLineWithColor(
                message + (error.Exception != null ? $" {error.Exception.Message}" : string.Empty),
                ConsoleColor.Red);

            if (error.Exception != null)
                _console.WriteLineWithColor(error.Exception.Message, ConsoleColor.Red);
        }

        protected virtual void WriteCommandUsage(ICliApplicationBase application, CliError error)
        {
            var sb = new StringBuilder($"{application.Options.CliName}");

            if (error.AffectedCommand != null)
                AppendCommandName(error.AffectedCommand);

            var childCommands = error.AffectedCommand?.ChildCommands ?? application.Commands.GetRootCommands().ToArray();
            if (childCommands.Count > 0)
            {
                var childCommandRequired = error.AffectedCommand?.IsExecutable != true;
                sb.Append(' ')
                  .Append(childCommandRequired ? '<' : '[')
                  .Append("command")
                  .Append(childCommandRequired ? '>' : ']');
            }

            if (error.AffectedCommand != null && error.AffectedCommand.Options.Count > 0)
            {
                sb.Append(" [parameters]");
            }

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

        protected virtual void WriteCommandParameters(ICliApplicationBase application, CliError error)
        {
            if (error.AffectedCommand == null || error.AffectedCommand.Options.Count == 0)
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
            _console.WriteLine();
            _console.WriteLine("Commands:");
            table.Rows.Add(commands.OrderBy(x => x.Name).Select(x => new Row
            {
                Values = new[]
                {
                    string.Empty,
                    x.Name,
                    (x.IsDefault ? "(Default) " : string.Empty) + x.HelpText,
                },
            }));
            table.Render();
        }

        private static string GetOptionName(ICliCommandOptionInfo option)
            => string.Join(", ", option.ShortAliases.Select(y => $"-{y}").Concat(option.Aliases.Select(y => $"--{y}")));
    }
}
