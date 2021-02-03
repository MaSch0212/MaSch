using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ThemeValues
{
    /// <summary>
    /// Registry for theme values.
    /// </summary>
    public static class ThemeValueRegistry
    {
        static ThemeValueRegistry()
        {
            var defaultTypeMapping = new (string typeName, Type type, Type themeValueType)[]
            {
                ("Color", typeof(Color), typeof(ColorThemeValue)),
                ("Boolean", typeof(bool), typeof(BooleanThemeValue)),
                ("SolidColorBrush", typeof(SolidColorBrush), typeof(SolidColorBrushThemeValue)),
                ("Thickness", typeof(Thickness), typeof(ThicknessThemeValue)),
                ("CornerRadius", typeof(CornerRadius), typeof(CornerRadiusThemeValue)),
                ("Icon", typeof(Icon), typeof(IconThemeValue)),
                ("Double", typeof(double), typeof(DoubleThemeValue)),
                ("FontFamily", typeof(FontFamily), typeof(FontFamilyThemeValue)),
                ("FontStyle", typeof(FontStyle), typeof(FontStyleThemeValue)),
                ("ImageSource", typeof(ImageSource), typeof(ImageSourceThemeValue)),
                ("FontWeight", typeof(FontWeight), typeof(FontWeightThemeValue)),
            };

            TypeEnumToTypeMapping = defaultTypeMapping.ToDictionary(x => x.typeName, x => x.themeValueType);
            TypeTypeToEnumMapping = defaultTypeMapping.ToDictionary(x => x.themeValueType, x => x.typeName);
            TypeThemeValueMapping = defaultTypeMapping.ToList();
        }

        private static readonly IDictionary<string, Type> TypeEnumToTypeMapping;
        private static readonly IDictionary<Type, string> TypeTypeToEnumMapping;
        private static readonly IList<(string typeName, Type type, Type themeValueType)> TypeThemeValueMapping;

        /// <summary>
        /// Gets the tuntime type of the value type.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>The runtime type of the specified value type.</returns>
        public static Type GetRuntimeValueType(string valueType) => TypeEnumToTypeMapping[valueType];

        /// <summary>
        /// Gets the value type enum.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        /// <returns>The name of the value type.</returns>
        public static string GetValueTypeEnum(Type valueType) => TypeTypeToEnumMapping[valueType];

        /// <summary>
        /// Gets the theme value type of the runtime type.
        /// </summary>
        /// <param name="actualValueType">Runtime type of the value.</param>
        /// <returns>The type of the <see cref="IThemeValue"/> class.</returns>
        /// <exception cref="InvalidOperationException">The type \"{actualValueType.FullName}\" is currently not supported for Theming.</exception>
        public static Type GetThemeValueType(Type actualValueType)
            => TypeThemeValueMapping.TryFirst(x => x.type.IsAssignableFrom(actualValueType), out var map)
                ? map.themeValueType
                : throw new InvalidOperationException($"The type \"{actualValueType.FullName}\" is currently not supported for Theming.");

        /// <summary>
        /// Registers a new mapping between runtime and theme value type..
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="type">The runtime type.</param>
        /// <param name="themeValueType">Type of the theme value.</param>
        /// <exception cref="InvalidOperationException">
        /// A type with name <paramref name="typeName"/> is already registered.
        /// or
        /// The value type class <paramref name="themeValueType"/> is already registered.
        /// or
        /// A type mapping for type <paramref name="type"/> already exists.
        /// </exception>
        public static void RegisterType(string typeName, Type type, Type themeValueType)
        {
            if (TypeEnumToTypeMapping.ContainsKey(typeName))
                throw new InvalidOperationException($"A type with name \"{typeName}\" is already registered.");
            if (TypeTypeToEnumMapping.ContainsKey(themeValueType))
                throw new InvalidOperationException($"The value type class \"{themeValueType}\" is already registered.");
            if (TypeThemeValueMapping.Any(x => x.type.IsAssignableFrom(type)))
                throw new InvalidOperationException($"A type mapping for type \"{type.FullName}\" already exists.");

            TypeEnumToTypeMapping.Add(typeName, themeValueType);
            TypeTypeToEnumMapping.Add(themeValueType, typeName);
            TypeThemeValueMapping.Add((typeName, type, themeValueType));
        }
    }
}
