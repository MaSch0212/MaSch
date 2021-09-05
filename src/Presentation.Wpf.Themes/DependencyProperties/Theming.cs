using MaSch.Presentation.Wpf.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties used for theming.
    /// </summary>
    public static class Theming
    {
        /// <summary>
        /// Dependency property. Gets or sets the theme manager for this control. This property is inherited by child elements.
        /// </summary>
        public static readonly DependencyProperty ThemeManagerProperty =
            DependencyProperty.RegisterAttached(
                "ThemeManager",
                typeof(IThemeManager),
                typeof(Theming),
                new FrameworkPropertyMetadata(ThemeManager.DefaultThemeManager, FrameworkPropertyMetadataOptions.Inherits, ThemeManagerPropertyChanged));

        /// <summary>
        /// Dependency property. Gets or sets the theme overrides used for this element.
        /// </summary>
        public static readonly DependencyProperty ThemeOverridesProperty =
            DependencyProperty.RegisterAttached(
                "ThemeOverrides",
                typeof(ThemeOverrideCollection),
                typeof(Theming),
                new UIPropertyMetadata(ThemeOverridesPropertyChanged));

        /// <summary>
        /// Sets the value of the <see cref="ThemeManagerProperty"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetThemeManager(DependencyObject element, IThemeManager value)
        {
            element.SetValue(ThemeManagerProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="ThemeManagerProperty"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The value of the <see cref="ThemeManagerProperty"/>.</returns>
        public static IThemeManager GetThemeManager(DependencyObject element)
        {
            return (IThemeManager)element.GetValue(ThemeManagerProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="ThemeOverridesProperty"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetThemeOverrides(DependencyObject element, ThemeOverrideCollection value)
        {
            element.SetValue(ThemeOverridesProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="ThemeOverridesProperty"/>.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The value of the <see cref="ThemeOverridesProperty"/>.</returns>
        public static ThemeOverrideCollection GetThemeOverrides(DependencyObject element)
        {
            return (ThemeOverrideCollection)element.GetValue(ThemeOverridesProperty);
        }

        private static void ThemeOverridesPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as ThemeOverrideCollection;

            IThemeManager? themeManager = null;
            if (DependencyPropertyHelper.GetValueSource(source, ThemeManagerProperty).BaseValueSource == BaseValueSource.Local)
            {
                themeManager = GetThemeManager(source);
            }
            else if (newValue != null)
            {
                themeManager = new ThemeManager(GetThemeManager(source));
                SetThemeManager(source, themeManager);
            }

            if (e.OldValue is ThemeOverrideCollection oldValue)
                oldValue.UnregisterThemeManager(themeManager);

            if (newValue != null && themeManager != null)
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

            DependencyObject? parent = null;
            if (source is FrameworkElement element)
                parent = element.Parent;
            else if (source is Visual || source is Visual3D)
                parent = VisualTreeHelper.GetParent(source);
            if (parent != null && GetThemeManager(parent) != e.NewValue)
            {
                if (e.OldValue is not IThemeManager oldTm || oldTm.ParentThemeManager == null)
                    DependencyPropertyDescriptor.FromProperty(ThemeManagerProperty, parent.GetType()).AddValueChanged(parent, OnParentThemeManagerChanged);
                else if (e.NewValue is not IThemeManager newTm || newTm.ParentThemeManager == null)
                    DependencyPropertyDescriptor.FromProperty(ThemeManagerProperty, parent.GetType()).RemoveValueChanged(parent, OnParentThemeManagerChanged);
            }

            void OnParentThemeManagerChanged(object? sender, EventArgs ea)
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
    }
}
