namespace MaSch.Core.Extensions;

/// <summary>
/// Provides extensions methods for the <see cref="DateTime"/> class.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Converts the value of the current System.DateTime object to Coordinated Universal Time (UTC).
    /// </summary>
    /// <param name="dateTime">The value to convert.</param>
    /// <param name="kindToAssumeWhenUnspecified">The date time kind to assume when the kind of <paramref name="dateTime"/> is unspecified.</param>
    /// <returns>
    /// An object whose System.DateTime.Kind property is System.DateTimeKind.Utc, and
    /// whose value is the UTC equivalent to the value of the current System.DateTime
    /// object, or System.DateTime.MaxValue if the converted value is too large to be
    /// represented by a System.DateTime object, or System.DateTime.MinValue if the converted
    /// value is too small to be represented by a System.DateTime object.
    /// </returns>
    /// <exception cref="ArgumentException">The DateTime kind is unknown.</exception>
    public static DateTime ToUniversalTime(this DateTime dateTime, DateTimeKind kindToAssumeWhenUnspecified)
    {
        Guard.OneOf(kindToAssumeWhenUnspecified, new[] { DateTimeKind.Utc, DateTimeKind.Local });
        return dateTime.Kind switch
        {
            DateTimeKind.Unspecified => kindToAssumeWhenUnspecified == DateTimeKind.Utc
                ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime(),
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => throw new ArgumentException("The DateTime kind is unknown.", nameof(dateTime)),
        };
    }

    /// <summary>
    /// Converts the value of the current System.DateTime object to local time.
    /// </summary>
    /// <param name="dateTime">The value to convert.</param>
    /// <param name="kindToAssumeWhenUnspecified">The date time kind to assume when the kind of <paramref name="dateTime"/> is unspecified.</param>
    /// <returns>
    /// An object whose System.DateTime.Kind property is System.DateTimeKind.Local, and
    /// whose value is the local time equivalent to the value of the current System.DateTime
    /// object, or System.DateTime.MaxValue if the converted value is too large to be
    /// represented by a System.DateTime object, or System.DateTime.MinValue if the converted
    /// value is too small to be represented as a System.DateTime object.
    /// </returns>
    /// <exception cref="ArgumentException">The DateTime kind is unknown.</exception>
    public static DateTime ToLocalTime(this DateTime dateTime, DateTimeKind kindToAssumeWhenUnspecified)
    {
        Guard.OneOf(kindToAssumeWhenUnspecified, new[] { DateTimeKind.Utc, DateTimeKind.Local });
        return dateTime.Kind switch
        {
            DateTimeKind.Unspecified => kindToAssumeWhenUnspecified == DateTimeKind.Utc
                ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc).ToLocalTime()
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Local),
            DateTimeKind.Utc => dateTime.ToLocalTime(),
            DateTimeKind.Local => dateTime,
            _ => throw new ArgumentException("The DateTime kind is unknown.", nameof(dateTime)),
        };
    }
}
