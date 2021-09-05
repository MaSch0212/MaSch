using MaSch.Core;
using MaSch.Core.Attributes;
using MaSch.Core.Extensions;
using MaSch.Core.Observable;
using MaSch.Presentation.Wpf.JsonConverters;
using MaSch.Presentation.Wpf.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// Base class for theme values.
    /// </summary>
    /// <seealso cref="ObservableObject" />
    /// <seealso cref="IThemeValue" />
    [SuppressMessage("Major Code Smell", "S2971:\"IEnumerable\" LINQs should be simplified", Justification = "ToArray is needed so no error is raised when a collection changes.")]
    [JsonConverter(typeof(ThemeValueJsonConverter), false)]
    public abstract class ThemeValueBase : ObservableObject, IThemeValue
    {
        private readonly Dictionary<string, ThemeValueReference?> _references;

        private string? _key;
        private IThemeManager? _themeManager;
        private object? _rawValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeValueBase"/> class.
        /// </summary>
        protected ThemeValueBase()
        {
            _references = (from p in GetType().GetProperties()
                           let ppa = p.GetCustomAttribute<ThemeValueParsedPropertyAttribute>()
                           where ppa != null
                           select ppa.RawPropertyName).ToDictionary(x => x, x => (ThemeValueReference?)null);
            PropertyChanged += OnPropertyChanged;
        }

        /// <inheritdoc/>
        [JsonIgnore]
        public string? Key
        {
            get => _key;
            set => SetProperty(ref _key, value);
        }

        /// <inheritdoc/>
        [JsonIgnore]
        public IThemeManager? ThemeManager
        {
            get => _themeManager;
            set
            {
                _ = Guard.NotNull(value, nameof(value));

                if (_themeManager != null)
                    _themeManager.ThemeValueChanged -= ThemeManagerOnThemeValueChanged;
                value.ThemeValueChanged += ThemeManagerOnThemeValueChanged;

                _references.Where(x => x.Value != null).ForEach(x => UnsubscribePropertyChange(x.Value));
                SetProperty(ref _themeManager, value);
                _references.Where(x => x.Value != null).ForEach(x => SubscribePropertyChange(x.Value));
            }
        }

        /// <inheritdoc/>
        [JsonProperty("Value")]
        public virtual object? RawValue
        {
            get => _rawValue;
            set => SetProperty(ref _rawValue, value);
        }

        /// <inheritdoc/>
        [JsonIgnore]
        [DependsOn(nameof(RawValue))]
        public object? ValueBase
        {
            get => ParseValue(RawValue);
            set => RawValue = value;
        }

        /// <inheritdoc/>
        public object? this[string propertyName]
        {
            get => ParseValue(GetProperty(propertyName, false).GetValue(this));
            set => GetProperty(propertyName, true).SetValue(this, value);
        }

        /// <inheritdoc/>
        public TValue? GetPropertyValue<TValue>(string propertyName)
        {
            var property = GetProperty(propertyName, true);
            return ParseValue<TValue>(property.GetValue(this));
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is IThemeValue other && Equals(other.RawValue, RawValue);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return RawValue?.GetHashCode() ?? 0;
        }

        /// <inheritdoc/>
        [SuppressMessage("ReflectionAnalyzers.SystemReflection", "REFL026:No parameterless constructor defined for this object.", Justification = "Implementation of this abstract class is expected to have an empty constrcutor.")]
        public virtual object Clone()
        {
            var result = (IThemeValue)Activator.CreateInstance(GetType())!;
            result.Key = Key;
            result.RawValue = RawValue.CloneIfPossible();
            return result;
        }

        /// <summary>
        /// Parses the value and handles <see cref="ThemeValueReference"/>s.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The actual value.</returns>
        protected TValue? ParseValue<TValue>(object? value)
        {
            if (value is not ThemeValueReference reference)
                return (TValue?)value;

            if (string.IsNullOrEmpty(reference.Property))
            {
                var themeValue = ThemeManager?.GetValue<TValue>(reference.CustomKey);
                return themeValue == null ? default : themeValue.Value;
            }
            else
            {
                var themeValue = ThemeManager?.GetValue(reference.CustomKey);
                return themeValue == null ? default : themeValue.GetPropertyValue<TValue>(reference.Property);
            }
        }

        /// <summary>
        /// Parses the value and handles <see cref="ThemeValueReference"/>s.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The actual value.</returns>
        protected object? ParseValue(object? value)
        {
            if (value is not ThemeValueReference reference)
                return value;

            return string.IsNullOrEmpty(reference.Property)
                ? ThemeManager?[reference.CustomKey]
                : ThemeManager?[reference.CustomKey, reference.Property];
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null && _references.TryGetValue(e.PropertyName, out var oldReference))
            {
                UnsubscribePropertyChange(oldReference);

                var reference = GetType().GetProperty(e.PropertyName)?.GetValue(this) as ThemeValueReference;
                _references[e.PropertyName] = reference;

                SubscribePropertyChange(reference);
            }
        }

        private void ReferenceTargetOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is IThemeValue themeValue)
            {
                var refsToUpdate = _references.Where(x => x.Value != null && x.Value.CustomKey == themeValue.Key && ((string.IsNullOrEmpty(x.Value.Property) && e.PropertyName == nameof(IThemeValue.ValueBase)) || e.PropertyName == x.Value.Property));
                refsToUpdate.ToArray().ForEach(x => NotifyPropertyChanged(x.Key));
            }
        }

        private void ThemeManagerOnThemeValueChanged(object? sender, ThemeValueChangedEventArgs e)
        {
            switch (e.ChangeType)
            {
                case ThemeValueChangeType.Add:
                    if (e.AddedValues != null)
                    {
                        _references.Where(x => x.Value != null && e.AddedValues.ContainsKey(x.Value.CustomKey)).ToArray().ForEach(x =>
                        {
                            SubscribePropertyChange(x.Value, e.AddedValues[x.Value!.CustomKey]);
                            NotifyPropertyChanged(x.Key);
                        });
                    }

                    break;
                case ThemeValueChangeType.Remove:
                    if (e.RemovedValues != null)
                    {
                        _references.Where(x => x.Value != null && e.RemovedValues.ContainsKey(x.Value.CustomKey)).ToArray().ForEach(x =>
                        {
                            UnsubscribePropertyChange(x.Value, e.RemovedValues[x.Value!.CustomKey]);
                            NotifyPropertyChanged(x.Key);
                        });
                    }

                    break;
                case ThemeValueChangeType.Change:
                    _references.Where(x => x.Value != null && e.HasChangeForKey(x.Value.CustomKey)).ToArray().ForEach(x =>
                    {
                        if (e.RemovedValues != null && e.RemovedValues.TryGetValue(x.Value!.CustomKey, out var removedValue))
                            UnsubscribePropertyChange(x.Value, removedValue);
                        if (e.AddedValues != null && e.AddedValues.TryGetValue(x.Value!.CustomKey, out var addedValue))
                            SubscribePropertyChange(x.Value, addedValue);
                        NotifyPropertyChanged(x.Key);
                    });
                    break;
                case ThemeValueChangeType.Clear:
                    _references.Where(x => x.Value != null).ToArray().ForEach(x =>
                    {
                        if (e.RemovedValues != null && e.RemovedValues.TryGetValue(x.Value!.CustomKey, out var removedValue))
                            UnsubscribePropertyChange(x.Value, removedValue);
                        NotifyPropertyChanged(x.Key);
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The change type \"{e.ChangeType}\" is unknown.");
            }
        }

        private void SubscribePropertyChange(ThemeValueReference? reference, IThemeValue? refTarget = null)
        {
            if (reference != null)
            {
                if (refTarget == null)
                    refTarget = ThemeManager?.GetValue(reference.CustomKey);
                if (refTarget != null)
                    refTarget.PropertyChanged += ReferenceTargetOnPropertyChanged;
            }
        }

        private void UnsubscribePropertyChange(ThemeValueReference? reference, IThemeValue? refTarget = null)
        {
            if (reference != null)
            {
                if (refTarget == null)
                    refTarget = ThemeManager?.GetValue(reference.CustomKey);
                if (refTarget != null)
                    refTarget.PropertyChanged -= ReferenceTargetOnPropertyChanged;
            }
        }

        private PropertyInfo GetProperty(string propertyName, bool rawProperty)
        {
            var type = GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (rawProperty)
            {
                var ppa = property?.GetCustomAttribute<ThemeValueParsedPropertyAttribute>();
                if (ppa != null)
                {
                    propertyName = ppa.RawPropertyName;
                    property = type.GetProperty(ppa.RawPropertyName, BindingFlags.Public | BindingFlags.Instance);
                }
            }

            return property ?? throw new InvalidOperationException($"A property with the name \"{propertyName}\" was not found on type \"{type.Name}\".");
        }
    }

    /// <summary>
    /// Generic base class for a theme value.
    /// </summary>
    /// <typeparam name="T">The type of value the theme value is for.</typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Generic representation can be in same file.")]
    public abstract class ThemeValueBase<T> : ThemeValueBase, IThemeValue<T>
    {
        /// <inheritdoc/>
        [JsonIgnore]
        [ThemeValueParsedProperty(nameof(RawValue))]
        [DependsOn(nameof(RawValue))]
        public virtual T? Value
        {
            get => ParseValue<T>(RawValue);
            set => RawValue = value;
        }
    }
}
