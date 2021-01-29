using MaSch.Presentation.Wpf.Themes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Provides members to manage themes.
    /// </summary>
    public interface IThemeManager
    {
        /// <summary>
        /// Occurs when a theme values changed.
        /// </summary>
        event EventHandler<ThemeValueChangedEventArgs> ThemeValueChanged;

        /// <summary>
        /// Gets the current theme.
        /// </summary>
        ITheme CurrentTheme { get; }

        /// <summary>
        /// Gets the binding factory for this <see cref="IThemeManager"/>.
        /// </summary>
        IThemeManagerBindingFactory Bindings { get; }

        /// <summary>
        /// Gets or sets the parent theme manager.
        /// </summary>
        IThemeManager ParentThemeManager { get; set; }

        /// <summary>
        /// Determines whether the theme contains a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the theme contains a spefific key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey(string key);

        /// <summary>
        /// Determines whether the theme contains a specific key.
        /// </summary>
        /// <typeparam name="T">The type of value expected.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the theme contains a spefific key; otherwise, <c>false</c>.
        /// </returns>
        bool ContainsKey<T>(string key);

        /// <summary>
        /// Gets a theme value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The theme value.</returns>
        IThemeValue GetValue(string key);

        /// <summary>
        /// Gets a theme value.
        /// </summary>
        /// <typeparam name="T">The type of the value expected.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The theme value.</returns>
        IThemeValue<T> GetValue<T>(string key);

        /// <summary>
        /// Loads a theme.
        /// </summary>
        /// <param name="theme">The theme.</param>
        void LoadTheme(ITheme theme);

        /// <summary>
        /// Sets a theme value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetValue(string key, object value);

        /// <summary>
        /// Adds a new theme value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void AddValue(string key, object value);

        /// <summary>
        /// Removes a theme value.
        /// </summary>
        /// <param name="key">The key.</param>
        void RemoveValue(string key);

        /// <summary>
        /// Gets or sets the theme value with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The theme value.</returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Gets or sets the property value of a theme value with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property value of the theme value.</returns>
        object this[string key, string propertyName] { get; set; }
    }

    /// <summary>
    /// Provides extensions for the <see cref="IThemeManager"/> interface.
    /// </summary>
    public static class ThemeManagerExtensions
    {
        public static bool ContainsKey(this IThemeManager themeManager, ThemeKey key) => themeManager.ContainsKey(key.ToString());
        public static bool ContainsKey<T>(this IThemeManager themeManager, ThemeKey key) => themeManager.ContainsKey<T>(key.ToString());
        public static IThemeValue GetValue(this IThemeManager themeManager, ThemeKey key) => themeManager.GetValue(key.ToString());
        public static IThemeValue<T> GetValue<T>(this IThemeManager themeManager, ThemeKey key) => themeManager.GetValue<T>(key.ToString());
        public static void SetValue(this IThemeManager themeManager, ThemeKey key, object value) => themeManager.SetValue(key.ToString(), value);
        public static void AddValue(this IThemeManager themeManager, ThemeKey key, object value) => themeManager.AddValue(key.ToString(), value);
        public static void RemoveValue(this IThemeManager themeManager, ThemeKey key) => themeManager.RemoveValue(key.ToString());
    }

    /// <summary>
    /// Event arguments for the <see cref="IThemeManager.ThemeValueChanged"/> event.
    /// </summary>
    /// <seealso cref="EventArgs" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Event arguments can be in same file.")]
    public class ThemeValueChangedEventArgs : EventArgs
    {
        public ThemeValueChangeType ChangeType { get; }
        public IReadOnlyDictionary<string, IThemeValue> AddedValues { get; }
        public IReadOnlyDictionary<string, IThemeValue> RemovedValues { get; }
        public IReadOnlyDictionary<string, IThemeValue> ChangedValues { get; }

        private ThemeValueChangedEventArgs(ThemeValueChangeType type, IEnumerable<IThemeValue> addedValues, IEnumerable<IThemeValue> removedValues, IEnumerable<IThemeValue> changedValues)
        {
            ChangeType = type;
            AddedValues = addedValues?.ToDictionary(x => x.Key);
            RemovedValues = removedValues?.ToDictionary(x => x.Key);
            ChangedValues = changedValues?.ToDictionary(x => x.Key);
        }

        public bool HasChangeForKey(string key)
            => AddedValues?.ContainsKey(key) == true || RemovedValues?.ContainsKey(key) == true || ChangedValues?.ContainsKey(key) == true;

        public static ThemeValueChangedEventArgs ForAdd(IEnumerable<IThemeValue> addedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Add, addedValues, null, null);
        public static ThemeValueChangedEventArgs ForRemove(IEnumerable<IThemeValue> removedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Remove, null, removedValues, null);
        public static ThemeValueChangedEventArgs ForChange(IEnumerable<IThemeValue> addedValues, IEnumerable<IThemeValue> removedValues, IEnumerable<IThemeValue> changedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Change, addedValues, removedValues, changedValues);
        public static ThemeValueChangedEventArgs ForClear(IEnumerable<IThemeValue> removedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Clear, null, removedValues, null);
    }
}
