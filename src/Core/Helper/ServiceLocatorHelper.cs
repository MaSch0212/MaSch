using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace MaSch.Core.Helper
{
    /// <summary>
    /// Provides helper methods for Service Location.
    /// </summary>
    public static class ServiceLocatorHelper
    {
        private static bool _serviceLocatorLoaded;
        private static object? _currentServiceLocator;

        private static object? CurrentServiceLocator
        {
            get
            {
                if (!_serviceLocatorLoaded)
                {
                    _serviceLocatorLoaded = true;
                    try
                    {
                        var assembly = Assembly.Load("CommonServiceLocator");
                        var slType = assembly.GetType("CommonServiceLocator.ServiceLocator", true, true) ?? throw new Exception("ServiceLocator type not found.");
                        _currentServiceLocator =
                            slType.GetProperty("Current", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            var assembly = Assembly.Load("Microsoft.Practices.ServiceLocation");
                            var slType = assembly.GetType("Microsoft.Practices.ServiceLocation.ServiceLocator", true, true) ?? throw new Exception("ServiceLocator type not found.");
                            _currentServiceLocator =
                                slType.GetProperty("Current", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                    }
                }

                return _currentServiceLocator;
            }
        }

        /// <summary>
        /// Gets an instance of the specified type from the current service locator.
        /// </summary>
        /// <typeparam name="T">The type of the instance to retrieve.</typeparam>
        /// <returns>An instance of <typeparamref name="T"/> that is provided by the current service locator.</returns>
        [SuppressMessage("ReflectionAnalyzers.SystemReflection", "REFL009:The referenced member is not known to exist.", Justification = "Actual type is unknown")]
        public static T? GetInstance<T>()
        {
            if (CurrentServiceLocator == null)
                return ServiceContext.Instance.TryGetService<T>();
            var method = CurrentServiceLocator.GetType().GetMethod("GetInstance", Type.EmptyTypes) ?? throw new InvalidOperationException("GetInstance method not found in ServiceLocator.");
            var gMethod = method.MakeGenericMethod(typeof(T));
            return (T?)gMethod.Invoke(CurrentServiceLocator, null);
        }

        /// <summary>
        /// Gets an instance of the specified type from the current service locator.
        /// </summary>
        /// <typeparam name="T">The type of the instance to retrieve.</typeparam>
        /// <param name="key">The key of the instance to get.</param>
        /// <returns>An instance of <typeparamref name="T"/> with the specified <paramref name="key"/> that is provided by the current service locator.</returns>
        [SuppressMessage("ReflectionAnalyzers.SystemReflection", "REFL009:The referenced member is not known to exist.", Justification = "Actual type is unknown")]
        public static T? GetInstance<T>(string key)
        {
            if (CurrentServiceLocator == null)
                return ServiceContext.Instance.TryGetService<T>();
            var method = CurrentServiceLocator.GetType().GetMethod("GetInstance", new[] { typeof(string) }) ?? throw new InvalidOperationException("GetInstance method not found in ServiceLocator.");
            var gMethod = method.MakeGenericMethod(typeof(T));
            return (T?)gMethod.Invoke(CurrentServiceLocator, new object[] { key });
        }
    }
}
