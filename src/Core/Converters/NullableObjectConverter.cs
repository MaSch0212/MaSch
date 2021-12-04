namespace MaSch.Core.Converters;

/// <summary>
/// A <see cref="IObjectConverter"/> that can <see cref="Nullable{T}"/> values.
/// </summary>
public class NullableObjectConverter : IObjectConverter
{
    private readonly int _priority;

    /// <summary>
    /// Initializes a new instance of the <see cref="NullableObjectConverter"/> class.
    /// </summary>
    /// <param name="priority">The priority for this <see cref="IObjectConverter"/>.</param>
    public NullableObjectConverter(int priority = 0)
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
        var (sourceNullable, targetNullable, realSourceType, realTargetType) = GetTypeInformation(sourceType, targetType);
        if (sourceType == null)
            return targetNullable;
        return (sourceNullable || targetNullable) && convertManager.CanConvert(realSourceType, realTargetType);
    }

    /// <inheritdoc />
    public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
    {
        var (sourceNullable, targetNullable, realSourceType, realTargetType) = GetTypeInformation(sourceType, targetType);
        if (obj == null)
            return targetNullable ? (object?)null : throw new InvalidCastException($"null cannot be converted to not nullable type \"{targetType.FullName}\".");

        object? val = obj;
        if (sourceNullable)
        {
            var property = sourceType?.GetProperty("Value");
            if (property == null)
                throw new InvalidOperationException($"The property \"Value\" was not found on type \"{sourceType?.FullName ?? "(null)"}\".");
            val = property.GetValue(obj);
        }

        var result = convertManager.Convert(val, realSourceType, realTargetType, formatProvider);
        return targetNullable ? Activator.CreateInstance(targetType, result) : result;
    }

    private static (bool SourceNullable, bool TargetNullable, Type? RealSourceType, Type RealTargetType) GetTypeInformation(Type? sourceType, Type targetType)
    {
        var sourceNullable = sourceType == null || (sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == typeof(Nullable<>));
        var targetNullable = targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>);
        var realSourceType = sourceNullable ? sourceType?.GetGenericArguments()[0] : sourceType;
        var realTargetType = targetNullable ? targetType.GetGenericArguments()[0] : targetType;
        return (sourceNullable, targetNullable, realSourceType, realTargetType);
    }
}
