using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using MaSch.Common;
using MaSch.Common.Extensions;
using MaSch.Presentation.Observable.Collections;
using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;

namespace MaSch.Presentation.Wpf
{
    public class ThemeManager : IThemeManager
    {
        public static readonly string ResourceDictionaryKeyPrefix = "ThemeManagerValue_";
        private static IThemeManager _defaultThemeManager;
        public static IThemeManager DefaultThemeManager 
            => _defaultThemeManager ?? (_defaultThemeManager = new ThemeManager(Theme.FromDefaultTheme(DefaultTheme.Light)));

        #region Fields
        private IThemeManager _parentThemeManager;
        #endregion

        #region Events
        public event EventHandler<ThemeValueChangedEventArgs> ThemeValueChanged;
        #endregion

        #region Properties
        public ITheme CurrentTheme { get; }
        public IThemeManagerBindingFactory Bindings { get; }
        public IThemeManager ParentThemeManager
        {
            get => _parentThemeManager;
            set
            {
                var removed = _parentThemeManager?.CurrentTheme.Values.Where(x => !CurrentTheme.Values.ContainsKey(x.Key)).Select(x => x.Value).ToArray();
                var added = value?.CurrentTheme.Values.Where(x => !CurrentTheme.Values.ContainsKey(x.Key)).Select(x => x.Value).ToArray();
                _parentThemeManager = value;

                if (!removed.IsNullOrEmpty() || !added.IsNullOrEmpty())
                    OnThemeValueChanged(ThemeValueChangedEventArgs.ForChange(added, removed, new IThemeValue[0]));
            }
        }
        #endregion

        #region Constructors
        public ThemeManager() : this(null, new Theme()) { }
        public ThemeManager(ITheme theme) : this(null, theme) { }
        public ThemeManager(IThemeManager parentThemeManager) : this(parentThemeManager, new Theme()) { }
        public ThemeManager(IThemeManager parentThemeManager, ITheme theme)
        {
            Guard.NotNull(theme, nameof(theme));

            _parentThemeManager = parentThemeManager;
            CurrentTheme = theme;
            Bindings = new ThemeManagerBindingFactory(this);

            theme.Values.Values.ForEach(x => x.ThemeManager = this);
            theme.Values.CollectionChanged += ThemeValuesOnCollectionChanged;
            theme.Values.DictionaryItemChanged += ThemeValuesOnDictionaryItemChanged;
            if (parentThemeManager != null)
                parentThemeManager.ThemeValueChanged += ParentThemeManagerOnThemeValueChanged;
        }
        #endregion

        #region Public Methods
        public bool ContainsKey(string key) 
            => CurrentTheme.Values.ContainsKey(key) || ParentThemeManager?.ContainsKey(key) == true;

        public bool ContainsKey<T>(string key) 
            => CurrentTheme.Values.TryGetValue(key, out var value) && value is IThemeValue<T> || ParentThemeManager?.ContainsKey<T>(key) == true;

        public IThemeValue GetValue(string key)
        {
            if (CurrentTheme.Values.TryGetValue(key, out var result))
                return result;
            result = (IThemeValue)ParentThemeManager?.GetValue(key)?.Clone();
            if (result != null)
                result.ThemeManager = this;
            return result;
        }

        public IThemeValue<T> GetValue<T>(string key) => (IThemeValue<T>)GetValue(key);

        public void SetValue(string key, object value)
        {
            if (value is null)
            {
                CurrentTheme.Values.TryRemove(key);
                return;
            }

            Type themeValueType;
            if (value is IThemeValue)
                themeValueType = value.GetType();
            else if (value is ThemeValueReference reference)
            {
                var refValue = GetValue(reference.CustomKey);
                themeValueType = refValue.GetType();
            }
            else
                themeValueType = ThemeValueRegistry.GetThemeValueType(value.GetType());

            if (CurrentTheme.Values.ContainsKey(key))
            {
                if (themeValueType.IsInstanceOfType(CurrentTheme.Values[key]))
                    CurrentTheme.Values[key].RawValue = value;
                else
                    CurrentTheme.Values[key] = GetThemeValue(value);
            }
            else
                CurrentTheme.Values.Add(key, GetThemeValue(value));
        }

        public void AddValue(string key, object value) 
            => CurrentTheme.Values.Add(key, GetThemeValue(value));

        public void RemoveValue(string key) 
            => CurrentTheme.Values.TryRemove(key);
        
        public void LoadTheme(ITheme theme)
        {
            var valuesToReplace = new List<IThemeValue>();
            foreach (var value in theme.Values)
            {
                if (CurrentTheme.Values.TryGetValue(value.Key, out var eValue))
                {
                    if (!Equals(value.Value, eValue))
                        valuesToReplace.Add(value.Value);
                }
                else
                {
                    CurrentTheme.Values.Add(value);
                }
            }
            foreach (var value in valuesToReplace)
            {
                value.ThemeManager = this;
                CurrentTheme.Values[value.Key] = value;
            }
        }

        public object this[string key]
        {
            get => GetValue(key)?.ValueBase;
            set => SetValue(key, value);
        }

