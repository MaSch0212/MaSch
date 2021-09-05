using MaSch.Presentation.Wpf.Attributes;
using MaSch.Presentation.Wpf.Observable;
using MaSch.Presentation.Wpf.Themes;
using System;
using System.Windows;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Models
{
    /// <summary>
    /// Defines an override for a specific theme value.
    /// </summary>
    /// <seealso cref="ObservableDependencyObject" />
    [ContentProperty(nameof(Value))]
    public class ThemeOverride : ObservableDependencyObject
    {
        /// <summary>
        /// Dependency property. Gets or sets the custom key to override.
        /// </summary>
        public static readonly DependencyProperty CustomKeyProperty =
            DependencyProperty.Register(nameof(CustomKey), typeof(string), typeof(ThemeOverride), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Dependency property. Gets or sets the new value value to use for the specified key.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(ThemeOverride), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeOverride"/> class.
        /// </summary>
        public ThemeOverride()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeOverride"/> class.
        /// </summary>
        /// <param name="customKey">The custom key to override.</param>
        /// <param name="value">The new value to use.</param>
        public ThemeOverride(string customKey, object value)
        {
            CustomKey = customKey;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeOverride"/> class.
        /// </summary>
        /// <param name="key">The key to override.</param>
        /// <param name="value">The new value to use.</param>
        public ThemeOverride(ThemeKey key, object value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the key to override.
        /// </summary>
        public ThemeKey Key
        {
            get => (ThemeKey)Enum.Parse(typeof(ThemeKey), CustomKey);
            set => CustomKey = value.ToString();
        }

        /// <summary>
        /// Gets or sets the custom key to override.
        /// </summary>
        [NotifyDependencyPropertyChanged(nameof(CustomKey))]
        public string CustomKey
        {
            get => (string)GetValue(CustomKeyProperty);
            set => SetValue(CustomKeyProperty, value);
        }

        /// <summary>
        /// Gets or sets the new value value to use for the specified key.
        /// </summary>
        [NotifyDependencyPropertyChanged(nameof(Value))]
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
    }
}
