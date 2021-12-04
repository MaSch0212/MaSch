using MaSch.Console.Cli.Configuration;
using MaSch.Console.Cli.Internal;
using MaSch.Console.Cli.Runtime.Executors;
using MaSch.Core;
using MaSch.Core.Extensions;

namespace MaSch.Console.Cli.Runtime;

/// <inheritdoc/>
public class CliCommandInfo : ICliCommandInfo
{
    private static readonly Regex IllegalNameCharactersRegex = new(@"[\p{Cc}\s]", RegexOptions.Compiled);

    private readonly List<ICliCommandInfo> _childCommands = new();
    private readonly List<ICliCommandOptionInfo> _options = new();
    private readonly List<ICliCommandValueInfo> _values = new();
    private readonly ICliCommandExecutor? _executor;
    private readonly Cache _cache = new();

    internal CliCommandInfo(Type commandType, Type? executorType, object? optionsInstance, object? executorFunc, object? executorInstance)
    {
        CommandType = Guard.NotNull(commandType, nameof(commandType));
        OptionsInstance = Guard.OfType(optionsInstance, nameof(optionsInstance), true, commandType);

        Attribute = (from t in commandType.FlattenHierarchy()
                     let attr = t.GetCustomAttribute<CliCommandAttribute>()
                     where attr != null
                     select attr).FirstOrDefault()
            ?? throw new ArgumentException($"The type \"{commandType.Name}\" does not have a {nameof(CliCommandAttribute)}.", nameof(commandType));

        if (string.IsNullOrWhiteSpace(Attribute.Name))
            throw new ArgumentException($"The name of command \"{commandType.Name}\" cannot be empty.", nameof(commandType));
        if (IllegalNameCharactersRegex.IsMatch(Attribute.Name))
            throw new ArgumentException($"The name of command \"{commandType.Name}\" contains illegal characters. All characters but control and whitespace characters are allowed.", nameof(commandType));

        ParserOptions = GetParserOptions(commandType);

        if (executorFunc != null)
            _executor = FunctionExecutor.GetExecutor(executorFunc);
        else if (executorType != null)
            _executor = ExternalExecutor.GetExecutor(executorType, commandType, executorInstance);
        else if (DirectExecutor.IsExecutable(commandType))
            _executor = new DirectExecutor(commandType);

        var extensionStorage = new ObjectExtensionDataStorage();
        var members = from t in commandType.FlattenHierarchy()
                      from p in t.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                      from a in p.GetCustomAttributes()
                      where a is CliCommandOptionAttribute || a is CliCommandValueAttribute
                      group (p, a) by (p.Name, a.GetType()) into g
                      let first = g.First()
                      let m = first.a switch
                      {
                          CliCommandOptionAttribute oa => (ICliCommandMemberInfo)new CliCommandOptionInfo(extensionStorage, this, first.p, oa),
                          CliCommandValueAttribute va => new CliCommandValueInfo(extensionStorage, this, first.p, va),
                          _ => throw new Exception("Impossible Exception"),
                      }
                      select m;
        foreach (var member in members)
        {
            if (member is ICliCommandOptionInfo option)
                _options.Add(option);
            else if (member is ICliCommandValueInfo value)
                _values.Add(value);
        }
    }

    /// <inheritdoc/>
    public CliCommandAttribute Attribute { get; }

    /// <inheritdoc/>
    public Type CommandType { get; }

    /// <inheritdoc/>
    public object? OptionsInstance { get; }

    /// <inheritdoc/>
    public string Name => Attribute.Name;

    /// <inheritdoc/>
    public IReadOnlyList<string> Aliases => Attribute.Aliases;

    /// <inheritdoc/>
    public bool IsDefault => Attribute.IsDefault;

    /// <inheritdoc/>
    public string? HelpText => Attribute.HelpText;

    /// <inheritdoc/>
    public int Order => Attribute.HelpOrder;

    /// <inheritdoc/>
    public bool IsExecutable => _executor != null;

    /// <inheritdoc/>
    public CliParserOptions ParserOptions { get; }

    /// <inheritdoc/>
    public bool Hidden => Attribute.Hidden;

