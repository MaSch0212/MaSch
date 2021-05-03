using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Core
{
    /// <summary>
    /// Provides extension methods for the <see cref="IServiceContext"/> and <see cref="IServiceContext{T}"/> interfaces.
    /// </summary>
    public static class ServiceContextExtensions
    {
        #region ServiceContext

        /// <summary>
        /// Gets all services.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to get the services from.</param>
        /// <returns>All services of this <see cref="IServiceContext"/>.</returns>
        [ExcludeFromCodeCoverage]
        public static IReadOnlyDictionary<(Type Type, string? Name), object> GetAllServices(this ServiceContext context)
            => ((IServiceContext)context).GetAllServices();

        /// <summary>
        /// Gets all services of a specified type.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to get the services from.</param>
        /// <param name="serviceType">The type of services to get.</param>
        /// <returns>All services of this <see cref="IServiceContext"/> of type <paramref name="serviceType"/>.</returns>
        [ExcludeFromCodeCoverage]
        public static IEnumerable<(string? Name, object Service)> GetAllServices(this ServiceContext context, Type serviceType)
            => ((IServiceContext)context).GetAllServices(serviceType);

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to add the service to.</param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        [ExcludeFromCodeCoverage]
        public static void AddService(this ServiceContext context, Type serviceType, object serviceInstance, string? name)
            => ((IServiceContext)context).AddService(serviceType, serviceInstance, name);

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        [ExcludeFromCodeCoverage]
        public static object GetService(this ServiceContext context, Type serviceType, string? name)
            => ((IServiceContext)context).GetService(serviceType, name);

        /// <summary>
        /// Determines wether a service with the specified type and name exists.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to check.</param>
        /// <param name="serviceType">The type of the service to search for.</param>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><c>true</c> if a service of type <paramref name="serviceType"/> and name <paramref name="name"/> was found in this <see cref="IServiceContext"/>; otherwise, <c>false</c>.</returns>
        [ExcludeFromCodeCoverage]
        public static bool ContainsService(this ServiceContext context, Type serviceType, string? name)
            => ((IServiceContext)context).ContainsService(serviceType, name);

        /// <summary>
        /// Removes the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        [ExcludeFromCodeCoverage]
        public static void RemoveService(this ServiceContext context, Type serviceType, string? name)
            => ((IServiceContext)context).RemoveService(serviceType, name);

        /// <summary>
        /// Removes all services with the specified type or all if no type is provided.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to clear.</param>
        /// <param name="type">The type of services to remove or <c>null</c> to remove all services.</param>
        [ExcludeFromCodeCoverage]
        public static void Clear(this ServiceContext context, Type? type)
            => ((IServiceContext)context).Clear(type);

        #endregion

        #region IServiceContext

        /// <summary>
        /// Gets all services of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of services to get.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the services from.</param>
        /// <returns>All services of this <see cref="IServiceContext"/> of type <typeparamref name="T"/>.</returns>
        public static IEnumerable<(string? Name, T Service)> GetAllServices<T>(this IServiceContext context)
            => context.GetAllServices(typeof(T)).Select(x => (x.Name, (T)x.Service));

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to add the service to.</param>
        /// <param name="serviceType">The type of the service to add.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <exception cref="ArgumentException"><paramref name="serviceInstance"/> is not an instance of type <paramref name="serviceType"/>.</exception>
        public static void AddService(this IServiceContext context, Type serviceType, object serviceInstance)
            => context.AddService(serviceType, serviceInstance, null);

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to add the service to.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        public static void AddService<T>(this IServiceContext context, [DisallowNull] T serviceInstance)
            => context.AddService(typeof(T), serviceInstance, null);

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to add the service to.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        public static void AddService<T>(this IServiceContext context, [DisallowNull] T serviceInstance, string? name)
            => context.AddService(typeof(T), serviceInstance, name);

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        public static object GetService(this IServiceContext context, Type serviceType)
            => context.GetService(serviceType, null);

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        public static void GetService<T>(this IServiceContext context, [NotNull] out T result)
            => result = (T)context.GetService(typeof(T), null);

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        public static void GetService<T>(this IServiceContext context, [NotNull] out T result, string? name)
            => result = (T)context.GetService(typeof(T), name);

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <returns>The resultung service.</returns>
        [return: NotNull]
        public static T GetService<T>(this IServiceContext context)
            => (T)context.GetService(typeof(T), null);

        /// <summary>
        /// Gets the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        [return: NotNull]
        public static T GetService<T>(this IServiceContext context, string? name)
            => (T)context.GetService(typeof(T), name);

        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> was found in this <see cref="IServiceContext"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetService<T>(this IServiceContext context, [NotNullWhen(true)] out T? result)
            => TryGetService(context, out result, null);

        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="IServiceContext"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetService<T>(this IServiceContext context, [NotNullWhen(true)] out T? result, string? name)
        {
            var exists = context.ContainsService(typeof(T), name);
            result = exists ? (T)context.GetService(typeof(T), name) : default;
            return exists;
        }

        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <returns>
        /// The service of type <typeparamref name="T"/> that was found in this <see cref="IServiceContext"/>.
        /// If no service was found the default value of <typeparamref name="T"/> is returned.
        /// </returns>
        public static T? TryGetService<T>(this IServiceContext context)
            => context.ContainsService(typeof(T), null) ? (T)context.GetService(typeof(T), null) : default;

        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>
        /// The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in this <see cref="IServiceContext"/>.
        /// If no service was found the default value of <typeparamref name="T"/> is returned.
        /// </returns>
        public static T? TryGetService<T>(this IServiceContext context, string? name)
            => context.ContainsService(typeof(T), name) ? (T)context.GetService(typeof(T), name) : default;

        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <returns>
        /// The service of type <paramref name="serviceType"/> that was found in this <see cref="IServiceContext"/>.
        /// If no service was found <c>null</c> is returned.
        /// </returns>
        public static object? TryGetService(this IServiceContext context, Type serviceType)
            => context.ContainsService(serviceType, null) ? context.GetService(serviceType, null) : null;

        /// <summary>
        /// Tries to get the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>
        /// The service of type <paramref name="serviceType"/> and name <paramref name="name"/> that was found in this <see cref="IServiceContext"/>.
        /// If no service was found <c>null</c> is returned.
        /// </returns>
        public static object? TryGetService(this IServiceContext context, Type serviceType, string? name)
            => context.ContainsService(serviceType, name) ? context.GetService(serviceType, name) : null;

        /// <summary>
        /// Determines wether a service with the specified type and name exists.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to check.</param>
        /// <param name="serviceType">The type of the service to search for.</param>
        /// <returns><c>true</c> if a service of type <paramref name="serviceType"/> was found in this <see cref="IServiceContext"/>; otherwise, <c>false</c>.</returns>
        public static bool ContainsService(this IServiceContext context, Type serviceType)
            => context.ContainsService(serviceType, null);

        /// <summary>
        /// Determines wether a service with the specified type and name exists.
        /// </summary>
        /// <typeparam name="T">The type of the service to search for.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to check.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> was found in this <see cref="IServiceContext"/>; otherwise, <c>false</c>.</returns>
        public static bool ContainsService<T>(this IServiceContext context)
            => context.ContainsService(typeof(T), null);

        /// <summary>
        /// Determines wether a service with the specified type and name exists.
        /// </summary>
        /// <typeparam name="T">The type of the service to search for.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to check.</param>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="IServiceContext"/>; otherwise, <c>false</c>.</returns>
        public static bool ContainsService<T>(this IServiceContext context, string? name)
            => context.ContainsService(typeof(T), name);

        /// <summary>
        /// Removes the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        /// <param name="serviceType">The type of the service to remove.</param>
        public static void RemoveService(this IServiceContext context, Type serviceType)
            => context.RemoveService(serviceType, null);

        /// <summary>
        /// Removes the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        public static void RemoveService<T>(this IServiceContext context)
            => context.RemoveService(typeof(T), null);

        /// <summary>
        /// Removes the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        /// <param name="name">The name of the service to remove.</param>
        public static void RemoveService<T>(this IServiceContext context, string? name)
            => context.RemoveService(typeof(T), name);

        /// <summary>
        /// Tries to remove the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        public static void TryRemoveService<T>(this IServiceContext context)
            => TryRemoveService(context, typeof(T), null);

        /// <summary>
        /// Tries to remove the service with the specified type and name.
        /// </summary>
        /// <typeparam name="T">The type of the service to remove.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService<T>(this IServiceContext context, string? name)
            => TryRemoveService(context, typeof(T), name);

        /// <summary>
        /// Tries to remove the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        /// <param name="serviceType">The type of the service to remove.</param>
        public static void TryRemoveService(this IServiceContext context, Type serviceType)
            => TryRemoveService(context, serviceType, null);

        /// <summary>
        /// Tries to remove the service with the specified type and name.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        /// <param name="serviceType">The type of the service to remove.</param>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService(this IServiceContext context, Type serviceType, string? name)
        {
            if (context.ContainsService(serviceType, name))
                context.RemoveService(serviceType, name);
        }

        /// <summary>
        /// Removes all services.
        /// </summary>
        /// <param name="context">The <see cref="IServiceContext"/> to clear.</param>
        public static void Clear(this IServiceContext context)
            => context.Clear(null);

        /// <summary>
        /// Removes all services with the specified type.
        /// </summary>
        /// <typeparam name="T">The type of services to remove.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to clear.</param>
        public static void Clear<T>(this IServiceContext context)
            => context.Clear(typeof(T));

        #endregion

        #region ServiceContext<T>

        /// <summary>
        /// Gets all services of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the services from.</param>
        /// <returns>All services of this <see cref="IServiceContext{T}"/>.</returns>
        [ExcludeFromCodeCoverage]
        public static IEnumerable<(string? Name, T Service)> GetAllServices<T>(this ServiceContext<T> context)
            => ((IServiceContext<T>)context).GetAllServices();

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to add the service to.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        /// <param name="name">The name of the service.</param>
        [ExcludeFromCodeCoverage]
        public static void AddService<T>(this ServiceContext<T> context, [DisallowNull] T serviceInstance, string? name)
            => ((IServiceContext<T>)context).AddService(serviceInstance, name);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to get the service from.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The resultung service.</returns>
        [ExcludeFromCodeCoverage]
        [return: NotNull]
        public static T GetService<T>(this ServiceContext<T> context, string? name)
            => ((IServiceContext<T>)context).GetService(name);

        /// <summary>
        /// Determines wether a service with the type <typeparamref name="T"/> and name exists.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> to check.</param>
        /// <param name="name">The name of the service to search for.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="IServiceContext{T}"/>; otherwise, <c>false</c>.</returns>
        [ExcludeFromCodeCoverage]
        public static bool ContainsService<T>(this ServiceContext<T> context, string? name)
            => ((IServiceContext<T>)context).ContainsService(name);

        /// <summary>
        /// Removes the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext"/> from which to remove the service.</param>
        /// <param name="name">The name of the service to remove.</param>
        [ExcludeFromCodeCoverage]
        public static void RemoveService<T>(this ServiceContext<T> context, string? name)
            => ((IServiceContext<T>)context).RemoveService(name);

        #endregion

        #region IServiceContext<T>

        /// <summary>
        /// Adds or replaces a specified service.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to add the service to.</param>
        /// <param name="serviceInstance">The service instance to add.</param>
        public static void AddService<T>(this IServiceContext<T> context, [DisallowNull] T serviceInstance)
            => context.AddService(serviceInstance, null);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to get the service from.</param>
        /// <returns>The resultung service.</returns>
        [return: NotNull]
        public static T GetService<T>(this IServiceContext<T> context)
            => context.GetService(null);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> was not found in this <see cref="IServiceContext{T}"/>.</exception>
        public static void GetService<T>(this IServiceContext<T> context, [NotNull] out T result)
            => result = context.GetService(null);

        /// <summary>
        /// Gets the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <exception cref="KeyNotFoundException">A service of type <typeparamref name="T"/> and name <paramref name="name"/> was not found in this <see cref="IServiceContext{T}"/>.</exception>
        public static void GetService<T>(this IServiceContext<T> context, [NotNull] out T result, string? name)
            => result = context.GetService(name);

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> was found in this <see cref="IServiceContext{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetService<T>(this IServiceContext<T> context, [NotNullWhen(true)] out T? result)
            => TryGetService(context, out result, null);

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to get the service from.</param>
        /// <param name="result">The resultung service.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> and name <paramref name="name"/> was found in this <see cref="IServiceContext{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool TryGetService<T>(this IServiceContext<T> context, [NotNullWhen(true)] out T? result, string? name)
        {
            var exists = context.ContainsService(name);
            result = exists ? context.GetService(name) : default;
            return exists;
        }

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to get the service from.</param>
        /// <returns>The service of type <typeparamref name="T"/> that was found in this <see cref="IServiceContext{T}"/>. If no service was found the default value of <typeparamref name="T"/> is returned.</returns>
        public static T? TryGetService<T>(this IServiceContext<T> context)
            => context.ContainsService(null) ? context.GetService(null) : default;

        /// <summary>
        /// Tries to get the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> to get the service from.</param>
        /// <param name="name">The name of the service to retrieve.</param>
        /// <returns>The service of type <typeparamref name="T"/> and name <paramref name="name"/> that was found in this <see cref="IServiceContext{T}"/>. If no service was found the default value of <typeparamref name="T"/> is returned.</returns>
        public static T? TryGetService<T>(this IServiceContext<T> context, string? name)
            => context.ContainsService(name) ? context.GetService(name) : default;

        /// <summary>
        /// Removes the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> from which to remove the service.</param>
        public static void RemoveService<T>(this IServiceContext<T> context)
            => context.RemoveService(null);

        /// <summary>
        /// Tries to remove the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> from which to remove the service.</param>
        public static void TryRemoveService<T>(this IServiceContext<T> context)
            => TryRemoveService(context, null);

        /// <summary>
        /// Tries to remove the service with the type <typeparamref name="T"/> and name.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> from which to remove the service.</param>
        /// <param name="name">The name of the service to remove.</param>
        public static void TryRemoveService<T>(this IServiceContext<T> context, string? name)
        {
            if (context.ContainsService(name))
                context.RemoveService(name);
        }

        /// <summary>
        /// Determines wether a service with the type <typeparamref name="T"/> and name exists.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="IServiceContext{T}"/>.</typeparam>
        /// <param name="context">The <see cref="IServiceContext{T}"/> from which to remove the service.</param>
        /// <returns><c>true</c> if a service of type <typeparamref name="T"/> was found in this <see cref="IServiceContext{T}"/>; otherwise, <c>false</c>.</returns>
        public static bool ContainsService<T>(this IServiceContext<T> context)
            => context.ContainsService(null);

        #endregion
    }
}
