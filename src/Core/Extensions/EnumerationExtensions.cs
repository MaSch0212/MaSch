using System.ComponentModel;

namespace MaSch.Core.Extensions;

/// <summary>
/// Contains extensions for <see cref="Enum"/>.
/// </summary>
public static class EnumerationExtensions
{
    /// <summary>
    /// Determines the value of the <see cref="DescriptionAttribute"/> of the <see cref="Enum"/> value.
    /// </summary>
    /// <param name="enumValue">The <see cref="Enum"/> value.</param>
    /// <returns>Return the Description of the <see cref="Enum"/> value if the attribute <see cref="DescriptionAttribute"/> is set; otherwise <see langword="null"/>.</returns>
    public static string? GetDescription(this Enum enumValue)
    {
        return Guard.NotNull(enumValue)
                       .GetType()
                       .GetField(enumValue.ToString())?
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description;
    }

    /// <summary>
    /// Combines two enum values of a specified enum type using the bitwise or operator.
    /// </summary>
    /// <typeparam name="T">The type of enum values to combine.</typeparam>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>Combined values of enum <typeparamref name="T"/> using the bitwise or operator.</returns>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "false positive.")]
    [SuppressMessage("Minor Code Smell", "S3247:Duplicate casts should not be made", Justification = "false positive.")]
    public static T BitwiseOr<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(this T a, T b)
        where T : struct, Enum, IConvertible
    {
        var valueField = typeof(T).GetField("value__", BindingFlags.Public | BindingFlags.Instance)
            ?? throw new ArgumentException("Could not get value of enum.", nameof(a));
        object result = valueField.GetValue(a) switch
        {
            sbyte aCasted => aCasted | (sbyte)valueField.GetValue(b)!,
            byte aCasted => aCasted | (byte)valueField.GetValue(b)!,
            short aCasted => aCasted | (short)valueField.GetValue(b)!,
            ushort aCasted => aCasted | (ushort)valueField.GetValue(b)!,
            int aCasted => aCasted | (int)valueField.GetValue(b)!,
            uint aCasted => aCasted | (uint)valueField.GetValue(b)!,
            long aCasted => aCasted | (long)valueField.GetValue(b)!,
            ulong aCasted => aCasted | (ulong)valueField.GetValue(b)!,
            nint aCasted => aCasted | (nint)valueField.GetValue(b)!,
            nuint aCasted => aCasted | (nuint)valueField.GetValue(b)!,
            _ => throw new InvalidOperationException($"Can not execute bitwise or on type \"{valueField.FieldType}\"."),
        };
        return (T)result;
    }

    /// <summary>
    /// Combines two enum values of a specified enum type using the bitwise and operator.
    /// </summary>
    /// <typeparam name="T">The type of enum values to combine.</typeparam>
    /// <param name="a">First value.</param>
    /// <param name="b">Second value.</param>
    /// <returns>Combined values of enum <typeparamref name="T"/> using the bitwise and operator.</returns>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "false positive.")]
    [SuppressMessage("Minor Code Smell", "S3247:Duplicate casts should not be made", Justification = "false positive.")]
    public static T BitwiseAnd<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(this T a, T b)
        where T : struct, Enum, IConvertible
    {
        var valueField = typeof(T).GetField("value__", BindingFlags.Public | BindingFlags.Instance)
            ?? throw new ArgumentException("Could not get value of enum.", nameof(a));
        object result = valueField.GetValue(a) switch
        {
            sbyte aCasted => aCasted & (sbyte)valueField.GetValue(b)!,
            byte aCasted => aCasted & (byte)valueField.GetValue(b)!,
            short aCasted => aCasted & (short)valueField.GetValue(b)!,
            ushort aCasted => aCasted & (ushort)valueField.GetValue(b)!,
            int aCasted => aCasted & (int)valueField.GetValue(b)!,
            uint aCasted => aCasted & (uint)valueField.GetValue(b)!,
            long aCasted => aCasted & (long)valueField.GetValue(b)!,
            ulong aCasted => aCasted & (ulong)valueField.GetValue(b)!,
            nint aCasted => aCasted & (nint)valueField.GetValue(b)!,
            nuint aCasted => aCasted & (nuint)valueField.GetValue(b)!,
            _ => throw new InvalidOperationException($"Can not execute bitwise or on type \"{valueField.FieldType}\"."),
        };
        return (T)result;
    }

    /// <summary>
    /// Builds a lambda that combines two enum values of a specified enum type using the bitwise or operator.
    /// </summary>
    /// <typeparam name="T">The type of enum values to combine.</typeparam>
    /// <returns>A lambda that combines two values of enum <typeparamref name="T"/> using the bitwise or operator.</returns>
    public static Func<T, T, T> BuildBitwiseOrLambda<T>()
        where T : struct, Enum
    {
        return BuildOperatorLambda<T>(Expression.Or);
    }

    /// <summary>
    /// Builds a lambda that combines two enum values of a specified enum type using the bitwise and operator.
    /// </summary>
    /// <typeparam name="T">The type of enum values to combine.</typeparam>
    /// <returns>A lambda that combines two values of enum <typeparamref name="T"/> using the bitwise and operator.</returns>
    public static Func<T, T, T> BuildBitwiseAndLambda<T>()
        where T : struct, Enum
    {
        return BuildOperatorLambda<T>(Expression.And);
    }

    private static Func<T, T, T> BuildOperatorLambda<T>(Func<Expression, Expression, Expression> operatorExpression)
        where T : struct, Enum
    {
        var enumType = Enum.GetUnderlyingType(typeof(T));

        var paramA = Expression.Parameter(typeof(T));
        var paramB = Expression.Parameter(typeof(T));

        var castA = Expression.Convert(paramA, enumType);
        var castB = Expression.Convert(paramB, enumType);
        var orExpr = operatorExpression(castA, castB);
        var castResult = Expression.Convert(orExpr, typeof(T));

        var lambda = Expression.Lambda(castResult, paramA, paramB);
        return (Func<T, T, T>)lambda.Compile();
    }
}
