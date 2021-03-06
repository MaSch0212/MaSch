using System;
using System.ComponentModel;
using MaSch.Presentation.Wpf.JsonConverters;
using Newtonsoft.Json;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Represents a value inside a <see cref="ITheme"/>.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.ICloneable" />
    [JsonConverter(typeof(ThemeValueJsonConverter))]
    public interface IThemeValue : INotifyPropertyChanged, ICloneable
    {
        /// <summary>
        /// Gets or sets the theme manager the value is part of.
        /// </summary>
        IThemeManager? ThemeManager { get; set; }

        /// <summary>
        /// Gets or sets the key of this value.
        /// </summary>
        string? Key { get; set; }

        /// <summary>
        /// Gets or sets the raw value.
        /// </summary>
        object? RawValue { get; set; }

        /// <summary>
        /// Gets or sets the value base.
        /// </summary>
        object? ValueBase { get; set; }

        /// <summary>
        /// Gets the value of a property of this <see cref="IThemeValue"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of the specified property of this <see cref="IThemeValue"/>.</returns>
        TValue? GetPropertyValue<TValue>(string propertyName);

        /// <summary>
        /// Gets or sets the value of a property with the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The value of the specified property.</returns>
        object? this[string propertyName] { get; set; }
    }

    /// <summary>
    /// Represents a value inside a <see cref="ITheme"/>.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.ICloneable" />
    public interface IThemeValue<T> : IThemeValue
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        T? Value { get; set; }
    }
}
