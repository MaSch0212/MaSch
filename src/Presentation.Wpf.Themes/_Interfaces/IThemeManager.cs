using MaSch.Presentation.Wpf.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MaSch.Presentation.Wpf
{
    public interface IThemeManager
    {
        event EventHandler<ThemeValueChangedEventArgs> ThemeValueChanged;

        ITheme CurrentTheme { get; }
        IThemeManagerBindingFactory Bindings { get; }
        IThemeManager ParentThemeManager { get; set; }

        bool ContainsKey(string key);
        bool ContainsKey<T>(string key);
        IThemeValue GetValue(string key);
        IThemeValue<T> GetValue<T>(string key);

        void LoadTheme(ITheme theme);

        void SetValue(string key, object value);
        void AddValue(string key, object value);
        void RemoveValue(string key);

        object this[string key] { get; set; }
        object this[string key, string propertyName] { get; set; }
    }

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
