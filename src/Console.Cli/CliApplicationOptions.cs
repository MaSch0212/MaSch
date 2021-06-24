using MaSch.Console.Cli.Runtime;
using System;
using System.Globalization;
using System.Reflection;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Options for an <see cref="ICliApplicationBase"/>.
    /// </summary>
    public class CliApplicationOptions : CliParserOptions
    {
        /// <summary>
        /// Gets or sets the exit code that should be returned when parsing of command line arguments fails.
        /// </summary>
        public int ParseErrorExitCode { get; set; } = -1;

        /// <summary>
        /// Gets or sets the console service that should be used for the application.
        /// </summary>
        public IConsoleService ConsoleService { get; set; } = new ConsoleService();

        /// <summary>
        /// Gets or sets a value indicating whether unknown options should be ignored while parsing command line arguments.
        /// </summary>
        public new bool IgnoreUnknownOptions
        {
            get => base.IgnoreUnknownOptions ?? false;
            set => base.IgnoreUnknownOptions = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether additional values should be ignored while parsing command line arguments.
        /// </summary>
        public new bool IgnoreAdditionalValues
        {
            get => base.IgnoreAdditionalValues ?? false;
            set => base.IgnoreAdditionalValues = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "help" command should be automatically provided.
        /// </summary>
        public new bool ProvideHelpCommand
        {
            get => base.ProvideHelpCommand ?? true;
            set => base.ProvideHelpCommand = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "version" command should be automatically provided.
        /// </summary>
        public new bool ProvideVersionCommand
        {
            get => base.ProvideVersionCommand ?? true;
            set => base.ProvideVersionCommand = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "help" option should be automatically provided to all commands.
        /// </summary>
        public new bool ProvideHelpOptions
        {
            get => base.ProvideHelpOptions ?? true;
            set => base.ProvideHelpOptions = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "version" option should be automatically provided to all commands.
        /// </summary>
        public new bool ProvideVersionOptions
        {
            get => base.ProvideVersionOptions ?? true;
            set => base.ProvideVersionOptions = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CliApplicationOptions"/> class.
        /// </summary>
        public CliApplicationOptions()
        {
            Name = Assembly.GetEntryAssembly()?.GetName().Name;
            Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3);
            Year = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            Author = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
            CliName = Assembly.GetEntryAssembly()?.GetName().Name;
        }
    }
}
