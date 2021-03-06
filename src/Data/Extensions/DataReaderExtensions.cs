using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MaSch.Core;

namespace MaSch.Data.Extensions
{
    /// <summary>
    /// Contains extensions for <see cref="IDataReader"/> and <see cref="IDataRecord"/>.
    /// </summary>
    public static class DataReaderExtensions
    {
        private static readonly Dictionary<Type, Func<IDataRecord, int, object>> GetFunctions = new Dictionary<Type, Func<IDataRecord, int, object>>
        {
            [typeof(bool)] = (r, i) => r.GetBoolean(i),
            [typeof(byte)] = (r, i) => r.GetByte(i),
            [typeof(char)] = (r, i) => r.GetChar(i),
            [typeof(DateTime)] = (r, i) => r.GetDateTime(i),
            [typeof(decimal)] = (r, i) => r.GetDecimal(i),
            [typeof(double)] = (r, i) => r.GetDouble(i),
            [typeof(float)] = (r, i) => r.GetFloat(i),
            [typeof(Guid)] = (r, i) => r.GetGuid(i),
            [typeof(short)] = (r, i) => r.GetInt16(i),
            [typeof(int)] = (r, i) => r.GetInt32(i),
            [typeof(long)] = (r, i) => r.GetInt64(i),
            [typeof(string)] = (r, i) => r.GetString(i),
        };

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<object[]> ToEnumerable(this IDataReader reader)
            => reader.ToEnumerable(r => r.GetValues());

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the first column.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<T?> ToEnumerable<T>(this IDataReader reader)
            => reader.ToEnumerable(r => r.Get<T>(0));

#pragma warning disable SA1414 // Tuple types in signatures should have element names
        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T1">The type of the first column.</typeparam>
        /// <typeparam name="T2">The type of the second column.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<(T1?, T2?)> ToEnumerable<T1, T2>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1)));

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T1">The type of the first column.</typeparam>
        /// <typeparam name="T2">The type of the second column.</typeparam>
        /// <typeparam name="T3">The type of the third column.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<(T1?, T2?, T3?)> ToEnumerable<T1, T2, T3>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2)));

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T1">The type of the first column.</typeparam>
        /// <typeparam name="T2">The type of the second column.</typeparam>
        /// <typeparam name="T3">The type of the third column.</typeparam>
        /// <typeparam name="T4">The type of the fourth column.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<(T1?, T2?, T3?, T4?)> ToEnumerable<T1, T2, T3, T4>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3)));

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T1">The type of the first column.</typeparam>
        /// <typeparam name="T2">The type of the second column.</typeparam>
        /// <typeparam name="T3">The type of the third column.</typeparam>
        /// <typeparam name="T4">The type of the fourth column.</typeparam>
        /// <typeparam name="T5">The type of the fifth column.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<(T1?, T2?, T3?, T4?, T5?)> ToEnumerable<T1, T2, T3, T4, T5>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3), r.Get<T5>(4)));

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T1">The type of the first column.</typeparam>
        /// <typeparam name="T2">The type of the second column.</typeparam>
        /// <typeparam name="T3">The type of the third column.</typeparam>
        /// <typeparam name="T4">The type of the fourth column.</typeparam>
        /// <typeparam name="T5">The type of the fifth column.</typeparam>
        /// <typeparam name="T6">The type of the sixth column.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<(T1?, T2?, T3?, T4?, T5?, T6?)> ToEnumerable<T1, T2, T3, T4, T5, T6>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3), r.Get<T5>(4), r.Get<T6>(5)));

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T1">The type of the first column.</typeparam>
        /// <typeparam name="T2">The type of the second column.</typeparam>
        /// <typeparam name="T3">The type of the third column.</typeparam>
        /// <typeparam name="T4">The type of the fourth column.</typeparam>
        /// <typeparam name="T5">The type of the fifth column.</typeparam>
        /// <typeparam name="T6">The type of the sixth column.</typeparam>
        /// <typeparam name="T7">The type of the seventh column.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<(T1?, T2?, T3?, T4?, T5?, T6?, T7?)> ToEnumerable<T1, T2, T3, T4, T5, T6, T7>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3), r.Get<T5>(4), r.Get<T6>(5), r.Get<T7>(6)));
#pragma warning restore SA1414 // Tuple types in signatures should have element names

