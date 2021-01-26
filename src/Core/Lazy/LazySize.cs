using System;
using System.Drawing;

namespace MaSch.Common.Lazy
{
    public class LazySize : AdvancedLazy<int, int>
    {
        private bool _bSize;
        private Size _iSize;

        public virtual int Width => Item1;
        public virtual int Height => Item2;
        public virtual Size Size => GetValue(ref _bSize, ref _iSize, () => new Size(Item1, Item2));

        public LazySize(Func<int> widthFactory, Func<int> heightFactory) : base(widthFactory, heightFactory)
        {
        }

        public LazySize(Func<int> widthFactory, Func<int> heightFactory, bool useCaching) : base(widthFactory, heightFactory, useCaching)
        {
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _bSize = false;
            _iSize = default;
        }
    }

    public class ModifiableLazySize : ModifiableAdvancedLazy<int, int>
    {
        private bool _bSize;
        private Size _iSize;

        public virtual int Width
        {
            get => Item1;
            set => Item1 = value;
        }
        public virtual int Height
        {
            get => Item2;
            set => Item2 = value;
        }
        public virtual Size Size
        {
            get => GetValue(ref _bSize, ref _iSize, () => new Size(Item1, Item2));
            set => SetValue(ref _bSize, ref _iSize, value, p => (Item1, Item2) = (p.Width, p.Height));
        }

        public ModifiableLazySize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback) : base(widthFactory, widthCallback, heightFactory, heightCallback)
        {
        }

        public ModifiableLazySize(Func<int> widthFactory, Action<int> widthCallback, Func<int> heightFactory, Action<int> heightCallback, bool useCaching) : base(widthFactory, widthCallback, heightFactory, heightCallback, useCaching)
        {
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _bSize = false;
            _iSize = default;
        }
    }
}
