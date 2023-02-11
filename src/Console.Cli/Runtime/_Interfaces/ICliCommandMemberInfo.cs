namespace MaSch.Console.Cli.Runtime;

/// <summary>
/// Represents information about a member of a command line interface command.
/// </summary>
public interface ICliCommandMemberInfo
{
    /// <summary>
    /// Gets the command to which this member belongs to.
    /// </summary>
    ICliCommandInfo Command { get; }

    /// <summary>
    /// Gets the property name that this member represents from the class that is represented by the <see cref="Command"/>.
    /// </summary>
    string PropertyName { get; }

    /// <summary>
    /// Gets the type of the property refered by the <see cref="PropertyName"/>.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    Type PropertyType { get; }

    /// <summary>
    /// Gets a value indicating whether this member is required.
    /// </summary>
    bool IsRequired { get; }

    /// <summary>
    /// Gets the default value of this member.
    /// </summary>
    object? DefaultValue { get; }

    /// <summary>
    /// Gets a help text that is displayed on the help page.
    /// </summary>
    string? HelpText { get; }

    /// <summary>
    /// Gets a value indicating whether this member should be hidden from the help page.
    /// </summary>
    bool Hidden { get; }

    /// <summary>
    /// Sets the default value of this member to the specified command instance.
    /// </summary>
    /// <param name="options">The command instance.</param>
    void SetDefaultValue(object options);

    /// <summary>
    /// Sets the specified value to the specified command instance.
    /// </summary>
    /// <param name="options">The command instance.</param>
    /// <param name="value">The value to set.</param>
    void SetValue(object options, object? value);

    /// <summary>
    /// Gets the value of the specified command instance.
    /// </summary>
    /// <param name="options">The command instance.</param>
    /// <returns>The value of <paramref name="options"/>.</returns>
    object? GetValue(object options);

    /// <summary>
    /// Determines whether the specified command instance has a value.
    /// </summary>
    /// <param name="options">The command instance.</param>
    /// <returns><c>true</c> when <paramref name="options"/> has a value for this member; otherwise <c>false</c>.</returns>
    bool HasValue(object options);
}
