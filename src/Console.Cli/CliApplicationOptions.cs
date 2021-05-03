using MaSch.Console.Cli.Help;
using System;
using System.Globalization;
using System.Reflection;

namespace MaSch.Console.Cli
{
    public sealed class CliApplicationOptions
    {
        public ICliHelpPage HelpPage { get; init; } = new CliHelpPage(new ConsoleService());
        public string? Name { get; init; } = Assembly.GetEntryAssembly()?.GetName().Name;
        public string? Version { get; init; } = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3);
        public string? Year { get; init; } = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
        public string? Author { get; init; } = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
        public string? CliName { get; init; } = Assembly.GetEntryAssembly()?.GetName().Name;

        public bool IgnoreUnknownOptions { get; init; }
        public bool IgnoreAdditionalValues { get; init; }
        public bool ProvideHelpCommand { get; init; }
        public bool ProvideVersionCommand { get; init; }
        public bool ProvideHelpOptions { get; init; }
        public bool ProvideVersionOptions { get; init; }
    }
}
