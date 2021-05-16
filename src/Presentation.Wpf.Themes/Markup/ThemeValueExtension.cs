using MaSch.Core.Extensions;
using MaSch.Presentation.Wpf.DependencyProperties;
using MaSch.Presentation.Wpf.Themes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Markup
{
    /// <summary>
    /// Binding that is used to reference theme values.
    /// </summary>
    /// <seealso cref="System.Windows.Data.Binding" />
    public class ThemeValueExtension : Binding
    {
        private string? _customKey;
        private string? _propertyName;
        private int _ancestorLevel = 0;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [ConstructorArgument("key")]
        public ThemeKey? Key
        {
            get => CustomKey == null ? null : Enum.Parse(typeof(ThemeKey), CustomKey) as ThemeKey?;
            set => CustomKey = value.ToString();
        }

        /// <summary>
        /// Gets or sets the custom key.
        /// </summary>
        [ConstructorArgument("customKey")]
        public string? CustomKey
        {
            get => _customKey;
            set
            {
                _customKey = value;
                RebuildPropertyPath();
            }
        }

        /// <summary>
        /// Gets or sets the name of the property to reference from the theme value.
        /// </summary>
        public string? PropertyName
        {
            get => _propertyName;
            set
            {
                _propertyName = value;
                RebuildPropertyPath();
            }
        }

        /// <summary>
        /// Gets or sets the ancestor level for the theme managers.
        /// </summary>
        public int AncestorLevel
        {
            get => _ancestorLevel;
            set
            {
                _ancestorLevel = value;
                RebuildPropertyPath();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeValueExtension"/> class.
        /// </summary>
        public ThemeValueExtension()
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.Self);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeValueExtension"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public ThemeValueExtension(ThemeKey key)
            : this(key.ToString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeValueExtension"/> class.
        /// </summary>
        /// <param name="customKey">The custom key.</param>
        public ThemeValueExtension(string customKey)
            : this()
        {
            CustomKey = customKey;
        }

        private void RebuildPropertyPath()
        {
            var propertyName = string.IsNullOrEmpty(PropertyName) ? string.Empty : $".{PropertyName}";
            var parentExpression = string.Concat(Enumerable.Range(0, AncestorLevel).Select(x => ".ParentThemeManager"));
            Path = new PropertyPath($"(0){parentExpression}.Bindings[{CustomKey}].Value.Value{propertyName}", Theming.ThemeManagerProperty);
        }
    }
}
