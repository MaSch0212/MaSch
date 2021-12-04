using MaSch.Core.Converters;

namespace MaSch.Core.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IObjectConvertManager"/> interface.
/// </summary>
public static class ObjectConvertManagerExtensions
{
    /// <summary>
    /// Checks wether an object can be converted to another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The type of the object to convert.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <returns>
    ///   <c>true</c> if the source type can be converted to the target type; otherwise, <c>false</c>.
    /// </returns>
    public static bool CanConvert<TSource, TTarget>(this IObjectConvertManager manager)
    {
        return manager.CanConvert(typeof(TSource), typeof(TTarget));
    }

    /// <summary>
    /// onverts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The source type of the object.</typeparam>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <returns>An instance of the target type representing the object that was given to convert.</returns>
    public static TTarget? Convert<TSource, TTarget>(this IObjectConvertManager manager, TSource objectToConvert)
    {
        return (TTarget?)manager.Convert(objectToConvert, typeof(TSource), typeof(TTarget), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// onverts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The source type of the object.</typeparam>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="formatProvider">A provider that is used for formatting.</param>
    /// <returns>An instance of the target type representing the object that was given to convert.</returns>
    public static TTarget? Convert<TSource, TTarget>(this IObjectConvertManager manager, TSource objectToConvert, IFormatProvider formatProvider)
    {
        return (TTarget?)manager.Convert(objectToConvert, typeof(TSource), typeof(TTarget), formatProvider);
    }

    /// <summary>
    /// onverts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <returns>An instance of the target type representing the object that was given to convert.</returns>
    public static TTarget? Convert<TTarget>(this IObjectConvertManager manager, object? objectToConvert)
    {
        return (TTarget?)manager.Convert(objectToConvert, objectToConvert?.GetType(), typeof(TTarget), CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// onverts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="formatProvider">A provider that is used for formatting.</param>
    /// <returns>An instance of the target type representing the object that was given to convert.</returns>
    public static TTarget? Convert<TTarget>(this IObjectConvertManager manager, object? objectToConvert, IFormatProvider formatProvider)
    {
        return (TTarget?)manager.Convert(objectToConvert, objectToConvert?.GetType(), typeof(TTarget), formatProvider);
    }

    /// <summary>
    /// Converts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="targetType">The desired target type.</param>
    /// <returns>An instance of the target type representing the object that was given to convert.</returns>
    public static object? Convert(this IObjectConvertManager manager, object? objectToConvert, Type targetType)
    {
        return manager.Convert(objectToConvert, objectToConvert?.GetType(), targetType, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Converts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="targetType">The desired target type.</param>
    /// <param name="formatProvider">A provider that is used for formatting.</param>
    /// <returns>An instance of the target type representing the object that was given to convert.</returns>
    public static object? Convert(this IObjectConvertManager manager, object? objectToConvert, Type targetType, IFormatProvider formatProvider)
    {
        return manager.Convert(objectToConvert, objectToConvert?.GetType(), targetType, formatProvider);
    }

    /// <summary>
    /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The source type of the object.</typeparam>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
    /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
    public static bool TryConvert<TSource, TTarget>(this IObjectConvertManager manager, TSource objectToConvert, out TTarget? convertedObject)
    {
        return TryConvert(manager, objectToConvert, typeof(TSource), typeof(TTarget), CultureInfo.CurrentCulture, out convertedObject);
    }

    /// <summary>
    /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The source type of the object.</typeparam>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="formatProvider">A provider that is used for formatting.</param>
    /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
    /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
    public static bool TryConvert<TSource, TTarget>(this IObjectConvertManager manager, TSource objectToConvert, IFormatProvider formatProvider, out TTarget? convertedObject)
    {
        return TryConvert(manager, objectToConvert, typeof(TSource), typeof(TTarget), formatProvider, out convertedObject);
    }

    /// <summary>
    /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
    /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
    public static bool TryConvert<TTarget>(this IObjectConvertManager manager, object? objectToConvert, out TTarget? convertedObject)
    {
        return TryConvert(manager, objectToConvert, objectToConvert?.GetType(), typeof(TTarget), CultureInfo.CurrentCulture, out convertedObject);
    }

    /// <summary>
    /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <typeparam name="TTarget">The desired target type.</typeparam>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="formatProvider">A provider that is used for formatting.</param>
    /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
    /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
    public static bool TryConvert<TTarget>(this IObjectConvertManager manager, object? objectToConvert, IFormatProvider formatProvider, out TTarget? convertedObject)
    {
        return TryConvert(manager, objectToConvert, objectToConvert?.GetType(), typeof(TTarget), formatProvider, out convertedObject);
    }

    /// <summary>
    /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="targetType">The desired target type.</param>
    /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
    /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
    public static bool TryConvert(this IObjectConvertManager manager, object? objectToConvert, Type targetType, out object? convertedObject)
    {
        return TryConvert(manager, objectToConvert, objectToConvert?.GetType(), targetType, CultureInfo.CurrentCulture, out convertedObject);
    }

    /// <summary>
    /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
    /// </summary>
    /// <param name="manager">The manager to use.</param>
    /// <param name="objectToConvert">The object to convert.</param>
    /// <param name="targetType">The desired target type.</param>
    /// <param name="formatProvider">A provider that is used for formatting.</param>
    /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
    /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
    public static bool TryConvert(this IObjectConvertManager manager, object? objectToConvert, Type targetType, IFormatProvider formatProvider, out object? convertedObject)
    {
        return TryConvert(manager, objectToConvert, objectToConvert?.GetType(), targetType, formatProvider, out convertedObject);
    }

    /// <summary>
    /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <param name="manager">The manager to register to.</param>
    /// <param name="converterFunction">The converter function.</param>
    public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, TTarget> converterFunction)
    {
        manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction));
    }

    /// <summary>
    /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <param name="manager">The manager to register to.</param>
    /// <param name="converterFunction">The converter function.</param>
    /// <param name="priority">The priority of the created <see cref="IObjectConverter"/>.</param>
    public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, TTarget> converterFunction, int priority)
    {
        manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction, priority));
    }

    /// <summary>
    /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <param name="manager">The manager to register to.</param>
    /// <param name="converterFunction">The converter function.</param>
    public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, IFormatProvider, TTarget> converterFunction)
    {
        manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction));
    }

    /// <summary>
    /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    /// <param name="manager">The manager to register to.</param>
    /// <param name="converterFunction">The converter function.</param>
    /// <param name="priority">The priority of the created <see cref="IObjectConverter"/>.</param>
    public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, IFormatProvider, TTarget> converterFunction, int priority)
    {
        manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction, priority));
    }

    private static bool TryConvert<T>(IObjectConvertManager manager, object? objectToConvert, Type? sourceType, Type targetType, IFormatProvider formatProvider, out T? convertedObject)
    {
        if (manager.CanConvert(sourceType, targetType))
        {
            try
            {
                convertedObject = (T?)manager.Convert(objectToConvert, sourceType, targetType, formatProvider);
                return true;
            }
            catch (InvalidCastException)
            {
                // Ignore this exception -> fall through to return false
            }
        }

        convertedObject = default;
        return false;
    }
}
