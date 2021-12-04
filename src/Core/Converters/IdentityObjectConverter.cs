namespace MaSch.Core.Converters;

/// <summary>
/// A <see cref="IObjectConverter"/> that is using no convertion.
/// </summary>
public class IdentityObjectConverter : IObjectConverter
{
    private readonly int _priority;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityObjectConverter"/> class.
    /// </summary>
    /// <param name="priority">The priority for this <see cref="IObjectConverter"/>.</param>
    public IdentityObjectConverter(int priority)
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
        return sourceType == targetType || targetType == typeof(object);
    }

    /// <inheritdoc />
    public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
    {
        return obj;
    }
}
