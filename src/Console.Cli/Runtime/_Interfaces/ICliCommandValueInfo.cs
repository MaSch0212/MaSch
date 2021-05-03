using MaSch.Console.Cli.Configuration;
using System;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandValueInfo
    {
        ICliCommandInfo Command { get; }
        string PropertyName { get; }
        Type PropertyType { get; }
        CliCommandValueAttribute Attribute { get; }

        string DisplayName { get; }
        int Order { get; }
        object? DefaultValue { get; }
        bool IsRequired { get; }
        string? HelpText { get; }

        void SetValue(object options, object? value);
    }
}
