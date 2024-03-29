﻿namespace MaSch.Core.Extensions;

/// <summary>
/// Provides extensions for <see cref="Array"/>s.
/// </summary>
public static class ArrayExtensions
{
    private static readonly char[] HexChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

    /// <summary>
    /// Converts the <see cref="Array"/> of <see cref="byte"/>s to a hexadecimal representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/>s to convert.</param>
    /// <returns>A <see cref="string"/> that contains the hexadecimal representation of the given <see cref="byte"/>s.</returns>
    public static string ToHexString(this byte[] bytes)
    {
        return new string(ToHexChars(bytes));
    }

    /// <summary>
    /// Converts the <see cref="byte"/> to a hexadecimal representation.
    /// </summary>
    /// <param name="byte">The <see cref="byte"/> to convert.</param>
    /// <returns>A <see cref="string"/> that contains the hexadecimal representation of the given <see cref="byte"/>.</returns>
    public static string ToHexString(this byte @byte)
    {
        return new string(ToHexChars(@byte));
    }

    /// <summary>
    /// Converts the <see cref="Array"/> of <see cref="byte"/>s to a hexadecimal representation.
    /// </summary>
    /// <param name="bytes">The <see cref="byte"/>s to convert.</param>
    /// <returns>An <see cref="Array"/> of <see cref="char"/>s that contains the hexadecimal representation of the given <see cref="byte"/>s.</returns>
    public static char[] ToHexChars(this byte[] bytes)
    {
        var result = new char[bytes.Length * 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            result[i * 2] = HexChars[bytes[i] >> 4];
            result[(i * 2) + 1] = HexChars[bytes[i] & 0xF];
        }

        return result;
    }

    /// <summary>
    /// Converts the <see cref="byte"/> to a hexadecimal representation.
    /// </summary>
    /// <param name="byte">The <see cref="byte"/> to convert.</param>
    /// <returns>An <see cref="Array"/> of <see cref="char"/>s that contains the hexadecimal representation of the given <see cref="byte"/>.</returns>
    public static char[] ToHexChars(this byte @byte)
    {
        return new[]
        {
            HexChars[@byte >> 4],
            HexChars[@byte & 0xF],
        };
    }
}
