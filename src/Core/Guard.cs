using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MaSch.Core
{
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
        /// <exception cref="ArgumentNullException">The value is null.</exception>
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
        /// <exception cref="ArgumentException">The value is null or empty.</exception>
        /// <returns>The same instance as <paramref name="value"/>.</returns>
        public static string NotNullOrEmpty([NotNull] string? value, string name)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("The parameter value cannot be empty.", name);
            return value;
        }

        /// <summary>
        /// Verifies that a value is inside a specified range.
        /// </summary>
        /// <typeparam name="T">The type of the value to verify.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="name">The name of the parameter to verify.</param>
        /// <param name="min">The minimum accepted value.</param>
        /// <param name="max">The maximum accepted value.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="value"/> is out of the specified range.</exception>
        /// <returns>The same instance as <paramref name="value"/>.</returns>
        public static T? NotOutOfRange<T>(T value, string name, T min, T max)
            where T : IComparable
        {
            if (value != null && (value.CompareTo(min) < 0 || value.CompareTo(max) > 0))
                throw new ArgumentOutOfRangeException(name, $"The parameter value cannot be outside of the range <{min}> to <{max}>.");
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
        public static T OfType<T>(object? value, string name) => OfType<T>(value, name, false)!;

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
                return allowNull ? default(T) : throw new ArgumentNullException(name);
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
        public static object OfType(object? value, string name, params Type[] allowedTypes) => OfType(value, name, false, allowedTypes)!;

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
            if (value is null)
            {
                if (allowedTypes.Any(x => x.IsClass) && allowNull)
                    return null;
            }
            else if (allowedTypes.Any(x => x.IsInstanceOfType(value)))
            {
                return value;
            }

            var typesStr = allowedTypes.Any() ? $"{Environment.NewLine}- {string.Join($"{Environment.NewLine}- ", allowedTypes.Select(x => x.FullName))}" : string.Empty;
            throw new ArgumentException($"The parameter value should be one of the following types but it is not: {typesStr}{Environment.NewLine}Actual type is \"{value?.GetType().FullName ?? "(null)"}\".", name);
        }
    }
}
