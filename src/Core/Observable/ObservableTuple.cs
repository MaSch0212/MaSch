using MaSch.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable SA1402 // File may only contain a single type

namespace MaSch.Core.Observable
{
    /// <summary>
    /// Provides extension methods for the observable tuple classes.
    /// </summary>
    public static class ObservableTuple
    {
        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <returns>An <see cref="ObservableTuple{T1}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1> Create<T1>(T1 item1)
            => new(item1);

        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1,T2}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <param name="item2">The second value.</param>
        /// <returns>An <see cref="ObservableTuple{T1,T2}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
            => new(item1, item2);

        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1,T2,T3}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <param name="item2">The second value.</param>
        /// <param name="item3">The thrid value.</param>
        /// <returns>An <see cref="ObservableTuple{T1,T2,T3}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
            => new(item1, item2, item3);

        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1,T2,T3,T4}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <param name="item2">The second value.</param>
        /// <param name="item3">The thrid value.</param>
        /// <param name="item4">The fourth value.</param>
        /// <returns>An <see cref="ObservableTuple{T1,T2,T3,T4}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
            => new(item1, item2, item3, item4);

        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1,T2,T3,T4,T5}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <param name="item2">The second value.</param>
        /// <param name="item3">The thrid value.</param>
        /// <param name="item4">The fourth value.</param>
        /// <param name="item5">The fifth value.</param>
        /// <returns>An <see cref="ObservableTuple{T1,T2,T3,T4,T5}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
            => new(item1, item2, item3, item4, item5);

        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1,T2,T3,T4,T5,T6}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <param name="item2">The second value.</param>
        /// <param name="item3">The thrid value.</param>
        /// <param name="item4">The fourth value.</param>
        /// <param name="item5">The fifth value.</param>
        /// <param name="item6">The sixth value.</param>
        /// <returns>An <see cref="ObservableTuple{T1,T2,T3,T4,T5,T6}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
            => new(item1, item2, item3, item4, item5, item6);

        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1,T2,T3,T4,T5,T6,T7}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <param name="item2">The second value.</param>
        /// <param name="item3">The thrid value.</param>
        /// <param name="item4">The fourth value.</param>
        /// <param name="item5">The fifth value.</param>
        /// <param name="item6">The sixth value.</param>
        /// <param name="item7">The seventh value.</param>
        /// <returns>An <see cref="ObservableTuple{T1,T2,T3,T4,T5,T6,T7}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
            => new(item1, item2, item3, item4, item5, item6, item7);

        /// <summary>
        /// Creates a new <see cref="ObservableTuple{T1,T2,T3,T4,T5,T6,T7,TRest}"/> of the specified values.
        /// </summary>
        /// <typeparam name="T1">The type of the first value.</typeparam>
        /// <typeparam name="T2">The type of the second value.</typeparam>
        /// <typeparam name="T3">The type of the third value.</typeparam>
        /// <typeparam name="T4">The type of the fourth value.</typeparam>
        /// <typeparam name="T5">The type of the fifth value.</typeparam>
        /// <typeparam name="T6">The type of the sixth value.</typeparam>
        /// <typeparam name="T7">The type of the seventh value.</typeparam>
        /// <typeparam name="TRest">The type of the rest of the values.</typeparam>
        /// <param name="item1">The first value.</param>
        /// <param name="item2">The second value.</param>
        /// <param name="item3">The thrid value.</param>
        /// <param name="item4">The fourth value.</param>
        /// <param name="item5">The fifth value.</param>
        /// <param name="item6">The sixth value.</param>
        /// <param name="item7">The seventh value.</param>
        /// <param name="rest">The rest of the values.</param>
        /// <returns>An <see cref="ObservableTuple{T1,T2,T3,T4,T5,T6,T7,TRest}"/> that contains the specified values.</returns>
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, TRest> Create<T1, T2, T3, T4, T5, T6, T7, TRest>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
            where TRest : ObservableTupleBase
            => new(item1, item2, item3, item4, item5, item6, item7, rest);
    }

    /// <summary>
    /// Represents an observable tuple.
    /// </summary>
    /// <seealso cref="ObservableObject" />
    /// <seealso cref="IComparable" />
    /// <seealso cref="IComparable{T}" />
    public abstract class ObservableTupleBase : ObservableObject, IComparable, IComparable<ObservableTupleBase>
    {
        /// <summary>
        /// Gets an array of all items in the current <see cref="ObservableTupleBase"/>.
        /// </summary>
        /// <returns>All items in the current <see cref="ObservableTupleBase"/>.</returns>
        public object?[] GetItems()
            => GetType().GetProperties().OrderBy(x => x.Name).Select(x => x.GetValue(this)).ToArray();

        /// <inheritdoc/>
        public override bool Equals(object? obj)
            => obj is ObservableTupleBase other && other.GetItems().SequenceEqual(GetItems());

        /// <inheritdoc/>
        public override int GetHashCode()
            => CommonHelper.GetHashCode(GetItems());

        /// <inheritdoc/>
        public int CompareTo(object? obj)
            => obj is ObservableTupleBase other ? CompareTo(other) : 1;

        /// <inheritdoc/>
        public int CompareTo(ObservableTupleBase? other)
        {
            if (other is null)
                return Comparer<object>.Default.Compare(this, other);

            var myItems = GetItems();
            var otherItems = other.GetItems();

            for (int i = 0; i < Math.Min(myItems.Length, otherItems.Length); i++)
            {
                var x = myItems[i];
                var y = otherItems[i];
                int c;
                if (x == null || y == null)
                {
                    c = x == null && y == null
                        ? 0
                        : x == null ? -1 : 1;
                }
                else
                {
                    c = Comparer<object>.Default.Compare(x, y);
                }

                if (c != 0)
                    return c;
            }

            return 0;
        }

        /// <inheritdoc/>
        public override string ToString()
            => $"({ToStringInternal()})";

        /// <summary>
        /// Converts this object to a string.
        /// </summary>
        /// <returns>A string representation of this instance.</returns>
        internal virtual string ToStringInternal()
            => string.Join(", ", GetItems());

        public static bool operator ==(ObservableTupleBase a, ObservableTupleBase b)
            => a?.Equals(b) == true;

        public static bool operator !=(ObservableTupleBase a, ObservableTupleBase b)
            => a?.Equals(b) != true;

        public static bool operator <(ObservableTupleBase a, ObservableTupleBase b)
            => (a is null ? Comparer<object>.Default.Compare(a, b) : a.CompareTo(b)) < 0;

        public static bool operator <=(ObservableTupleBase a, ObservableTupleBase b)
            => (a is null ? Comparer<object>.Default.Compare(a, b) : a.CompareTo(b)) <= 0;

        public static bool operator >(ObservableTupleBase a, ObservableTupleBase b)
            => (a is null ? Comparer<object>.Default.Compare(a, b) : a.CompareTo(b)) > 0;

        public static bool operator >=(ObservableTupleBase a, ObservableTupleBase b)
            => (a is null ? Comparer<object>.Default.Compare(a, b) : a.CompareTo(b)) >= 0;
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 1 item.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    public class ObservableTuple<T1> : ObservableTupleBase
    {
        private T1? _item1;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        public ObservableTuple(T1? item1)
        {
            _item1 = item1;
        }
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 2 items.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    public class ObservableTuple<T1, T2> : ObservableTupleBase
    {
        private T1? _item1;
        private T2? _item2;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        /// <value>
        /// The second item.
        /// </value>
        public T2? Item2
        {
            get => _item2;
            set => SetProperty(ref _item2, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        public ObservableTuple(T1? item1, T2? item2)
        {
            _item1 = item1;
            _item2 = item2;
        }
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 3 items.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    /// <typeparam name="T3">The type of the third item.</typeparam>
    public class ObservableTuple<T1, T2, T3> : ObservableTupleBase
    {
        private T1? _item1;
        private T2? _item2;
        private T3? _item3;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        /// <value>
        /// The second item.
        /// </value>
        public T2? Item2
        {
            get => _item2;
            set => SetProperty(ref _item2, value);
        }

        /// <summary>
        /// Gets or sets the third item.
        /// </summary>
        /// <value>
        /// The third item.
        /// </value>
        public T3? Item3
        {
            get => _item3;
            set => SetProperty(ref _item3, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default, default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        public ObservableTuple(T1? item1, T2? item2, T3? item3)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
        }
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 4 items.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    /// <typeparam name="T3">The type of the third item.</typeparam>
    /// <typeparam name="T4">The type of the fourth item.</typeparam>
    public class ObservableTuple<T1, T2, T3, T4> : ObservableTupleBase
    {
        private T1? _item1;
        private T2? _item2;
        private T3? _item3;
        private T4? _item4;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        /// <value>
        /// The second item.
        /// </value>
        public T2? Item2
        {
            get => _item2;
            set => SetProperty(ref _item2, value);
        }

        /// <summary>
        /// Gets or sets the third item.
        /// </summary>
        /// <value>
        /// The third item.
        /// </value>
        public T3? Item3
        {
            get => _item3;
            set => SetProperty(ref _item3, value);
        }

        /// <summary>
        /// Gets or sets the fourth item.
        /// </summary>
        /// <value>
        /// The fourth item.
        /// </value>
        public T4? Item4
        {
            get => _item4;
            set => SetProperty(ref _item4, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default, default, default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        public ObservableTuple(T1? item1, T2? item2, T3? item3, T4? item4)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
        }
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 5 items.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    /// <typeparam name="T3">The type of the third item.</typeparam>
    /// <typeparam name="T4">The type of the fourth item.</typeparam>
    /// <typeparam name="T5">The type of the fifth item.</typeparam>
    public class ObservableTuple<T1, T2, T3, T4, T5> : ObservableTupleBase
    {
        private T1? _item1;
        private T2? _item2;
        private T3? _item3;
        private T4? _item4;
        private T5? _item5;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        /// <value>
        /// The second item.
        /// </value>
        public T2? Item2
        {
            get => _item2;
            set => SetProperty(ref _item2, value);
        }

        /// <summary>
        /// Gets or sets the third item.
        /// </summary>
        /// <value>
        /// The third item.
        /// </value>
        public T3? Item3
        {
            get => _item3;
            set => SetProperty(ref _item3, value);
        }

        /// <summary>
        /// Gets or sets the fourth item.
        /// </summary>
        /// <value>
        /// The fourth item.
        /// </value>
        public T4? Item4
        {
            get => _item4;
            set => SetProperty(ref _item4, value);
        }

        /// <summary>
        /// Gets or sets the fifth item.
        /// </summary>
        /// <value>
        /// The fifth item.
        /// </value>
        public T5? Item5
        {
            get => _item5;
            set => SetProperty(ref _item5, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default, default, default, default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// <param name="item5">The fifth item.</param>
        public ObservableTuple(T1? item1, T2? item2, T3? item3, T4? item4, T5? item5)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
        }
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 6 items.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    /// <typeparam name="T3">The type of the third item.</typeparam>
    /// <typeparam name="T4">The type of the fourth item.</typeparam>
    /// <typeparam name="T5">The type of the fifth item.</typeparam>
    /// <typeparam name="T6">The type of the sixth item.</typeparam>
    public class ObservableTuple<T1, T2, T3, T4, T5, T6> : ObservableTupleBase
    {
        private T1? _item1;
        private T2? _item2;
        private T3? _item3;
        private T4? _item4;
        private T5? _item5;
        private T6? _item6;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        /// <value>
        /// The second item.
        /// </value>
        public T2? Item2
        {
            get => _item2;
            set => SetProperty(ref _item2, value);
        }

        /// <summary>
        /// Gets or sets the third item.
        /// </summary>
        /// <value>
        /// The third item.
        /// </value>
        public T3? Item3
        {
            get => _item3;
            set => SetProperty(ref _item3, value);
        }

        /// <summary>
        /// Gets or sets the fourth item.
        /// </summary>
        /// <value>
        /// The fourth item.
        /// </value>
        public T4? Item4
        {
            get => _item4;
            set => SetProperty(ref _item4, value);
        }

        /// <summary>
        /// Gets or sets the fifth item.
        /// </summary>
        /// <value>
        /// The fifth item.
        /// </value>
        public T5? Item5
        {
            get => _item5;
            set => SetProperty(ref _item5, value);
        }

        /// <summary>
        /// Gets or sets the sixth item.
        /// </summary>
        /// <value>
        /// The sixth item.
        /// </value>
        public T6? Item6
        {
            get => _item6;
            set => SetProperty(ref _item6, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5, T6}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default, default, default, default, default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5, T6}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// <param name="item5">The fifth item.</param>
        /// <param name="item6">The sixth item.</param>
        public ObservableTuple(T1? item1, T2? item2, T3? item3, T4? item4, T5? item5, T6? item6)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
        }
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 7 items.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    /// <typeparam name="T3">The type of the third item.</typeparam>
    /// <typeparam name="T4">The type of the fourth item.</typeparam>
    /// <typeparam name="T5">The type of the fifth item.</typeparam>
    /// <typeparam name="T6">The type of the sixth item.</typeparam>
    /// <typeparam name="T7">The type of the seventh item.</typeparam>
    public class ObservableTuple<T1, T2, T3, T4, T5, T6, T7> : ObservableTupleBase
    {
        private T1? _item1;
        private T2? _item2;
        private T3? _item3;
        private T4? _item4;
        private T5? _item5;
        private T6? _item6;
        private T7? _item7;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        /// <value>
        /// The second item.
        /// </value>
        public T2? Item2
        {
            get => _item2;
            set => SetProperty(ref _item2, value);
        }

        /// <summary>
        /// Gets or sets the third item.
        /// </summary>
        /// <value>
        /// The third item.
        /// </value>
        public T3? Item3
        {
            get => _item3;
            set => SetProperty(ref _item3, value);
        }

        /// <summary>
        /// Gets or sets the fourth item.
        /// </summary>
        /// <value>
        /// The fourth item.
        /// </value>
        public T4? Item4
        {
            get => _item4;
            set => SetProperty(ref _item4, value);
        }

        /// <summary>
        /// Gets or sets the fifth item.
        /// </summary>
        /// <value>
        /// The fifth item.
        /// </value>
        public T5? Item5
        {
            get => _item5;
            set => SetProperty(ref _item5, value);
        }

        /// <summary>
        /// Gets or sets the sixth item.
        /// </summary>
        /// <value>
        /// The sixth item.
        /// </value>
        public T6? Item6
        {
            get => _item6;
            set => SetProperty(ref _item6, value);
        }

        /// <summary>
        /// Gets or sets the seventh item.
        /// </summary>
        /// <value>
        /// The seventh item.
        /// </value>
        public T7? Item7
        {
            get => _item7;
            set => SetProperty(ref _item7, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5, T6, T7}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default, default, default, default, default, default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5, T6, T7}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// <param name="item5">The fifth item.</param>
        /// <param name="item6">The sixth item.</param>
        /// <param name="item7">The seventh item.</param>
        public ObservableTuple(T1? item1, T2? item2, T3? item3, T4? item4, T5? item5, T6? item6, T7? item7)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
        }
    }

    /// <summary>
    /// Repesetns an <see cref="ObservableTupleBase"/> with 7 items and another <see cref="ObservableTupleBase"/> with more items.
    /// </summary>
    /// <typeparam name="T1">The type of the first item.</typeparam>
    /// <typeparam name="T2">The type of the second item.</typeparam>
    /// <typeparam name="T3">The type of the third item.</typeparam>
    /// <typeparam name="T4">The type of the fourth item.</typeparam>
    /// <typeparam name="T5">The type of the fifth item.</typeparam>
    /// <typeparam name="T6">The type of the sixth item.</typeparam>
    /// <typeparam name="T7">The type of the seventh item.</typeparam>
    /// <typeparam name="TRest">The type of the <see cref="ObservableTupleBase"/> that contains the rest of the values.</typeparam>
    public class ObservableTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : ObservableTupleBase
        where TRest : ObservableTupleBase
    {
        private T1? _item1;
        private T2? _item2;
        private T3? _item3;
        private T4? _item4;
        private T5? _item5;
        private T6? _item6;
        private T7? _item7;
        private TRest? _rest;

        /// <summary>
        /// Gets or sets the first item.
        /// </summary>
        /// <value>
        /// The first item.
        /// </value>
        public T1? Item1
        {
            get => _item1;
            set => SetProperty(ref _item1, value);
        }

        /// <summary>
        /// Gets or sets the second item.
        /// </summary>
        /// <value>
        /// The second item.
        /// </value>
        public T2? Item2
        {
            get => _item2;
            set => SetProperty(ref _item2, value);
        }

        /// <summary>
        /// Gets or sets the third item.
        /// </summary>
        /// <value>
        /// The third item.
        /// </value>
        public T3? Item3
        {
            get => _item3;
            set => SetProperty(ref _item3, value);
        }

        /// <summary>
        /// Gets or sets the fourth item.
        /// </summary>
        /// <value>
        /// The fourth item.
        /// </value>
        public T4? Item4
        {
            get => _item4;
            set => SetProperty(ref _item4, value);
        }

        /// <summary>
        /// Gets or sets the fifth item.
        /// </summary>
        /// <value>
        /// The fifth item.
        /// </value>
        public T5? Item5
        {
            get => _item5;
            set => SetProperty(ref _item5, value);
        }

        /// <summary>
        /// Gets or sets the sixth item.
        /// </summary>
        /// <value>
        /// The sixth item.
        /// </value>
        public T6? Item6
        {
            get => _item6;
            set => SetProperty(ref _item6, value);
        }

        /// <summary>
        /// Gets or sets the seventh item.
        /// </summary>
        /// <value>
        /// The seventh item.
        /// </value>
        public T7? Item7
        {
            get => _item7;
            set => SetProperty(ref _item7, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="ObservableTupleBase"/> that contains the rest of the values.
        /// </summary>
        /// <value>
        /// The <see cref="ObservableTupleBase"/> that contains the rest of the values.
        /// </value>
        public TRest? Rest
        {
            get => _rest;
            set => SetProperty(ref _rest, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> class.
        /// </summary>
        public ObservableTuple()
            : this(default, default, default, default, default, default, default, default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <param name="item3">The third item.</param>
        /// <param name="item4">The fourth item.</param>
        /// <param name="item5">The fifth item.</param>
        /// <param name="item6">The sixth item.</param>
        /// <param name="item7">The seventh item.</param>
        /// <param name="rest">The <see cref="ObservableTupleBase"/> that contains the rest of the values.</param>
        public ObservableTuple(T1? item1, T2? item2, T3? item3, T4? item4, T5? item5, T6? item6, T7? item7, TRest? rest)
        {
            _item1 = item1;
            _item2 = item2;
            _item3 = item3;
            _item4 = item4;
            _item5 = item5;
            _item6 = item6;
            _item7 = item7;
            _rest = rest;
        }

        /// <inheritdoc />
        internal override string ToStringInternal()
            => string.Join(", ", GetItems().Take(7)) + (Rest is not null ? ", " + Rest.ToStringInternal() : string.Empty);
    }
}
