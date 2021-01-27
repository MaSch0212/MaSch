using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Core.Collections
{
    /// <summary>
    /// Represents a <see cref="T:System.Collections.Generic.HashSet`1" /> that enables the ability to use a specific hashing function.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in this <see cref="T:MaSch.Core.Collections.ExtendedHashSet`2" />.</typeparam>
    /// <typeparam name="THash">The type of the hash values in this <see cref="T:MaSch.Core.Collections.ExtendedHashSet`2" />.</typeparam>
    public class ExtendedHashSet<TItem, THash> : ICollection<TItem>, IReadOnlyCollection<TItem>
        where THash : notnull
    {
        private readonly Dictionary<THash, TItem> _dict;
        private readonly Func<TItem, THash> _hashFunction;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ExtendedHashSet{TItem,THash}"></see>.
        /// </summary>
        public int Count => _dict.Count;

        /// <inheritdoc />
        bool ICollection<TItem>.IsReadOnly => false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedHashSet{TItem, THash}"/> class.
        /// </summary>
        /// <param name="hashFunction">The hash function.</param>
        public ExtendedHashSet(Func<TItem, THash> hashFunction)
            : this(null, hashFunction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedHashSet{TItem, THash}"/> class.
        /// </summary>
        /// <param name="list">The list of items to use for this hash set.</param>
        /// <param name="hashFunction">The hash function.</param>
        public ExtendedHashSet(IEnumerable<TItem>? list, Func<TItem, THash> hashFunction)
        {
            _hashFunction = hashFunction;
            _dict = list?.ToDictionary(GetItemHashCode, x => x) ?? new Dictionary<THash, TItem>();
        }

        /// <summary>
        /// Gets the items hash code.
        /// </summary>
        /// <param name="item">The item for which a hash code should be generated.</param>
        /// <returns>Returns the hash code for the given item.</returns>
        protected virtual THash GetItemHashCode(TItem item) => _hashFunction(item);

        /// <summary>
        /// Determines whether the <see cref="ExtendedHashSet{TItem,THash}"/> contains a value with a specific hash code.
        /// </summary>
        /// <param name="hashcode">The hash code.</param>
        /// <returns>
        ///   <c>true</c> if an item with the <paramref name="hashcode">hash code</paramref> is found in the <see cref="ExtendedHashSet{TItem,THash}"/>; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool ContainsHashCode(THash hashcode) => _dict.ContainsKey(hashcode);

        #region ICollection

        /// <summary>
        /// Adds an item to the <see cref="ExtendedHashSet{TItem, THash}"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ExtendedHashSet{TItem, THash}"></see>.</param>
        public virtual void Add(TItem item) => _dict.Add(GetItemHashCode(item), item);

        /// <summary>
        /// Removes all items from the <see cref="ExtendedHashSet{TItem, THash}"></see>.
        /// </summary>
        public virtual void Clear() => _dict.Clear();

        /// <summary>
        /// Determines whether the <see cref="ExtendedHashSet{TItem, THash}"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="ExtendedHashSet{TItem, THash}"></see>.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> is found in the <see cref="ExtendedHashSet{TItem, THash}"></see>; otherwise, false.
        /// </returns>
        public virtual bool Contains(TItem item) => _dict.ContainsKey(GetItemHashCode(item));

        /// <summary>
        /// Copies the elements of the <see cref="ExtendedHashSet{TItem, THash}"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="ExtendedHashSet{TItem, THash}"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public virtual void CopyTo(TItem[] array, int arrayIndex) => _dict.Values.CopyTo(array, arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ExtendedHashSet{TItem, THash}"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="ExtendedHashSet{TItem, THash}"></see>.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> was successfully removed from the <see cref="ExtendedHashSet{TItem, THash}"></see>; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="ExtendedHashSet{TItem, THash}"></see>.
        /// </returns>
        public virtual bool Remove(TItem item) => _dict.Remove(GetItemHashCode(item));

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TItem> GetEnumerator() => _dict.Values.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_dict.Values).GetEnumerator();

        /// <summary>
        /// Gets the <typeparamref name="TItem"/> with the specified hashcode.
        /// </summary>
        /// <value>
        /// The <typeparamref name="TItem"/>.
        /// </value>
        /// <param name="hashcode">The hashcode.</param>
        /// <returns>The <typeparamref name="TItem"/> with the specified hashcode.</returns>
        public virtual TItem this[THash hashcode] => _dict[hashcode];

        #endregion
    }
}
