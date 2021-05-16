﻿using MaSch.Core.Observable;
using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
#nullable disable

namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extensions for tuples.
    /// </summary>
    public static class TupleExtensions
    {
        public static ObservableTuple<T1> ToObservableTuple<T1>(this Tuple<T1> tuple)
            => new(tuple.Item1);
        public static ObservableTuple<T1, T2> ToObservableTuple<T1, T2>(this Tuple<T1, T2> tuple)
            => new(tuple.Item1, tuple.Item2);
        public static ObservableTuple<T1, T2, T3> ToObservableTuple<T1, T2, T3>(this Tuple<T1, T2, T3> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3);
        public static ObservableTuple<T1, T2, T3, T4> ToObservableTuple<T1, T2, T3, T4>(this Tuple<T1, T2, T3, T4> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        public static ObservableTuple<T1, T2, T3, T4, T5> ToObservableTuple<T1, T2, T3, T4, T5>(this Tuple<T1, T2, T3, T4, T5> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
        public static ObservableTuple<T1, T2, T3, T4, T5, T6> ToObservableTuple<T1, T2, T3, T4, T5, T6>(this Tuple<T1, T2, T3, T4, T5, T6> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7>(this Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7);
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18, T19>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18, T19, T20>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20, T21>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18, T19, T20, T21>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToObservableTuple());

#pragma warning disable SA1414 // Tuple types in signatures should have element names
        public static ObservableTuple<T1, T2> ToObservableTuple<T1, T2>(this (T1, T2) tuple)
            => new(tuple.Item1, tuple.Item2);
        public static ObservableTuple<T1, T2, T3> ToObservableTuple<T1, T2, T3>(this (T1, T2, T3) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3);
        public static ObservableTuple<T1, T2, T3, T4> ToObservableTuple<T1, T2, T3, T4>(this (T1, T2, T3, T4) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        public static ObservableTuple<T1, T2, T3, T4, T5> ToObservableTuple<T1, T2, T3, T4, T5>(this (T1, T2, T3, T4, T5) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
        public static ObservableTuple<T1, T2, T3, T4, T5, T6> ToObservableTuple<T1, T2, T3, T4, T5, T6>(this (T1, T2, T3, T4, T5, T6) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7>(this (T1, T2, T3, T4, T5, T6, T7) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7);
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8>(this (T1, T2, T3, T4, T5, T6, T7, T8) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8>(tuple.Item8));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9>(tuple.Item8, tuple.Item9));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10>(tuple.Item8, tuple.Item9, tuple.Item10));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15>>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14, new ObservableTuple<T15>(tuple.Item15)));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16>>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14, new ObservableTuple<T15, T16>(tuple.Item15, tuple.Item16)));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17>>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14, new ObservableTuple<T15, T16, T17>(tuple.Item15, tuple.Item16, tuple.Item17)));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18>>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14, new ObservableTuple<T15, T16, T17, T18>(tuple.Item15, tuple.Item16, tuple.Item17, tuple.Item18)));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19>>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14, new ObservableTuple<T15, T16, T17, T18, T19>(tuple.Item15, tuple.Item16, tuple.Item17, tuple.Item18, tuple.Item19)));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20>>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14, new ObservableTuple<T15, T16, T17, T18, T19, T20>(tuple.Item15, tuple.Item16, tuple.Item17, tuple.Item18, tuple.Item19, tuple.Item20)));
        public static ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20, T21>>> ToObservableTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21) tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, new ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20, T21>>(tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11, tuple.Item12, tuple.Item13, tuple.Item14, new ObservableTuple<T15, T16, T17, T18, T19, T20, T21>(tuple.Item15, tuple.Item16, tuple.Item17, tuple.Item18, tuple.Item19, tuple.Item20, tuple.Item21)));
