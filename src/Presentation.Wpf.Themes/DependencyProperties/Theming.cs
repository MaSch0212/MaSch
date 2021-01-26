using MaSch.Presentation.Wpf.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public class Theming
    {
        public static readonly DependencyProperty ThemeManagerProperty =
            DependencyProperty.RegisterAttached("ThemeManager",
                                                typeof(IThemeManager),
                                                typeof(Theming),
                                                new FrameworkPropertyMetadata(ThemeManager.DefaultThemeManager, FrameworkPropertyMetadataOptions.Inherits, ThemeManagerPropertyChanged));

        public static readonly DependencyProperty ThemeOverridesProperty =
            DependencyProperty.RegisterAttached("ThemeOverrides",
                                                typeof(ThemeOverrideCollection),
                                                typeof(Theming),
                                                new UIPropertyMetadata(ThemeOverridesPropertyChanged));

        private static void ThemeOverridesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as ThemeOverrideCollection;
            var newValue = e.NewValue as ThemeOverrideCollection;

            IThemeManager themeManager = null;
            if (DependencyPropertyHelper.GetValueSource(source, ThemeManagerProperty).BaseValueSource == BaseValueSource.Local)
                themeManager = GetThemeManager(source);
            else if (newValue != null)
            {
                themeManager = new ThemeManager(GetThemeManager(source));
                SetThemeManager(source, themeManager);
            }

            if (oldValue != null)
                oldValue.UnregisterThemeManager(themeManager);

            if (newValue != null)
                newValue.RegisterThemeManager(themeManager);
        }

        private static void ThemeManagerPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var overrides = GetThemeOverrides(source);
            if (overrides != null)
            {
                overrides.UnregisterThemeManager((IThemeManager)e.OldValue);
                overrides.RegisterThemeManager((IThemeManager)e.NewValue);
            }

            DependencyObject parent = null;
            if (source is FrameworkElement element)
                parent = element.Parent;
            else if (source is Visual || source is Visual3D)
                parent = VisualTreeHelper.GetParent(source);
            if (parent != null && GetThemeManager(parent) != e.NewValue)
            {
                if (!(e.OldValue is IThemeManager oldTm) || oldTm.ParentThemeManager == null)
                    DependencyPropertyDescriptor.FromProperty(ThemeManagerProperty, parent.GetType()).AddValueChanged(parent, OnParentThemeManagerChanged);
                else if (!(e.NewValue is IThemeManager newTm) || newTm.ParentThemeManager == null)
                    DependencyPropertyDescriptor.FromProperty(ThemeManagerProperty, parent.GetType()).RemoveValueChanged(parent, OnParentThemeManagerChanged);
            }

            void OnParentThemeManagerChanged(object sender, EventArgs ea)
            {
                if (sender is DependencyObject obj)
                {
                    var childThemeManager = GetThemeManager(source);
                    var parentThemeManager = GetThemeManager(obj);
                    if (childThemeManager != null)
                        childThemeManager.ParentThemeManager = parentThemeManager;
                }
            }
        }


        public static void SetThemeManager(DependencyObject element, IThemeManager value)
            => element.SetValue(ThemeManagerProperty, value);
        public static IThemeManager GetThemeManager(DependencyObject element)
            => (IThemeManager)element.GetValue(ThemeManagerProperty);

        public static void SetThemeOverrides(DependencyObject element, ThemeOverrideCollection value)
            => element.SetValue(ThemeOverridesProperty, value);
        public static ThemeOverrideCollection GetThemeOverrides(DependencyObject element)
            => (ThemeOverrideCollection)element.GetValue(ThemeOverridesProperty);
    }
}
