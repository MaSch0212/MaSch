using System;

namespace MaSch.Core
{
    /// <summary>
    /// Provides methods for converting an object to another type.
    /// </summary>
    public interface IObjectConverter
    {
        /// <summary>
        /// Gets the priority for this converter based in the convertion.
        /// </summary>
        /// <param name="sourceType">The source type.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns>A number representing the priority of this <see cref="IObjectConverter"/> for this specific convertion.</returns>
        int GetPriority(Type? sourceType, Type targetType);

        /// <summary>
        /// Checks wether an object can be converted to another type using this <see cref="IObjectConverter"/>.
        /// </summary>
        /// <param name="sourceType">The type of the object to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="convertManager">The <see cref="IObjectConvertManager"/> that is used for nested convertions.</param>
        /// <returns><c>true</c> if the source type can be converted to the target type; otherwise, <c>false</c>.</returns>
        bool CanConvert(Type? sourceType, Type targetType, IObjectConvertManager convertManager);

        /// <summary>
        /// Converts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <param name="obj">The object to convert.</param>
        /// <param name="sourceType">The source type of the object. The <paramref name="obj"/> has to be an instance of this type.</param>
        /// <param name="targetType">The desired target type.</param>
        /// <param name="convertManager">The <see cref="IObjectConvertManager"/> that is used for nested convertions.</param>
        /// <param name="formatProvider">A provider that is used for formatting.</param>
        /// <returns>An instance of the target type representing the object that was given to convert.</returns>
        object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider);
    }

    /// <summary>
    /// Provides methods for managing multiple <see cref="IObjectConverter"/>.
    /// </summary>
    public interface IObjectConvertManager
    {
        /// <summary>
        /// Checks wether an object can be converted to another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <param name="sourceType">The type of the object to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <returns><c>true</c> if the source type can be converted to the target type; otherwise, <c>false</c>.</returns>
        bool CanConvert(Type? sourceType, Type targetType);

        /// <summary>
        /// Converts the given object the another type using the registered <see cref="IObjectConverter"/> objects.
        /// </summary>
        /// <param name="objectToConvert">The object to convert.</param>
        /// <param name="sourceType">The source type of the object. The <paramref name="objectToConvert"/> has to be an instance of this type.</param>
        /// <param name="targetType">The desired target type.</param>
        /// <param name="formatProvider">A provider that is used for formatting.</param>
        /// <returns>An instance of the target type representing the object that was given to convert.</returns>
        object? Convert(object? objectToConvert, Type? sourceType, Type targetType, IFormatProvider formatProvider);

        /// <summary>
        /// Registers the given <see cref="IObjectConverter"/> to be used for convertions with this <see cref="IObjectConvertManager"/>.
        /// </summary>
        /// <param name="converter">The converter to register.</param>
        void RegisterConverter(IObjectConverter converter);
    }
}
