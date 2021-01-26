using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MaSch.Presentation.Observable.Collections;

namespace MaSch.Presentation.Extensions
{
    public static class EnumerableExtensions
    {
        public static ObservableDictionary<TKey, TValue> ToObservableDictionary<TEnum, TKey, TValue>(this IEnumerable<TEnum> enumerable, Func<TEnum, TKey> keySelector, Func<TEnum, TValue> valueSelector)
        {
            var result = new ObservableDictionary<TKey, TValue>();
            foreach (var item in enumerable)
            {
                result.Add(keySelector(item), valueSelector(item));
            }
            return result;
        }

        public static ObservableDictionary<TKey, TValue> ToObservableDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dict)
            => new ObservableDictionary<TKey, TValue>(dict);

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
            => new ObservableCollection<T>(enumerable);

        public static FullyObservableCollection<T> ToFullyObservableCollection<T>(this IEnumerable<T> enumerable) where T : INotifyPropertyChanged
            => new FullyObservableCollection<T>(enumerable);
    }
}
