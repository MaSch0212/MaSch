using System.Windows;
using Win = System.Windows.Controls;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    public static class TextBox
    {
        #region TrackCaretIndex attached property

        public static bool GetTrackCaretIndex(DependencyObject depObj)
        {
            return (bool)depObj.GetValue(TrackCaretIndexProperty);
        }

        public static void SetTrackCaretIndex(DependencyObject depObj, bool value)
        {
            depObj.SetValue(TrackCaretIndexProperty, value);
        }

        public static readonly DependencyProperty TrackCaretIndexProperty =
            DependencyProperty.RegisterAttached("TrackCaretIndex", typeof(bool),
            typeof(TextBox), new PropertyMetadata(false, OnTrackCaretIndexPropertyChanged));

        private static void OnTrackCaretIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.TextBox tb && e.NewValue is bool bn && e.OldValue is bool bo)
            {
                if (!bo && bn)
                    tb.SelectionChanged += OnTextBoxSelectionChanged;
                else if (bo && !bn)
                    tb.SelectionChanged -= OnTextBoxSelectionChanged;
            }
        }

        private static void OnTextBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Win.TextBox tb)
                SetCursorPosition(tb, tb.CaretIndex);
        }

        #endregion

        #region CursorPosition attached property

        public static int GetCursorPosition(DependencyObject depObj)
        {
            return (int)depObj.GetValue(CursorPositionProperty);
        }

        public static void SetCursorPosition(DependencyObject depObj, int value)
        {
            depObj.SetValue(CursorPositionProperty, value);
        }

        public static readonly DependencyProperty CursorPositionProperty =
            DependencyProperty.RegisterAttached("CursorPosition", typeof(int),
            typeof(TextBox), new PropertyMetadata(0, OnCursorPositionPropertyChanged));

        private static void OnCursorPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.TextBox tb && e.NewValue is int i && tb.CaretIndex != i)
            {
                tb.CaretIndex = i;
            }
        }

        #endregion

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached("Description", typeof(string), typeof(TextBox),
                new PropertyMetadata(""));
        public static void SetDescription(DependencyObject target, string value)
        {
            target.SetValue(DescriptionProperty, value);
        }
        public static string GetDescription(DependencyObject target)
        {
            return (string)target.GetValue(DescriptionProperty);
        }
    }
}
