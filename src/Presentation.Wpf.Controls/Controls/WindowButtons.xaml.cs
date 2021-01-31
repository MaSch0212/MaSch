using System;
using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    // ReSharper disable once InconsistentNaming
    public partial class WindowButtons : UserControl
    {
        public static readonly DependencyProperty MaximizableProperty =
            DependencyProperty.Register(
                "Maximizable",
                typeof(bool),
                typeof(WindowButtons),
                new PropertyMetadata(true));

        public static readonly DependencyProperty OnlyCloseProperty =
            DependencyProperty.Register(
                "OnlyClose",
                typeof(bool),
                typeof(WindowButtons),
                new PropertyMetadata(false));

        public static readonly DependencyProperty MaximizedProperty =
            DependencyProperty.Register(
                "Maximized",
                typeof(bool),
                typeof(WindowButtons),
                new PropertyMetadata(false));

        public event Action CloseButtonClicked;
        public event Action MaximizeButtonClicked;
        public event Action MinimizeButtonClicked;
        public event Action NormalizeButtonClicked;

        public bool Maximizable
        {
            get => GetValue(MaximizableProperty) as bool? ?? true;
            set => SetValue(MaximizableProperty, value);
        }

        public bool OnlyClose
        {
            get => GetValue(OnlyCloseProperty) as bool? ?? false;
            set => SetValue(OnlyCloseProperty, value);
        }

        public bool Maximized
        {
            get => GetValue(MaximizedProperty) as bool? ?? false;
            set => SetValue(MaximizedProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowButtons"/> class.
        /// </summary>
        public WindowButtons()
        {
            InitializeComponent();
        }

        public void SetWindowState(WindowState state)
        {
            switch (state)
            {
                case WindowState.Maximized:
                    Maximized = true;
                    break;
                case WindowState.Normal:
                    Maximized = false;
                    break;
                case WindowState.Minimized:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) => CloseButtonClicked?.Invoke();

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => MinimizeButtonClicked?.Invoke();

        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => MaximizeButtonClicked?.Invoke();

        private void NormalizeButton_Click(object sender, RoutedEventArgs e) => NormalizeButtonClicked?.Invoke();
    }
}
