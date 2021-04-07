using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Core
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Already documented in ServiceContext.cs")]
    public partial class ServiceContext
    {
        private static IServiceContext? _instance;

        /// <summary>
        /// Gets the current instance of the <see cref="ServiceContext"/>.
        /// </summary>
        public static IServiceContext Instance => _instance ??= new ServiceContext();

        /// <summary>
        /// Creates a new instance of the <see cref="ServiceContext"/> class.
        /// </summary>
        /// <returns>A new instance of the <see cref="ServiceContext"/> class.</returns>
        public static IServiceContext CreateContext()
            => new ServiceContext();

        /// <summary>
        /// Gets all services of the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <returns>All services of the current <see cref="ServiceContext"/>.</returns>
        public static IReadOnlyDictionary<(Type Type, string? Name), object> GetAllServices()
            => Instance.GetAllServices();

        /// <summary>
        /// Gets all services of the current <see cref="ServiceContext"/> of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of services to get.</typeparam>
        /// <returns>All services of the current <see cref="ServiceContext"/> of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<(string? Name, T Service)> GetAllServices<T>()
            => Instance.GetAllServices<T>();

        /// <summary>
        /// Gets all services of the current <see cref="ServiceContext"/> of a specified type.
        /// </summary>
        /// <param name="serviceType">The type of services to get.</param>
        /// <returns>All services of the current <see cref="ServiceContext"/> of type <paramref name="serviceType"/>.</returns>
        public static IEnumerable<(string? Name, object Service)> GetAllServices(Type serviceType)
            => Instance.GetAllServices(serviceType);

        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="serviceInstance">The service instance to add.</param>
        public static void AddService<T>([DisallowNull] T serviceInstance)
            => Instance.AddService(serviceInstance);

        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        public static void AddService<T>([DisallowNull] T serviceInstance, string? name)
            => Instance.AddService(serviceInstance, name);

        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <exception cref="ArgumentException"><paramref name="serviceInstance"/> is not an instance of type <paramref name="serviceType"/>.</exception>
        public static void AddService(Type serviceType, object serviceInstance)
            => Instance.AddService(serviceType, serviceInstance);

        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        /// <exception cref="ArgumentException"><paramref name="serviceInstance"/> is not an instance of type <paramref name="serviceType"/>.</exception>
        public static void AddService(Type serviceType, object serviceInstance, string? name)
            => Instance.AddService(serviceType, serviceInstance, name);

        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static void GetService<T>(out T result)
            => Instance.GetService(out result);

        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static void GetService<T>(out T result, string? name)
            => Instance.GetService(out result, name);

        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static T GetService<T>()
            => Instance.GetService<T>();

        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static T GetService<T>(string? name)
            => Instance.GetService<T>(name);

        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static object GetService(Type serviceType)
            => Instance.GetService(serviceType);

        /// <summary>
        /// Gets the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static object GetService(Type serviceType, string? name)
            => Instance.GetService(serviceType, name);

        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> was found in the current <see cref="ServiceContext"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetService<T>(out T? result)
            => Instance.TryGetService(out result);

        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContext"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetService<T>(out T? result, string? name)
            => Instance.TryGetService(out result, name);

        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>The service of type <typeparamref name="T"/> that was found in the current <see cref="ServiceContext"/>. If no service was found <see langword="default"/> is returned.</returns>
        public static T? TryGetService<T>()
            => Instance.TryGetService<T>();

        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in the current <see cref="ServiceContext"/>. If no service was found <see langword="default"/> is returned.</returns>
        public static T? TryGetService<T>(string? name)
            => Instance.TryGetService<T>(name);

        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <returns>The service of type <paramref name="serviceType"/> that was found in the current <see cref="ServiceContext"/>. If no service was found <see langword="default"/> is returned.</returns>
        public static object? TryGetService(Type serviceType)
            => Instance.TryGetService(serviceType);

        /// <summary>
        /// Tries to get the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <paramref name="serviceType"/> and name <paramref name="name"/> that was found in the current <see cref="ServiceContext"/>. If no service was found <see langword="default"/> is returned.</returns>
        public static object? TryGetService(Type serviceType, string? name)
            => Instance.TryGetService(serviceType, name);

        /// <summary>
        /// Removes the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static void RemoveService<T>()
            => Instance.RemoveService<T>();

        /// <summary>
        /// Removes the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static void RemoveService<T>(string? name)
            => Instance.RemoveService<T>(name);

        /// <summary>
        /// Removes the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static void RemoveService(Type serviceType)
            => Instance.RemoveService(serviceType);

        /// <summary>
        /// Removes the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <paramref name="serviceType"/> and name <paramref name="name"/> was not found in the current <see cref="ServiceContext"/>.</exception>
        public static void RemoveService(Type serviceType, string? name)
            => Instance.RemoveService(serviceType, name);

        /// <summary>
        /// Tries to remove the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        public static void TryRemoveService<T>()
            => Instance.TryRemoveService<T>();

        /// <summary>
        /// Tries to remove the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService<T>(string? name)
            => Instance.TryRemoveService<T>(name);

        /// <summary>
        /// Tries to remove the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        public static void TryRemoveService(Type serviceType)
            => Instance.TryRemoveService(serviceType);

        /// <summary>
        /// Tries to remove the service with the specified type and name from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService(Type serviceType, string? name)
            => Instance.TryRemoveService(serviceType, name);

        /// <summary>
        /// Determines wether a service with the specified type and name exists in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to search for.</typeparam>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> was found in the current <see cref="ServiceContext"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService<T>()
            => Instance.ContainsService<T>();

        /// <summary>
        /// Determines wether a service with the specified type and name exists in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of the service to search for.</typeparam>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContext"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService<T>(string? name)
            => Instance.ContainsService<T>(name);

        /// <summary>
        /// Determines wether a service with the specified type and name exists in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <paramref name="serviceType"/> was found in the current <see cref="ServiceContext"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService(Type serviceType)
            => Instance.ContainsService(serviceType);

        /// <summary>
        /// Determines wether a service with the specified type and name exists in the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="serviceType">The type of the service to search for.</param>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <paramref name="serviceType"/> and name <paramref name="name"/> was found in the current <see cref="ServiceContext"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService(Type serviceType, string? name)
            => Instance.ContainsService(serviceType, name);

        /// <summary>
        /// Removes all services from the current <see cref="ServiceContext"/>.
        /// </summary>
        public static void Clear()
            => Instance.Clear();

        /// <summary>
        /// Removes all services with the specified type from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <typeparam name="T">The type of services to remove.</typeparam>
        public static void Clear<T>()
            => Instance.Clear<T>();

        /// <summary>
        /// Removes all services with the specified type from the current <see cref="ServiceContext"/>.
        /// </summary>
        /// <param name="type">The type of services to remove.</param>
        public static void Clear(Type? type)
            => Instance.Clear(type);
    }

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Already documented in ServiceContext.cs")]
    public partial class ServiceContext<T>
    {
        /// <summary>
        /// Gets the current instance of the <see cref="IServiceContext{T}"/>.
        /// </summary>
        public static IServiceContext<T> Instance => ServiceContext.Instance.GetView<T>();

        /// <summary>
        /// Gets all services of the current <see cref="IServiceContext{T}"/> of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>All services of the current <see cref="IServiceContext{T}"/> of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<(string? Name, T Service)> GetAllServices()
            => Instance.GetAllServices();

        /// <summary>
        /// Adds or replaces a specified service in the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        public static void AddService([DisallowNull] T serviceInstance, string? name = null)
            => Instance.AddService(serviceInstance, name);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name from the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="IServiceContext{T}"/>.</exception>
        public static void GetService(out T result, string? name = null)
            => Instance.GetService(out result, name);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name from the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="IServiceContext{T}"/>.</exception>
        public static T GetService(string? name = null)
            => Instance.GetService(name);

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name from the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="IServiceContext{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetService(out T? result, string? name = null)
            => Instance.TryGetService(out result, name);

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name from the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in the current <see cref="IServiceContext{T}"/>. If no service was found <see langword="default"/> is returned.</returns>
        [return: MaybeNull]
        public static T TryGetService(string? name = null)
            => Instance.TryGetService(name);

        /// <summary>
        /// Removes the service with the type <typeparamref name="T"/> and name from the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to remove.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in the current <see cref="IServiceContext{T}"/>.</exception>
        public static void RemoveService(string? name = null)
            => Instance.RemoveService(name);

        /// <summary>
        /// Tries to remove the service with the type <typeparamref name="T"/> and name from the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService(string? name = null)
            => Instance.TryRemoveService(name);

        /// <summary>
        /// Determines wether a service with the type <typeparamref name="T"/> and name exists in the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><see langword="true"/> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in the current <see cref="IServiceContext{T}"/>; otherwise, <see langword="false"/>.</returns>
        public static bool ContainsService(string? name = null)
            => Instance.ContainsService(name);

        /// <summary>
        /// Removes all services with the type <typeparamref name="T"/> from the current <see cref="IServiceContext{T}"/>.
        /// </summary>
        public static void Clear()
            => Instance.Clear();
    }
}
