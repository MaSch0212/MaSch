using System;
using System.Globalization;
using System.Reflection;

namespace MaSch.Console.Cli
{
    public class CliApplicationOptions
    {
        public string? Name { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;
        public string? Version { get; set; } = Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3);
        public string? Year { get; set; } = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
        public string? Author { get; set; } = Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company;
        public string? CliName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        public bool IgnoreUnknownOptions { get; set; } = false;
        public bool IgnoreAdditionalValues { get; set; } = false;
        public bool ProvideHelpCommand { get; set; } = true;
        public bool ProvideVersionCommand { get; set; } = true;
        public bool ProvideHelpOptions { get; set; } = true;
        public bool ProvideVersionOptions { get; set; } = true;
    }
}