        /// <summary>
        /// Converts this <see cref="IDataReader"/> to an enumerable.
        /// </summary>
        /// <typeparam name="T">The target type for each row.</typeparam>
        /// <param name="reader">The reader to convert.</param>
        /// <param name="func">The function that is used to create an instance of type <typeparamref name="T"/> from each row of this <see cref="IDataReader"/>.</param>
        /// <returns>
        /// Returns a disposable enumerable that enumerates through all rows of this <see cref="IDataReader"/>.
        /// When the enumerable is disposed, the <see cref="IDataReader"/> is disposed as well.
        /// </returns>
        public static IDisposableEnumerable<T> ToEnumerable<T>(this IDataReader reader, Func<IDataReader, T> func)
            => new DataReaderEnumerable<T>(reader, func);

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <typeparam name="T">The type of the column.</typeparam>
        /// <param name="record">The data row.</param>
        /// <param name="columnId">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="InvalidCastException">The DBNull value cannot be converted.</exception>
        public static T? Get<T>(this IDataRecord record, int columnId)
        {
            if (record.IsDBNull(columnId))
                return typeof(T).IsClass ? default(T) : throw new InvalidCastException($"The DBNull value cannot be converted to \"{typeof(T).FullName}\".");

            if (GetFunctions.TryGetValue(typeof(T), out var func))
                return (T)func(record, columnId);
            else
                return (T)record.GetValue(columnId);
        }

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <typeparam name="T">The type of the column.</typeparam>
        /// <param name="record">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="InvalidCastException">The DBNull value cannot be converted.</exception>
        public static T? Get<T>(this IDataRecord record, string columnName)
            => record.Get<T>(record.GetOrdinal(columnName));

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="record">The data row.</param>
        /// <param name="columnId">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        public static object? Get(this IDataRecord record, int columnId)
            => record.IsDBNull(columnId) ? default : record.GetValue(columnId);

        /// <summary>
        /// Gets the value of the specified column.
        /// </summary>
        /// <param name="record">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>The value of the column.</returns>
        public static object? Get(this IDataRecord record, string columnName)
            => record.Get(record.GetOrdinal(columnName));

        /// <summary>
        /// Gets the values of all columns.
        /// </summary>
        /// <param name="record">The data row.</param>
        /// <returns>The values of all columns.</returns>
        public static object[] GetValues(this IDataRecord record)
        {
            var result = new object[record.FieldCount];
            record.GetValues(result);
            return result;
        }

        private class DataReaderEnumerable<T> : IDisposableEnumerable<T>
        {
            public event EventHandler? Disposing;
            public event EventHandler? Disposed;

            private readonly DataReaderEnumerator<T> _enumerator;

            public DataReaderEnumerable(IDataReader reader, Func<IDataReader, T> itemFunction)
            {
                _enumerator = new DataReaderEnumerator<T>(reader, itemFunction);
            }

            public IEnumerator<T> GetEnumerator() => _enumerator;
            IEnumerator IEnumerable.GetEnumerator() => _enumerator;

            public void Dispose()
            {
                Disposing?.Invoke(this, new EventArgs());
                _enumerator.Dispose();
                Disposed?.Invoke(this, new EventArgs());
            }
        }

        private class DataReaderEnumerator<T> : IEnumerator<T>
        {
            private readonly Func<IDataReader, T> _itemFunction;
            private readonly IDataReader _reader;

            public T Current => _itemFunction(_reader);
            object? IEnumerator.Current => Current;

            public DataReaderEnumerator(IDataReader reader, Func<IDataReader, T> itemFunction)
            {
                _itemFunction = itemFunction;
                _reader = reader;
            }

            public bool MoveNext()
            {
                if (_reader.IsClosed)
                    return false;
                var result = _reader.Read();
                return result;
            }

            public void Reset() => throw new NotSupportedException("This enumerable can only be iterated once.");
            public void Dispose() => _reader.Dispose();
        }
    }
}
