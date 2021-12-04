using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ThemeValues;

/// <summary>
/// Registry for theme values.
/// </summary>
public static class ThemeValueRegistry
{
    private static readonly IDictionary<string, Type> TypeEnumToTypeMapping;
    private static readonly IDictionary<Type, string> TypeTypeToEnumMapping;
    private static readonly IList<(string TypeName, Type Type, Type ThemeValueType)> TypeThemeValueMapping;

    static ThemeValueRegistry()
    {
        var defaultTypeMapping = new (string TypeName, Type Type, Type ThemeValueType)[]
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

        TypeEnumToTypeMapping = defaultTypeMapping.ToDictionary(x => x.TypeName, x => x.ThemeValueType);
        TypeTypeToEnumMapping = defaultTypeMapping.ToDictionary(x => x.ThemeValueType, x => x.TypeName);
        TypeThemeValueMapping = defaultTypeMapping.ToList();
    }

    /// <summary>
    /// Gets the tuntime type of the value type.
    /// </summary>
    /// <param name="valueType">Type of the value.</param>
    /// <returns>The runtime type of the specified value type.</returns>
    public static Type GetRuntimeValueType(string valueType)
    {
        return TypeEnumToTypeMapping[valueType];
    }

    /// <summary>
    /// Gets the value type enum.
    /// </summary>
    /// <param name="valueType">Type of the value.</param>
    /// <returns>The name of the value type.</returns>
    public static string GetValueTypeEnum(Type valueType)
    {
        return TypeTypeToEnumMapping[valueType];
    }

    /// <summary>
    /// Gets the theme value type of the runtime type.
    /// </summary>
    /// <param name="actualValueType">Runtime type of the value.</param>
    /// <returns>The type of the <see cref="IThemeValue"/> class.</returns>
    /// <exception cref="InvalidOperationException">The type \"{actualValueType.FullName}\" is currently not supported for Theming.</exception>
    public static Type GetThemeValueType(Type actualValueType)
    {
        return TypeThemeValueMapping.TryFirst(x => x.Type.IsAssignableFrom(actualValueType), out var map)
                       ? map.ThemeValueType
                       : throw new InvalidOperationException($"The type \"{actualValueType.FullName}\" is currently not supported for Theming.");
    }

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
        if (TypeThemeValueMapping.Any(x => x.Type.IsAssignableFrom(type)))
            throw new InvalidOperationException($"A type mapping for type \"{type.FullName}\" already exists.");

        TypeEnumToTypeMapping.Add(typeName, themeValueType);
        TypeTypeToEnumMapping.Add(themeValueType, typeName);
        TypeThemeValueMapping.Add((typeName, type, themeValueType));
    }
}
