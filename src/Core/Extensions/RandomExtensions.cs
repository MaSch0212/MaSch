using System;
using System.Security.Cryptography;

namespace MaSch.Core.Extensions
{
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
            var bytes = new byte[Guard.NotOutOfRange(byteCount, nameof(byteCount), 0, int.MaxValue)];
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
            var bytes = new byte[Guard.NotOutOfRange(byteCount, nameof(byteCount), 0, int.MaxValue)];
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
            => NextBytes(random, byteCount).ToHexString();

        /// <summary>
        /// Generates a random hexadecimal string using a cryptographically strong random number generator.
        /// </summary>
        /// <param name="random">The object that is used to create random numbers.</param>
        /// <param name="byteCount">The number of bytes to randomize. The resulting string will be double that size.</param>
        /// <returns>A string with a length double the size of <paramref name="byteCount"/> that contains random hexadecimal numbers.</returns>
        public static string GetHexString(this RandomNumberGenerator random, int byteCount)
            => GetBytes(random, byteCount).ToHexString();

        /// <summary>
        /// Returns an <see cref="Array"/> of random hexadecimal <see cref="char"/>s.
        /// </summary>
        /// <param name="random">The object that is used to create random numbers.</param>
        /// <param name="byteCount">The number of bytes to randomize. The resulting array will be double that size.</param>
        /// <returns>An <see cref="Array"/> of <see cref="char"/>s with a length double the size of <paramref name="byteCount"/> that contains random hexadecimal numbers.</returns>
        public static char[] NextHexChars(this Random random, int byteCount)
            => NextBytes(random, byteCount).ToHexChars();

        /// <summary>
        /// Generates an <see cref="Array"/> of random hexadecimal <see cref="char"/>s using a cryptographically strong random number generator.
        /// </summary>
        /// <param name="random">The object that is used to create random numbers.</param>
        /// <param name="byteCount">The number of bytes to randomize. The resulting array will be double that size.</param>
        /// <returns>An <see cref="Array"/> of <see cref="char"/>s with a length double the size of <paramref name="byteCount"/> that contains random hexadecimal numbers.</returns>
        public static char[] GetHexChars(this RandomNumberGenerator random, int byteCount)
            => GetBytes(random, byteCount).ToHexChars();
    }
}
