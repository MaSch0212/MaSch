using MaSch.Presentation.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MaSch.Presentation.Wpf.Controls
{
    public class Window : System.Windows.Window, IWindow
    {
        public static readonly DependencyProperty MaximizableProperty =
            DependencyProperty.Register(
                "Maximizable",
                typeof(bool),
                typeof(Window),
                new PropertyMetadata(true));

        public static readonly DependencyProperty OnlyCloseProperty =
            DependencyProperty.Register(
                "OnlyClose",
                typeof(bool),
                typeof(Window),
                new PropertyMetadata(false));

        public static readonly DependencyProperty DragMoveOnContentBorderProperty =
            DependencyProperty.Register(
                "DragMoveOnContentBorder",
                typeof(bool),
                typeof(Window),
                new PropertyMetadata(false));

        public static readonly DependencyProperty CustomIconProperty =
            DependencyProperty.Register(
                "CustomIcon",
                typeof(object),
                typeof(Window),
                new PropertyMetadata(null));

        public static readonly DependencyProperty TitleAlignmentProperty =
            DependencyProperty.Register(
                "TitleAlignment",
                typeof(HorizontalAlignment),
                typeof(Window),
                new PropertyMetadata(HorizontalAlignment.Left));

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
        }

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

        public bool DragMoveOnContentBorder
        {
            get => GetValue(DragMoveOnContentBorderProperty) as bool? ?? false;
            set => SetValue(DragMoveOnContentBorderProperty, value);
        }

        public object CustomIcon
        {
            get => GetValue(CustomIconProperty);
            set => SetValue(CustomIconProperty, value);
        }

        public HorizontalAlignment TitleAlignment
        {
            get => GetValue(TitleAlignmentProperty) as HorizontalAlignment? ?? HorizontalAlignment.Left;
            set => SetValue(TitleAlignmentProperty, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
        {
            SizeChanged += ModernUIWindow_SizeChanged;
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitializeTemplateComponents();
        }

        /// <summary>
        /// Initialisiert alles, was im Template-XAML nicht gemacht werden kann (z.B. Event setzen).
        /// </summary>
        public void InitializeTemplateComponents()
        {
            InitializeTitleButtons();
            InitializeMainBorder();
            InitializeContentBorder();
        }

        /// <summary>
        /// Initialisiert die Buttons in der Titelleiste.
        /// </summary>
        public void InitializeTitleButtons()
        {
            if (GetTemplateChild("MaSch_TitleButtons") is WindowButtons titleButtons)
            {
                titleButtons.SetWindowState(WindowState);
                titleButtons.CloseButtonClicked += Close;
                titleButtons.MinimizeButtonClicked += () => WindowState = WindowState.Minimized;
                titleButtons.NormalizeButtonClicked += () => WindowState = WindowState.Normal;
                titleButtons.MaximizeButtonClicked += () => WindowState = WindowState.Maximized;
                StateChanged += (s, e) => titleButtons.SetWindowState(WindowState);
            }
        }

        /// <summary>
        /// Initialisiert das MainGrid.
        /// </summary>
        public void InitializeMainBorder()
        {
            if (GetTemplateChild("MaSch_MainBorder") is Border mainBorder)
            {
                mainBorder.Margin = WindowState == WindowState.Maximized ? new Thickness(5) : new Thickness(0);
                StateChanged += (s, e) =>
                {
                    mainBorder.Margin = WindowState == WindowState.Maximized ? new Thickness(5) : new Thickness(0);
                };
            }
        }

        /// <summary>
        /// Initialisiert das Content Grid.
        /// </summary>
        public void InitializeContentBorder()
        {
            if (GetTemplateChild("MaSch_ContentBorder") is Border contentBorder)
            {
                // StateChanged += (s, e) =>
                // {
                //     contentBorder.Margin = WindowState == WindowState.Maximized ? new Thickness(0, 30, 0, 0) : new Thickness(5, 35, 5, 5);
                // };
                contentBorder.MouseLeftButtonDown += (s, e) =>
                {
                    if (DragMoveOnContentBorder)
                        DragMove();
                };
            }
        }

        /// <inheritdoc/>
        WindowVisualState IWindow.WindowState
        {
            get => (WindowVisualState)(int)WindowState;
            set => WindowState = (WindowState)(int)value;
        }

        /// <inheritdoc/>
        double IWindow.Width
        {
            get => ActualWidth;
            set => Width = value;
        }

        /// <inheritdoc/>
        double IWindow.Height
        {
            get => ActualHeight;
            set => Height = value;
        }

        private void ModernUIWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var s = new Size(Math.Ceiling(e.NewSize.Width), Math.Ceiling(e.NewSize.Height));
            if (Math.Abs(s.Height - e.NewSize.Height) > 0.00001)
                Height = s.Height;
            else if (Math.Abs(s.Width - e.NewSize.Width) > 0.00001)
                Width = s.Width;
        }
    }
}
