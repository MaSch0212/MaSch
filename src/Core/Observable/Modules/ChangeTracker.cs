using MaSch.Core.Attributes;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MaSch.Core.Observable.Modules
{
    /// <summary>
    /// The default <see cref="IChangeTracker"/> implementation.
    /// </summary>
    /// <seealso cref="IChangeTracker" />
    public class ChangeTracker : IChangeTracker
    {
        #region Fields

        private const string FixedChangeKey = "{FixedChange}";

        private readonly IDictionary<string, ChangeTrackingEntry> _baseValues = new Dictionary<string, ChangeTrackingEntry>();
        private readonly Type _trackedObjectType;

        private Func<bool, bool> _hasChangesExtension;
        private bool _lastHasChanges;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeTracker"/> class.
        /// </summary>
        /// <param name="trackedObjectType">Type of the tracked object.</param>
        /// <param name="implicitlyRecurse">If set to <c>true</c> change tracking for <see cref="IChangeTrackedObject"/> properties is used recursively.</param>
        public ChangeTracker(Type trackedObjectType, bool implicitlyRecurse)
        {
            ImplicitlyRecurse = implicitlyRecurse;
            _trackedObjectType = trackedObjectType;
            _hasChangesExtension = b => b;
        }

        #region Events

        /// <inheritdoc/>
        public event EventHandler? HasChangesChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the properties that are excluded from change tracking.
        /// </summary>
        /// <value>
        /// The properties that are excluded from change tracking.
        /// </value>
        public ICollection<string> ChangeTrackingDisabledProperties { get; set; } = new HashSet<string>();

        /// <inheritdoc/>
        public virtual bool HasChanges => HasChangesExtension(_baseValues.Any(x => x.Value.HasChanged));

        /// <inheritdoc/>
        public virtual bool ImplicitlyRecurse { get; }

        /// <inheritdoc/>
        public Func<bool, bool> HasChangesExtension
        {
            get => _hasChangesExtension;
            set
            {
                if (value == null)
                    _hasChangesExtension = b => b;
                else
                    _hasChangesExtension = value;
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public virtual string[] GetChangedProperties(bool recursive)
        {
            var result = new List<string>();
            result.AddRange(_baseValues.Where(x => x.Value.HasChanged).Select(x => x.Key));
            if (recursive)
            {
                result.AddRange(_baseValues.SelectMany(x => (x.Value.CurrentValue as IChangeTracker)?.GetChangedProperties(true)
                    .Select(y => $"{x.Key}.{y}") ?? Array.Empty<string>()));
            }

            return result.ToArray();
        }

        /// <inheritdoc/>
        public virtual bool HasPropertyChanged([CallerMemberName] string propertyName = "")
        {
            return _baseValues.ContainsKey(propertyName) && _baseValues[propertyName].HasChanged;
        }

        /// <inheritdoc/>
        public virtual void SetBaseValue(object? value, [CallerMemberName] string propertyName = "")
        {
            var propInfo = _trackedObjectType.GetProperty(propertyName);
            if (!ChangeTrackingDisabledProperties.Contains(propertyName) && propInfo?.GetCustomAttribute<NoChangeTrackingAttribute>() == null)
            {
                var recurse = ImplicitlyRecurse;
                if (!recurse)
                    recurse = propInfo?.GetCustomAttribute<RecursiveChangeTrackingAttribute>() != null;
                _baseValues[propertyName] = new ChangeTrackingEntry(this, recurse) { BaseValue = value, CurrentValue = value };
            }
        }

        /// <inheritdoc/>
        public void OnSetValue(object? value, [CallerMemberName] string propertyName = "")
        {
            if (!_baseValues.ContainsKey(propertyName))
            {
                SetBaseValue(value, propertyName);
            }
            else
            {
                _baseValues[propertyName].CurrentValue = value;
                EvaluateHasChangesChanged();
            }
        }

        /// <inheritdoc/>
        public void EvaluateHasChangesChanged()
        {
            var hasChanges = HasChanges;
            if (_lastHasChanges != hasChanges)
                HasChangesChanged?.Invoke(this, new EventArgs());
            _lastHasChanges = hasChanges;
        }

        /// <inheritdoc/>
        public void ResetChangeTracking()
        {
            _baseValues.ForEach(x => x.Value.Reset());
            RemoveFixedChangeInternal();
            EvaluateHasChangesChanged();
        }

        /// <inheritdoc/>
        public void ResetChangeTracking(params string[] propertyNames)
        {
            propertyNames.Where(x => _baseValues.ContainsKey(x)).ForEach(x => _baseValues[x].Reset());
            EvaluateHasChangesChanged();
        }

        /// <inheritdoc/>
        public void AddFixedChange()
        {
            if (!_baseValues.ContainsKey(FixedChangeKey))
            {
                _baseValues[FixedChangeKey] = new ChangeTrackingEntry(this, false) { IsFixed = true };
                EvaluateHasChangesChanged();
            }
        }

        /// <inheritdoc/>
        public void RemoveFixedChange()
        {
            if (RemoveFixedChangeInternal())
                EvaluateHasChangesChanged();
        }

        #endregion

        #region Private Methods

        private bool RemoveFixedChangeInternal()
        {
            if (_baseValues.ContainsKey(FixedChangeKey))
            {
                _baseValues.Remove(FixedChangeKey);
                return true;
            }

            return false;
        }

        #endregion

        #region Classes

        private class ChangeTrackingEntry
        {
            private readonly IChangeTracker _owner;
            private readonly bool _recurse;
            private object? _currentValue;
            private object? _baseValue;

            public ChangeTrackingEntry(IChangeTracker owner, bool recurse)
            {
                _owner = owner ?? throw new ArgumentNullException(nameof(owner));
                _recurse = recurse;
            }

            public bool HasChanged
            {
                get
                {
                    if (IsFixed)
                        return true;
                    if (BaseValue is IEnumerable<object> baseValueList && CurrentValue is IEnumerable<object> currentValueList)
                    {
                        var currentValueArray = currentValueList as object[] ?? currentValueList.ToArray();
                        if (!baseValueList.SequenceEqual(currentValueArray))
                            return true;
                        var ictChange = _recurse && currentValueArray.OfType<IChangeTrackedObject>().Any(x => x.HasChanges);
                        return ictChange;
                    }
                    else
                    {
                        if (CurrentValue is double d1 && BaseValue is double d2)
                            return Math.Abs(d1 - d2) > 0.00001;
                        if (CurrentValue is float f1 && BaseValue is float f2)
                            return Math.Abs(f1 - f2) > 0.00001;
                        if (!(CurrentValue?.Equals(BaseValue) ?? BaseValue is null))
                            return true;
                        if (CurrentValue is IChangeTrackedObject cto && _recurse && cto.HasChanges)
                            return true;
                        return false;
                    }
                }
            }

            public object? BaseValue
            {
                get => _baseValue;
                set
                {
                    if (value is IEnumerable<object> list)
                        _baseValue = list.ToList();
                    else
                        _baseValue = value;
                }
            }

            public object? CurrentValue
            {
                get => _currentValue;
                set
                {
                    if (_currentValue is IChangeTrackedObject ct1 && ct1.ChangeTracker != null)
                        ct1.ChangeTracker.HasChangesChanged -= CurrentValue_HasChangesChanged;

                    _currentValue = value;

                    if (_currentValue is IChangeTrackedObject ct2 && ct2.ChangeTracker != null)
                        ct2.ChangeTracker.HasChangesChanged += CurrentValue_HasChangesChanged;
                }
            }

            public bool IsFixed { get; set; }

            public void Reset()
            {
                if (CurrentValue is IEnumerable<object> currentValueList && _recurse)
                    currentValueList.OfType<IChangeTrackedObject>().ForEach(x => x.ResetChangeTracking());
                if (CurrentValue is IChangeTrackedObject cto && _recurse)
                    cto.ResetChangeTracking();
                BaseValue = CurrentValue;
            }

            private void CurrentValue_HasChangesChanged(object? sender, EventArgs e)
            {
                _owner.EvaluateHasChangesChanged();
            }
        }

        #endregion
    }
}
