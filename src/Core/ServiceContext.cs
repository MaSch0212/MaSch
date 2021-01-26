using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MaSch.Common.Extensions;

namespace MaSch.Common
{
    /// <summary>
    /// Represents a static wrapper for the <see cref="ServiceContextInstance"/> class.
    /// </summary>
    public static class ServiceContext
    {
        private static ServiceContextInstance? _instance;
        /// <summary>
        /// Gets the current instance of the <see cref="ServiceContextInstance"/>.
        /// </summary>
        public static ServiceContextInstance Instance => _instance ??= new ServiceContextInstance();

        /// <summary>
        /// Creates a new instance of the <see cref="ServiceContextInstance"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="ServiceContextInstance"/> class.</returns>
        public static ServiceContextInstance CreateContext() => new ServiceContextInstance();

        /// <summary>
        /// Gets all services of the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <returns>All services of the current <see cref="ServiceContextInstance"/>.</returns>
        public static IReadOnlyDictionary<(Type type, string? name), object> GetAllServices()
            => Instance.GetAllServices();
        /// <summary>
        /// Gets all services of the current <see cref="ServiceContextInstance"/> of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of services to get.</typeparam>
        /// <returns>All services of the current <see cref="ServiceContextInstance"/> of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<(string? name, T service)> GetAllServices<T>()
            => Instance.GetAllServices<T>();
        /// <summary>
        /// Gets all services of the current <see cref="ServiceContextInstance"/> of a specified type.
        /// </summary>
        /// <param name="serviceType">The type of services to get.</param>
        /// <returns>All services of the current <see cref="ServiceContextInstance"/> of type <paramref name="serviceType"/>.</returns>
        public static IEnumerable<(string? name, object service)> GetAllServices(Type serviceType)
            => Instance.GetAllServices(serviceType);

        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        public static void AddService<T>([DisallowNull] T serviceInstance, string? name = null)
            => Instance.AddService(serviceInstance, name);
        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        /// <exception cref="ArgumentException"><paramref name="serviceInstance"/> is not an instance of type <paramref name="serviceType"/>.</exception>
        public static void AddService(Type serviceType, object serviceInstance, string? name = null)
            => Instance.AddService(serviceType, serviceInstance, name);

        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance"/>.</exception>
        public static void GetService<T>(out T result, string? name = null)
            => Instance.GetService(out result, name);
        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance"/>.</exception>
        public static T GetService<T>(string? name = null)
            => Instance.GetService<T>(name);
        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance"/>.</exception>
        public static object GetService(Type serviceType, string? name = null)
            => Instance.GetService(serviceType, name);

        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContextInstance"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetService<T>(out T? result, string? name = null)
            => Instance.TryGetService(out result, name);
        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in the current <see cref="ServiceContextInstance"/>. If no service was found <see langword="default"/> is returned.</returns>
        [return: MaybeNull]
        public static T TryGetService<T>(string? name = null)
            => Instance.TryGetService<T>(name);
        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <paramref name="serviceType"/> and name <paramref name="name"/> that was found in the current <see cref="ServiceContextInstance"/>. If no service was found <see langword="default"/> is returned.</returns>
        public static object? TryGetService(Type serviceType, string? name = null)
            => Instance.TryGetService(serviceType, name);

        /// <summary>
        /// Removes the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance"/>.</exception>
        public static void RemoveService<T>(string? name = null)
            => Instance.RemoveService<T>(name);
        /// <summary>
        /// Removes the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance"/>.</exception>
        public static void RemoveService(Type serviceType, string? name = null)
            => Instance.RemoveService(serviceType, name);

        /// <summary>
        /// Tries to remove the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService<T>(string? name = null)
            => Instance.TryRemoveService<T>(name);
        /// <summary>
        /// Tries to remove the service with the specified type and name from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService(Type serviceType, string? name = null)
            => Instance.TryRemoveService(serviceType, name);

        /// <summary>
        /// Determines wether a service with the specified type and name exists in the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to search for.</typeparam>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContextInstance"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService<T>(string? name = null)
            => Instance.ContainsService<T>(name);
        /// <summary>
        /// Determines wether a service with the specified type and name exists in the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to search for.</param>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <paramref name="serviceType"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContextInstance"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService(Type serviceType, string? name = null)
            => Instance.ContainsService(serviceType, name);

