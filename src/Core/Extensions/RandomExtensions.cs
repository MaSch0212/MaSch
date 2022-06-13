using System.Security.Cryptography;

namespace MaSch.Core.Extensions;

/// <summary>
/// Provides extensions for the <see cref="Random"/> and <see cref="RandomNumberGenerator"/> classes.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Returns a random <see cref="Array"/> of <see cref="byte"/>s.
    /// </summary>
    /// <param name="random">The object that is used to create random numbers.</param>
    /// <param name="byteCount">The number of bytes to randomize.</param>
    /// <returns>An <see cref="Array"/> of the specified number of random <see cref="byte"/>s.</returns>
    public static byte[] NextBytes(this Random random, int byteCount)
    {
        var bytes = new byte[Guard.NotOutOfRange(byteCount, 0, int.MaxValue)];
        random.NextBytes(bytes);
        return bytes;
    }

    /// <summary>
    /// Generates a random <see cref="Array"/> of <see cref="byte"/>s using a cryptographically strong random number generator.
    /// </summary>
    /// <param name="random">The object that is used to create random numbers.</param>
    /// <param name="byteCount">The number of bytes to randomize.</param>
    /// <returns>An <see cref="Array"/> of the specified number of random <see cref="byte"/>s.</returns>
    public static byte[] GetBytes(this RandomNumberGenerator random, int byteCount)
    {
        var bytes = new byte[Guard.NotOutOfRange(byteCount, 0, int.MaxValue)];
        random.GetBytes(bytes);
        return bytes;
    }

    /// <summary>
    /// Returns a random hexadecimal string.
    /// </summary>
    /// <param name="random">The object that is used to create random numbers.</param>
    /// <param name="byteCount">The number of bytes to randomize. The resulting string will be double that size.</param>
    /// <returns>A string with a length double the size of <paramref name="byteCount"/> that contains random hexadecimal numbers.</returns>
    public static string NextHexString(this Random random, int byteCount)
    {
        return NextBytes(random, byteCount).ToHexString();
    }

    /// <summary>
    /// Generates a random hexadecimal string using a cryptographically strong random number generator.
    /// </summary>
    /// <param name="random">The object that is used to create random numbers.</param>
    /// <param name="byteCount">The number of bytes to randomize. The resulting string will be double that size.</param>
    /// <returns>A string with a length double the size of <paramref name="byteCount"/> that contains random hexadecimal numbers.</returns>
    public static string GetHexString(this RandomNumberGenerator random, int byteCount)
    {
        return GetBytes(random, byteCount).ToHexString();
    }

    /// <summary>
    /// Returns an <see cref="Array"/> of random hexadecimal <see cref="char"/>s.
    /// </summary>
    /// <param name="random">The object that is used to create random numbers.</param>
    /// <param name="byteCount">The number of bytes to randomize. The resulting array will be double that size.</param>
    /// <returns>An <see cref="Array"/> of <see cref="char"/>s with a length double the size of <paramref name="byteCount"/> that contains random hexadecimal numbers.</returns>
    public static char[] NextHexChars(this Random random, int byteCount)
    {
        return NextBytes(random, byteCount).ToHexChars();
    }

    /// <summary>
    /// Generates an <see cref="Array"/> of random hexadecimal <see cref="char"/>s using a cryptographically strong random number generator.
    /// </summary>
    /// <param name="random">The object that is used to create random numbers.</param>
    /// <param name="byteCount">The number of bytes to randomize. The resulting array will be double that size.</param>
    /// <returns>An <see cref="Array"/> of <see cref="char"/>s with a length double the size of <paramref name="byteCount"/> that contains random hexadecimal numbers.</returns>
    public static char[] GetHexChars(this RandomNumberGenerator random, int byteCount)
    {
        return GetBytes(random, byteCount).ToHexChars();
    }

    /// <summary>
    /// Gets a random enumeration value for the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type from which to get random enum value.</typeparam>
    /// <param name="rng">The object that is used to create random numbers.</param>
    /// <returns>A random enum value of type <typeparamref name="T"/>.</returns>
    public static T NextEnum<T>(this Random rng)
        where T : struct, Enum
    {
        bool isFlagEnum = typeof(T).GetCustomAttribute<FlagsAttribute>() != null;
        return NextEnum<T>(rng, isFlagEnum);
    }

    /// <summary>
    /// Gets a random enumeration value for the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type from which to get random enum value.</typeparam>
    /// <param name="rng">The object that is used to create random numbers.</param>
    /// <param name="combineValues">Determines whether multiple enum values should be combined using bitwise or.</param>
    /// <returns>A random enum value of type <typeparamref name="T"/>.</returns>
    public static T NextEnum<T>(this Random rng, bool combineValues)
        where T : struct, Enum
    {
#if NET5_0_OR_GREATER
        var enumValues = Enum.GetValues<T>().Distinct().ToArray();
#else
        var enumValues = Enum.GetValues(typeof(T)).Cast<T>().Distinct().ToArray();
#endif

        if (combineValues)
        {
            return enumValues
                .OrderBy(x => Guid.NewGuid())
                .Take(rng.Next(1, enumValues.Length))
                .Aggregate((a, b) => a.BitwiseOr(b));
        }
        else
        {
            return enumValues[rng.Next(0, enumValues.Length)];
        }
    }
}
