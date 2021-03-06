using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using MaSch.Core;
using MaSch.Core.Extensions;
using MaSch.Core.Observable.Collections;
using MaSch.Presentation.Wpf.Models;
using MaSch.Presentation.Wpf.ThemeValues;

namespace MaSch.Presentation.Wpf
{
    /// <summary>
    /// Default implementation of the <see cref="IThemeManager"/> interface.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.IThemeManager" />
    public class ThemeManager : IThemeManager
    {
        private static IThemeManager? _defaultThemeManager;

        /// <summary>
        /// The prefix that is used to store theme values in the resource dictionary.
        /// </summary>
        public static readonly string ResourceDictionaryKeyPrefix = "ThemeManagerValue_";

        /// <summary>
        /// Gets the default theme manager.
        /// </summary>
        public static IThemeManager DefaultThemeManager
            => _defaultThemeManager ??= new ThemeManager(Theme.FromDefaultTheme(DefaultTheme.Light));

        private IThemeManager? _parentThemeManager;

        /// <inheritdoc/>
        public event EventHandler<ThemeValueChangedEventArgs>? ThemeValueChanged;

        /// <inheritdoc/>
        public ITheme CurrentTheme { get; }

        /// <inheritdoc/>
        public IThemeManagerBindingFactory Bindings { get; }

        /// <inheritdoc/>
        public IThemeManager? ParentThemeManager
        {
            get => _parentThemeManager;
            set
            {
                var removed = _parentThemeManager?.CurrentTheme.Values.Where(x => !CurrentTheme.Values.ContainsKey(x.Key)).Select(x => x.Value).ToArray();
                var added = value?.CurrentTheme.Values.Where(x => !CurrentTheme.Values.ContainsKey(x.Key)).Select(x => x.Value).ToArray();
                _parentThemeManager = value;

                if (!removed.IsNullOrEmpty() || !added.IsNullOrEmpty())
                    OnThemeValueChanged(ThemeValueChangedEventArgs.ForChange(added, removed, Array.Empty<IThemeValue>()));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        public ThemeManager()
            : this(null, new Theme())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        /// <param name="theme">The theme to use.</param>
        public ThemeManager(ITheme theme)
            : this(null, theme)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        /// <param name="parentThemeManager">The parent theme manager.</param>
        public ThemeManager(IThemeManager? parentThemeManager)
            : this(parentThemeManager, new Theme())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        /// <param name="parentThemeManager">The parent theme manager.</param>
        /// <param name="theme">The theme to use.</param>
        public ThemeManager(IThemeManager? parentThemeManager, ITheme theme)
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

        /// <inheritdoc/>
        public bool ContainsKey(string key)
            => CurrentTheme.Values.ContainsKey(key) || ParentThemeManager?.ContainsKey(key) == true;

        /// <inheritdoc/>
        public bool ContainsKey<T>(string key)
            => (CurrentTheme.Values.TryGetValue(key, out var value) && value is IThemeValue<T>) || ParentThemeManager?.ContainsKey<T>(key) == true;

        /// <inheritdoc/>
        public IThemeValue? GetValue(string key)
        {
            if (CurrentTheme.Values.TryGetValue(key, out var result))
                return result;
            result = (IThemeValue?)ParentThemeManager?.GetValue(key)?.Clone();
            if (result != null)
                result.ThemeManager = this;
            return result;
        }

        /// <inheritdoc/>
        public IThemeValue<T>? GetValue<T>(string key) => (IThemeValue<T>?)GetValue(key);

        /// <inheritdoc/>
        public void SetValue(string key, object? value)
        {
            if (value is null)
            {
                CurrentTheme.Values.TryRemove(key);
                return;
            }

            Type? themeValueType;
            if (value is IThemeValue)
            {
                themeValueType = value.GetType();
            }
            else if (value is ThemeValueReference reference)
            {
                var refValue = GetValue(reference.CustomKey);
                themeValueType = refValue?.GetType();
            }
            else
            {
                themeValueType = ThemeValueRegistry.GetThemeValueType(value.GetType());
            }

            if (CurrentTheme.Values.ContainsKey(key))
            {
                if (themeValueType == null || themeValueType.IsInstanceOfType(CurrentTheme.Values[key]))
                {
                    CurrentTheme.Values[key].RawValue = value;
                }
                else
                {
                    var themeValue = GetThemeValue(value);
                    if (themeValue != null)
                        CurrentTheme.Values[key] = themeValue;
                }
            }
            else
            {
                CurrentTheme.Values.Add(key, GetThemeValue(value));
            }
        }

        /// <inheritdoc/>
        public void AddValue(string key, object? value)
            => CurrentTheme.Values.Add(key, GetThemeValue(value));

        /// <inheritdoc/>
        public void RemoveValue(string key)
            => CurrentTheme.Values.TryRemove(key);

        /// <inheritdoc/>
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
                if (value.Key != null)
                    CurrentTheme.Values[value.Key] = value;
            }
        }

