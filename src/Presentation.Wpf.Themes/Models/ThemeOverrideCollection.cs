using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace MaSch.Presentation.Wpf.Models
{
    public class ThemeOverrideCollection : ObservableCollection<ThemeOverride>
    {
        private readonly List<IThemeManager> ThemeManagers = new List<IThemeManager>();

        public ThemeOverrideCollection() { }
        public ThemeOverrideCollection(IEnumerable<ThemeOverride> overrides) : base(overrides) { }

        public void RegisterThemeManager(IThemeManager themeManager)
        {
            if (!ThemeManagers.Contains(themeManager) && themeManager != null && themeManager != ThemeManager.DefaultThemeManager)
            {
                ThemeManagers.Add(themeManager);
                this.ForEach(x => AddOverride(themeManager, x));
            }
        }

        public void UnregisterThemeManager(IThemeManager themeManager)
        {
            if (themeManager != null && themeManager != Wpf.ThemeManager.DefaultThemeManager)
            {
                this.ForEach(x => RemoveOverride(themeManager, x));
                ThemeManagers.TryRemove(themeManager);
            }
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

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            foreach(var themeManager in ThemeManagers)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        e.NewItems.OfType<ThemeOverride>().ForEach(x => AddOverride(themeManager, x));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        e.OldItems.OfType<ThemeOverride>().ForEach(x => RemoveOverride(themeManager, x));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        e.OldItems.OfType<ThemeOverride>().ForEach(x => RemoveOverride(themeManager, x));
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

        private void ThemeOverrideOnDependencyPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var @override = (ThemeOverride)sender;
            if (e.Property == ThemeOverride.ValueProperty)
                ThemeManagers.ForEach(x => x.SetValue(@override.CustomKey, e.NewValue));
            if (e.Property == ThemeOverride.CustomKeyProperty)
            {
                foreach (var themeManager in ThemeManagers)
                {
                    themeManager.RemoveValue((string)e.OldValue);
                    themeManager.SetValue((string)e.NewValue, @override.Value);
                }
            }
        }
    }
}
