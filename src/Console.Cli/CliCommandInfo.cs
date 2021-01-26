using MaSch.Console.Cli.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MaSch.Core;
using MaSch.Console.Cli.Internal;

namespace MaSch.Console.Cli
{
    public class CliCommandInfo
    {
        private static readonly Regex IllegalNameCharactersRegex = new Regex(@"[\p{Cc}\s]", RegexOptions.Compiled);

        private readonly List<CliCommandInfo> _childCommands = new List<CliCommandInfo>();
        private readonly List<CliCommandOptionInfo> _options = new List<CliCommandOptionInfo>();
        private readonly IExecutor? _executor;

        private IReadOnlyList<CliCommandInfo>? _readonlyChildCommands;
        private IReadOnlyList<CliCommandOptionInfo>? _readonlyOptions;

        public Type CommandType { get; }
        public string Name => Attribute.Name;
        public string[] Aliases => Attribute.Aliases;
        public bool IsDefault => Attribute.IsDefault;
        public string? HelpText => Attribute.HelpText;
        public int Order => Attribute.HelpOrder;
        public CliCommandInfo? ParentCommand { get; private set; }
        public IReadOnlyList<CliCommandInfo> ChildCommands => _readonlyChildCommands ??= _childCommands.AsReadOnly();
        public IReadOnlyList<CliCommandOptionInfo> Options => _readonlyOptions ??= _options.AsReadOnly();

        internal CliCommandAttribute Attribute { get; }

        private CliCommandInfo(Type commandType, Type? executorType, object? executorFunc)
        {
            CommandType = Guard.NotNull(commandType, nameof(commandType));

            Attribute = commandType.GetCustomAttribute<CliCommandAttribute>(true) ?? throw new ArgumentException($"The type \"{commandType.Name}\" does not have a {nameof(CliCommandAttribute)}.", nameof(commandType));
            if (string.IsNullOrWhiteSpace(Attribute.Name))
                throw new ArgumentException($"The name of command \"{commandType.Name}\" cannot be empty.", nameof(commandType));
            if (IllegalNameCharactersRegex.IsMatch(Attribute.Name))
                throw new ArgumentException($"The name of command \"{commandType.Name}\" contains illegal characters. All characters but control and whitespace characters are allowed.", nameof(commandType));

            if (Attribute.Executable)
            {
                if (executorFunc != null)
                    _executor = FunctionExecutor.GetExecutor(executorFunc);
                else if (executorType != null)
                    _executor = ExternalExecutor.GetExecutor(executorType, commandType);
                else
                    _executor = new DirectExecutor(commandType);
            }
        }

        public int Execute(object obj)
            => _executor?.Execute(obj) ?? throw new InvalidOperationException($"The command {Name} is not executable.");
        public async Task<int> ExecuteAsync(object obj)
            => _executor != null ? await _executor.ExecuteAsync(obj) : throw new InvalidOperationException($"The command {Name} is not executable.");

        internal void AddChildCommand(CliCommandInfo childCommand)
        {
            _childCommands.Add(childCommand);
            childCommand.ParentCommand = this;
        }

        internal void RemoveChildCommand(CliCommandInfo childCommand)
        {
            _childCommands.Remove(childCommand);
            if (childCommand.ParentCommand == this)
                childCommand.ParentCommand = null;
        }

        public static CliCommandInfo From<TCommand>() => new CliCommandInfo(typeof(TCommand), null, null);
        public static CliCommandInfo From(Type commandType) => new CliCommandInfo(commandType, null, null);
        public static CliCommandInfo From<TCommand, TExecutor>() => new CliCommandInfo(typeof(TCommand), typeof(TExecutor), null);
        public static CliCommandInfo From(Type commandType, Type? executorType) => new CliCommandInfo(commandType, executorType, null);
        public static CliCommandInfo From<TCommand>(Func<TCommand, int> executorFunction) => new CliCommandInfo(typeof(TCommand), null, executorFunction);
        public static CliCommandInfo From<TCommand>(Func<TCommand, Task<int>> executorFunction) => new CliCommandInfo(typeof(TCommand), null, executorFunction);
        public static CliCommandInfo From(Type commandType, Func<object, int> executorFunction) => new CliCommandInfo(commandType, null, executorFunction);
        public static CliCommandInfo From(Type commandType, Func<object, Task<int>> executorFunction) => new CliCommandInfo(commandType, null, executorFunction);
    }
}
