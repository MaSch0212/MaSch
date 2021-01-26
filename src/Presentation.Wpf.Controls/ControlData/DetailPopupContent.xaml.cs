using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using MaSch.Presentation.Wpf.Extensions;

namespace MaSch.Presentation.Wpf.ControlData
{
    [ContentProperty(nameof(PopupContent))]
    public class DetailPopupContent : Control
    {
        #region Fields

        private readonly Dictionary<AnchorStyle, Geometry> _arrowDataDict = new Dictionary<AnchorStyle, Geometry>
        {
            { AnchorStyle.None, null },
            { AnchorStyle.Left, Geometry.Parse("M 1,0 0,1 1,2") },
            { AnchorStyle.Top, Geometry.Parse("M 0,1 1,0 2,1") },
            { AnchorStyle.Right, Geometry.Parse("M 0,0 1,1 0,2") },
            { AnchorStyle.Bottom, Geometry.Parse("M 0,0 1,1 2,0") },
        };

        private Path _arrow;
        private FrameworkElement _border;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty ArrowPositionProperty =
            DependencyProperty.Register("ArrowPosition", typeof(AnchorStyle), typeof(DetailPopupContent), new PropertyMetadata(AnchorStyle.Top, OnArrowChanged));
        public static readonly DependencyProperty ArrowSizeProperty =
            DependencyProperty.Register("ArrowSize", typeof(double), typeof(DetailPopupContent), new PropertyMetadata(16D, OnArrowChanged));
        public static readonly DependencyProperty PopupContentProperty =
            DependencyProperty.Register("PopupContent", typeof(UIElement), typeof(DetailPopupContent), new PropertyMetadata(null));

        #endregion

        #region Dependency Property Changed Handler

        public static void OnArrowChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var owner = obj as DetailPopupContent;
            owner?.RefreshArrow();
        }

        #endregion

        #region Properties

        public AnchorStyle ArrowPosition
        {
            get => GetValue(ArrowPositionProperty) as AnchorStyle? ?? AnchorStyle.Top;
            set => SetValue(ArrowPositionProperty, value);
        }

        public double ArrowSize
        {
            get => GetValue(ArrowSizeProperty) as double? ?? 16D;
            set => SetValue(ArrowSizeProperty, value);
        }

        public UIElement PopupContent
        {
            get => (UIElement)GetValue(PopupContentProperty);
            set => SetValue(PopupContentProperty, value);
        }

        #endregion

        static DetailPopupContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DetailPopupContent), new FrameworkPropertyMetadata(typeof(DetailPopupContent)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _arrow = GetTemplateChild("PART_Arrow") as Path ?? throw new KeyNotFoundException("Control could not be found: PART_Arrow");
            _border = GetTemplateChild("PART_Border") as FrameworkElement ?? throw new KeyNotFoundException("Control could not be found: PART_Border");

            RefreshArrow();
        }

        #region Private Methods

        private void RefreshArrow()
        {
            double shortSize = ArrowSize + 2, longSize = shortSize * 2;
            if (_arrow == null)
                return;
            _arrow.Data = _arrowDataDict[ArrowPosition];
            switch (ArrowPosition)
            {
                case AnchorStyle.Left:
                    _arrow.SetSize(shortSize, longSize);
                    _arrow.SetAlignment(HorizontalAlignment.Left, VerticalAlignment.Center);
                    _arrow.StrokeThickness = BorderThickness.Left;
                    _border.Margin = new Thickness(ArrowSize, 0, 0, 0);
                    _border.SetMinSize(0, longSize * 1.25);
                    break;
                case AnchorStyle.Top:
                    _arrow.SetSize(longSize, shortSize);
                    _arrow.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Top);
                    _arrow.StrokeThickness = BorderThickness.Top;
                    _border.Margin = new Thickness(0, ArrowSize, 0, 0);
                    _border.SetMinSize(longSize * 1.25, 0);
                    break;
                case AnchorStyle.Right:
                    _arrow.SetSize(shortSize, longSize);
                    _arrow.SetAlignment(HorizontalAlignment.Right, VerticalAlignment.Center);
                    _arrow.StrokeThickness = BorderThickness.Right;
                    _border.Margin = new Thickness(0, 0, ArrowSize, 0);
                    _border.SetMinSize(0, longSize * 1.25);
                    break;
                case AnchorStyle.Bottom:
                    _arrow.SetSize(longSize, shortSize);
                    _arrow.SetAlignment(HorizontalAlignment.Center, VerticalAlignment.Bottom);
                    _arrow.StrokeThickness = BorderThickness.Bottom;
                    _border.Margin = new Thickness(0, 0, 0, ArrowSize);
                    _border.SetMinSize(longSize * 1.25, 0);
                    break;
                case AnchorStyle.None:
                    _border.Margin = new Thickness(0);
                    _border.SetMinSize(0, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