        /// <summary>
        /// Subscribes to changes to a service with the specified type and name in the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to subscribe for changes on.</typeparam>
        /// <param name="action">The action to execute when the service changes.</param>
        /// <param name="name">The name of the service to subscribe for changes on.</param>
        /// <returns>An <see cref="IDisposable"/> object that unsubscribes when disposed.</returns>
        public static IDisposable Subscribe<T>(Action<T?> action, string? name = null)
            => Instance.Subscribe(action, name);
        
        /// <summary>
        /// Removes all services from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        public static void Reset()
            => Instance.Reset();
        /// <summary>
        /// Removes all services with the specified type from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <typeparam name="T">The type of services to remove.</typeparam>
        public static void Reset<T>()
            => Instance.Reset<T>();
        /// <summary>
        /// Removes all services with the specified type from the current <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="type">The type of services to remove.</param>
        public static void Reset(Type type)
            => Instance.Reset(type);
    }

    /// <summary>
    /// Represents a container containing instances of services.
    /// </summary>
    public class ServiceContextInstance
    {
        /// <summary>
        /// Occurs before a service changes.
        /// </summary>
        public event ServiceContextEventHandler? Changing;
        /// <summary>
        /// Occurs after a service changed.
        /// </summary>
        public event ServiceContextEventHandler? Changed;
        
        private readonly Dictionary<(Type type, string? name), object> _services = new Dictionary<(Type type, string? name), object>();

        internal ServiceContextInstance() { }

