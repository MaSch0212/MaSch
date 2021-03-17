using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Internal;
using MaSch.Core;

namespace MaSch.Console.Cli
{
    public class CliCommandInfo
    {
        private static readonly Regex IllegalNameCharactersRegex = new(@"[\p{Cc}\s]", RegexOptions.Compiled);

        private readonly List<CliCommandInfo> _childCommands = new();
        private readonly List<CliCommandOptionInfo> _options = new();
        private readonly List<CliCommandValueInfo> _values = new();
        private readonly IExecutor? _executor;
        private readonly Cache _cache = new();

        public Type CommandType { get; }
        public object? OptionsInstance { get; }
        public string Name => Attribute.Name;
        public IReadOnlyList<string> Aliases => Attribute.Aliases;
        public bool IsDefault => Attribute.IsDefault;
        public string? HelpText => Attribute.HelpText;
        public int Order => Attribute.HelpOrder;
        public CliCommandInfo? ParentCommand { get; private set; }
        public IReadOnlyList<CliCommandInfo> ChildCommands => _cache.GetValue(() => _childCommands.AsReadOnly())!;
        public IReadOnlyList<CliCommandOptionInfo> Options => _cache.GetValue(() => _options.AsReadOnly())!;
        public IReadOnlyList<CliCommandValueInfo> Values => _cache.GetValue(() => _values.AsReadOnly())!;

        internal CliCommandAttribute Attribute { get; }

        private CliCommandInfo(Type commandType, Type? executorType, object? optionsInstance, object? executorFunc, object? executorInstance)
        {
            CommandType = Guard.NotNull(commandType, nameof(commandType));
            OptionsInstance = Guard.OfType(optionsInstance, nameof(optionsInstance), commandType);

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
                    _executor = ExternalExecutor.GetExecutor(executorType, commandType, executorInstance);
                else
                    _executor = new DirectExecutor(commandType);
            }

            var properties = commandType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in properties)
            {
                var optionAttr = p.GetCustomAttribute<CliCommandOptionAttribute>(true);
                var valueAttr = p.GetCustomAttribute<CliCommandValueAttribute>(true);

                if (optionAttr != null)
                    _options.Add(new CliCommandOptionInfo(this, p, optionAttr));
                if (valueAttr != null)
                    _values.Add(new CliCommandValueInfo(this, p, valueAttr));
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

        public static CliCommandInfo From<TCommand>() => new(typeof(TCommand), null, null, null, null);
        public static CliCommandInfo From<TCommand>(TCommand optionsInstance) => new(typeof(TCommand), null, optionsInstance, null, null);
        public static CliCommandInfo From(Type commandType) => new(commandType, null, null, null, null);
        public static CliCommandInfo From(Type commandType, object? optionsInstance) => new(commandType, null, optionsInstance, null, null);
        public static CliCommandInfo From<TCommand, TExecutor>() => new(typeof(TCommand), typeof(TExecutor), null, null, null);
        public static CliCommandInfo From<TCommand, TExecutor>(TExecutor executorInstance) => new(typeof(TCommand), typeof(TExecutor), null, null, executorInstance);
        public static CliCommandInfo From<TCommand, TExecutor>(TCommand optionsInstance) => new(typeof(TCommand), typeof(TExecutor), optionsInstance, null, null);
        public static CliCommandInfo From<TCommand, TExecutor>(TCommand optionsInstance, TExecutor executorInstance) => new(typeof(TCommand), typeof(TExecutor), optionsInstance, null, executorInstance);
        public static CliCommandInfo From(Type commandType, Type? executorType) => new(commandType, executorType, null, null, null);
        public static CliCommandInfo From(Type commandType, Type? executorType, object? executorInstance) => new(commandType, executorType, null, null, executorInstance);
        public static CliCommandInfo From(Type commandType, object? optionsInstance, Type? executorType) => new(commandType, executorType, optionsInstance, null, null);
        public static CliCommandInfo From(Type commandType, object? optionsInstance, Type? executorType, object? executorInstance) => new(commandType, executorType, optionsInstance, null, executorInstance);
        public static CliCommandInfo From<TCommand>(Func<TCommand, int> executorFunction) => new(typeof(TCommand), null, null, executorFunction, null);
        public static CliCommandInfo From<TCommand>(Func<TCommand, int> executorFunction, TCommand optionsInstance) => new(typeof(TCommand), null, optionsInstance, executorFunction, null);
        public static CliCommandInfo From<TCommand>(Func<TCommand, Task<int>> executorFunction) => new(typeof(TCommand), null, null, executorFunction, null);
        public static CliCommandInfo From<TCommand>(Func<TCommand, Task<int>> executorFunction, TCommand optionsInstance) => new(typeof(TCommand), null, optionsInstance, executorFunction, null);
        public static CliCommandInfo From(Type commandType, Func<object, int> executorFunction) => new(commandType, null, null, executorFunction, null);
        public static CliCommandInfo From(Type commandType, Func<object, int> executorFunction, object? optionsInstance) => new(commandType, null, optionsInstance, executorFunction, null);
        public static CliCommandInfo From(Type commandType, Func<object, Task<int>> executorFunction) => new(commandType, null, null, executorFunction, null);
        public static CliCommandInfo From(Type commandType, Func<object, Task<int>> executorFunction, object? optionsInstance) => new(commandType, null, optionsInstance, executorFunction, null);
    }
}
