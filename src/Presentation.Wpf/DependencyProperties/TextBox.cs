using System.Windows;
using Win = System.Windows.Controls;

namespace MaSch.Presentation.Wpf.DependencyProperties
{
    /// <summary>
    /// Provides dependency properties for the <see cref="Win.TextBox"/> control.
    /// </summary>
    public static class TextBox
    {
        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether the carte index should be tracked. Use for the <see cref="CursorPositionProperty"/>.
        /// </summary>
        public static readonly DependencyProperty TrackCaretIndexProperty =
            DependencyProperty.RegisterAttached(
                "TrackCaretIndex",
                typeof(bool),
                typeof(TextBox),
                new PropertyMetadata(false, OnTrackCaretIndexPropertyChanged));

        /// <summary>
        /// Dependency property. Gets or sets the current cursor position. This property only works when <see cref="TrackCaretIndexProperty"/> is set to <c>true</c>.
        /// </summary>
        public static readonly DependencyProperty CursorPositionProperty =
            DependencyProperty.RegisterAttached(
                "CursorPosition",
                typeof(int),
                typeof(TextBox),
                new PropertyMetadata(0, OnCursorPositionPropertyChanged));

        /// <summary>
        /// Dependency property. Gets or sets the Descriptions for the text box in form of a placeholder.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.RegisterAttached(
                "Description",
                typeof(string),
                typeof(TextBox),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets the value of the <see cref="TrackCaretIndexProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="TrackCaretIndexProperty"/>.</returns>
        public static bool GetTrackCaretIndex(DependencyObject depObj)
        {
            return (bool)depObj.GetValue(TrackCaretIndexProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="TrackCaretIndexProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetTrackCaretIndex(DependencyObject depObj, bool value)
        {
            depObj.SetValue(TrackCaretIndexProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="CursorPositionProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to get the value from.</param>
        /// <returns>The value of the <see cref="CursorPositionProperty"/>.</returns>
        public static int GetCursorPosition(DependencyObject depObj)
        {
            return (int)depObj.GetValue(CursorPositionProperty);
        }

        /// <summary>
        /// Sets the value of the <see cref="CursorPositionProperty"/>.
        /// </summary>
        /// <param name="depObj">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetCursorPosition(DependencyObject depObj, int value)
        {
            depObj.SetValue(CursorPositionProperty, value);
        }

        /// <summary>
        /// Sets the value of the <see cref="DescriptionProperty"/>.
        /// </summary>
        /// <param name="target">The element to set the value to.</param>
        /// <param name="value">The value to set.</param>
        public static void SetDescription(DependencyObject target, string value)
        {
            target.SetValue(DescriptionProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="DescriptionProperty"/>.
        /// </summary>
        /// <param name="target">The element to get the value from.</param>
        /// <returns>The value of the <see cref="DescriptionProperty"/>.</returns>
        public static string GetDescription(DependencyObject target)
        {
            return (string)target.GetValue(DescriptionProperty);
        }

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

        private static void OnCursorPositionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Win.TextBox tb && e.NewValue is int i && tb.CaretIndex != i)
            {
                tb.CaretIndex = i;
            }
        }
    }
}
