using System;

namespace MaSch.Core.Converters
{
    /// <summary>
    /// A <see cref="IObjectConverter"/> that can handle null values.
    /// </summary>
    public class NullObjectConverter : IObjectConverter
    {
        private readonly int _priority;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectConverter"/> class.
        /// </summary>
        /// <param name="priority">The priority for this <see cref="IObjectConverter"/>.</param>
        public NullObjectConverter(int priority)
        {
            _priority = priority;
        }

        /// <inheritdoc />
        public int GetPriority(Type? sourceType, Type targetType) => _priority;

        /// <inheritdoc />
        public bool CanConvert(Type? sourceType, Type targetType, IObjectConvertManager convertManager)
        {
            return sourceType == null && targetType.IsClass;
        }

        /// <inheritdoc />
        public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
        {
            return obj == null ? (object?) null : throw new InvalidCastException("This converter cannot convert objects that are not null.");
        }
    }
}
