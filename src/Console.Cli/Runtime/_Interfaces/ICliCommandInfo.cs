using MaSch.Console.Cli.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MaSch.Console.Cli.Runtime
{
    /// <summary>
    /// Represents information about a command line interface command.
    /// </summary>
    public interface ICliCommandInfo : ICliValidator<object>
    {
        /// <summary>
        /// Gets the command code attribute.
        /// </summary>
        CliCommandAttribute Attribute { get; }

        /// <summary>
        /// Gets the command type.
        /// </summary>
        Type CommandType { get; }

        /// <summary>
        /// Gets the options instance that should be used when executing.
        /// </summary>
        object? OptionsInstance { get; }

        /// <summary>
        /// Gets the name of this command.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the aliases of this command.
        /// </summary>
        IReadOnlyList<string> Aliases { get; }

        /// <summary>
        /// Gets a value indicating whether this command is the default command in its context.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Gets the help text that should be displayed in the command list on the help page.
        /// </summary>
        string? HelpText { get; }

        /// <summary>
        /// Gets a number that is used for sorting when displaying the command list on the help page.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Gets a value indicating whether this command can be executed.
        /// </summary>
        bool IsExecutable { get; }

        /// <summary>
        /// Gets the parent command.
        /// </summary>
        ICliCommandInfo? ParentCommand { get; }

        /// <summary>
        /// Gets the child commands.
        /// </summary>
        IReadOnlyList<ICliCommandInfo> ChildCommands { get; }

        /// <summary>
        /// Gets the options of this command.
        /// </summary>
        IReadOnlyList<ICliCommandOptionInfo> Options { get; }

        /// <summary>
        /// Gets the values of this command.
        /// </summary>
        IReadOnlyList<ICliCommandValueInfo> Values { get; }

        /// <summary>
        /// Executed this command.
        /// </summary>
        /// <param name="obj">The options instance to use for execution.</param>
        /// <returns>The exit code.</returns>
        int Execute(object obj);

        /// <summary>
        /// Executed this command asynchronously.
        /// </summary>
        /// <param name="obj">The options instance to use for execution.</param>
        /// <returns>The exit code.</returns>
        Task<int> ExecuteAsync(object obj);

        /// <summary>
        /// Adds a child command to this comand.
        /// </summary>
        /// <param name="childCommand">The child command to add.</param>
        void AddChildCommand(ICliCommandInfo childCommand);

        /// <summary>
        /// Removed a child command from this command.
        /// </summary>
        /// <param name="childCommand">The child command to remove.</param>
        void RemoveChildCommand(ICliCommandInfo childCommand);
    }
}
