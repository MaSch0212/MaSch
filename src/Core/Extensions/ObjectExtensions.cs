using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="object"/>.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets or sets the convert manager that is used by the extension methods defined in <see cref="ObjectExtensions"/>.
        /// </summary>
        public static IObjectConvertManager ConvertManager { get; set; } = new DefaultObjectConvertManager();

        /// <summary>
        /// Checks if this object is contained in a specified <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="list"/>.</typeparam>
        /// <param name="obj">The object to check if it contained in <paramref name="list"/>..</param>
        /// <param name="list">The <see cref="IEnumerable{T}"/> in which to check for the existance of <paramref name="obj"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is contained in <paramref name="list"/>; otherwise <see langword="false"/>.</returns>
        public static bool In<T>(this T obj, IEnumerable<T> list)
        {
            Guard.NotNull(list, nameof(list));
            return list.Contains(obj);
        }

        /// <summary>
        /// Checks if this object is contained in a specified array.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="list"/>.</typeparam>
        /// <param name="obj">The object to check if it contained in <paramref name="list"/>..</param>
        /// <param name="list">The array in which to check for the existance of <paramref name="obj"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is contained in <paramref name="list"/>; otherwise <see langword="false"/>.</returns>
        public static bool In<T>(this T obj, params T[] list)
        {
            return In(obj, (IEnumerable<T>)list);
        }

        /// <summary>
        /// Checks if this object is contained in a specified <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="list"/>.</typeparam>
        /// <param name="obj">The object to check if it contained in <paramref name="list"/>..</param>
        /// <param name="list">The <see cref="IEnumerable{T}"/> in which to check for the existance of <paramref name="obj"/>.</param>
        /// <param name="equalityComparer">An <see cref="IEqualityComparer{T}" /> to compare values.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is contained in <paramref name="list"/>; otherwise <see langword="false"/>.</returns>
        public static bool In<T>(this T obj, IEnumerable<T> list, IEqualityComparer<T> equalityComparer)
        {
            Guard.NotNull(list, nameof(list));
            Guard.NotNull(equalityComparer, nameof(equalityComparer));
            return list.Contains(obj, equalityComparer);
        }

        /// <summary>
        /// Converts this object to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The type of the object to convert.</typeparam>
        /// <typeparam name="TTarget">The type to which <paramref name="source"/> should be converted to.</typeparam>
        /// <param name="source">The object to convert to <typeparamref name="TTarget"/>.</param>
        /// <returns>The converted <paramref name="source"/> to <typeparamref name="TTarget"/>.</returns>
        public static TTarget? ConvertTo<TSource, TTarget>(this TSource source)
            => ConvertManager.Convert<TSource, TTarget>(source);

        /// <summary>
        /// Converts this object to the specified type.
        /// </summary>
        /// <typeparam name="TSource">The type of the object to convert.</typeparam>
        /// <typeparam name="TTarget">The type to which <paramref name="source"/> should be converted to.</typeparam>
        /// <param name="source">The object to convert to <typeparamref name="TTarget"/>.</param>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> to use for conversion.</param>
        /// <returns>The converted <paramref name="source"/> to <typeparamref name="TTarget"/>.</returns>
        public static TTarget? ConvertTo<TSource, TTarget>(this TSource source, IFormatProvider formatProvider)
            => ConvertManager.Convert<TSource, TTarget>(source, Guard.NotNull(formatProvider, nameof(formatProvider)));

        /// <summary>
        /// Converts this object to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which <paramref name="obj"/> should be converted to.</typeparam>
        /// <param name="obj">The object to convert to <typeparamref name="T"/>.</param>
        /// <returns>The converted <paramref name="obj"/> to <typeparamref name="T"/>.</returns>
        public static T? ConvertTo<T>(this object? obj)
            => ConvertManager.Convert<T>(obj);

        /// <summary>
        /// Converts this object to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which <paramref name="obj"/> should be converted to.</typeparam>
        /// <param name="obj">The object to convert to <typeparamref name="T"/>.</param>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> to use for conversion.</param>
        /// <returns>The converted <paramref name="obj"/> to <typeparamref name="T"/>.</returns>
        public static T? ConvertTo<T>(this object? obj, IFormatProvider formatProvider)
            => ConvertManager.Convert<T>(obj, Guard.NotNull(formatProvider, nameof(formatProvider)));

        /// <summary>
        /// Converts this object to the specified type.
        /// </summary>
        /// <param name="obj">The object to convert to <paramref name="targetType"/>.</param>
        /// <param name="targetType">The type to which <paramref name="obj"/> should be converted to.</param>
        /// <returns>The converted <paramref name="obj"/> to <paramref name="targetType"/>.</returns>
        public static object? ConvertTo(this object? obj, Type targetType)
            => ConvertManager.Convert(obj, Guard.NotNull(targetType, nameof(targetType)));

        /// <summary>
        /// Converts this object to the specified type.
        /// </summary>
        /// <param name="obj">The object to convert to <paramref name="targetType"/>.</param>
        /// <param name="targetType">The type to which <paramref name="obj"/> should be converted to.</param>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> to use for conversion.</param>
        /// <returns>The converted <paramref name="obj"/> to <paramref name="targetType"/>.</returns>
        public static object? ConvertTo(this object? obj, Type targetType, IFormatProvider formatProvider)
            => ConvertManager.Convert(obj, Guard.NotNull(targetType, nameof(targetType)), Guard.NotNull(formatProvider, nameof(formatProvider)));

        /// <summary>
        /// Casts this object to the specified type.
        /// </summary>
        /// <param name="obj">The object to convert to <paramref name="targetType"/>.</param>
        /// <param name="targetType">The type to which <paramref name="obj"/> should be converted to.</param>
        /// <returns>The converted <paramref name="obj"/> to <paramref name="targetType"/>.</returns>
        public static object? CastTo(this object? obj, Type targetType)
        {
            var method = typeof(ObjectExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(x => x.Name == nameof(CastTo) && x.IsGenericMethod && x.GetGenericArguments().Length == 1);
            if (method == null)
                throw new InvalidOperationException("The generic method \"Cast\" was not found.");
            return method.MakeGenericMethod(targetType).Invoke(null, new[] { obj });
        }

        /// <summary>
        /// Casts this object to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which <paramref name="obj"/> should be converted to.</typeparam>
        /// <param name="obj">The object to convert to <typeparamref name="T"/>.</param>
        /// <returns>The converted <paramref name="obj"/> to <typeparamref name="T"/>.</returns>
        public static T? CastTo<T>(this object? obj)
        {
            return (T?)obj;
        }

        /// <summary>
        /// Creates a Switch statement from the specified <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <paramref name="source"/>.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>The Switch statement which is based on <paramref name="source"/> and has <typeparamref name="TTarget"/> as the result type.</returns>
        [Obsolete("Use the switch statement from C# 8 instead.")]
        public static Switch<TSource, TTarget> Switch<TSource, TTarget>(this TSource source)
            where TSource : notnull
            => new(source);

        /// <summary>
        /// Tries to clone an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to clone.</typeparam>
        /// <param name="obj">The object to clone.</param>
        /// <returns>A clone of <paramref name="obj"/> if cloning was possible; otherwise <paramref name="obj"/>.</returns>
        [return: MaybeNull]
        public static T CloneIfPossible<T>(this T obj)
            where T : class?
            => TryClone(obj, out var clone) ? clone : obj;

        /// <summary>
        /// Tries to clone an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to clone.</typeparam>
        /// <param name="obj">The object to clone.</param>
        /// <param name="clone">If cloning was possible, containes the clone; otherwise <see langword="default"/>.</param>
        /// <returns><see langword="true"/> if cloning was possible; otherwise <see langword="false"/>.</returns>
        public static bool TryClone<T>(this T obj, out T? clone)
            where T : class?
        {
            if (obj is ICloneable clonable)
            {
                clone = (T)clonable.Clone();
                return true;
            }

            var cloneMethod = obj?.GetType().GetMethod("Clone", BindingFlags.Public | BindingFlags.Instance);
            if (cloneMethod != null)
            {
                clone = cloneMethod.Invoke(obj, null) is T t ? t : default;
                return true;
            }

            clone = default;
            return false;
        }

        /// <summary>
        /// Returns a string that represents the current object formatted using the invariant culture.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>A string that represents the current object formatted using the invariant culture.</returns>
        public static string ToInvariantString(this object obj)
        {
            Guard.NotNull(obj, nameof(obj));
            return FormattableString.Invariant($"{obj}");
        }

        /// <summary>
        /// Gets the initial hash code of the given object (reference hash).
        /// </summary>
        /// <param name="obj">The object to get the hash from.</param>
        /// <returns>The initial hash code of <paramref name="obj"/> (reference hash).</returns>
        public static int GetInitialHashCode(this object obj)
            => RuntimeHelpers.GetHashCode(obj);
    }
}
