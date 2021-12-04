namespace MaSch.Core.Converters;

/// <summary>
/// A <see cref="IObjectConverter"/> that is using the <see cref="IConvertible"/> interface.
/// </summary>
public class ConvertibleObjectConverter : IObjectConverter
{
    private static readonly Dictionary<Type, List<Func<Type, bool>>> CanConvertFunctions = new();
    private readonly int _priority;

    static ConvertibleObjectConverter()
    {
        AddCanConvertFunction(typeof(bool), x => x.In(typeof(bool), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(char), x => x.In(typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(sbyte), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(byte), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(short), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(int), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(long), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(ushort), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(uint), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(ulong), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(float), x => x.In(typeof(bool), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(double), x => x.In(typeof(bool), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(decimal), x => x.In(typeof(bool), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(DateTime), x => x.In(typeof(DateTime), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(string), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(DateTime), typeof(string), typeof(object)));
        AddCanConvertFunction(typeof(Enum), x => x.In(typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(int), typeof(long), typeof(ushort), typeof(uint), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(string), typeof(object), typeof(Enum)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConvertibleObjectConverter"/> class.
    /// </summary>
    /// <param name="priority">The priority for this converter.</param>
    public ConvertibleObjectConverter(int priority = 0)
    {
        _priority = priority;
    }

    /// <summary>
    /// Registers a custom function which determines if a source type can be converted to a target type.
    /// </summary>
    /// <param name="sourceType">The source type.</param>
    /// <param name="canConvertFunc">The function that determines wether a convertion is possible.</param>
    public static void AddCanConvertFunction(Type sourceType, Func<Type, bool> canConvertFunc)
    {
        _ = Guard.NotNull(sourceType, nameof(sourceType));
        _ = Guard.NotNull(canConvertFunc, nameof(canConvertFunc));
        if (!CanConvertFunctions.TryGetValue(sourceType, out var functions))
        {
            functions = new List<Func<Type, bool>>();
            CanConvertFunctions.Add(sourceType, functions);
        }

        functions.Add(canConvertFunc);
    }

    /// <inheritdoc />
    public int GetPriority(Type? sourceType, Type targetType)
    {
        return _priority;
    }

    /// <inheritdoc />
    public bool CanConvert(Type? sourceType, Type targetType, IObjectConvertManager convertManager)
    {
        if (sourceType == null || !typeof(IConvertible).IsAssignableFrom(sourceType))
            return false;

        var canConvertMethod = sourceType.GetMethod("CanConvertTo", BindingFlags.Static | BindingFlags.Public, null, new[] { typeof(Type) }, null);
        bool? canConvert = null;
        if (canConvertMethod != null && canConvertMethod.ReturnType == typeof(bool))
            canConvert = (bool)canConvertMethod.Invoke(null, new object[] { targetType })!;

        if (CanConvertFunctions.TryGetValue(sourceType, out var canConvertFunctions))
            canConvert = canConvertFunctions.Any(x => x(targetType));

        return canConvert ?? true;
    }

    /// <inheritdoc />
    public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
    {
        if (obj == null)
            throw new InvalidCastException("The object to convert cannot be null.");
        return ((IConvertible)obj).ToType(targetType, formatProvider);
    }
}
