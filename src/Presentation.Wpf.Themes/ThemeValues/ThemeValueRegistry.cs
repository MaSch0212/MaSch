using MaSch.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ThemeValues
{
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

        public static Type GetRuntimeValueType(string valueType) => TypeEnumToTypeMapping[valueType];
        public static string GetValueTypeEnum(Type valueType) => TypeTypeToEnumMapping[valueType];
        public static Type GetThemeValueType(Type actualValueType) 
            => TypeThemeValueMapping.TryFirst(x => x.type.IsAssignableFrom(actualValueType), out var map) 
                ? map.themeValueType 
                : throw new InvalidOperationException($"The type \"{actualValueType.FullName}\" is currently not supported for Theming.");

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
