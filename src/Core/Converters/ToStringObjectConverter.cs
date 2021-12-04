namespace MaSch.Core.Converters;

/// <summary>
/// A <see cref="IObjectConverter"/> that converts any object by using the ToString-Method to string.
/// </summary>
public class ToStringObjectConverter : IObjectConverter
{
    private readonly int _priority;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToStringObjectConverter"/> class.
    /// </summary>
    /// <param name="priority">The priority for this <see cref="IObjectConverter"/>.</param>
    public ToStringObjectConverter(int priority)
    {
        _priority = priority;
    }

    /// <inheritdoc />
    public int GetPriority(Type? sourceType, Type targetType)
    {
        return _priority;
    }

    /// <inheritdoc />
    public bool CanConvert(Type? sourceType, Type targetType, IObjectConvertManager convertManager)
    {
        return targetType == typeof(string);
    }

    /// <inheritdoc />
    public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
    {
        return obj?.ToString();
    }
}
