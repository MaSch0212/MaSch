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
        IThemeManager? ParentThemeManager { get; set; }

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
        IThemeValue? GetValue(string key);

        /// <summary>
        /// Gets a theme value.
        /// </summary>
        /// <typeparam name="T">The type of the value expected.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The theme value.</returns>
        IThemeValue<T>? GetValue<T>(string key);

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
        void SetValue(string key, object? value);

        /// <summary>
        /// Adds a new theme value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void AddValue(string key, object? value);

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
        object? this[string key] { get; set; }

        /// <summary>
        /// Gets or sets the property value of a theme value with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>The property value of the theme value.</returns>
        object? this[string key, string propertyName] { get; set; }
    }

    /// <summary>
    /// Provides extensions for the <see cref="IThemeManager"/> interface.
    /// </summary>
    public static class ThemeManagerExtensions
    {
        /// <summary>
        /// Determines whether the theme contains a specific key.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the theme contains a spefific key; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsKey(this IThemeManager themeManager, ThemeKey key)
            => themeManager.ContainsKey(key.ToString());

        /// <summary>
        /// Determines whether the theme contains a specific key.
        /// </summary>
        /// <typeparam name="T">The type of value expected.</typeparam>
        /// <param name="themeManager">The theme manager.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the theme contains a spefific key; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsKey<T>(this IThemeManager themeManager, ThemeKey key)
            => themeManager.ContainsKey<T>(key.ToString());

        /// <summary>
        /// Gets a theme value.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        /// <param name="key">The key.</param>
        /// <returns>The theme value.</returns>
        public static IThemeValue? GetValue(this IThemeManager themeManager, ThemeKey key)
            => themeManager.GetValue(key.ToString());

        /// <summary>
        /// Gets a theme value.
        /// </summary>
        /// <typeparam name="T">The type of the value expected.</typeparam>
        /// <param name="themeManager">The theme manager.</param>
        /// <param name="key">The key.</param>
        /// <returns>The theme value.</returns>
        public static IThemeValue<T>? GetValue<T>(this IThemeManager themeManager, ThemeKey key)
            => themeManager.GetValue<T>(key.ToString());

        /// <summary>
        /// Sets a theme value.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(this IThemeManager themeManager, ThemeKey key, object value)
            => themeManager.SetValue(key.ToString(), value);

        /// <summary>
        /// Adds a new theme value.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddValue(this IThemeManager themeManager, ThemeKey key, object value)
            => themeManager.AddValue(key.ToString(), value);

        /// <summary>
        /// Removes a theme value.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        /// <param name="key">The key.</param>
        public static void RemoveValue(this IThemeManager themeManager, ThemeKey key) => themeManager.RemoveValue(key.ToString());
    }

    /// <summary>
    /// Event arguments for the <see cref="IThemeManager.ThemeValueChanged"/> event.
    /// </summary>
    /// <seealso cref="EventArgs" />
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Event arguments can be in same file.")]
    public class ThemeValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of the change.
        /// </summary>
        public ThemeValueChangeType ChangeType { get; }

        /// <summary>
        /// Gets the added values.
        /// </summary>
        public IReadOnlyDictionary<string, IThemeValue>? AddedValues { get; }

        /// <summary>
        /// Gets the removed values.
        /// </summary>
        public IReadOnlyDictionary<string, IThemeValue>? RemovedValues { get; }

        /// <summary>
        /// Gets the changed values.
        /// </summary>
        public IReadOnlyDictionary<string, IThemeValue>? ChangedValues { get; }

        private ThemeValueChangedEventArgs(ThemeValueChangeType type, IEnumerable<IThemeValue>? addedValues, IEnumerable<IThemeValue>? removedValues, IEnumerable<IThemeValue>? changedValues)
        {
            ChangeType = type;
            AddedValues = addedValues?.Where(x => x.Key != null).ToDictionary(x => x.Key!);
            RemovedValues = removedValues?.Where(x => x.Key != null).ToDictionary(x => x.Key!);
            ChangedValues = changedValues?.Where(x => x.Key != null).ToDictionary(x => x.Key!);
        }

        /// <summary>
        /// Determines whether a specific key has been changed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>
        ///   <c>true</c> if the key has been chnaged; otherwise, <c>false</c>.
        /// </returns>
        public bool HasChangeForKey(string key)
            => AddedValues?.ContainsKey(key) == true || RemovedValues?.ContainsKey(key) == true || ChangedValues?.ContainsKey(key) == true;

        /// <summary>
        /// Create a new <see cref="ThemeValueChangedEventArgs"/> object for when keys are added to an <see cref="IThemeManager"/>.
        /// </summary>
        /// <param name="addedValues">The added values.</param>
        /// <returns>The created <see cref="ThemeValueChangedEventArgs"/>.</returns>
        public static ThemeValueChangedEventArgs ForAdd(IEnumerable<IThemeValue>? addedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Add, addedValues, null, null);

        /// <summary>
        /// Create a new <see cref="ThemeValueChangedEventArgs"/> object for when keys are removed from an <see cref="IThemeManager"/>.
        /// </summary>
        /// <param name="removedValues">The removed values.</param>
        /// <returns>The created <see cref="ThemeValueChangedEventArgs"/>.</returns>
        public static ThemeValueChangedEventArgs ForRemove(IEnumerable<IThemeValue>? removedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Remove, null, removedValues, null);

        /// <summary>
        /// Create a new <see cref="ThemeValueChangedEventArgs"/> object for when keys are changed on an <see cref="IThemeManager"/>.
        /// </summary>
        /// <param name="addedValues">The added values.</param>
        /// <param name="removedValues">The removed values.</param>
        /// <param name="changedValues">The changed values.</param>
        /// <returns>The created <see cref="ThemeValueChangedEventArgs"/>.</returns>
        public static ThemeValueChangedEventArgs ForChange(IEnumerable<IThemeValue>? addedValues, IEnumerable<IThemeValue>? removedValues, IEnumerable<IThemeValue>? changedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Change, addedValues, removedValues, changedValues);

        /// <summary>
        /// Create a new <see cref="ThemeValueChangedEventArgs"/> object for when keys are cleared from an <see cref="IThemeManager"/>.
        /// </summary>
        /// <param name="removedValues">The removed values.</param>
        /// <returns>The created <see cref="ThemeValueChangedEventArgs"/>.</returns>
        public static ThemeValueChangedEventArgs ForClear(IEnumerable<IThemeValue>? removedValues)
            => new ThemeValueChangedEventArgs(ThemeValueChangeType.Clear, null, removedValues, null);
    }
}
