using System;
using System.Globalization;
using System.Reflection;

namespace MaSch.Console.Cli
{
    /// <summary>
    /// Options for an <see cref="ICliApplicationBase"/>.
    /// </summary>
    public class CliApplicationOptions
    {
        /// <summary>
        /// Gets or sets the exit code that should be returned when parsing of command line arguments fails.
        /// </summary>
        public int ParseErrorExitCode { get; set; } = -1;

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        public string? Name { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        /// <summary>
        /// Gets or sets the version of the application.
        /// </summary>
        public string? Version { get; set; } = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3);

        /// <summary>
        /// Gets or sets the year in which the version of the application has been released.
        /// </summary>
        public string? Year { get; set; } = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Gets or sets the author of the application.
        /// </summary>
        public string? Author { get; set; } = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;

        /// <summary>
        /// Gets or sets the name of the executable file of the application.
        /// </summary>
        public string? CliName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        /// <summary>
        /// Gets or sets a value indicating whether unknown options should be ignored while parsing command line arguments in the application.
        /// </summary>
        public bool IgnoreUnknownOptions { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether additional values should be ignored while parsing command line arguments in the application.
        /// </summary>
        public bool IgnoreAdditionalValues { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the "help" command should be automatically provided with the application.
        /// </summary>
        public bool ProvideHelpCommand { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the "version" command should be automatically provided with the application.
        /// </summary>
        public bool ProvideVersionCommand { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the "help" option should be automatically provided to all commands of the application.
        /// </summary>
        public bool ProvideHelpOptions { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the "version" option should be automatically provided to all commands of the application.
        /// </summary>
        public bool ProvideVersionOptions { get; set; } = true;
    }
}
