using MaSch.Console.Cli.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    public interface ICliCommandInfo : ICliValidator<object>
    {
        CliCommandAttribute Attribute { get; }
        Type CommandType { get; }
        object? OptionsInstance { get; }
        string Name { get; }
        IReadOnlyList<string> Aliases { get; }
        bool IsDefault { get; }
        string? HelpText { get; }
        int Order { get; }
        bool IsExecutable { get; }
        ICliCommandInfo? ParentCommand { get; }
        IReadOnlyList<ICliCommandInfo> ChildCommands { get; }
        IReadOnlyList<ICliCommandOptionInfo> Options { get; }
        IReadOnlyList<ICliCommandValueInfo> Values { get; }

        int Execute(object obj);
        Task<int> ExecuteAsync(object obj);

        void AddChildCommand(ICliCommandInfo childCommand);
        void RemoveChildCommand(ICliCommandInfo childCommand);
    }
}