        /// <summary>
        /// Gets all services.
        /// </summary>
        /// <returns>All services of this <see cref="ServiceContextInstance"/>.</returns>
        public IReadOnlyDictionary<(Type type, string? name), object> GetAllServices() 
            => new ReadOnlyDictionary<(Type type, string? name), object>(_services);
        /// <summary>
        /// Gets all services of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of services to get.</typeparam>
        /// <returns>All services of this <see cref="ServiceContextInstance"/> of type <typeparamref name="T"/>.</returns>
        public IEnumerable<(string? name, T service)> GetAllServices<T>() 
            => _services.Where(x => x.Key.type == typeof(T)).Select(x => ((string?)x.Key.name, (T)x.Value));
        /// <summary>
        /// Gets all services of a specified type.
        /// </summary>
        /// <param name="serviceType">The type of services to get.</param>
        /// <returns>All services of this <see cref="ServiceContextInstance"/> of type <paramref name="serviceType"/>.</returns>
        public IEnumerable<(string? name, object service)> GetAllServices(Type serviceType) 
            => _services.Where(x => x.Key.type == serviceType).Select(x => ((string?)x.Key.name, x.Value));

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        public void AddService<T>([DisallowNull] T serviceInstance, string? name = null) => AddService(typeof(T), serviceInstance, name);
        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        /// <exception cref="ArgumentException"><paramref name="serviceInstance"/> is not an instance of type <paramref name="serviceType"/>.</exception>
        public void AddService(Type serviceType, object serviceInstance, string? name = null)
        {
            Guard.NotNull(serviceInstance, nameof(serviceInstance));
            if (!serviceType.IsInstanceOfType(serviceInstance))
                throw new ArgumentException($"The type \"{serviceInstance.GetType().FullName}\" is not assignable to \"{serviceType.FullName}\".");
            var key = (serviceType, name);
            var eventArgs = new ServiceContextEventArgs(name, serviceType, null, serviceInstance, ServiceAction.None);
            if (_services.ContainsKey(key))
            {
                eventArgs.OldInstance = _services[key];
                eventArgs.Action = ServiceAction.Changed;
                Changing?.Invoke(this, eventArgs);
                _services[key] = serviceInstance;
            }
            else
            {
                eventArgs.Action = ServiceAction.Added;
                Changing?.Invoke(this, eventArgs);
                _services.Add(key, serviceInstance);
            }
            Changed?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance"/>.</exception>
        public void GetService<T>(out T result, string? name = null)
            => result = GetService<T>(name);
        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance"/>.</exception>
        public T GetService<T>(string? name = null)
            => (T)GetService(typeof(T), name);
        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance"/>.</exception>
        public object GetService(Type serviceType, string? name = null)
        {
            var key = (serviceType, name);
            if (_services.ContainsKey(key))
                return _services[key];
            else
                throw new KeyNotFoundException($"A service with the Type \"{key}\" could not be found! You need to add an instance of this class to this context.");
        }

        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="ServiceContextInstance"/>; otherwise, <see langword="false"/>.</returns>
        public bool TryGetService<T>(out T? result, string? name = null)
        {
            var key = (typeof(T), name);
            var r = _services.TryGetValue(key, out var v);
            result = r ? (T)v : default;
            return r;
        }
        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in this <see cref="ServiceContextInstance"/>. If no service was found <see langword="default"/> is returned.</returns>
        public T? TryGetService<T>(string? name = null) => (T)(TryGetService(typeof(T), name) ?? default(T));
        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <paramref name="serviceType"/> and name <paramref name="name"/> that was found in this <see cref="ServiceContextInstance"/>. If no service was found <see langword="default"/> is returned.</returns>
        public object? TryGetService(Type serviceType, string? name = null)
        {
            var key = (serviceType, name);
            return _services.ContainsKey(key) ? _services[key] : null;
        }

        /// <summary>
        /// Removes the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance"/>.</exception>
        public void RemoveService<T>(string? name = null) => RemoveService(typeof(T), name);
        /// <summary>
        /// Removes the service with the specified type and name.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance"/>.</exception>
        public void RemoveService(Type serviceType, string? name = null)
        {
            var key = (serviceType, name);
            if (_services.ContainsKey(key))
            {
                var eventArgs = new ServiceContextEventArgs(name, serviceType, _services[key], null, ServiceAction.Removed);
                Changing?.Invoke(this, eventArgs);
                _services.Remove(key);
                Changed?.Invoke(this, eventArgs);
            }
            else
                throw new KeyNotFoundException($"A service with the Type \"{key}\" could not be found!");
        }

        /// <summary>
        /// Tries to remove the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="name">The name of the service to remove.</param>
        public void TryRemoveService<T>(string? name = null) => TryRemoveService(typeof(T), name);
        /// <summary>
        /// Tries to remove the service with the specified type and name.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        public void TryRemoveService(Type serviceType, string? name = null)
        {
            var key = (serviceType, name);
            if (_services.ContainsKey(key))
            {
                var eventArgs = new ServiceContextEventArgs(name, serviceType, _services[key], null, ServiceAction.Removed);
                Changing?.Invoke(this, eventArgs);
                _services.Remove(key);
                Changed?.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Determines wether a service with the specified type and name exists.
        /// </summary>
        /// <typeparam name="T">The type of the service to search for.</typeparam>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="ServiceContextInstance"/>; otherwise, <see langword="false"/>.</returns>
        public bool ContainsService<T>(string? name = null) => ContainsService(typeof(T), name);
        /// <summary>
        /// Determines wether a service with the specified type and name exists.
        /// </summary>
        /// <param name="serviceType">The type of the service to search for.</param>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <paramref name="serviceType"/> and name <paramref name="name"/> was found in this <see cref="ServiceContextInstance"/>; otherwise, <see langword="false"/>.</returns>
        public bool ContainsService(Type serviceType, string? name = null)
        {
            var key = (serviceType, name);
            return _services.ContainsKey(key);
        }

        /// <summary>
        /// Subscribes to changes to a service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to subscribe for changes on.</typeparam>
        /// <param name="action">The action to execute when the service changes.</param>
        /// <param name="name">The name of the service to subscribe for changes on.</param>
        /// <returns>An <see cref="IDisposable"/> object that unsubscribes when disposed.</returns>
        public IDisposable Subscribe<T>(Action<T?> action, string? name = null)
            => ServiceContextInstance<T>.Create(this).Subscribe(action, name);

        /// <summary>
        /// Removes all services.
        /// </summary>
        public void Reset()
            => _services.Select(x => x.Key).ToArray().ForEach(x => RemoveService(x.type, x.name));
        /// <summary>
        /// Removes all services with the specified type.
        /// </summary>
        /// <typeparam name="T">The type of services to remove.</typeparam>
        public void Reset<T>()
            => Reset(typeof(T));
        /// <summary>
        /// Removes all services with the specified type.
        /// </summary>
        /// <param name="type">The type of services to remove.</param>
        public void Reset(Type type)
            => _services.Where(x => x.Key.type == type).Select(x => x.Key).ToArray().ForEach(x => RemoveService(x.type, x.name));
    }

    /// <summary>
    /// Represents a static wrapper for the <see cref="ServiceContextInstance{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The type of services to manage.</typeparam>
    public static class ServiceContext<T>
    {
        private static ServiceContextInstance<T>? _instance;
        /// <summary>
        /// Gets the current instance of the <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        public static ServiceContextInstance<T> Instance => _instance ??= ServiceContextInstance<T>.Create(ServiceContext.Instance);

        /// <summary>
        /// Gets all services of the current <see cref="ServiceContextInstance{T}"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>All services of the current <see cref="ServiceContextInstance{T}"/> of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<(string? name, T service)> GetAllServices()
            => Instance.GetAllServices();

        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        public static void AddService([DisallowNull] T serviceInstance, string? name = null)
            => Instance.AddService(serviceInstance, name);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name from the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance{T}"/>.</exception>
        public static void GetService(out T result, string? name = null)
            => Instance.GetService(out result, name);
        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name from the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance{T}"/>.</exception>
        public static T GetService(string? name = null)
            => Instance.GetService(name);

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name from the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContextInstance{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetService(out T? result, string? name = null)
            => Instance.TryGetService(out result, name);
        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name from the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in the current <see cref="ServiceContextInstance{T}"/>. If no service was found <see langword="default"/> is returned.</returns>
        [return: MaybeNull]
        public static T TryGetService(string? name = null)
            => Instance.TryGetService(name);

        /// <summary>
        /// Removes the service with the type <typeparamref name="T"/> and name from the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContextInstance{T}"/>.</exception>
        public static void RemoveService(string? name = null)
            => Instance.RemoveService(name);

        /// <summary>
        /// Tries to remove the service with the type <typeparamref name="T"/> and name from the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService(string? name = null)
            => Instance.TryRemoveService(name);

        /// <summary>
        /// Determines wether a service with the type <typeparamref name="T"/> and name exists in the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContextInstance{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService(string? name = null)
            => Instance.ContainsService(name);

        /// <summary>
        /// Subscribes to changes to a service with the type <typeparamref name="T"/> and name in the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        /// <param name="action">The action to execute when the service changes.</param>
        /// <param name="name">The name of the service to subscribe for changes on.</param>
        /// <returns>An <see cref="IDisposable"/> object that unsubscribes when disposed.</returns>
        public static IDisposable Subscribe(Action<T?> action, string? name = null)
            => Instance.Subscribe(action, name);

        /// <summary>
        /// Removes all services with the type <typeparamref name="T"/> from the current <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        public static void Reset()
            => Instance.Reset();

        /// <summary>
        /// Creates a <see cref="ServiceContextInstance{T}"/> from an already existing <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="context">The existing context to wrap.</param>
        /// <returns>A new instance of the <see cref="ServiceContextInstance{T}"/> class which wraps <paramref name="context"/>.</returns>
        public static ServiceContextInstance<T> CreateFromContext(ServiceContextInstance context) => ServiceContextInstance<T>.Create(context);
        /// <summary>
        /// Creates a new instance of the <see cref="ServiceContextInstance{T}"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="ServiceContextInstance{T}"/> class.</returns>
        public static ServiceContextInstance<T> CreateContext() => ServiceContextInstance<T>.Create(ServiceContext.CreateContext());
    }

    /// <summary>
    /// Represents a generic wrapper for the <see cref="ServiceContextInstance"/> class that handles only events with the speicified type.
    /// </summary>
    /// <typeparam name="T">The type of services to manage in this <see cref="ServiceContextInstance{T}"/>.</typeparam>
    public class ServiceContextInstance<T>
    {
        private static readonly Dictionary<ServiceContextInstance, ServiceContextInstance<T>> ContextCache = new Dictionary<ServiceContextInstance, ServiceContextInstance<T>>();

        /// <summary>
        /// Occurs before a service changes.
        /// </summary>
        public event EventHandler<ServiceContextEventArgs<T>>? Changing;
        /// <summary>
        /// Occurs after a service changed.
        /// </summary>
        public event EventHandler<ServiceContextEventArgs<T>>? Changed;

        /// <summary>
        /// The <see cref="ServiceContextInstance"/> that is wrapped by this <see cref="ServiceContextInstance{T}"/>.
        /// </summary>
        public ServiceContextInstance Context { get; }

        private ServiceContextInstance(ServiceContextInstance context)
        {
            Context = context;
            Context.Changing += (s, e) =>
            {
                if (typeof(T) == e.Type)
                {
                    var eventArgs = new ServiceContextEventArgs<T>(e.Name, (T)e.OldInstance, (T)e.NewInstance, e.Action);
                    Changing?.Invoke(this, eventArgs);
                }
            };
            Context.Changed += (s, e) =>
            {
                if (typeof(T) == e.Type)
                {
                    var eventArgs = new ServiceContextEventArgs<T>(e.Name, (T)e.OldInstance, (T)e.NewInstance, e.Action);
                    Changed?.Invoke(this, eventArgs);
                }
            };
        }

        /// <summary>
        /// Gets all services of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>All services of this <see cref="ServiceContextInstance{T}"/> of type <typeparamref name="T"/>.</returns>
        public IEnumerable<(string? name, T service)> GetAllServices()
            => Context.GetAllServices<T>();

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        public void AddService([DisallowNull] T serviceInstance, string? name = null)
            => Context.AddService(typeof(T), serviceInstance, name);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance{T}"/>.</exception>
        public void GetService(out T result, string? name = null)
            => Context.GetService(out result, name);
        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance{T}"/>.</exception>
        public T GetService(string? name = null)
            => (T)Context.GetService(typeof(T), name);

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="ServiceContextInstance{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool TryGetService(out T? result, string? name = null)
            => Context.TryGetService(out result, name);
        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in this <see cref="ServiceContextInstance{T}"/>. If no service was found <see langword="default"/> is returned.</returns>
        [return: MaybeNull]
        public T TryGetService(string? name = null)
            => (T)(Context.TryGetService(typeof(T), name) ?? default(T));

        /// <summary>
        /// Removes the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in this <see cref="ServiceContextInstance{T}"/>.</exception>
        public void RemoveService(string? name = null)
            => Context.RemoveService(typeof(T), name);

        /// <summary>
        /// Tries to remove the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="name">The name of the service to remove.</param>
        public void TryRemoveService(string? name = null)
            => Context.TryRemoveService(typeof(T), name);

        /// <summary>
        /// Determines wether a service with the type <typeparamref name="T"/> and name exists.
        /// </summary>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="ServiceContextInstance{T}"/>; otherwise, <see langword="false"/>.</returns>
        public bool ContainsService(string? name = null)
            => Context.ContainsService<T>(name);

        /// <summary>
        /// Subscribes to changes to a service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="action">The action to execute when the service changes.</param>
        /// <param name="name">The name of the service to subscribe for changes on.</param>
        /// <returns>An <see cref="IDisposable"/> object that unsubscribes when disposed.</returns>
        public IDisposable Subscribe(Action<T?> action, string? name = null)
        {
            void EventHandler(object? sender, ServiceContextEventArgs<T> e) => action(e.NewInstance);

            if (TryGetService(out T service, name))
                action(service);
            Changed += EventHandler;
            return new ActionOnDispose(() => Changed -= EventHandler);
        }

        /// <summary>
        /// Removes all services with the type <typeparamref name="T"/>.
        /// </summary>
        public void Reset()
            => Context.Reset<T>();

        /// <summary>
        /// Creates a <see cref="ServiceContextInstance{T}"/> from an already existing <see cref="ServiceContextInstance"/>.
        /// </summary>
        /// <param name="context">The existing context to wrap.</param>
        /// <returns>A new instance of the <see cref="ServiceContextInstance{T}"/> class which wraps <paramref name="context"/>.</returns>
        public static ServiceContextInstance<T> Create(ServiceContextInstance context)
        {
            if (!ContextCache.TryGetValue(context, out var result))
            {
                result = new ServiceContextInstance<T>(context);
                ContextCache.Add(context, result);
            }
            return result;
        }
    }

    /// <summary>
    /// Defines actions that can be taken on a service inside a <see cref="ServiceContextInstance"/>.
    /// </summary>
    public enum ServiceAction 
    {
        /// <summary>
        /// No action has been taken.
        /// </summary>
        None,

        /// <summary>
        /// The service has been added.
        /// </summary>
        Added, 

        /// <summary>
        /// The service has been replaced/changed.
        /// </summary>
        Changed, 

        /// <summary>
        /// The service has been removed.
        /// </summary>
        Removed 
    }

    /// <summary>
    /// Event Handler for the <see cref="ServiceContextInstance.Changing"/> and <see cref="ServiceContextInstance.Changed"/> events.
    /// </summary>
    /// <param name="sender">The send of this event.</param>
    /// <param name="e">The event argument of this event.</param>
    public delegate void ServiceContextEventHandler(object sender, ServiceContextEventArgs e);
    /// <summary>
    /// The event argument for <see cref="ServiceContextEventHandler"/>.
    /// </summary>
    public class ServiceContextEventArgs
    {
        /// <summary>
        /// Gets the name of the service on which an action has been taken on.
        /// </summary>
        public string? Name { get; internal set; }
        /// <summary>
        /// Gets the type of the service on which an action has been taken on.
        /// </summary>
        public Type Type { get; internal set; }
        /// <summary>
        /// Gets the old instance of the service on which an action has been taken on.
        /// </summary>
        public object? OldInstance { get; internal set; }
        /// <summary>
        /// Gets the new instance of the service on which an action has been taken on.
        /// </summary>
        public object? NewInstance { get; internal set; }
        /// <summary>
        /// Gets the type of action that has been taken.
        /// </summary>
        public ServiceAction Action { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContextEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of the service on which an action has been taken on.</param>
        /// <param name="type">The type of the service on which an action has been taken on.</param>
        /// <param name="oldInstance">The old instance of the service on which an action has been taken on.</param>
        /// <param name="newInstance">The new instance of the service on which an action has been taken on.</param>
        /// <param name="action">The type of action that has been taken.</param>
        public ServiceContextEventArgs(string? name, Type type, object? oldInstance, object? newInstance, ServiceAction action)
        {
            Name = name;
            Type = type;
            OldInstance = oldInstance;
            NewInstance = newInstance;
            Action = action;
        }
    }

    /// <summary>
    /// Event Handler for the <see cref="ServiceContextInstance{T}.Changing"/> and <see cref="ServiceContextInstance{T}.Changed"/> events.
    /// </summary>
    /// <param name="sender">The send of this event.</param>
    /// <param name="e">The event argument of this event.</param>
    public delegate void ServiceContextEventHandler<T>(object sender, ServiceContextEventArgs<T> e);
    /// <summary>
    /// The event argument for <see cref="ServiceContextEventHandler{T}"/>.
    /// </summary>
    public class ServiceContextEventArgs<T>
    {
        /// <summary>
        /// Gets the name of the service on which an action has been taken on.
        /// </summary>
        public string? Name { get; internal set; }
        /// <summary>
        /// Gets the type of the service on which an action has been taken on.
        /// </summary>
        public Type Type => typeof(T);
        /// <summary>
        /// Gets the old instance of the service on which an action has been taken on.
        /// </summary>
        public T? OldInstance { get; internal set; }
        /// <summary>
        /// Gets the new instance of the service on which an action has been taken on.
        /// </summary>
        public T? NewInstance { get; internal set; }
        /// <summary>
        /// Gets the type of action that has been taken.
        /// </summary>
        public ServiceAction Action { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContextEventArgs{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the service on which an action has been taken on.</param>
        /// <param name="oldInstance">The old instance of the service on which an action has been taken on.</param>
        /// <param name="newInstance">The new instance of the service on which an action has been taken on.</param>
        /// <param name="action">The type of action that has been taken.</param>
        public ServiceContextEventArgs(string? name, T? oldInstance, T? newInstance, ServiceAction action)
        {
            Name = name;
            OldInstance = oldInstance;
            NewInstance = newInstance;
            Action = action;
        }
    }
}
