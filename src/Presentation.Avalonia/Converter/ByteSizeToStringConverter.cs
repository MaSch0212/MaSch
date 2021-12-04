using Avalonia.Data.Converters;

namespace MaSch.Presentation.Avalonia.Converter;

/// <summary>
/// Converter for convertion from byte size to string.
/// </summary>
public class ByteSizeToStringConverter : IValueConverter
{
    private static readonly string[] Suffixes =
    {
        "Byte", "KB", "MB", "GB", "TB",
    };

    /// <summary>
    /// Formats the specified value to a byte representation.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A <see cref="ValueTuple{T1, T2}"/> containing the number as first item and byte-size-name as the second item.</returns>
    public static (double Value, string Suffix) Format(double value)
    {
        int i = 0;
        while (value > 1000 && i < Suffixes.Length)
        {
            value /= 1024D;
            i++;
        }

        return (value, Suffixes[i]);
    }

    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var (bytes, suffix) = Format(System.Convert.ToDouble(value));
        return $"{bytes:0.00} {suffix}";
    }

    /// <inheritdoc />
    public object? ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var s = System.Convert.ToString(value)?.Split(' ');
        if (s == null || s.Length < 2)
            return null;
        return double.Parse(s[0]) * Suffixes.ToList().IndexOf(s[1]);
    }
}
