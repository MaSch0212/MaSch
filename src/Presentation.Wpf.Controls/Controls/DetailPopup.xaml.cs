﻿using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using MaSch.Presentation.Wpf.ControlData;

namespace MaSch.Presentation.Wpf.Controls
{
    public class DetailPopup : Popup
    {
        public static readonly DependencyProperty ArrowSizeProperty =
            DependencyProperty.Register(
                "ArrowSize",
                typeof(int),
                typeof(DetailPopup),
                new PropertyMetadata(16));

        public static readonly DependencyProperty ArrowPositionProperty =
            DependencyProperty.Register(
                "ArrowPosition",
                typeof(AnchorStyle),
                typeof(DetailPopup),
                new PropertyMetadata(AnchorStyle.Top));

        public int ArrowSize
        {
            get => GetValue(ArrowSizeProperty) as int? ?? 16;
            set => SetValue(ArrowSizeProperty, value);
        }

        public AnchorStyle ArrowPosition
        {
            get => GetValue(ArrowPositionProperty) as AnchorStyle? ?? AnchorStyle.Top;
            set => SetValue(ArrowPositionProperty, value);
        }

        static DetailPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DetailPopup), new FrameworkPropertyMetadata(typeof(DetailPopup)));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IsOpenProperty && (bool)e.NewValue)
                RefreshOffset();
            base.OnPropertyChanged(e);
            if (Child is DetailPopupContent popupContent)
            {
                if (e.Property == ArrowPositionProperty)
                    popupContent.ArrowPosition = ArrowPosition;
                else if (e.Property == ArrowSizeProperty)
                    popupContent.ArrowSize = ArrowSize;
            }
            else if (e.Property == ChildProperty)
            {
                var content = new DetailPopupContent { PopupContent = Child };
                content.SizeChanged += (s, ex) => RefreshOffset();
                Child = content;
            }
        }

        private void RefreshOffset()
        {
            Placement = PlacementMode.Center;
            var target = PlacementTarget as FrameworkElement;
            if (Child is not FrameworkElement content)
                return;
            switch (ArrowPosition)
            {
                case AnchorStyle.Left:
                    HorizontalOffset = (((target?.ActualWidth ?? 0) + content.ActualWidth) / 2) + Margin.Left;
                    VerticalOffset = 0;
                    break;
                case AnchorStyle.Top:
                    HorizontalOffset = 0;
                    VerticalOffset = (((target?.ActualHeight ?? 0) + content.ActualHeight) / 2) + Margin.Top;
                    break;
                case AnchorStyle.Right:
                    HorizontalOffset = (-((target?.ActualWidth ?? 0) + content.ActualWidth) / 2) - Margin.Right;
                    VerticalOffset = 0;
                    break;
                case AnchorStyle.Bottom:
                    HorizontalOffset = 0;
                    VerticalOffset = (-((target?.ActualHeight ?? 0) + content.ActualHeight) / 2) - Margin.Bottom;
                    break;
                case AnchorStyle.None:
                    HorizontalOffset = Margin.Left - Margin.Right;
                    VerticalOffset = Margin.Top - Margin.Bottom;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
