using MaSch.Core.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

        /// <summary>
        /// Gets the registered object converters.
        /// </summary>
        internal IReadOnlyCollection<IObjectConverter> ObjectConverters => _objectConverters.AsReadOnly();

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
            _ = Guard.NotNull(targetType, nameof(targetType));
            _ = Guard.NotNull(formatProvider, nameof(formatProvider));

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
                             let canConvertType = CanConvertWithConverter(c, sourceType, targetType)
                             let canConvertNull = !canConvertType && objectToConvert == null && sourceType != null && CanConvertWithConverter(c, null, targetType)
                             where canConvertType || canConvertNull
                             let type = canConvertType ? sourceType : null
                             orderby GetPriorityForConverter(c, type, targetType) descending
                             select (Converter: c, Type: type);

            var errors = new List<string>();
            foreach (var (converter, type) in converters)
            {
                try
                {
                    return converter.Convert(objectToConvert, type, targetType, this, formatProvider);
                }
                catch (Exception ex)
                {
                    errors.Add($"{converter.GetType().Name}: {ex.Message}");
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
            _ = Guard.NotNull(converter, nameof(converter));
            _objectConverters.Add(converter);
        }

        private static int GetPriorityForConverter(IObjectConverter converter, Type? sourceType, Type targetType)
        {
            try
            {
                return converter.GetPriority(sourceType, targetType);
            }
            catch
            {
                return int.MinValue;
            }
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
    }

    /// <summary>
    /// Represents a <see cref="IObjectConvertManager"/> that already contains common <see cref="IObjectConverter"/>s.
    /// </summary>
    /// <remarks>
    ///     Contains the following <see cref="IObjectConverter"/>s:
    ///     <see cref="NullableObjectConverter"/>,
    ///     <see cref="ConvertibleObjectConverter"/>,
    ///     <see cref="EnumConverter"/>,
    ///     <see cref="EnumerableConverter"/>.
    ///     <see cref="ToStringObjectConverter"/>,
    ///     <see cref="NullObjectConverter"/>,
    ///     <see cref="IdentityObjectConverter"/>.
    /// </remarks>
    /// <seealso cref="ObjectConvertManager" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "This class is related to ObjectConvertManager.")]
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
}
