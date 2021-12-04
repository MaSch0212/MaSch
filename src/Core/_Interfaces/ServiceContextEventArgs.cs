namespace MaSch.Core;

/// <summary>
/// Event Handler for the <see cref="IServiceContext.Changing"/> and <see cref="IServiceContext.Changed"/> events.
/// </summary>
/// <param name="sender">The send of this event.</param>
/// <param name="e">The event argument of this event.</param>
public delegate void ServiceContextEventHandler(object? sender, ServiceContextEventArgs e);

/// <summary>
/// Event Handler for the <see cref="IServiceContext{T}.Changing"/> and <see cref="IServiceContext{T}.Changed"/> events.
/// </summary>
/// <typeparam name="T">The type of the instance that changed.</typeparam>
/// <param name="sender">The send of this event.</param>
/// <param name="e">The event argument of this event.</param>
public delegate void ServiceContextEventHandler<T>(object? sender, ServiceContextEventArgs<T> e);

/// <summary>
/// The event argument for <see cref="ServiceContextEventHandler"/>.
/// </summary>
/// <param name="Name">The name of the service on which an action has been taken on.</param>
/// <param name="Type">The type of the service on which an action has been taken on.</param>
/// <param name="OldInstance">The old instance of the service on which an action has been taken on.</param>
/// <param name="NewInstance">The new instance of the service on which an action has been taken on.</param>
/// <param name="Action">The type of action that has been taken.</param>
public sealed record ServiceContextEventArgs(string? Name, Type Type, object? OldInstance, object? NewInstance, ServiceAction Action)
{
    /// <summary>
    /// Gets the name of the service on which an action has been taken on.
    /// </summary>
    public string? Name { get; init; } = Name;

    /// <summary>
    /// Gets the type of the service on which an action has been taken on.
    /// </summary>
    public Type Type { get; init; } = Type;

    /// <summary>
    /// Gets the old instance of the service on which an action has been taken on.
    /// </summary>
    public object? OldInstance { get; init; } = OldInstance;

    /// <summary>
    /// Gets the new instance of the service on which an action has been taken on.
    /// </summary>
    public object? NewInstance { get; init; } = NewInstance;

    /// <summary>
    /// Gets the type of action that has been taken.
    /// </summary>
    public ServiceAction Action { get; init; } = Action;
}

/// <summary>
/// The event argument for <see cref="ServiceContextEventHandler{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the instance that changed.</typeparam>
/// <param name="Name">The name of the service on which an action has been taken on.</param>
/// <param name="OldInstance">The old instance of the service on which an action has been taken on.</param>
/// <param name="NewInstance">The new instance of the service on which an action has been taken on.</param>
/// <param name="Action">The type of action that has been taken.</param>
public sealed record ServiceContextEventArgs<T>(string? Name, T? OldInstance, T? NewInstance, ServiceAction Action)
{
    /// <summary>
    /// Gets the name of the service on which an action has been taken on.
    /// </summary>
    public string? Name { get; init; } = Name;

    /// <summary>
    /// Gets the type of the service on which an action has been taken on.
    /// </summary>
    public Type Type => typeof(T);

    /// <summary>
    /// Gets the old instance of the service on which an action has been taken on.
    /// </summary>
    public T? OldInstance { get; init; } = OldInstance;

    /// <summary>
    /// Gets the new instance of the service on which an action has been taken on.
    /// </summary>
    public T? NewInstance { get; init; } = NewInstance;

    /// <summary>
    /// Gets the type of action that has been taken.
    /// </summary>
    public ServiceAction Action { get; init; } = Action;
}
