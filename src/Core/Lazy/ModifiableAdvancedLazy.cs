using System;

namespace MaSch.Core.Lazy
{
    public class ModifiableAdvancedLazy<T1>
    {
        private readonly Func<T1> _i1Factory;
        private readonly Action<T1> _i1Callback;

        private bool _b1;
        private T1? _i1;

        protected bool UseCaching { get; }

        public T1 Item1
        {
            get => GetValue(ref _b1, ref _i1, _i1Factory);
            set => SetValue(ref _b1, ref _i1, value, _i1Callback);
        }

        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback)
            : this(i1Factory, i1Callback, true)
        {
        }
        
        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, bool useCaching)
        {
            _i1Factory = Guard.NotNull(i1Factory, nameof(i1Factory));
            _i1Callback = Guard.NotNull(i1Callback, nameof(i1Callback));
            UseCaching = useCaching;
        }

        public virtual void ClearCache()
        {
            _b1 = false;
            _i1 = default;
        }

        protected T GetValue<T>(ref bool bField, ref T? vField, Func<T> factory) => UseCaching ? (bField ? vField : vField = factory())! : factory();
        protected void SetValue<T>(ref bool bField, ref T? vField, T value, Action<T> callback)
        {
            if (UseCaching)
            {
                bField = true;
                callback(vField = value);
            }
            else
                callback(value);
        }
    }

    public class ModifiableAdvancedLazy<T1, T2> : ModifiableAdvancedLazy<T1>
    {
        private readonly Func<T2> _i2Factory;
        private readonly Action<T2> _i2Callback;

        private bool _b2;
        private T2? _i2;

        public T2 Item2
        {
            get => GetValue(ref _b2, ref _i2, _i2Factory);
            set => SetValue(ref _b2, ref _i2, value, _i2Callback);
        }

        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback)
            : this(i1Factory, i1Callback, i2Factory, i2Callback, true)
        {
        }

        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, bool useCaching)
            : base(i1Factory, i1Callback, useCaching)
        {
            _i2Factory = Guard.NotNull(i2Factory, nameof(i2Factory));
            _i2Callback = Guard.NotNull(i2Callback, nameof(i2Callback));
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _b2 = false;
            _i2 = default;
        }
    }

    public class ModifiableAdvancedLazy<T1, T2, T3> : ModifiableAdvancedLazy<T1, T2>
    {
        private readonly Func<T3> _i3Factory;
        private readonly Action<T3> _i3Callback;

        private bool _b3;
        private T3? _i3;

        public T3 Item3
        {
            get => GetValue(ref _b3, ref _i3, _i3Factory);
            set => SetValue(ref _b3, ref _i3, value, _i3Callback);
        }

        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback)
            : this(i1Factory, i1Callback, i2Factory, i2Callback, i3Factory, i3Callback, true)
        {
        }

        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback, bool useCaching)
            : base(i1Factory, i1Callback, i2Factory, i2Callback, useCaching)
        {
            _i3Factory = Guard.NotNull(i3Factory, nameof(i3Factory));
            _i3Callback = Guard.NotNull(i3Callback, nameof(i3Callback));
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _b3 = false;
            _i3 = default;
        }
    }

    public class ModifiableAdvancedLazy<T1, T2, T3, T4> : ModifiableAdvancedLazy<T1, T2, T3>
    {
        private readonly Func<T4> _i4Factory;
        private readonly Action<T4> _i4Callback;

        private bool _b4;
        private T4? _i4;

        public T4 Item4
        {
            get => GetValue(ref _b4, ref _i4, _i4Factory);
            set => SetValue(ref _b4, ref _i4, value, _i4Callback);
        }

        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback, Func<T4> i4Factory, Action<T4> i4Callback)
            : this(i1Factory, i1Callback, i2Factory, i2Callback, i3Factory, i3Callback, i4Factory, i4Callback, true)
        {
        }

        public ModifiableAdvancedLazy(Func<T1> i1Factory, Action<T1> i1Callback, Func<T2> i2Factory, Action<T2> i2Callback, Func<T3> i3Factory, Action<T3> i3Callback, Func<T4> i4Factory, Action<T4> i4Callback, bool useCaching)
            : base(i1Factory, i1Callback, i2Factory, i2Callback, i3Factory, i3Callback, useCaching)
        {
            _i4Factory = Guard.NotNull(i4Factory, nameof(i4Factory));
            _i4Callback = Guard.NotNull(i4Callback, nameof(i4Callback));
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _b4 = false;
            _i4 = default;
        }
    }
}
