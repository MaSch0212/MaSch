﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WinControls=System.Windows.Controls;

#pragma warning disable SA1600 // Elements should be documented

namespace MaSch.Presentation.Wpf.Controls
{
    public class VirtualizingWrapPanel : VirtualizingPanel, IScrollInfo
    {
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                "ItemHeight",
                typeof(double),
                typeof(VirtualizingWrapPanel),
                new FrameworkPropertyMetadata(double.PositiveInfinity));

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(
                "ItemWidth",
                typeof(double),
                typeof(VirtualizingWrapPanel),
                new FrameworkPropertyMetadata(double.PositiveInfinity));

        public static readonly DependencyProperty OrientationProperty =
            StackPanel.OrientationProperty.AddOwner(
                typeof(VirtualizingWrapPanel),
                new FrameworkPropertyMetadata(Orientation.Horizontal));

        private readonly Dictionary<UIElement, Rect> _realizedChildLayout = new Dictionary<UIElement, Rect>();
        private UIElementCollection _children;
        private ItemsControl _itemsControl;
        private IItemContainerGenerator _generator;
        private Point _offset = new Point(0, 0);
        private Size _extent = new Size(0, 0);
        private Size _viewport = new Size(0, 0);
        private int _firstIndex = 0;
        private Size _childSize;
        private Size _pixelMeasuredViewport = new Size(0, 0);
        private WrapPanelAbstraction _abstractPanel;
        private bool _canHScroll = false;
        private bool _canVScroll = false;
        private WinControls.ScrollViewer _owner;

        private Size ChildSlotSize => new Size(ItemWidth, ItemHeight);

        [TypeConverter(typeof(LengthConverter))]
        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        [TypeConverter(typeof(LengthConverter))]
        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public bool CanHorizontallyScroll
        {
            get => _canHScroll;
            set => _canHScroll = value;
        }

        public bool CanVerticallyScroll
        {
            get => _canVScroll;
            set => _canVScroll = value;
        }

        public double ExtentHeight => _extent.Height;

        public double ExtentWidth => _extent.Width;

        public double HorizontalOffset => _offset.X;

        public double VerticalOffset => _offset.Y;

        public double ViewportHeight => _viewport.Height;

        public double ViewportWidth => _viewport.Width;

        public WinControls.ScrollViewer ScrollOwner
        {
            get => _owner;
            set => _owner = value;
        }

        public void SetFirstRowViewItemIndex(int index)
        {
            SetVerticalOffset(index / Math.Floor(_viewport.Width / _childSize.Width));
            SetHorizontalOffset(index / Math.Floor(_viewport.Height / _childSize.Height));
        }

        public int GetFirstVisibleSection()
        {
            int section;
            var maxSection = _abstractPanel.Max(x => x.Section);
            if (Orientation == Orientation.Horizontal)
            {
                section = (int)_offset.Y;
            }
            else
            {
                section = (int)_offset.X;
            }

            if (section > maxSection)
                section = maxSection;
            return section;
        }

        public int GetFirstVisibleIndex()
        {
            int section = GetFirstVisibleSection();
            var item = _abstractPanel.FirstOrDefault(x => x.Section == section);
            if (item != null)
                return item.Index;
            return 0;
        }

        public void LineDown()
        {
            if (Orientation == Orientation.Vertical)
                SetVerticalOffset(VerticalOffset + 20);
            else
                SetVerticalOffset(VerticalOffset + 1);
        }

        public void LineLeft()
        {
            if (Orientation == Orientation.Horizontal)
                SetHorizontalOffset(HorizontalOffset - 20);
            else
                SetHorizontalOffset(HorizontalOffset - 1);
        }

        public void LineRight()
        {
            if (Orientation == Orientation.Horizontal)
                SetHorizontalOffset(HorizontalOffset + 20);
            else
                SetHorizontalOffset(HorizontalOffset + 1);
        }

        public void LineUp()
        {
            if (Orientation == Orientation.Vertical)
                SetVerticalOffset(VerticalOffset - 20);
            else
                SetVerticalOffset(VerticalOffset - 1);
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            var gen = (ItemContainerGenerator)_generator.GetItemContainerGeneratorForPanel(this);
            var element = (UIElement)visual;
            int itemIndex = gen.IndexFromContainer(element);
            while (itemIndex == -1)
            {
                element = (UIElement)VisualTreeHelper.GetParent(element);
                itemIndex = gen.IndexFromContainer(element);
            }

            Rect elementRect = _realizedChildLayout[element];
            if (Orientation == Orientation.Horizontal)
            {
                double viewportHeight = _pixelMeasuredViewport.Height;
                if (elementRect.Bottom > viewportHeight)
                    _offset.Y += 1;
                else if (elementRect.Top < 0)
                    _offset.Y -= 1;
            }
            else
            {
                double viewportWidth = _pixelMeasuredViewport.Width;
                if (elementRect.Right > viewportWidth)
                    _offset.X += 1;
                else if (elementRect.Left < 0)
                    _offset.X -= 1;
            }

            InvalidateMeasure();
            return elementRect;
        }

