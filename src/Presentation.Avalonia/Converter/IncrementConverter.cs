using Avalonia.Data.Converters;

namespace MaSch.Presentation.Avalonia.Converter;

/// <summary>
/// A <see cref="IValueConverter"/> that increments a number by one. Can also increased by more using the parameter.
/// </summary>
/// <seealso cref="IValueConverter" />
public class IncrementConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!int.TryParse(parameter?.ToString(), out int p))
            p = 1;
        if (int.TryParse(value?.ToString(), out int i))
            return i + p;
        return 0;
    }

    /// <inheritdoc />
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (!int.TryParse(parameter?.ToString(), out int p))
            p = 1;
        if (int.TryParse(value?.ToString(), out int i))
            return i - p;
        return 0;
    }
}
