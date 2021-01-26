using MaSch.Presentation.Wpf.Attributes;
using MaSch.Presentation.Wpf.Observable;
using MaSch.Presentation.Wpf.Themes;
using System;
using System.Windows;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Models
{
    [ContentProperty(nameof(Value))]
    public class ThemeOverride : ObservableDependencyObject
    {
        public static readonly DependencyProperty CustomKeyProperty =
            DependencyProperty.Register(nameof(CustomKey), typeof(string), typeof(ThemeOverride), new PropertyMetadata(""));
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(ThemeOverride), new PropertyMetadata(null));

        public ThemeKey Key
        {
            get => (ThemeKey)Enum.Parse(typeof(ThemeKey), CustomKey);
            set => CustomKey = value.ToString();
        }

        [NotifyDependencyPropertyChanged(nameof(CustomKey))]
        public string CustomKey
        {
            get => (string)GetValue(CustomKeyProperty);
            set => SetValue(CustomKeyProperty, value);
        }

        [NotifyDependencyPropertyChanged(nameof(Value))]
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public ThemeOverride() { }
        public ThemeOverride(string customKey, object value)
        {
            CustomKey = customKey;
            Value = value;
        }
        public ThemeOverride(ThemeKey key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
