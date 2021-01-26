using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using MaSch.Core;
using MaSch.Core.Extensions;

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

        public static IDisposableEnumerable<object[]> ToEnumerable(this IDataReader reader)
            => reader.ToEnumerable(r => r.GetValues());

        public static IDisposableEnumerable<T> ToEnumerable<T>(this IDataReader reader)
            => reader.ToEnumerable(r => r.Get<T>(0));

        public static IDisposableEnumerable<(T1, T2)> ToEnumerable<T1, T2>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1)));

        public static IDisposableEnumerable<(T1, T2, T3)> ToEnumerable<T1, T2, T3>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2)));

        public static IDisposableEnumerable<(T1, T2, T3, T4)> ToEnumerable<T1, T2, T3, T4>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3)));

        public static IDisposableEnumerable<(T1, T2, T3, T4, T5)> ToEnumerable<T1, T2, T3, T4, T5>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3), r.Get<T5>(4)));

        public static IDisposableEnumerable<(T1, T2, T3, T4, T5, T6)> ToEnumerable<T1, T2, T3, T4, T5, T6>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3), r.Get<T5>(4), r.Get<T6>(5)));

        public static IDisposableEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ToEnumerable<T1, T2, T3, T4, T5, T6, T7>(this IDataReader reader)
            => reader.ToEnumerable(r => (r.Get<T1>(0), r.Get<T2>(1), r.Get<T3>(2), r.Get<T4>(3), r.Get<T5>(4), r.Get<T6>(5), r.Get<T7>(6)));

        public static IDisposableEnumerable<T> ToEnumerable<T>(this IDataReader reader, Func<IDataReader, T> func) 
            => new DataReaderEnumerable<T>(reader, func);


        public static T Get<T>(this IDataRecord record, int columnId)
        {
            if (record.IsDBNull(columnId))
                return typeof(T).IsClass ? default(T) : throw new InvalidCastException("The DBNull value cannot be converted to " + typeof(T).FullName);

            if (GetFunctions.TryGetValue(typeof(T), out var func))
                return (T)func(record, columnId);
            else
                return (T)record.GetValue(columnId);
        }

        public static T Get<T>(this IDataRecord record, string columnName)
            => record.Get<T>(record.GetOrdinal(columnName));

        public static object Get(this IDataRecord record, int columnId) 
            => record.IsDBNull(columnId) ? default : record.GetValue(columnId);

        public static object Get(this IDataRecord record, string columnName)
            => record.Get(record.GetOrdinal(columnName));

        public static object[] GetValues(this IDataRecord record)
        {
            var result = new object[record.FieldCount];
            record.GetValues(result);
            return result;
        }

        
        private class DataReaderEnumerable<T> : IDisposableEnumerable<T>
        {
            public event EventHandler Disposing;
            public event EventHandler Disposed;

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
            object IEnumerator.Current => Current;

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
