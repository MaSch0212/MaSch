using System;

namespace MaSch.Console.Cli.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CliCommandValueAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public int Order { get; set; }
        public object? Default { get; set; }
        public bool Required { get; set; }
        public string? HelpText { get; set; }

        public CliCommandValueAttribute(int order, string displayName)
        {
            Order = order;
            DisplayName = displayName;
        }
    }
}
