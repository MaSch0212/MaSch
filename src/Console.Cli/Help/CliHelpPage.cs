using MaSch.Console.Cli.ErrorHandling;
using MaSch.Console.Cli.Runtime;
using MaSch.Console.Controls;
using MaSch.Console.Controls.Table;
using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Linq;
using System.Text;

namespace MaSch.Console.Cli.Help
{
    public class CliHelpPage : ICliHelpPage
    {
        private readonly IConsoleService _console;

        public CliHelpPage(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));
        }

        public virtual void Write(ICliApplicationBase application, CliError error)
        {
            switch (error.Type)
            {
                case CliErrorType.VersionRequested:
                    WriteVersionPage(application, error);
                    break;
                case CliErrorType.HelpRequested:
                    WriteHelpPage(application, error);
                    break;
                default:
                    WriteErrorPage(application, error);
                    break;
            }
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

        protected virtual void WriteErrorPage(ICliApplicationBase application, CliError error)
        {
            WriteCommandNameAndVersion(application, error);
            WriteCopyright(application, error);

            _console.WriteLine();
            WriteErrorMessage(application, error);

            _console.WriteLine();
            WriteCommandUsage(application, error);
            WriteCommandParameters(application, error);
            WriteCommands(application, error);
        }

        protected virtual void WriteCommandNameAndVersion(ICliApplicationBase application, CliError error)
            => _console.WriteLine($"{application.Options.Name} {application.Options.Version}");

        protected virtual void WriteCopyright(ICliApplicationBase application, CliError error)
            => _console.WriteLine($"Copyright {(_console.IsFancyConsole ? "©" : "(C)")} {application.Options.Year} {application.Options.Author}");

        protected virtual void WriteErrorMessage(ICliApplicationBase application, CliError error)
        {
            _console.WriteLineWithColor(
                error.Type switch
                {
                    CliErrorType.UnknownCommand => $"The command \"{error.CommandName}\" is unknown.",
                    CliErrorType.UnknownOption => $"The option \"{error.OptionName}\" is unknown.",
                    CliErrorType.UnknownValue => $"Too many values given.",
                    CliErrorType.MissingCommand => $"No command has been provided.",
                    CliErrorType.MissingOption => $"The option {GetOptionName(error.AffectedOption!)} is required.",
                    CliErrorType.MissingValue => $"One or more values for this command are missing.",
                    CliErrorType.WrongOptionFormat => $"The value for option {GetOptionName(error.AffectedOption!)} has the wrong format.",
                    CliErrorType.WrongValueFormat => $"The value {error.AffectedValue!.DisplayName} has the wrong format.",
                    CliErrorType.Custom => error.CustomErrorMessage,
                    _ => "Unknown error.",
                },
                ConsoleColor.Red);
        }

        protected virtual void WriteCommandUsage(ICliApplicationBase application, CliError error)
        {
            var sb = new StringBuilder($"{application.Options.CliName}");

            if (error.AffectedCommand != null)
                AppendCommandName(error.AffectedCommand);

            var childCommands = error.AffectedCommand?.ChildCommands ?? application.Commands;
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
            var commands = error.AffectedCommand?.ChildCommands ?? application.Commands;
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
