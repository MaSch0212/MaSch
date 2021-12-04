namespace MaSch.Core;

/// <summary>
/// Provides methods to check for correctness of arguments.
/// </summary>
public static class Guard
{
    /// <summary>
    /// Verifies that the value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
    /// <returns>The same instance as <paramref name="value"/>.</returns>
    [return: NotNull]
    public static T NotNull<T>([NotNull] T value, string name)
    {
        if (value == null)
            throw new ArgumentNullException(name);
        return value;
    }

    /// <summary>
    /// Verifies that the value is not null or empty.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <exception cref="ArgumentException">The <paramref name="value"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
    /// <returns>The same instance as <paramref name="value"/>.</returns>
    public static string NotNullOrEmpty([NotNull] string? value, string name)
    {
        _ = NotNull(value, name);
        if (string.IsNullOrEmpty(value))
            throw new ArgumentException("The parameter value cannot be empty.", name);
        return value;
    }

    /// <summary>
    /// Verifies that the collection is not null or empty.
    /// </summary>
    /// <typeparam name="T">The type of collection to verify.</typeparam>
    /// <param name="collection">The collection.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <exception cref="ArgumentException">The <paramref name="collection"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">The <paramref name="collection"/> is null.</exception>
    /// <returns>The same instance as <paramref name="collection"/>.</returns>
    public static T NotNullOrEmpty<T>([NotNull] T? collection, string name)
        where T : ICollection
    {
        _ = NotNull(collection, name);
        if (collection.Count == 0)
            throw new ArgumentException("The parameter value cannot be empty.", name);
        return collection;
    }

    /// <summary>
    /// Verifies that a value is inside a specified range.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="min">The minimum accepted value.</param>
    /// <param name="max">The maximum accepted value.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is out of the specified range.</exception>
    /// <returns>The same instance as <paramref name="value"/>.</returns>
    public static T? NotOutOfRange<T>(T? value, string name, T min, T max)
        where T : IComparable
    {
        _ = NotNull(value, name);
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(name, $"The parameter value cannot be outside of the range <{min}> to <{max}>.");
        return value;
    }

    /// <summary>
    /// Verifies that a value is not greater than a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="max">The maximum accepted value.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is greater than <paramref name="max"/>.</exception>
    /// <returns>The same instance as <paramref name="value"/>.</returns>
    public static T? NotGreaterThan<T>(T? value, string name, T max)
        where T : IComparable
    {
        _ = NotNull(value, name);
        if (value.CompareTo(max) > 0)
            throw new ArgumentOutOfRangeException(name, $"The parameter value cannot be greater than <{max}>.");
        return value;
    }

    /// <summary>
    /// Verifies that a value is not smaller than a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="min">The minimum accepted value.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is smaller than <paramref name="min"/>.</exception>
    /// <returns>The same instance as <paramref name="value"/>.</returns>
    public static T? NotSmallerThan<T>(T? value, string name, T min)
        where T : IComparable
    {
        _ = NotNull(value, name);
        if (value.CompareTo(min) < 0)
            throw new ArgumentOutOfRangeException(name, $"The parameter value cannot be smaller than <{min}>.");
        return value;
    }

    /// <summary>
    /// Verifies that a value is of a specific type.
    /// </summary>
    /// <typeparam name="T">The type expected for the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="value"/> is not of type <typeparamref name="T"/>.</exception>
    /// <returns>Returns the <paramref name="value"/> cast to type <typeparamref name="T"/>.</returns>
    public static T OfType<T>(object? value, string name)
    {
        return OfType<T>(value, name, false)!;
    }

    /// <summary>
    /// Verifies that a value is of a specific type.
    /// </summary>
    /// <typeparam name="T">The type expected for the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="allowNull">Determines whether null values are allowed. This parameter is ignored if <typeparamref name="T"/> is a value type.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null and <paramref name="allowNull"/> is set to false.</exception>
    /// <exception cref="ArgumentException">The <paramref name="value"/> is not of type <typeparamref name="T"/>.</exception>
    /// <returns>Returns the <paramref name="value"/> cast to type <typeparamref name="T"/>.</returns>
    public static T? OfType<T>(object? value, string name, bool allowNull)
    {
        if (value is T result)
            return result;
        if (typeof(T).IsClass && value is null)
            return allowNull ? default : throw new ArgumentNullException(name);
        throw new ArgumentException($"The parameter value should be of type \"{typeof(T).FullName}\" but it is not. Actual type is \"{value?.GetType().FullName ?? "(null)"}\".", name);
    }

    /// <summary>
    /// Verifies that a values type is in a list of types.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="allowedTypes">A list of types in which the values type should be in.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException">The <paramref name="value"/> is not of one of the allowed types.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static object OfType(object? value, string name, params Type[] allowedTypes)
    {
        return OfType(value, name, false, allowedTypes)!;
    }

