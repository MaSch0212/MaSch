using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Core
{
    /// <summary>
    /// Provides functionalities to manage services in an application.
    /// </summary>
    public interface IServiceContext : IDisposable
    {
        /// <summary>
        /// Occurs before a service changes.
        /// </summary>
        event ServiceContextEventHandler? Changing;

        /// <summary>
        /// Occurs after a service changed.
        /// </summary>
        event ServiceContextEventHandler? Changed;

        /// <summary>
        /// Gets all services.
        /// </summary>
        /// <returns>All services of this <see cref="IServiceContext"/>.</returns>
        IReadOnlyDictionary<(Type Type, string? Name), object> GetAllServices();

        /// <summary>
        /// Gets all services of a specified type.
        /// </summary>
        /// <param name="serviceType">The type of services to get.</param>
        /// <returns>All services of this <see cref="IServiceContext"/> of type <paramref name="serviceType"/>.</returns>
        IEnumerable<(string? Name, object Service)> GetAllServices(Type serviceType);

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        void AddService(Type serviceType, object serviceInstance, string? name);

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        object GetService(Type serviceType, string? name);

        /// <summary>
        /// Determines wether a service with the specified type and name exists.
        /// </summary>
        /// <param name="serviceType">The type of the service to search for.</param>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><c>true</c> if a service of type <paramref name="serviceType"/> and name <paramref name="name"/> was found in this <see cref="IServiceContext"/>; otherwise, <c>false</c>.</returns>
        bool ContainsService(Type serviceType, string? name);

        /// <summary>
        /// Removes the service with the specified type and name.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        void RemoveService(Type serviceType, string? name);

        /// <summary>
        /// Removes all services with the specified type or all if no type is provided.
        /// </summary>
        /// <param name="type">The type of services to remove or <c>null</c> to remove all services.</param>
        void Clear(Type? type);

        /// <summary>
        /// Creates a <see cref="IServiceContext{T}"/> that takes this <see cref="IServiceContext"/> as source for its services.
        /// All created <see cref="IServiceContext{T}"/> objects are disposed when this <see cref="IServiceContext"/> is disposed.
        /// </summary>
        /// <typeparam name="T">The type of service to create the <see cref="IServiceContext{T}"/> for.</typeparam>
        /// <returns>A <see cref="IServiceContext{T}"/> that takes this <see cref="IServiceContext"/> as source for its services.</returns>
        IServiceContext<T> GetView<T>();
    }

    /// <summary>
    /// Provides functionalities to manage specific kinds of services in an application.
    /// </summary>
    /// <typeparam name="T">The type of services to manage.</typeparam>
    public interface IServiceContext<T> : IDisposable
    {
        /// <summary>
        /// Occurs before a service changes.
        /// </summary>
        event EventHandler<ServiceContextEventArgs<T>>? Changing;

        /// <summary>
        /// Occurs after a service changed.
        /// </summary>
        event EventHandler<ServiceContextEventArgs<T>>? Changed;

        /// <summary>
        /// Gets all services of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>All services of this <see cref="IServiceContext{T}"/>.</returns>
        IEnumerable<(string? Name, T Service)> GetAllServices();

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        void AddService([DisallowNull] T serviceInstance, string? name);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        T GetService(string? name);

        /// <summary>
        /// Determines wether a service with the type <typeparamref name="T"/> and name exists.
        /// </summary>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="IServiceContext{T}"/>; otherwise, <c>false</c>.</returns>
        bool ContainsService(string? name);

        /// <summary>
        /// Removes the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <param name="name">The name of the service to remove.</param>
        void RemoveService(string? name);

        /// <summary>
        /// Removes all services with the type <typeparamref name="T"/>.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Event Handler for the <see cref="IServiceContext.Changing"/> and <see cref="IServiceContext.Changed"/> events.
    /// </summary>
    /// <param name="sender">The send of this event.</param>
    /// <param name="e">The event argument of this event.</param>
    public delegate void ServiceContextEventHandler(object? sender, ServiceContextEventArgs e);

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
    /// Event Handler for the <see cref="IServiceContext{T}.Changing"/> and <see cref="IServiceContext{T}.Changed"/> events.
    /// </summary>
    /// <typeparam name="T">The type of the instance that changed.</typeparam>
    /// <param name="sender">The send of this event.</param>
    /// <param name="e">The event argument of this event.</param>
    public delegate void ServiceContextEventHandler<T>(object? sender, ServiceContextEventArgs<T> e);

    /// <summary>
    /// The event argument for <see cref="ServiceContextEventHandler{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the instance that changed.</typeparam>
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
