using System;
using System.Drawing;

namespace MaSch.Core.Lazy
{
    public class LazyPoint : AdvancedLazy<int, int>
    {
        private bool _bPoint;
        private Point _iPoint;

        public virtual int X => Item1;
        public virtual int Y => Item2;
        public virtual Point Point => GetValue(ref _bPoint, ref _iPoint, () => new Point(Item1, Item2));

        public LazyPoint(Func<int> xFactory, Func<int> yFactory) : base(xFactory, yFactory)
        {
        }

        public LazyPoint(Func<int> xFactory, Func<int> yFactory, bool useCaching) : base(xFactory, yFactory, useCaching)
        {
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _bPoint = false;
            _iPoint = default;
        }
    }

    public class ModifiableLazyPoint : ModifiableAdvancedLazy<int, int>
    {
        private bool _bPoint;
        private Point _iPoint;

        public virtual int X
        {
            get => Item1;
            set => Item1 = value;
        }
        public virtual int Y
        {
            get => Item2;
            set => Item2 = value;
        }
        public virtual Point Point
        {
            get => GetValue(ref _bPoint, ref _iPoint, () => new Point(Item1, Item2));
            set => SetValue(ref _bPoint, ref _iPoint, value, p => (Item1, Item2) = (p.X, p.Y));
        }

        public ModifiableLazyPoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback) : base(xFactory, xCallback, yFactory, yCallback)
        {
        }

        public ModifiableLazyPoint(Func<int> xFactory, Action<int> xCallback, Func<int> yFactory, Action<int> yCallback, bool useCaching) : base(xFactory, xCallback, yFactory, yCallback, useCaching)
        {
        }

        public override void ClearCache()
        {
            base.ClearCache();
            _bPoint = false;
            _iPoint = default;
        }
    }
}
