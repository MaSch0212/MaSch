using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Core.Lazy
{
    public class AdvancedLazy<T1>
    {
        private readonly Func<T1> _i1Factory;

        private bool _b1;
        private T1? _i1;

        protected bool UseCaching { get; }

        public T1 Item1 => GetValue(ref _b1, ref _i1, _i1Factory);

        public AdvancedLazy(Func<T1> i1Factory)
            : this(i1Factory, true)
        {
        }

        public AdvancedLazy(Func<T1> i1Factory, bool useCaching)
        {
            _i1Factory = Guard.NotNull(i1Factory, nameof(i1Factory));
            UseCaching = useCaching;
        }

        public virtual void ClearCache()
        {
            _b1 = false;
            _i1 = default;
        }

        protected T GetValue<T>(ref bool bField, ref T? vField, Func<T> factory) => UseCaching ? (bField ? vField : vField = factory())! : factory();
    }

    public class AdvancedLazy<T1, T2> : AdvancedLazy<T1>
    {
        private readonly Func<T2> _i2Factory;

        private bool _b2;
        private T2? _i2;

        public T2 Item2 => GetValue(ref _b2, ref _i2, _i2Factory);

        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory)
            : this(i1Factory, i2Factory, true)
        {
        }

        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, bool useCaching)
            : base(i1Factory, useCaching)
        {
            _i2Factory = Guard.NotNull(i2Factory, nameof(i2Factory));
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _b2 = false;
            _i2 = default;
        }
    }

    public class AdvancedLazy<T1, T2, T3> : AdvancedLazy<T1, T2>
    {
        private readonly Func<T3> _i3Factory;

        private bool _b3;
        private T3? _i3;

        public T3 Item3 => GetValue(ref _b3, ref _i3, _i3Factory);

        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory)
            : this(i1Factory, i2Factory, i3Factory, true)
        {
        }

        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory, bool useCaching)
            : base(i1Factory, i2Factory, useCaching)
        {
            _i3Factory = Guard.NotNull(i3Factory, nameof(i3Factory));
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _b3 = false;
            _i3 = default;
        }
    }

    public class AdvancedLazy<T1, T2, T3, T4> : AdvancedLazy<T1, T2, T3>
    {
        private readonly Func<T4> _i4Factory;

        private bool _b4;
        private T4? _i4;

        public T4 Item4 => GetValue(ref _b4, ref _i4, _i4Factory);

        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory, Func<T4> i4Factory)
            : this(i1Factory, i2Factory, i3Factory, i4Factory, true)
        {
        }

        public AdvancedLazy(Func<T1> i1Factory, Func<T2> i2Factory, Func<T3> i3Factory, Func<T4> i4Factory, bool useCaching)
            : base(i1Factory, i2Factory, i3Factory, useCaching)
        {
            _i4Factory = Guard.NotNull(i4Factory, nameof(i4Factory));
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _b4 = false;
            _i4 = default;
        }
    }
}
