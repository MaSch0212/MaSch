using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using MaSch.Core.Extensions;
using MaSch.Presentation.Wpf.DependencyProperties;
using MaSch.Presentation.Wpf.Themes;

namespace MaSch.Presentation.Wpf.Markup
{
    public class ThemeValueExtension : Binding
    {
        private string _customKey;
        private string _propertyName;
        private int _ancestorLevel = 0;

        [ConstructorArgument("key")]
        public ThemeKey Key
        {
            get => (ThemeKey) Enum.Parse(typeof(ThemeKey), CustomKey);
            set => CustomKey = value.ToString();
        }

        [ConstructorArgument("customKey")]
        public string CustomKey
        {
            get => _customKey;
            set
            {
                _customKey = value;
                RebuildPropertyPath();
            }
        }

        public string PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                RebuildPropertyPath();
            }
        }

        public int AncestorLevel
        {
            get => _ancestorLevel;
            set
            {
                _ancestorLevel = value;
                RebuildPropertyPath();
            }
        }

        public ThemeValueExtension()
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.Self);
        }
        public ThemeValueExtension(ThemeKey key) : this(key.ToString()) { }
        public ThemeValueExtension(string customKey) : this() { CustomKey = customKey; }

        private void RebuildPropertyPath()
        {
            var propertyName = string.IsNullOrEmpty(PropertyName) ? "" : $".{PropertyName}";
            var parentExpression = string.Concat(Enumerable.Range(0, AncestorLevel).Select(x => ".ParentThemeManager"));
            Path = new PropertyPath($"(0){parentExpression}.Bindings[{CustomKey}].Value.Value{propertyName}", Theming.ThemeManagerProperty);
        }
    }
}
