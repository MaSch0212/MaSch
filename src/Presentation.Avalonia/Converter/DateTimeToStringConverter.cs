using Avalonia.Data.Converters;

namespace MaSch.Presentation.Avalonia.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that converts a <see cref="DateTime"/> into a string using the
/// <see cref="DateTimeFormatInfo.ShortDatePattern"/> and <see cref="DateTimeFormatInfo.ShortTimePattern"/> from the current culture.
/// </summary>
/// <seealso cref="IValueConverter" />
public class DateTimeToStringConverter : IValueConverter
{
    /// <inheritdoc />
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dt)
            return dt.ToString(culture.DateTimeFormat.ShortDatePattern + " " + culture.DateTimeFormat.ShortTimePattern, culture);
        return value?.ToString();
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new InvalidOperationException();
    }
}