        public void MouseWheelDown()
        {
            PageDown();
        }

        public void MouseWheelLeft()
        {
            PageLeft();
        }

        public void MouseWheelRight()
        {
            PageRight();
        }

        public void MouseWheelUp()
        {
            PageUp();
        }

        public void PageDown()
        {
            SetVerticalOffset(VerticalOffset + (_viewport.Height * 0.8));
        }

        public void PageLeft()
        {
            SetHorizontalOffset(HorizontalOffset - (_viewport.Width * 0.8));
        }

        public void PageRight()
        {
            SetHorizontalOffset(HorizontalOffset + (_viewport.Width * 0.8));
        }

        public void PageUp()
        {
            SetVerticalOffset(VerticalOffset - (_viewport.Height * 0.8));
        }

        public void SetHorizontalOffset(double offset)
        {
            if (offset < 0 || _viewport.Width >= _extent.Width)
            {
                offset = 0;
            }
            else
            {
                if (offset + _viewport.Width >= _extent.Width)
                {
                    offset = _extent.Width - _viewport.Width;
                }
            }

            _offset.X = offset;

            if (_owner != null)
                _owner.InvalidateScrollInfo();

            InvalidateMeasure();
            _firstIndex = GetFirstVisibleIndex();
        }

        public void SetVerticalOffset(double offset)
        {
            if (offset < 0 || _viewport.Height >= _extent.Height)
            {
                offset = 0;
            }
            else
            {
                if (offset + _viewport.Height >= _extent.Height)
                {
                    offset = _extent.Height - _viewport.Height;
                }
            }

            _offset.Y = offset;

            if (_owner != null)
                _owner.InvalidateScrollInfo();

            // _trans.Y = -offset;
            InvalidateMeasure();
            _firstIndex = GetFirstVisibleIndex();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    NavigateDown();
                    e.Handled = true;
                    break;
                case Key.Left:
                    NavigateLeft();
                    e.Handled = true;
                    break;
                case Key.Right:
                    NavigateRight();
                    e.Handled = true;
                    break;
                case Key.Up:
                    NavigateUp();
                    e.Handled = true;
                    break;
                default:
                    base.OnKeyDown(e);
                    break;
            }
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(object sender, ItemsChangedEventArgs args)
        {
            base.OnItemsChanged(sender, args);
            _abstractPanel = null;
            ResetScrollInfo();
        }

        /// <inheritdoc/>
        protected override void OnInitialized(EventArgs e)
        {
            SizeChanged += new SizeChangedEventHandler(Resizing);
            base.OnInitialized(e);
            _itemsControl = ItemsControl.GetItemsOwner(this);
            _children = InternalChildren;
            _generator = ItemContainerGenerator;
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (_itemsControl == null || _itemsControl.Items.Count == 0)
                return availableSize;
            if (_abstractPanel == null)
                _abstractPanel = new WrapPanelAbstraction(_itemsControl.Items.Count);

            _pixelMeasuredViewport = availableSize;

            _realizedChildLayout.Clear();

            Size realizedFrameSize = availableSize;

            int itemCount = _itemsControl.Items.Count;
            int firstVisibleIndex = GetFirstVisibleIndex();

            GeneratorPosition startPos = _generator.GeneratorPositionFromIndex(firstVisibleIndex);

            int childIndex = (startPos.Offset == 0) ? startPos.Index : startPos.Index + 1;
            int current = firstVisibleIndex;
            int visibleSections = 1;
            using (_generator.StartAt(startPos, GeneratorDirection.Forward, true))
            {
                bool stop = false;
                bool isHorizontal = Orientation == Orientation.Horizontal;
                double currentX = 0;
                double currentY = 0;
                double maxItemSize = 0;
                int currentSection = GetFirstVisibleSection();
                while (current < itemCount)
                {
                    // Get or create the child
                    UIElement child = _generator.GenerateNext(out bool newlyRealized) as UIElement;
                    if (newlyRealized)
                    {
                        // Figure out if we need to insert the child at the end or somewhere in the middle
                        if (childIndex >= _children.Count)
                        {
                            AddInternalChild(child);
                        }
                        else
                        {
                            InsertInternalChild(childIndex, child);
                        }

                        _generator.PrepareItemContainer(child);
                        child.Measure(ChildSlotSize);
                    }
                    else
                    {
                        // The child has already been created, let's be sure it's in the right spot
                        Debug.Assert(child == _children[childIndex], "Wrong child was generated");
                    }

                    _childSize = child.DesiredSize;
                    Rect childRect = new Rect(new Point(currentX, currentY), _childSize);
                    if (isHorizontal)
                    {
                        maxItemSize = Math.Max(maxItemSize, childRect.Height);

                        // wrap to a new line
                        if (childRect.Right > realizedFrameSize.Width)
                        {
                            currentY += maxItemSize;
                            currentX = 0;
                            maxItemSize = childRect.Height;
                            childRect.X = currentX;
                            childRect.Y = currentY;
                            currentSection++;
                            visibleSections++;
                        }

                        if (currentY > realizedFrameSize.Height)
                            stop = true;
                        currentX = childRect.Right;
                    }
                    else
                    {
                        maxItemSize = Math.Max(maxItemSize, childRect.Width);

                        // wrap to a new column
                        if (childRect.Bottom > realizedFrameSize.Height)
                        {
                            currentX += maxItemSize;
                            currentY = 0;
                            maxItemSize = childRect.Width;
                            childRect.X = currentX;
                            childRect.Y = currentY;
                            currentSection++;
                            visibleSections++;
                        }

                        if (currentX > realizedFrameSize.Width)
                            stop = true;

                        currentY = childRect.Bottom;
                    }

                    _realizedChildLayout.Add(child, childRect);
                    _abstractPanel.SetItemSection(current, currentSection);

                    if (stop)
                        break;
                    current++;
                    childIndex++;
                }
            }

            CleanUpItems(firstVisibleIndex, current - 1);

            ComputeExtentAndViewport(availableSize, visibleSections);

            return availableSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_children != null)
            {
                foreach (UIElement child in _children)
                {
                    var layoutInfo = _realizedChildLayout[child];
                    child.Arrange(layoutInfo);
                }
            }

