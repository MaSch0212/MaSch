using MaSch.Core.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace MaSch.Presentation.Wpf.Models
{
    /// <summary>
    /// Collection of <see cref="ThemeOverride"/>s.
    /// </summary>
    /// <seealso cref="ObservableCollection{T}" />
    public class ThemeOverrideCollection : ObservableCollection<ThemeOverride>
    {
        private readonly List<IThemeManager> _themeManagers = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeOverrideCollection"/> class.
        /// </summary>
        public ThemeOverrideCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeOverrideCollection"/> class.
        /// </summary>
        /// <param name="overrides">The overrides.</param>
        public ThemeOverrideCollection(IEnumerable<ThemeOverride> overrides)
            : base(overrides)
        {
        }

        /// <summary>
        /// Registers a theme manager with the overrides in this collection.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        public void RegisterThemeManager(IThemeManager themeManager)
        {
            if (!_themeManagers.Contains(themeManager) && themeManager != null && themeManager != ThemeManager.DefaultThemeManager)
            {
                _themeManagers.Add(themeManager);
                this.ForEach(x => AddOverride(themeManager, x));
            }
        }

        /// <summary>
        /// Unregisters a theme manager with the overrides in this collection.
        /// </summary>
        /// <param name="themeManager">The theme manager.</param>
        public void UnregisterThemeManager(IThemeManager? themeManager)
        {
            if (themeManager != null && themeManager != ThemeManager.DefaultThemeManager)
            {
                this.ForEach(x => RemoveOverride(themeManager, x));
                _ = _themeManagers.TryRemove(themeManager);
            }
        }

        /// <inheritdoc/>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            foreach (var themeManager in _themeManagers)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems != null)
                            e.NewItems.OfType<ThemeOverride>().ForEach(x => AddOverride(themeManager, x));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems != null)
                            e.OldItems.OfType<ThemeOverride>().ForEach(x => RemoveOverride(themeManager, x));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        if (e.OldItems != null)
                            e.OldItems.OfType<ThemeOverride>().ForEach(x => RemoveOverride(themeManager, x));
                        if (e.NewItems != null)
                            e.NewItems.OfType<ThemeOverride>().ForEach(x => AddOverride(themeManager, x));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        themeManager.CurrentTheme.Values.Clear();
                        break;
                    case NotifyCollectionChangedAction.Move:
                    default:
                        break;
                }
            }

            base.OnCollectionChanged(e);
        }

        private void AddOverride(IThemeManager themeManager, ThemeOverride @override)
        {
            @override.DependencyPropertyChanged += ThemeOverrideOnDependencyPropertyChanged;
            themeManager.SetValue(@override.CustomKey, @override.Value);
        }

        private void RemoveOverride(IThemeManager themeManager, ThemeOverride @override)
        {
            @override.DependencyPropertyChanged -= ThemeOverrideOnDependencyPropertyChanged;
            themeManager.RemoveValue(@override.CustomKey);
        }

        private void ThemeOverrideOnDependencyPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var @override = (ThemeOverride)sender;
            if (e.Property == ThemeOverride.ValueProperty)
                _themeManagers.ForEach(x => x.SetValue(@override.CustomKey, e.NewValue));
            if (e.Property == ThemeOverride.CustomKeyProperty)
            {
                foreach (var themeManager in _themeManagers)
                {
                    themeManager.RemoveValue((string)e.OldValue);
                    themeManager.SetValue((string)e.NewValue, @override.Value);
                }
            }
        }
    }
}
