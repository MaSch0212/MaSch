/*
 * Explanation why methods from IServiceContext and IServiceContext<T> are hidden:
 *
 * C# does not allow methods with same name and parameter list even if one is static and the other is not.
 * To accomplish this there is a workaround though. We can implement the interface using hidden methods and add
 * extension methods for each of the interface methods. After this everything works as expected.
 */

namespace MaSch.Core;

/// <summary>
/// Default implementation of the <see cref="IServiceContext"/> interface.
/// </summary>
public sealed partial class ServiceContext : IServiceContext
{
    private readonly Dictionary<(Type Type, string? Name), object> _services = new();
    private readonly Dictionary<Type, IDisposable> _views = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceContext" /> class.
    /// </summary>
    private ServiceContext()
    {
    }

    /// <inheritdoc/>
    public event ServiceContextEventHandler? Changing;

    /// <inheritdoc/>
    public event ServiceContextEventHandler? Changed;

    /// <inheritdoc/>
    IReadOnlyDictionary<(Type Type, string? Name), object> IServiceContext.GetAllServices()
    {
        return new ReadOnlyDictionary<(Type Type, string? Name), object>(_services);
    }

    /// <inheritdoc/>
    IEnumerable<(string? Name, object Service)> IServiceContext.GetAllServices(Type serviceType)
    {
        return _services.Where(x => x.Key.Type == serviceType).Select(x => (x.Key.Name, x.Value));
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"><paramref name="serviceInstance"/> is not an instance of type <paramref name="serviceType"/>.</exception>
    void IServiceContext.AddService(Type serviceType, object serviceInstance, string? name)
    {
        _ = Guard.OfType(serviceInstance, serviceType);

        var key = (serviceType, name);
        ServiceContextEventArgs eventArgs;
        if (_services.ContainsKey(key))
        {
            eventArgs = new ServiceContextEventArgs(name, serviceType, _services[key], serviceInstance, ServiceAction.Changed);
            Changing?.Invoke(this, eventArgs);
            _services[key] = serviceInstance;
        }
        else
        {
            eventArgs = new ServiceContextEventArgs(name, serviceType, null, serviceInstance, ServiceAction.Added);
            Changing?.Invoke(this, eventArgs);
            _services.Add(key, serviceInstance);
        }

        Changed?.Invoke(this, eventArgs);
    }

    /// <inheritdoc/>
    object? IServiceProvider.GetService(Type serviceType)
    {
        return ((IServiceContext)this).GetService(serviceType, null);
    }

    /// <inheritdoc/>
    /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in this <see cref="IServiceContext"/>.</exception>
    object IServiceContext.GetService(Type serviceType, string? name)
    {
        var key = (serviceType, name);
        if (_services.ContainsKey(key))
            return _services[key];
        else
            throw new KeyNotFoundException($"A service with the Type \"{key}\" could not be found! You need to add an instance of this class to this context.");
    }

    /// <inheritdoc/>
    /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in this <see cref="IServiceContext"/>.</exception>
    void IServiceContext.RemoveService(Type serviceType, string? name)
    {
        var key = (serviceType, name);
        if (_services.ContainsKey(key))
        {
            var eventArgs = new ServiceContextEventArgs(name, serviceType, _services[key], null, ServiceAction.Removed);
            Changing?.Invoke(this, eventArgs);
            _ = _services.Remove(key);
            Changed?.Invoke(this, eventArgs);
        }
        else
        {
            throw new KeyNotFoundException($"A service with the Type \"{key}\" could not be found!");
        }
    }

    /// <inheritdoc/>
    bool IServiceContext.ContainsService(Type serviceType, string? name)
    {
        var key = (serviceType, name);
        return _services.ContainsKey(key);
    }

    /// <inheritdoc/>
    [SuppressMessage("Major Code Smell", "S2971:\"IEnumerable\" LINQs should be simplified", Justification = "ToArray is needed here because items are removed in the ForEach.")]
    void IServiceContext.Clear(Type? type)
    {
        var servicesToRemove = type == null ? _services : _services.Where(x => x.Key.Type == type);

        servicesToRemove
            .Select(x => x.Key)
            .ToArray()
            .ForEach(x => ((IServiceContext)this).RemoveService(x.Type, x.Name));
    }

    /// <inheritdoc/>
    public IServiceContext<T> GetView<T>()
    {
        if (!_views.TryGetValue(typeof(T), out var view))
        {
            view = new ServiceContext<T>(this);
            _views.Add(typeof(T), view);
        }

        return (IServiceContext<T>)view;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _views.Values.ForEach(x => x.Dispose());
        _views.Clear();
        _services.Clear();
    }
}

/// <summary>
/// Default implementation of the <see cref="IServiceContext{T}"/> interface.
/// </summary>
/// <typeparam name="T">The type of services to manage.</typeparam>
public sealed partial class ServiceContext<T> : IServiceContext<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceContext{T}" /> class.
    /// </summary>
    /// <param name="context">The context to wrap.</param>
    internal ServiceContext(IServiceContext context)
    {
        Context = context;
        Context.Changing += OnContextChanging;
        Context.Changed += OnContextChanged;
    }

    /// <inheritdoc/>
    public event ServiceContextEventHandler<T>? Changing;

    /// <inheritdoc/>
    public event ServiceContextEventHandler<T>? Changed;

    /// <summary>
    /// Gets the <see cref="ServiceContext"/> that is wrapped by this <see cref="ServiceContext{T}"/>.
    /// </summary>
    public IServiceContext Context { get; }

    /// <inheritdoc/>
    IEnumerable<(string? Name, T Service)> IServiceContext<T>.GetAllServices()
    {
        return Context.GetAllServices<T>();
    }

    /// <inheritdoc/>
    void IServiceContext<T>.AddService([DisallowNull] T serviceInstance, string? name)
    {
        Context.AddService(serviceInstance, name);
    }

    /// <inheritdoc/>
    [return: NotNull]
    T IServiceContext<T>.GetService(string? name)
    {
        return Context.GetService<T>(name);
    }

    /// <inheritdoc/>
    bool IServiceContext<T>.ContainsService(string? name)
    {
        return Context.ContainsService<T>(name);
    }

    /// <inheritdoc/>
    void IServiceContext<T>.RemoveService(string? name)
    {
        Context.RemoveService(typeof(T), name);
    }

    /// <inheritdoc/>
    void IServiceContext<T>.Clear()
    {
        Context.Clear<T>();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Context.Changing -= OnContextChanging;
        Context.Changed -= OnContextChanged;
    }

    private void OnContextChanging(object? sender, ServiceContextEventArgs e)
    {
        if (typeof(T) == e.Type)
        {
            var eventArgs = new ServiceContextEventArgs<T>(e.Name, (T?)e.OldInstance, (T?)e.NewInstance, e.Action);
            Changing?.Invoke(this, eventArgs);
        }
    }

    private void OnContextChanged(object? sender, ServiceContextEventArgs e)
    {
        if (typeof(T) == e.Type)
        {
            var eventArgs = new ServiceContextEventArgs<T>(e.Name, (T?)e.OldInstance, (T?)e.NewInstance, e.Action);
            Changed?.Invoke(this, eventArgs);
        }
    }
}
