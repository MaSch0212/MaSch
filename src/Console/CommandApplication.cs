using System;
using System.Collections.Generic;
using System.Linq;
using static System.Environment;

namespace MaSch.Console
{
    /// <summary>
    /// Represents an interactive console application that can run different commands.
    /// </summary>
    public class CommandApplication
    {
        private readonly IConsoleService _console;

        /// <summary>
        /// Gets or sets the commands that can be executed by the user.
        /// </summary>
        public List<Command> Commands { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is exiting.
        /// If set to true, the main loop will be existed.
        /// </summary>
        public bool IsExiting { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApplication"/> class.
        /// </summary>
        /// <param name="console">The console to output to and get input from.</param>
        public CommandApplication(IConsoleService console)
            : this(console, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandApplication"/> class.
        /// </summary>
        /// <param name="console">The console to output to and get input from.</param>
        /// <param name="commands">The commands that can be executed by the user.</param>
        public CommandApplication(IConsoleService console, IEnumerable<Command>? commands)
        {
            _console = console;
            Commands = commands?.ToList() ?? new List<Command>();
            AppendDefaultCommands();
        }

        /// <summary>
        /// Appends the default commands to the <see cref="Commands"/> list. This method is executed by the constructor.
        /// </summary>
        protected virtual void AppendDefaultCommands()
        {
            Commands.Add(new Command
            {
                Action = ShowHelp,
                Names = new string[] { "Help" },
                Parameters = new Dictionary<string, string> { { "Command", "Shows the Description of the given command" } },
                OptionalParameters = 1,
                Description = "Lists all Parameters or shows details of a specific command.",
            });
            Commands.Add(new Command
            {
                Action = (s) => IsExiting = true,
                Names = new string[] { "Exit", "Stop", "End" },
                Parameters = new Dictionary<string, string>(),
                Description = "Stops the Application.",
            });
        }

        /// <summary>
        /// Starts to execute the <see cref="CommandApplication"/>.
        /// </summary>
        public void StartExecuteApplication()
        {
            _console.Write("Type \"help\" to see all possible commands!");
            while (!IsExiting)
            {
                var prevClr = _console.ForegroundColor;
                _console.ForegroundColor = ConsoleColor.Yellow;
                _console.Write($"{NewLine}> ");
                _console.ForegroundColor = prevClr;
                var input = GetFormattedInput(_console.ReadLine());
                if (input != null && input.Length > 0)
                {
                    var cmd = Commands.FirstOrDefault(x =>
                        x.Names.Any(y => y.ToUpper() == input[0].ToUpper()) &&
                        input.Length - 1 <= x.Parameters.Count && input.Length - 1 >= x.Parameters.Count - x.OptionalParameters);
                    if (cmd == null)
                    {
                        if (Commands.Any(x => x.Names.Any(y => y.ToUpper() == input[0].ToUpper())))
                            _console.WriteLine($"The command {input[0]} does not take {input.Length - 1} Arguments!");
                        else
                            _console.WriteLine($"The command {input[0]} does not exist!");
                    }
                    else
                    {
                        try
                        {
                            string[] parameters = new string[cmd.Parameters.Count];
                            for (int i = 1; i < input.Length && i - 1 < parameters.Length; i++)
                                parameters[i - 1] = input[i];
                            cmd.Action(parameters);
                        }
                        catch (Exception ex)
                        {
                            _console.WriteLineWithColor($"An error occured while executing this command:{NewLine}{ex}", ConsoleColor.Red);
                        }
                    }
                }
            }
        }

        private string[]? GetFormattedInput(string? input)
        {
            string[] splitted = input?.Split(new string[] { " " }, StringSplitOptions.None) ?? Array.Empty<string>();
            List<string> formatted = new List<string>();
            int qmCount = 0;
            string current = string.Empty;
            foreach (var s in splitted)
            {
                if (s.StartsWith("\"") && s.EndsWith("\"") && qmCount == 0)
                {
                    if (s == "\"\"")
                        formatted.Add(string.Empty);
                    else
                        formatted.Add(s[1..^1]);
                }
                else if (s.EndsWith("\""))
                {
                    current += " " + s[0..^1];
                    if (qmCount == 0)
                    {
                        _console.WriteLine("The syntax is not valid!");
                        return null;
                    }
                    else if (qmCount == 1)
                    {
                        qmCount = 0;
                        formatted.Add(current);
                    }
                    else if (qmCount > 1)
                    {
                        qmCount--;
                    }
                }
                else if (s.StartsWith("\"") && qmCount == 0)
                {
                    current = s[1..];
                    qmCount = 1;
                }
                else if (qmCount > 0)
                {
                    current += " " + s;
                }
                else
                {
                    formatted.Add(s);
                }
            }

            return formatted.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        /// <summary>
        /// Shows the help text for this instance of the <see cref="CommandApplication"/>.
        /// </summary>
        /// <param name="prms">The command line parameters.</param>
        public void ShowHelp(string[] prms)
        {
            if (string.IsNullOrEmpty(prms[0]))
            {
                _console.WriteLine("You can use the following commands:");
                foreach (var command in Commands.OrderBy(x => x.Names.FirstOrDefault()))
                {
                    _console.WriteLine($"- {string.Join(" | ", command.Names)}{NewLine}    {command.Description.Replace("\r", string.Empty).Replace("\n", $"{NewLine}    ")}");
                }
            }
            else
            {
                var cmd = Commands.OrderBy(x => x.Names.FirstOrDefault()).FirstOrDefault(x => x.Names.Any(y => y.ToUpper() == prms[0].ToUpper()));
                if (cmd == null)
                {
                    _console.WriteLine("The command {0} does not exist!", prms[0]);
                }
                else
                {
                    _console.WriteLine("Detailed informations about the following command:");
                    _console.WriteLine($"{string.Join(" | ", cmd.Names)}{NewLine}  {cmd.Description}");
                    _console.WriteLine($"{NewLine}Parameters:");
                    int i = 0;
                    foreach (var parameter in cmd.Parameters)
                    {
                        if (i < cmd.Parameters.Count - cmd.OptionalParameters)
                            _console.WriteLine($"- {parameter.Key}{NewLine}    {parameter.Value.Replace("\n", "\n    ")}");
                        else
                            _console.WriteLine($"- {parameter.Key} (Optional){NewLine}    {parameter.Value.Replace("\n", "\n    ")}");
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// Represents a Command of a <see cref="CommandApplication"/>.
        /// </summary>
        public class Command
        {
            /// <summary>
            /// Gets or sets the names that can be used to execute this <see cref="Command"/>.
            /// </summary>
            public string[] Names { get; set; }

            /// <summary>
            /// Gets or sets an action that is executed, when the user requested this <see cref="Command"/>.
            /// </summary>
            public Action<string[]> Action { get; set; }

            /// <summary>
            /// Gets or sets a dictionary of possible parameters that can be used for this <see cref="Command"/>.
            /// </summary>
            public Dictionary<string, string> Parameters { get; set; }

            /// <summary>
            /// Gets or sets the description of this <see cref="Command"/>.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the number of optional parameters of the <see cref="Command"/>.
            /// </summary>
            public int OptionalParameters { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Command" /> class.
            /// </summary>
            public Command()
            {
                Names = Array.Empty<string>();
                Action = (s) => { };
                Parameters = new Dictionary<string, string>();
                Description = "Unknown Description";
                OptionalParameters = 0;
            }
        }
    }
}
