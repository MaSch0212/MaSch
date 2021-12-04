namespace MaSch.Core.Converters;

/// <summary>
/// A <see cref="IObjectConverter"/> that is converting one Enum to another.
/// </summary>
public class EnumConverter : IObjectConverter
{
    private static readonly ConvertibleObjectConverter ConvertibleConverter = new();
    private readonly int _priority;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumConverter"/> class.
    /// </summary>
    /// <param name="priority">The priority for this <see cref="IObjectConverter"/>.</param>
    public EnumConverter(int priority = 0)
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
        if (sourceType == null || targetType == null)
            return false;
        return (typeof(Enum).IsAssignableFrom(targetType) && ConvertibleConverter.CanConvert(sourceType, Enum.GetUnderlyingType(targetType), convertManager)) ||
               (typeof(Enum).IsAssignableFrom(sourceType) && ConvertibleConverter.CanConvert(targetType, Enum.GetUnderlyingType(sourceType), convertManager));
    }

    /// <inheritdoc />
    public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
    {
        if (typeof(Enum).IsAssignableFrom(targetType))
        {
            var resultType = Enum.GetUnderlyingType(targetType);
            var result = ConvertibleConverter.Convert(obj, sourceType, resultType, convertManager, formatProvider);
            if (result == null || !targetType.IsEnumDefined(result))
                throw new ArgumentOutOfRangeException(nameof(obj), $"A enum member with value \"{result ?? "(null)"}\" is not defined in the enum \"{targetType.FullName}\".");
            return result.CastTo(targetType);
        }

        return ConvertibleConverter.Convert(obj, sourceType, targetType, convertManager, formatProvider);
    }
}