    /// <summary>
    /// Verifies that a values type is in a list of types.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="allowNull">Determines whether null values are allowed. This parameter is ignored if all allowed types are value types.</param>
    /// <param name="allowedTypes">A list of types in which the values type should be in.</param>
    /// <exception cref="ArgumentNullException">The <paramref name="value"/> is null and <paramref name="allowNull"/> is set to false.</exception>
    /// <exception cref="ArgumentException">The <paramref name="value"/> is not of one of the allowed types.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static object? OfType(object? value, string name, bool allowNull, params Type[] allowedTypes)
    {
        _ = NotNull(allowedTypes, nameof(allowedTypes));
        if (allowedTypes.Length == 0)
            throw new ArgumentException("At least one type needs to be provided.", nameof(allowedTypes));

        if (value is null)
        {
            if (allowedTypes.Any(x => x.IsClass) && allowNull)
                return null;
            else if (!allowNull)
                throw new ArgumentNullException(name);
        }
        else if (allowedTypes.Any(x => x.IsInstanceOfType(value)))
        {
            return value;
        }

        var typesStr = allowedTypes.Any() ? $"{Environment.NewLine}- {string.Join($"{Environment.NewLine}- ", allowedTypes.Select(x => x.FullName))}" : string.Empty;
        throw new ArgumentException($"The parameter value should be one of the following types but it is not: {typesStr}{Environment.NewLine}Actual type is \"{value?.GetType().FullName ?? "(null)"}\".", name);
    }

    /// <summary>
    /// Verifies that a value equals one of the specified values.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="allowedValues">The values of which <paramref name="value"/> needs to equal to at least one.</param>
    /// <exception cref="ArgumentException">The <paramref name="value"/> does not equal to any values from <paramref name="allowedValues"/>.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static T OneOf<T>(T value, string name, params T[] allowedValues)
    {
        return OneOf(value, name, EqualityComparer<T>.Default, allowedValues);
    }

    /// <summary>
    /// Verifies that a value equals one of the specified values.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> to use when comparing the values.</param>
    /// <param name="allowedValues">The allowed values.</param>
    /// <exception cref="ArgumentException">The <paramref name="value"/> does not equal to any values from <paramref name="allowedValues"/>.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static T OneOf<T>(T value, string name, IEqualityComparer<T> comparer, params T[] allowedValues)
    {
        if (!allowedValues.Contains(value, comparer))
            throw new ArgumentException($"The parameter value needs to equal one of the following values: {string.Join(", ", allowedValues)}", name);
        return value;
    }

    /// <summary>
    /// Verifies that a value does not equal any of the specified values.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="disallowedValues">The disallowed values.</param>
    /// <exception cref="ArgumentException">The <paramref name="value"/> equals to at least one value from <paramref name="disallowedValues"/>.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static T NotOneOf<T>(T value, string name, params T[] disallowedValues)
    {
        return NotOneOf(value, name, EqualityComparer<T>.Default, disallowedValues);
    }

    /// <summary>
    /// Verifies that a value does not equal any of the specified values.
    /// </summary>
    /// <typeparam name="T">The type of the value to verify.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> to use when comparing the values.</param>
    /// <param name="disallowedValues">The disallowed values.</param>
    /// <exception cref="ArgumentException">The <paramref name="value"/> equals to at least one value from <paramref name="disallowedValues"/>.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static T NotOneOf<T>(T value, string name, IEqualityComparer<T> comparer, params T[] disallowedValues)
    {
        if (disallowedValues.Contains(value, comparer))
            throw new ArgumentException($"The parameter value must not equal one of the following values: {string.Join(", ", disallowedValues)}", name);
        return value;
    }

    /// <summary>
    /// Verifies that an enum value is defined in the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type in which the <paramref name="value"/> needs to be defined.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <exception cref="ArgumentException">The <paramref name="value"/> is not defined in <typeparamref name="T"/>.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static T NotUndefinedEnumMember<T>(T value, string name)
        where T : Enum
    {
        _ = NotNull(value, name);
        if (!Enum.IsDefined(typeof(T), value))
            throw new ArgumentException($"The value \"{value}\" is not defined in the enum \"{typeof(T).Name}\".", name);
        return value;
    }

    /// <summary>
    /// Verifies that an enum value consist only of enum values defined in the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type in which the flags of <paramref name="value"/> needs to be defined.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="name">The name of the parameter to verify.</param>
    /// <exception cref="ArgumentException">At least one flag in <paramref name="value"/> is not defined in <typeparamref name="T"/>.</exception>
    /// <returns>Returns the <paramref name="value"/>.</returns>
    public static T NotUndefinedFlagInEnumValue<T>(T value, string name)
        where T : Enum
    {
        _ = NotNull(value, name);

        if (Equals(value, default(T)) && !Enum.IsDefined(typeof(T), default(T)!))
            throw new ArgumentException($"The value \"{value}\" is not defined in the enum \"{typeof(T).Name}\".", name);

        var xor = CompileEnumOperatorFunc<T>(Expression.ExclusiveOr);
        var and = CompileEnumOperatorFunc<T>(Expression.And);

        var result = Enum.GetValues(typeof(T)).Cast<T>().Aggregate(value, (a, x) => and(xor(a, x), a));
        if (!Equals(result, default(T)))
            throw new ArgumentException($"At least one flag in \"{value}\" is not defined in the enum \"{typeof(T).Name}\".", name);
        return value;
    }

    private static Func<T, T, T> CompileEnumOperatorFunc<T>(Func<Expression, Expression, BinaryExpression> operatorExpressionFactory)
        where T : Enum
    {
        var enumType = Enum.GetUnderlyingType(typeof(T));

        var param1 = Expression.Parameter(typeof(T), "a");
        var param2 = Expression.Parameter(typeof(T), "b");

        var param1Cast = Expression.Convert(param1, enumType);
        var param2Cast = Expression.Convert(param2, enumType);

        var @operator = operatorExpressionFactory(param1Cast, param2Cast);
        var result = Expression.Convert(@operator, typeof(T));

        var lambda = Expression.Lambda<Func<T, T, T>>(result, param1, param2);
        return lambda.Compile();
    }
}
