using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using MaSch.Core.Converters;

namespace MaSch.Core
{
    /// <summary>
    /// The default implementation of the <see cref="IObjectConvertManager"/> interface.
    /// </summary>
    /// <seealso cref="IObjectConvertManager" />
    public class ObjectConvertManager : IObjectConvertManager
    {
        private readonly List<IObjectConverter> _objectConverters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectConvertManager"/> class.
        /// </summary>
        public ObjectConvertManager()
        {
            _objectConverters = new List<IObjectConverter>();
        }

        /// <inheritdoc/>
        public virtual bool CanConvert(Type? sourceType, Type targetType)
        {
            return (from c in _objectConverters
                    where c.CanConvert(sourceType, targetType, this)
                    select c).Any();
        }

        /// <inheritdoc/>
        public virtual object? Convert(object? objectToConvert, Type? sourceType, Type targetType, IFormatProvider formatProvider)
        {
            Guard.NotNull(targetType, nameof(targetType));
            Guard.NotNull(formatProvider, nameof(formatProvider));

            if (objectToConvert == null && sourceType?.IsClass == false)
                throw new ArgumentException("The object cannot be null, because the sourceType is not nullable.");
            if (objectToConvert != null)
            {
                if (sourceType == null)
                    sourceType = objectToConvert.GetType();
                else if (!sourceType.IsInstanceOfType(objectToConvert))
                    throw new ArgumentException($"The object is not an instance of the sourceType \"{sourceType.FullName}\".");
            }

            var converters = from c in _objectConverters
                             where CanConvertWithConverter(c, sourceType, targetType) || (objectToConvert == null && sourceType != null && CanConvertWithConverter(c, null, targetType))
                             orderby GetPriorityForConverter(c, sourceType, targetType) descending
                             select c;

            var errors = new List<string>();
            foreach (var converter in converters)
            {
                try
                {
                    return converter.Convert(objectToConvert, sourceType, targetType, this, formatProvider);
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }

            if (errors.Count == 0)
                throw new InvalidCastException($"No converter was found, that can convert the source type \"{sourceType?.FullName ?? "(null)"}\" to target type \"{targetType.FullName}\".");
            throw new InvalidCastException($"Non of the found converters could convert the object to type \"{targetType.FullName}\":{Environment.NewLine}- " +
                                           string.Join(Environment.NewLine + "- ", errors));
        }

        /// <inheritdoc/>
        public virtual void RegisterConverter(IObjectConverter converter)
        {
            Guard.NotNull(converter, nameof(converter));
            _objectConverters.Add(converter);
        }

        private bool CanConvertWithConverter(IObjectConverter converter, Type? sourceType, Type targetType)
        {
            try
            {
                return converter.CanConvert(sourceType, targetType, this);
            }
            catch
            {
                return false;
            }
        }

        private static int GetPriorityForConverter(IObjectConverter converter, Type? sourceType, Type targetType)
        {
            try
            {
                return converter.GetPriority(sourceType, targetType);
            }
            catch
            {
                return -100;
            }
        }
    }

    /// <summary>
    /// Represents a <see cref="IObjectConvertManager"/> that already contains common <see cref="IObjectConverter"/>s.
    /// </summary>
    /// <remarks>
    ///     Contains the following <see cref="IObjectConverter"/>s:
    ///     <see cref="NullableObjectConverter"/>,
    ///     <see cref="ConvertibleObjectConverter"/>,
    ///     <see cref="EnumConverter"/>,
    ///     <see cref="ToStringObjectConverter"/>,
    ///     <see cref="NullObjectConverter"/>,
    ///     <see cref="IdentityObjectConverter"/>.
    /// </remarks>
    /// <seealso cref="ObjectConvertManager" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Can be in same file.")]
    public class DefaultObjectConvertManager : ObjectConvertManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultObjectConvertManager"/> class.
        /// </summary>
        public DefaultObjectConvertManager()
        {
            RegisterInitialConverters();
        }

        /// <summary>
        /// Registers the initial converters.
        /// </summary>
        protected virtual void RegisterInitialConverters()
        {
            RegisterConverter(new NullableObjectConverter());
            RegisterConverter(new ConvertibleObjectConverter());
            RegisterConverter(new EnumConverter());
            RegisterConverter(new EnumerableConverter());
            RegisterConverter(new ToStringObjectConverter(-98_000));
            RegisterConverter(new NullObjectConverter(-99_000));
            RegisterConverter(new IdentityObjectConverter(-100_000));
        }
    }

    /// <summary>
    /// Provides extension methods for the <see cref="IObjectConvertManager"/> interface.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Can be in same file.")]
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
            => manager.CanConvert(typeof(TSource), typeof(TTarget));

        /// <summary>
        /// onverts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <typeparam name="TSource">The source type of the object.</typeparam>
        /// <typeparam name="TTarget">The desired target type.</typeparam>
        /// <param name="manager">The manager to use.</param>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <returns>An instance of the target type representing the object that was given to convert.</returns>
        public static TTarget? Convert<TSource, TTarget>(this IObjectConvertManager manager, TSource objectToConvert)
            => (TTarget)manager.Convert(objectToConvert, typeof(TSource), typeof(TTarget), CultureInfo.CurrentCulture);

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
            => (TTarget)manager.Convert(objectToConvert, typeof(TSource), typeof(TTarget), formatProvider);