        /// <inheritdoc/>
        public object? this[string key]
        {
            get => GetValue(key)?.ValueBase;
            set => SetValue(key, value);
        }

        /// <inheritdoc/>
        public object? this[string key, string propertyName]
        {
            get => GetValue(key)?[propertyName];
            set
            {
                if (CurrentTheme.Values.ContainsKey(key))
                {
                    var themeValue = GetValue(key);
                    if (themeValue != null)
                        themeValue[propertyName] = value;
                }
                else if (ParentThemeManager?.ContainsKey(key) == true)
                {
                    var newValue = (IThemeValue?)ParentThemeManager?.GetValue(key)?.Clone();
                    if (newValue != null)
                    {
                        newValue[propertyName] = value;
                        AddValue(key, newValue);
                    }
                }
                else
                {
                    throw new KeyNotFoundException($"A theme value with the key \"{key}\" was not found.");
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:ThemeValueChanged" /> event.
        /// </summary>
        /// <param name="args">The <see cref="MaSch.Presentation.Wpf.ThemeValueChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnThemeValueChanged(ThemeValueChangedEventArgs args)
        {
            ThemeValueChanged?.Invoke(this, args);
        }

        private void ThemeValuesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
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

            IEnumerable<IThemeValue> Convert(IList? list)
            {
                foreach (KeyValuePair<string, IThemeValue> item in list ?? Array.Empty<KeyValuePair<string, IThemeValue>>())
                {
                    if (item.Value.ThemeManager != this)
                        item.Value.ThemeManager = this;
                    yield return item.Value;
                }
            }
        }

        private void ThemeValueOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IThemeValue.ValueBase) && sender is IThemeValue themeValue)
                OnThemeValueChanged(ThemeValueChangedEventArgs.ForChange(Array.Empty<IThemeValue>(), Array.Empty<IThemeValue>(), new[] { themeValue }));
        }

        private void ThemeValuesOnDictionaryItemChanged(object sender, DictionaryItemChangedEventArgs<string, IThemeValue> e)
        {
            if (e.NewValue != null && e.OldValue != null)
                OnThemeValueChanged(ThemeValueChangedEventArgs.ForChange(new[] { e.NewValue }, new[] { e.OldValue }, Array.Empty<IThemeValue>()));
        }

        private void ParentThemeManagerOnThemeValueChanged(object? sender, ThemeValueChangedEventArgs e)
        {
            IThemeValue[] GetFilteredValues(IReadOnlyDictionary<string, IThemeValue>? values) => values?.Where(x => !CurrentTheme.Values.ContainsKey(x.Key)).Select(x => x.Value).ToArray() ?? Array.Empty<IThemeValue>();

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
                    {
                        OnThemeValueChanged(ThemeValueChangedEventArgs.ForClear(e.RemovedValues?.Values));
                    }
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

        private IThemeValue? GetThemeValue(object? value)
        {
            if (value is IThemeValue themeValue)
                return themeValue;

            Type? themeValueType = null;
            if (value is ThemeValueReference reference)
            {
                var refValue = GetValue(reference.CustomKey);
                themeValueType = refValue?.GetType();
            }
            else
            {
                if (value != null)
                    themeValueType = ThemeValueRegistry.GetThemeValueType(value.GetType());
            }

            if (themeValueType == null)
                return null;

            var result = (IThemeValue)Activator.CreateInstance(themeValueType)!;
            result.RawValue = value;
            return result;
        }
    }
}
