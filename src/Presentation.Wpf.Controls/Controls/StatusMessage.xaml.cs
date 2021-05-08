using MaSch.Core;
using MaSch.Core.Attributes;
using MaSch.Presentation.Wpf.Services;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#pragma warning disable SA1649 // File name should match first type name

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Property Definition for <see cref="StatusMessage"/>.
    /// </summary>
    [ObservablePropertyDefinition]
    internal interface IStatusMessage_Props
    {
        /// <summary>
        /// Gets a value indicating whether this control is showing a loading indicator.
        /// </summary>
        [ObservablePropertyAccessModifier(SetModifier = AccessModifier.Private)]
        public bool IsLoading { get; }

        /// <summary>
        /// Gets the current loading text.
        /// </summary>
        [ObservablePropertyAccessModifier(SetModifier = AccessModifier.Private)]
        public string? LoadingText { get; }

        /// <summary>
        /// Gets the current status icon.
        /// </summary>
        [ObservablePropertyAccessModifier(SetModifier = AccessModifier.Private)]
        public Icon? StatusIcon { get; }

        /// <summary>
        /// Gets the current status brush.
        /// </summary>
        [ObservablePropertyAccessModifier(SetModifier = AccessModifier.Private)]
        public Brush? StatusBrush { get; }

        /// <summary>
        /// Gets the current status text.
        /// </summary>
        [ObservablePropertyAccessModifier(SetModifier = AccessModifier.Private)]
        public string? StatusText { get; }
    }

    /// <summary>
    /// Control that displays status messages.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    [GenerateObservableObject]
    public partial class StatusMessage : Control, IStatusMessage_Props
    {
        /// <summary>
        /// Dependency property. Gets or sets the success message icon.
        /// </summary>
        public static readonly DependencyProperty SuccessMessageIconProperty =
            DependencyProperty.Register(
                "SuccessMessageIcon",
                typeof(Icon),
                typeof(StatusMessage),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the failure message icon.
        /// </summary>
        public static readonly DependencyProperty FailureMessageIconProperty =
            DependencyProperty.Register(
                "FailureMessageIcon",
                typeof(Icon),
                typeof(StatusMessage),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the warning message icon.
        /// </summary>
        public static readonly DependencyProperty WarningMessageIconProperty =
            DependencyProperty.Register(
                "WarningMessageIcon",
                typeof(Icon),
                typeof(StatusMessage),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the information message icon.
        /// </summary>
        public static readonly DependencyProperty InformationMessageIconProperty =
            DependencyProperty.Register(
                "InformationMessageIcon",
                typeof(Icon),
                typeof(StatusMessage),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the brush for success messages.
        /// </summary>
        public static readonly DependencyProperty SuccessMessageBrushProperty =
            DependencyProperty.Register(
                "SuccessMessageBrush",
                typeof(Brush),
                typeof(StatusMessage),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 192, 0))));

        /// <summary>
        /// Dependency property. Gets or sets the brush for failure messages.
        /// </summary>
        public static readonly DependencyProperty FailureMessageBrushProperty =
            DependencyProperty.Register(
                "FailureMessageBrush",
                typeof(Brush),
                typeof(StatusMessage),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(192, 0, 0))));

        /// <summary>
        /// Dependency property. Gets or sets the brush for warning messages.
        /// </summary>
        public static readonly DependencyProperty WarningMessageBrushProperty =
            DependencyProperty.Register(
                "WarningMessageBrush",
                typeof(Brush),
                typeof(StatusMessage),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        /// <summary>
        /// Dependency property. Gets or sets the brush for information messages.
        /// </summary>
        public static readonly DependencyProperty InformationMessageBrushProperty =
            DependencyProperty.Register(
                "InformationMessageBrush",
                typeof(Brush),
                typeof(StatusMessage),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 128, 255))));

        /// <summary>
        /// Dependency property. Gets or sets the status icon padding.
        /// </summary>
        public static readonly DependencyProperty StatusIconPaddingProperty =
            DependencyProperty.Register(
                "StatusIconPadding",
                typeof(Thickness),
                typeof(StatusMessage),
                new PropertyMetadata(new Thickness(8)));

        /// <summary>
        /// Dependency property. Gets or sets the service that controls this control.
        /// </summary>
        public static readonly DependencyProperty ServiceProperty =
            DependencyProperty.Register(
                "Service",
                typeof(IStatusMessageService),
                typeof(StatusMessage),
                new PropertyMetadata(null, OnServiceChanged));

        private FrameworkElement? _rootElement;
        private Storyboard? _showMessageStoryboard;

        /// <summary>
        /// Gets or sets the success message icon.
        /// </summary>
        public Icon SuccessMessageIcon
        {
            get => (Icon)GetValue(SuccessMessageIconProperty);
            set => SetValue(SuccessMessageIconProperty, value);
        }

        /// <summary>
        /// Gets or sets the failure message icon.
        /// </summary>
        public Icon FailureMessageIcon
        {
            get => (Icon)GetValue(FailureMessageIconProperty);
            set => SetValue(FailureMessageIconProperty, value);
        }

        /// <summary>
        /// Gets or sets the warning message icon.
        /// </summary>
        public Icon WarningMessageIcon
        {
            get => (Icon)GetValue(WarningMessageIconProperty);
            set => SetValue(WarningMessageIconProperty, value);
        }

        /// <summary>
        /// Gets or sets the information message icon.
        /// </summary>
        public Icon InformationMessageIcon
        {
            get => (Icon)GetValue(InformationMessageIconProperty);
            set => SetValue(InformationMessageIconProperty, value);
        }

        /// <summary>
        /// Gets or sets the success message brush.
        /// </summary>
        public Brush SuccessMessageBrush
        {
            get => (Brush)GetValue(SuccessMessageBrushProperty);
            set => SetValue(SuccessMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the failure message brush.
        /// </summary>
        public Brush FailureMessageBrush
        {
            get => (Brush)GetValue(FailureMessageBrushProperty);
            set => SetValue(FailureMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the warning message brush.
        /// </summary>
        public Brush WarningMessageBrush
        {
            get => (Brush)GetValue(WarningMessageBrushProperty);
            set => SetValue(WarningMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the information message brush.
        /// </summary>
        public Brush InformationMessageBrush
        {
            get => (Brush)GetValue(InformationMessageBrushProperty);
            set => SetValue(InformationMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the status icon padding.
        /// </summary>
        public Thickness StatusIconPadding
        {
            get { return (Thickness)GetValue(StatusIconPaddingProperty); }
            set { SetValue(StatusIconPaddingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the service that controls this control.
        /// </summary>
        public IStatusMessageService Service
        {
            get { return (IStatusMessageService)GetValue(ServiceProperty); }
            set { SetValue(ServiceProperty, value); }
        }

        static StatusMessage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusMessage), new FrameworkPropertyMetadata(typeof(StatusMessage)));
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootElement = Guard.OfType<FrameworkElement>(GetTemplateChild("PART_Root"), "PART_Root");
            _showMessageStoryboard = Guard.OfType<Storyboard>(_rootElement.Resources["ShowMessageStoryboard"], "ShowMessageStoryboard");
        }

        /// <summary>
        /// Called when the <see cref="Service"/> property changed.
        /// </summary>
        /// <param name="oldService">The old service.</param>
        /// <param name="newService">The new service.</param>
        protected void OnServiceChanged(IStatusMessageService? oldService, IStatusMessageService? newService)
        {
            if (oldService != null)
                oldService.StatusChanged -= OnStatusChanged;
            if (newService != null)
                newService.StatusChanged += OnStatusChanged;
        }

        private void OnStatusChanged(object sender, StatusChangedEventArgs e)
        {
            _showMessageStoryboard!.SkipToFill();
            switch (e.Status)
            {
                case StatusType.None:
                    (IsLoading, LoadingText, StatusIcon, StatusBrush, StatusText) = (false, null, null, null, null);
                    break;
                case StatusType.Loading:
                    (IsLoading, LoadingText, StatusIcon, StatusBrush, StatusText) = (true, e.Text, null, null, null);
                    break;
                case StatusType.Information:
                    (IsLoading, LoadingText, StatusIcon, StatusBrush, StatusText) = (false, null, InformationMessageIcon, InformationMessageBrush, e.Text);
                    _showMessageStoryboard.Begin();
                    break;
                case StatusType.Success:
                    (IsLoading, LoadingText, StatusIcon, StatusBrush, StatusText) = (false, null, SuccessMessageIcon, SuccessMessageBrush, e.Text);
                    _showMessageStoryboard.Begin();
                    break;
                case StatusType.Warning:
                    (IsLoading, LoadingText, StatusIcon, StatusBrush, StatusText) = (false, null, WarningMessageIcon, WarningMessageBrush, e.Text);
                    _showMessageStoryboard.Begin();
                    break;
                case StatusType.Error:
                    (IsLoading, LoadingText, StatusIcon, StatusBrush, StatusText) = (false, null, FailureMessageIcon, FailureMessageBrush, e.Text);
                    _showMessageStoryboard.Begin();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(e.Status));
            }
        }

        private static void OnServiceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is StatusMessage sm)
                sm.OnServiceChanged(e.OldValue as IStatusMessageService, e.NewValue as IStatusMessageService);
        }
    }
}