#pragma warning restore SA1414 // Tuple types in signatures should have element names

        public static Tuple<T1> ToTuple<T1>(this ObservableTuple<T1> tuple)
            => new(tuple.Item1);
        public static Tuple<T1, T2> ToTuple<T1, T2>(this ObservableTuple<T1, T2> tuple)
            => new(tuple.Item1, tuple.Item2);
        public static Tuple<T1, T2, T3> ToTuple<T1, T2, T3>(this ObservableTuple<T1, T2, T3> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3);
        public static Tuple<T1, T2, T3, T4> ToTuple<T1, T2, T3, T4>(this ObservableTuple<T1, T2, T3, T4> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        public static Tuple<T1, T2, T3, T4, T5> ToTuple<T1, T2, T3, T4, T5>(this ObservableTuple<T1, T2, T3, T4, T5> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
        public static Tuple<T1, T2, T3, T4, T5, T6> ToTuple<T1, T2, T3, T4, T5, T6>(this ObservableTuple<T1, T2, T3, T4, T5, T6> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);
        public static Tuple<T1, T2, T3, T4, T5, T6, T7> ToTuple<T1, T2, T3, T4, T5, T6, T7>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7);
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18, T19>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18, T19, T20>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());
        public static Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16, T17, T18, T19, T20, T21>>> ToTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20, T21>>> tuple)
            => new(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.ToTuple());

#pragma warning disable SA1414 // Tuple types in signatures should have element names
        public static (T1, T2) ToValueTuple<T1, T2>(this ObservableTuple<T1, T2> tuple)
            => (tuple.Item1, tuple.Item2);
        public static (T1, T2, T3) ToValueTuple<T1, T2, T3>(this ObservableTuple<T1, T2, T3> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3);
        public static (T1, T2, T3, T4) ToValueTuple<T1, T2, T3, T4>(this ObservableTuple<T1, T2, T3, T4> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        public static (T1, T2, T3, T4, T5) ToValueTuple<T1, T2, T3, T4, T5>(this ObservableTuple<T1, T2, T3, T4, T5> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
        public static (T1, T2, T3, T4, T5, T6) ToValueTuple<T1, T2, T3, T4, T5, T6>(this ObservableTuple<T1, T2, T3, T4, T5, T6> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6);
        public static (T1, T2, T3, T4, T5, T6, T7) ToValueTuple<T1, T2, T3, T4, T5, T6, T7>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7);
        public static (T1, T2, T3, T4, T5, T6, T7, T8) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15>>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7, tuple.Rest.Rest.Item1);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16>>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7, tuple.Rest.Rest.Item1, tuple.Rest.Rest.Item2);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17>>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7, tuple.Rest.Rest.Item1, tuple.Rest.Rest.Item2, tuple.Rest.Rest.Item3);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18>>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7, tuple.Rest.Rest.Item1, tuple.Rest.Rest.Item2, tuple.Rest.Rest.Item3, tuple.Rest.Rest.Item4);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19>>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7, tuple.Rest.Rest.Item1, tuple.Rest.Rest.Item2, tuple.Rest.Rest.Item3, tuple.Rest.Rest.Item4, tuple.Rest.Rest.Item5);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20>>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7, tuple.Rest.Rest.Item1, tuple.Rest.Rest.Item2, tuple.Rest.Rest.Item3, tuple.Rest.Rest.Item4, tuple.Rest.Rest.Item5, tuple.Rest.Rest.Item6);
        public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21>(this ObservableTuple<T1, T2, T3, T4, T5, T6, T7, ObservableTuple<T8, T9, T10, T11, T12, T13, T14, ObservableTuple<T15, T16, T17, T18, T19, T20, T21>>> tuple)
            => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Rest.Item1, tuple.Rest.Item2, tuple.Rest.Item3, tuple.Rest.Item4, tuple.Rest.Item5, tuple.Rest.Item6, tuple.Rest.Item7, tuple.Rest.Rest.Item1, tuple.Rest.Rest.Item2, tuple.Rest.Rest.Item3, tuple.Rest.Rest.Item4, tuple.Rest.Rest.Item5, tuple.Rest.Rest.Item6, tuple.Rest.Rest.Item7);
#pragma warning restore SA1414 // Tuple types in signatures should have element names
    }
}
