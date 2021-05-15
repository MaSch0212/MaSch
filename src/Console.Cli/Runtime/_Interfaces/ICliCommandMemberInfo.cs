using System;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandMemberInfo
    {
        ICliCommandInfo Command { get; }
        string PropertyName { get; }
        Type PropertyType { get; }

        bool IsRequired { get; }
        object? DefaultValue { get; }
        string? HelpText { get; }

        void SetValue(object options, object? value);
        object? GetValue(object options);
    }
}