    /// <inheritdoc/>
    public ICliCommandInfo? ParentCommand { get; private set; }

    /// <inheritdoc/>
    public IReadOnlyList<ICliCommandInfo> ChildCommands => _cache.GetValue(() => _childCommands.AsReadOnly())!;

    /// <inheritdoc/>
    public IReadOnlyList<ICliCommandOptionInfo> Options => _cache.GetValue(() => _options.AsReadOnly())!;

    /// <inheritdoc/>
    public IReadOnlyList<ICliCommandValueInfo> Values => _cache.GetValue(() => _values.AsReadOnly())!;

    /// <inheritdoc/>
    public bool ValidateOptions(CliExecutionContext context, object parameters, [MaybeNullWhen(true)] out IEnumerable<CliError> errors)
    {
        if (_executor == null)
        {
            errors = null;
            return true;
        }
        else
        {
            return _executor.ValidateOptions(context, parameters, out errors);
        }
    }

    /// <inheritdoc/>
    public int Execute(CliExecutionContext context, object obj)
    {
        ValidateExecutionContext(context);
        return _executor?.Execute(context, obj)
            ?? throw new InvalidOperationException($"The command {Name} is not executable.");
    }

    /// <inheritdoc/>
    public async Task<int> ExecuteAsync(CliExecutionContext context, object obj)
    {
        ValidateExecutionContext(context);
        return _executor != null
            ? await _executor.ExecuteAsync(context, obj)
            : throw new InvalidOperationException($"The command {Name} is not executable.");
    }

    /// <inheritdoc/>
    public void AddChildCommand(ICliCommandInfo childCommand)
    {
        _childCommands.Add(childCommand);
        if (childCommand is CliCommandInfo cc)
            cc.ParentCommand = this;
    }

    /// <inheritdoc/>
    public void RemoveChildCommand(ICliCommandInfo childCommand)
    {
        _ = _childCommands.Remove(childCommand);
        if (childCommand.ParentCommand == this && childCommand is CliCommandInfo cc)
            cc.ParentCommand = null;
    }

    private static CliParserOptions GetParserOptions(Type type)
    {
        var result = new CliParserOptions();
        var attributes = from t in type.FlattenHierarchy()
                         from a in t.GetCustomAttributes()
                         where a is CliMetadataAttribute || a is CliParserOptionsAttribute
                         group a by a.GetType() into g
                         select g.First();

        foreach (var attribute in attributes)
        {
            if (attribute is CliMetadataAttribute metadata)
            {
                if (metadata.DisplayName != null)
                    result.Name = metadata.DisplayName;
                if (metadata.Version != null)
                    result.Version = metadata.Version;
                if (metadata.Year != null)
                    result.Year = metadata.Year;
                if (metadata.Author != null)
                    result.Author = metadata.Author;
                if (metadata.CliName != null)
                    result.CliName = metadata.CliName;
            }
            else if (attribute is CliParserOptionsAttribute options)
            {
                if (options.IgnoreUnknownOptionsValue.HasValue)
                    result.IgnoreUnknownOptions = options.IgnoreUnknownOptionsValue;
                if (options.IgnoreAdditionalValuesValue.HasValue)
                    result.IgnoreAdditionalValues = options.IgnoreAdditionalValuesValue;
                if (options.ProvideHelpCommandValue.HasValue)
                    result.ProvideHelpCommand = options.ProvideHelpCommandValue;
                if (options.ProvideVersionCommandValue.HasValue)
                    result.ProvideVersionCommand = options.ProvideVersionCommandValue;
                if (options.ProvideHelpOptionsValue.HasValue)
                    result.ProvideHelpOptions = options.ProvideHelpOptionsValue;
                if (options.ProvideVersionOptionsValue.HasValue)
                    result.ProvideVersionOptions = options.ProvideVersionOptionsValue;
            }
        }

        return result;
    }

    private void ValidateExecutionContext(CliExecutionContext context)
    {
        _ = Guard.NotNull(context, nameof(context));
        if (context.Command != this)
            throw new ArgumentException("The context contains the wrong command. Its instance needs to match this instance of the ICliCommandInfo.");
    }
}