            return finalSize;
        }

        private void Resizing(object sender, EventArgs e)
        {
            if (_viewport.Width != 0)
            {
                int firstIndexCache = _firstIndex;
                _abstractPanel = null;
                MeasureOverride(_viewport);
                SetFirstRowViewItemIndex(_firstIndex);
                _firstIndex = firstIndexCache;
            }
        }

        private void CleanUpItems(int minDesiredGenerated, int maxDesiredGenerated)
        {
            for (int i = _children.Count - 1; i >= 0; i--)
            {
                GeneratorPosition childGeneratorPos = new GeneratorPosition(i, 0);
                int itemIndex = _generator.IndexFromGeneratorPosition(childGeneratorPos);
                if (itemIndex < minDesiredGenerated || itemIndex > maxDesiredGenerated)
                {
                    _generator.Remove(childGeneratorPos, 1);
                    RemoveInternalChildRange(i, 1);
                }
            }
        }

        private void ComputeExtentAndViewport(Size pixelMeasuredViewportSize, int visibleSections)
        {
            if (Orientation == Orientation.Horizontal)
            {
                _viewport.Height = visibleSections;
                _viewport.Width = pixelMeasuredViewportSize.Width;
            }
            else
            {
                _viewport.Width = visibleSections;
                _viewport.Height = pixelMeasuredViewportSize.Height;
            }

            if (Orientation == Orientation.Horizontal)
            {
                _extent.Height = _abstractPanel.SectionCount + ViewportHeight - 1;
            }
            else
            {
                _extent.Width = _abstractPanel.SectionCount + ViewportWidth - 1;
            }

            _owner.InvalidateScrollInfo();
        }

        private void ResetScrollInfo()
        {
            _offset.X = 0;
            _offset.Y = 0;
        }

        private int GetNextSectionClosestIndex(int itemIndex)
        {
            var abstractItem = _abstractPanel[itemIndex];
            if (abstractItem.Section < _abstractPanel.SectionCount - 1)
            {
                var ret = _abstractPanel.
                    Where(x => x.Section == abstractItem.Section + 1).
                    OrderBy(x => Math.Abs(x.SectionIndex - abstractItem.SectionIndex)).
                    First();
                return ret.Index;
            }
            else
            {
                return itemIndex;
            }
        }

        private int GetLastSectionClosestIndex(int itemIndex)
        {
            var abstractItem = _abstractPanel[itemIndex];
            if (abstractItem.Section > 0)
            {
                var ret = _abstractPanel.
                    Where(x => x.Section == abstractItem.Section - 1).
                    OrderBy(x => Math.Abs(x.SectionIndex - abstractItem.SectionIndex)).
                    First();
                return ret.Index;
            }
            else
            {
                return itemIndex;
            }
        }

        private void NavigateDown()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }

            DependencyObject next;
            if (Orientation == Orientation.Horizontal)
            {
                int nextIndex = GetNextSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == _abstractPanel.ItemCount - 1)
                    return;
                next = gen.ContainerFromIndex(itemIndex + 1);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex + 1);
                }
            }

            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }

            (next as UIElement).Focus();
        }

        private void NavigateLeft()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);

            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }

            DependencyObject next;
            if (Orientation == Orientation.Vertical)
            {
                int nextIndex = GetLastSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == 0)
                    return;
                next = gen.ContainerFromIndex(itemIndex - 1);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex - 1);
                }
            }

            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }

            (next as UIElement).Focus();
        }

        private void NavigateRight()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }

            DependencyObject next;
            if (Orientation == Orientation.Vertical)
            {
                int nextIndex = GetNextSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == _abstractPanel.ItemCount - 1)
                    return;
                next = gen.ContainerFromIndex(itemIndex + 1);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset + 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex + 1);
                }
            }

            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }

            (next as UIElement).Focus();
        }

        private void NavigateUp()
        {
            var gen = _generator.GetItemContainerGeneratorForPanel(this);
            UIElement selected = (UIElement)Keyboard.FocusedElement;
            int itemIndex = gen.IndexFromContainer(selected);
            int depth = 0;
            while (itemIndex == -1)
            {
                selected = (UIElement)VisualTreeHelper.GetParent(selected);
                itemIndex = gen.IndexFromContainer(selected);
                depth++;
            }

            DependencyObject next;
            if (Orientation == Orientation.Horizontal)
            {
                int nextIndex = GetLastSectionClosestIndex(itemIndex);
                next = gen.ContainerFromIndex(nextIndex);
                while (next == null)
                {
                    SetVerticalOffset(VerticalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(nextIndex);
                }
            }
            else
            {
                if (itemIndex == 0)
                    return;
                next = gen.ContainerFromIndex(itemIndex - 1);
                while (next == null)
                {
                    SetHorizontalOffset(HorizontalOffset - 1);
                    UpdateLayout();
                    next = gen.ContainerFromIndex(itemIndex - 1);
                }
            }

            while (depth != 0)
            {
                next = VisualTreeHelper.GetChild(next, 0);
                depth--;
            }

            (next as UIElement).Focus();
        }

        private class ItemAbstraction
        {
            private readonly WrapPanelAbstraction _panel;
            private int _sectionIndex = -1;
            private int _section = -1;

            public int Index { get; }

            public int SectionIndex
            {
                get
                {
                    if (_sectionIndex == -1)
                    {
                        return (Index % _panel.AverageItemsPerSection) - 1;
                    }

                    return _sectionIndex;
                }
                set
                {
                    if (_sectionIndex == -1)
                        _sectionIndex = value;
                }
            }

            public int Section
            {
                get
                {
                    if (_section == -1)
                    {
                        return Index / _panel.AverageItemsPerSection;
                    }

                    return _section;
                }
                set
                {
                    if (_section == -1)
                        _section = value;
                }
            }

            public ItemAbstraction(WrapPanelAbstraction panel, int index)
            {
                _panel = panel;
                Index = index;
            }
        }

        private class WrapPanelAbstraction : IEnumerable<ItemAbstraction>
        {
            private readonly object _syncRoot = new object();
            private int _currentSetSection = -1;
            private int _currentSetItemIndex = -1;
            private int _itemsInCurrentSecction = 0;

            public int ItemCount { get; }
            public int AverageItemsPerSection { get; private set; }
            private ReadOnlyCollection<ItemAbstraction> Items { get; set; }

            public int SectionCount
            {
                get
                {
                    int ret = _currentSetSection + 1;
                    if (_currentSetItemIndex + 1 < Items.Count)
                    {
                        int itemsLeft = Items.Count - _currentSetItemIndex;
                        ret += (itemsLeft / AverageItemsPerSection) + 1;
                    }

                    return ret;
                }
            }

            public WrapPanelAbstraction(int itemCount)
            {
                List<ItemAbstraction> items = new List<ItemAbstraction>(itemCount);
                for (int i = 0; i < itemCount; i++)
                {
                    ItemAbstraction item = new ItemAbstraction(this, i);
                    items.Add(item);
                }

                Items = new ReadOnlyCollection<ItemAbstraction>(items);
                AverageItemsPerSection = itemCount;
                ItemCount = itemCount;
            }

            public void SetItemSection(int index, int section)
            {
                lock (_syncRoot)
                {
                    if (section <= _currentSetSection + 1 && index == _currentSetItemIndex + 1)
                    {
                        _currentSetItemIndex++;
                        Items[index].Section = section;
                        if (section == _currentSetSection + 1)
                        {
                            _currentSetSection = section;
                            if (section > 0)
                            {
                                AverageItemsPerSection = index / section;
                            }

                            _itemsInCurrentSecction = 1;
                        }
                        else
                        {
                            _itemsInCurrentSecction++;
                        }

                        Items[index].SectionIndex = _itemsInCurrentSecction - 1;
                    }
                }
            }

            public ItemAbstraction this[int index] => Items[index];

            public IEnumerator<ItemAbstraction> GetEnumerator()
            {
                return Items.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
