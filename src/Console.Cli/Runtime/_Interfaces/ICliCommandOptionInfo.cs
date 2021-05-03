using MaSch.Console.Cli.Configuration;
using System;
using System.Collections.Generic;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandOptionInfo
    {
        ICliCommandInfo Command { get; }
        string PropertyName { get; }
        Type PropertyType { get; }
        CliCommandOptionAttribute Attribute { get; }

        IReadOnlyList<char> ShortAliases { get; }
        IReadOnlyList<string> Aliases { get; }
        object? DefaultValue { get; }
        bool IsRequired { get; }
        int HelpOrder { get; }
        string? HelpText { get; }

        void SetValue(object options, object? value);
    }
}