        /// <summary>
        /// onverts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <typeparam name="TTarget">The desired target type.</typeparam>
        /// <param name="manager">The manager to use.</param>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <returns>An instance of the target type representing the object that was given to convert.</returns>
        public static TTarget? Convert<TTarget>(this IObjectConvertManager manager, object? objectToConvert)
            => (TTarget)manager.Convert(objectToConvert, objectToConvert?.GetType(), typeof(TTarget), CultureInfo.CurrentCulture);

        /// <summary>
        /// onverts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <typeparam name="TTarget">The desired target type.</typeparam>
        /// <param name="manager">The manager to use.</param>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <param name="formatProvider">A provider that is used for formatting.</param>
        /// <returns>An instance of the target type representing the object that was given to convert.</returns>
        public static TTarget? Convert<TTarget>(this IObjectConvertManager manager, object? objectToConvert, IFormatProvider formatProvider)
            => (TTarget)manager.Convert(objectToConvert, objectToConvert?.GetType(), typeof(TTarget), formatProvider);

        /// <summary>
        /// Converts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <param name="manager">The manager to use.</param>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <param name="targetType">The desired target type.</param>
        /// <returns>An instance of the target type representing the object that was given to convert.</returns>
        public static object? Convert(this IObjectConvertManager manager, object? objectToConvert, Type targetType)
            => manager.Convert(objectToConvert, objectToConvert?.GetType(), targetType, CultureInfo.CurrentCulture);

        /// <summary>
        /// Converts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <param name="manager">The manager to use.</param>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <param name="targetType">The desired target type.</param>
        /// <param name="formatProvider">A provider that is used for formatting.</param>
        /// <returns>An instance of the target type representing the object that was given to convert.</returns>
        public static object? Convert(this IObjectConvertManager manager, object? objectToConvert, Type targetType, IFormatProvider formatProvider)
            => manager.Convert(objectToConvert, objectToConvert?.GetType(), targetType, formatProvider);

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
            => TryConvert(manager, objectToConvert, typeof(TSource), typeof(TTarget), CultureInfo.CurrentCulture, out convertedObject);

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
            => TryConvert(manager, objectToConvert, typeof(TSource), typeof(TTarget), formatProvider, out convertedObject);

        /// <summary>
        /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <typeparam name="TTarget">The desired target type.</typeparam>
        /// <param name="manager">The manager to use.</param>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
        /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
        public static bool TryConvert<TTarget>(this IObjectConvertManager manager, object? objectToConvert, out TTarget? convertedObject)
            => TryConvert(manager, objectToConvert, objectToConvert?.GetType(), typeof(TTarget), CultureInfo.CurrentCulture, out convertedObject);

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
            => TryConvert(manager, objectToConvert, objectToConvert?.GetType(), typeof(TTarget), formatProvider, out convertedObject);

        /// <summary>
        /// Tries to convert the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <param name="manager">The manager to use.</param>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <param name="targetType">The desired target type.</param>
        /// <param name="convertedObject">An instance of the target type representing the object that was given to convert.</param>
        /// <returns><c>true</c> if the object could be converted; otherwise, <c>false</c>.</returns>
        public static bool TryConvert(this IObjectConvertManager manager, object? objectToConvert, Type targetType, out object? convertedObject)
            => TryConvert(manager, objectToConvert, objectToConvert?.GetType(), targetType, CultureInfo.CurrentCulture, out convertedObject);

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
            => TryConvert(manager, objectToConvert, objectToConvert?.GetType(), targetType, formatProvider, out convertedObject);

        /// <summary>
        /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="manager">The manager to register to.</param>
        /// <param name="converterFunction">The converter function.</param>
        public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, TTarget> converterFunction)
            => manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction));

        /// <summary>
        /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="manager">The manager to register to.</param>
        /// <param name="converterFunction">The converter function.</param>
        /// <param name="priority">The priority of the created <see cref="IObjectConverter"/>.</param>
        public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, TTarget> converterFunction, int priority)
            => manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction, priority));

        /// <summary>
        /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="manager">The manager to register to.</param>
        /// <param name="converterFunction">The converter function.</param>
        public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, IFormatProvider, TTarget> converterFunction)
            => manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction));

        /// <summary>
        /// Registers the given function as a <see cref="DelegateObjectConverter{TSource, TTarget}"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="manager">The manager to register to.</param>
        /// <param name="converterFunction">The converter function.</param>
        /// <param name="priority">The priority of the created <see cref="IObjectConverter"/>.</param>
        public static void RegisterConverter<TSource, TTarget>(this IObjectConvertManager manager, Func<TSource?, IFormatProvider, TTarget> converterFunction, int priority)
            => manager.RegisterConverter(new DelegateObjectConverter<TSource, TTarget>(converterFunction, priority));

        private static bool TryConvert<T>(IObjectConvertManager manager, object? objectToConvert, Type? sourceType, Type targetType, IFormatProvider formatProvider, out T? convertedObject)
        {
            bool result = false;
            convertedObject = default;
            if (manager.CanConvert(sourceType, targetType))
            {
                try
                {
                    convertedObject = (T)manager.Convert(objectToConvert, sourceType, targetType, formatProvider);
                    result = true;
                }
                catch (InvalidCastException)
                {
                }
            }

            return result;
        }
    }
}
