namespace MaSch.Core.Helper;

/// <summary>
/// Provides helper methods for Service Location.
/// </summary>
[RequiresUnreferencedCode("If trimming is enabled, this might not automatically find the service locator from CommonServiceLocator library.")]
public static class ServiceLocatorHelper
{
    private static bool _serviceLocatorLoaded;
    private static IServiceProvider? _currentServiceLocator;

    /// <summary>
    /// Gets or sets the current service locator.
    /// </summary>
    public static IServiceProvider? CurrentServiceLocator
    {
        get
        {
            if (!_serviceLocatorLoaded)
            {
                _serviceLocatorLoaded = true;
                try
                {
                    var assembly = Assembly.Load("CommonServiceLocator");
                    var serviceLocatorType = assembly.GetType("CommonServiceLocator.ServiceLocator", true, true) ?? throw new Exception("ServiceLocator type not found.");
                    _currentServiceLocator =
                        (IServiceProvider?)serviceLocatorType.GetProperty("Current", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
                }
                catch (Exception)
                {
                    try
                    {
                        var assembly = Assembly.Load("Microsoft.Practices.ServiceLocation");
                        var serviceLocatorType = assembly.GetType("Microsoft.Practices.ServiceLocation.ServiceLocator", true, true) ?? throw new Exception("ServiceLocator type not found.");
                        _currentServiceLocator =
                            (IServiceProvider?)serviceLocatorType.GetProperty("Current", BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }

            return _currentServiceLocator;
        }
        set
        {
            _currentServiceLocator = value;
        }
    }

    /// <summary>
    /// Gets an instance of the specified type from the current service locator.
    /// </summary>
    /// <typeparam name="T">The type of the instance to retrieve.</typeparam>
    /// <returns>An instance of <typeparamref name="T"/> that is provided by the current service locator.</returns>
    public static T? GetInstance<T>()
    {
        var serviceLocator = CurrentServiceLocator;
        if (serviceLocator == null)
            return ServiceContext.Instance.TryGetService<T>();
        return (T?)serviceLocator.GetService(typeof(T));
    }

    /// <summary>
    /// Gets an instance of the specified type from the current service locator.
    /// </summary>
    /// <typeparam name="T">The type of the instance to retrieve.</typeparam>
    /// <param name="key">The key of the instance to get.</param>
    /// <returns>An instance of <typeparamref name="T"/> with the specified <paramref name="key"/> that is provided by the current service locator.</returns>
    [SuppressMessage("ReflectionAnalyzers.SystemReflection", "REFL003:The member does not exist", Justification = "It has if it is from the ServiceLocator library.")]
    public static T? GetInstance<T>(string key)
    {
        var serviceLocator = CurrentServiceLocator;
        if (serviceLocator == null)
            return ServiceContext.Instance.TryGetService<T>(key);
        var method = serviceLocator.GetType().GetMethod("GetInstance", new[] { typeof(string) }) ?? throw new InvalidOperationException("GetInstance method not found in ServiceLocator.");
        var genericMethod = method.MakeGenericMethod(typeof(T));
        return (T?)genericMethod.Invoke(serviceLocator, new object[] { key });
    }
}