        public object this[string key, string propertyName]
        {
            get => GetValue(key)?[propertyName];
            set
            {
                if (CurrentTheme.Values.ContainsKey(key))
                    GetValue(key)[propertyName] = value;
                else if (ParentThemeManager?.ContainsKey(key) == true)
                {
                    var newValue = (IThemeValue)ParentThemeManager.GetValue(key).Clone();
                    newValue[propertyName] = value;
                    AddValue(key, newValue);
                }
                else
                    throw new KeyNotFoundException($"A theme value with the key \"{key}\" was not found.");
            }
        }
        #endregion

        #region Protected Methods
        protected virtual void OnThemeValueChanged(ThemeValueChangedEventArgs args)
        {
            ThemeValueChanged?.Invoke(this, args);
        }
        #endregion

        #region Private Methods
        private void ThemeValuesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var addValues = Convert(e.NewItems).ToArray();
                    foreach (var value in addValues)
                        value.PropertyChanged += ThemeValueOnPropertyChanged;
                    OnThemeValueChanged(ThemeValueChangedEventArgs.ForAdd(addValues));
                    break;

                case NotifyCollectionChangedAction.Remove:
                    var removeValues = Convert(e.OldItems).ToArray();
                    foreach (var value in removeValues)
                        value.PropertyChanged -= ThemeValueOnPropertyChanged;
                    OnThemeValueChanged(ThemeValueChangedEventArgs.ForRemove(removeValues));
                    break;

                case NotifyCollectionChangedAction.Reset:
                    var clearedValues = Convert(e.OldItems).ToArray();
                    foreach (var value in clearedValues)
                        value.PropertyChanged -= ThemeValueOnPropertyChanged;
                    OnThemeValueChanged(ThemeValueChangedEventArgs.ForClear(clearedValues));
                    break;

                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    throw new InvalidOperationException("The actions replace and move were not expected.");

                default:
                    throw new ArgumentOutOfRangeException();
            }

            IEnumerable<IThemeValue> Convert(IList list)
            {
                foreach (KeyValuePair<string, IThemeValue> item in list)
                {
                    if (item.Value.ThemeManager != this)
                        item.Value.ThemeManager = this;
                    yield return item.Value;
                }
            }
        }

        private void ThemeValueOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IThemeValue.ValueBase))
                OnThemeValueChanged(ThemeValueChangedEventArgs.ForChange(new IThemeValue[0], new IThemeValue[0], new[] { (IThemeValue)sender }));
        }

        private void ThemeValuesOnDictionaryItemChanged(object sender, DictionaryItemChangedEventArgs<string, IThemeValue> e)
        {
            OnThemeValueChanged(ThemeValueChangedEventArgs.ForChange(new[] { e.NewValue }, new[] { e.OldValue }, new IThemeValue[0]));
        }

        private void ParentThemeManagerOnThemeValueChanged(object sender, ThemeValueChangedEventArgs e)
        {
            IThemeValue[] GetFilteredValues(IReadOnlyDictionary<string, IThemeValue> values) => values.Where(x => !CurrentTheme.Values.ContainsKey(x.Key)).Select(x => x.Value).ToArray();

            switch (e.ChangeType)
            {
                case ThemeValueChangeType.Add:
                    var added = GetFilteredValues(e.AddedValues);
                    if (added.Length > 0)
                        OnThemeValueChanged(ThemeValueChangedEventArgs.ForAdd(added));
                    break;

                case ThemeValueChangeType.Change:
                    var cAdded = GetFilteredValues(e.AddedValues);
                    var cRemoved = GetFilteredValues(e.RemovedValues);
                    var cChanged = GetFilteredValues(e.ChangedValues);
                    if (cAdded.Length > 0 || cRemoved.Length > 0 || cChanged.Length > 0)
                        OnThemeValueChanged(ThemeValueChangedEventArgs.ForChange(cAdded, cRemoved, cChanged));
                    break;

                case ThemeValueChangeType.Remove:
                case ThemeValueChangeType.Clear:
                    if (CurrentTheme.Values.Count == 0 && e.ChangeType == ThemeValueChangeType.Clear)
                        OnThemeValueChanged(ThemeValueChangedEventArgs.ForClear(e.RemovedValues.Values));
                    else
                    {
                        var removed = GetFilteredValues(e.RemovedValues);
                        if (removed.Length > 0)
                            OnThemeValueChanged(ThemeValueChangedEventArgs.ForRemove(removed));
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(e.ChangeType));
            }
        }

        private IThemeValue GetThemeValue(object value)
        {
            if (value is IThemeValue themeValue)
                return themeValue;

            Type themeValueType;
            if (value is ThemeValueReference reference)
            {
                var refValue = GetValue(reference.CustomKey);
                themeValueType = refValue.GetType();
            }
            else
                themeValueType = ThemeValueRegistry.GetThemeValueType(value.GetType());

            var result = (IThemeValue)Activator.CreateInstance(themeValueType);
            result.RawValue = value;
            return result;
        }
        #endregion
    }
}
