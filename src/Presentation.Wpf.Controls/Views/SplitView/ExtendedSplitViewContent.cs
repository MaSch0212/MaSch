using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaSch.Core;
using MaSch.Presentation.Wpf.Controls;

namespace MaSch.Presentation.Wpf.Views.SplitView
{
    /// <summary>
    /// <see cref="SplitViewContent"/> with additional features.
    /// </summary>
    /// <seealso cref="MaSch.Presentation.Wpf.Views.SplitView.SplitViewContent" />
    public class ExtendedSplitViewContent : SplitViewContent
    {
        /// <summary>
        /// Dependency property. Gets or sets the success message icon.
        /// </summary>
        public static readonly DependencyProperty SuccessMessageIconProperty =
            DependencyProperty.Register(
                "SuccessMessageIcon",
                typeof(Icon),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the failure message icon.
        /// </summary>
        public static readonly DependencyProperty FailureMessageIconProperty =
            DependencyProperty.Register(
                "FailureMessageIcon",
                typeof(Icon),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the warning message icon.
        /// </summary>
        public static readonly DependencyProperty WarningMessageIconProperty =
            DependencyProperty.Register(
                "WarningMessageIcon",
                typeof(Icon),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the information message icon.
        /// </summary>
        public static readonly DependencyProperty InformationMessageIconProperty =
            DependencyProperty.Register(
                "InformationMessageIcon",
                typeof(Icon),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the brush for success messages.
        /// </summary>
        public static readonly DependencyProperty SuccessMessageBrushProperty =
            DependencyProperty.Register(
                "SuccessMessageBrush",
                typeof(Brush),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 192, 0))));

        /// <summary>
        /// Dependency property. Gets or sets the brush for failure messages.
        /// </summary>
        public static readonly DependencyProperty FailureMessageBrushProperty =
            DependencyProperty.Register(
                "FailureMessageBrush",
                typeof(Brush),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(192, 0, 0))));

        /// <summary>
        /// Dependency property. Gets or sets the brush for warning messages.
        /// </summary>
        public static readonly DependencyProperty WarningMessageBrushProperty =
            DependencyProperty.Register(
                "WarningMessageBrush",
                typeof(Brush),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        /// <summary>
        /// Dependency property. Gets or sets the brush for information messages.
        /// </summary>
        public static readonly DependencyProperty InformationMessageBrushProperty =
            DependencyProperty.Register(
                "InformationMessageBrush",
                typeof(Brush),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 128, 255))));

        /// <summary>
        /// Dependency property. Gets or sets the title of this page.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the content of the toolbar.
        /// </summary>
        public static readonly DependencyProperty ToolbarContentProperty =
            DependencyProperty.Register(
                "ToolbarContent",
                typeof(object),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the toolbar content template.
        /// </summary>
        public static readonly DependencyProperty ToolbarContentTemplateProperty =
            DependencyProperty.Register(
                "ToolbarContentTemplate",
                typeof(DataTemplate),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the end content of the toolbar.
        /// </summary>
        public static readonly DependencyProperty ToolbarEndContentProperty =
            DependencyProperty.Register(
                "ToolbarEndContent",
                typeof(object),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the toolbar end content template.
        /// </summary>
        public static readonly DependencyProperty ToolbarEndContentTemplateProperty =
            DependencyProperty.Register(
                "ToolbarEndContentTemplate",
                typeof(DataTemplate),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(null));

        /// <summary>
        /// Dependency property. Gets or sets the toolbar visibility.
        /// </summary>
        public static readonly DependencyProperty ToolbarVisibilityProperty =
            DependencyProperty.Register(
                "ToolbarVisibility",
                typeof(Visibility),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Dependency property. Gets or sets a value indicating whether this page is loading something.
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(
                "IsLoading",
                typeof(bool),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(false));

        /// <summary>
        /// Dependency property. Gets or sets the loading text.
        /// </summary>
        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register(
                "LoadingText",
                typeof(string),
                typeof(ExtendedSplitViewContent),
                new PropertyMetadata(string.Empty));

        private Storyboard? _showMessageStoryboard;
        private IconPresenter? _messageIcon;
        private TextBlock? _messageText;
        private FrameworkElement? _rootElement;

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
        /// Gets or sets the brush for success messages.
        /// </summary>
        public Brush SuccessMessageBrush
        {
            get => (Brush)GetValue(SuccessMessageBrushProperty);
            set => SetValue(SuccessMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush for failure messages.
        /// </summary>
        public Brush FailureMessageBrush
        {
            get => (Brush)GetValue(FailureMessageBrushProperty);
            set => SetValue(FailureMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush for warning messages.
        /// </summary>
        public Brush WarningMessageBrush
        {
            get => (Brush)GetValue(WarningMessageBrushProperty);
            set => SetValue(WarningMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush for information messages.
        /// </summary>
        public Brush InformationMessageBrush
        {
            get => (Brush)GetValue(InformationMessageBrushProperty);
            set => SetValue(InformationMessageBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the title of this page.
        /// </summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets the content of the toolbar.
        /// </summary>
        public object ToolbarContent
        {
            get => GetValue(ToolbarContentProperty);
            set => SetValue(ToolbarContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the toolbar content template.
        /// </summary>
        public DataTemplate ToolbarContentTemplate
        {
            get => (DataTemplate)GetValue(ToolbarContentTemplateProperty);
            set => SetValue(ToolbarContentTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the end content of the toolbar.
        /// </summary>
        public object ToolbarEndContent
        {
            get => GetValue(ToolbarEndContentProperty);
            set => SetValue(ToolbarEndContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the toolbar end content template.
        /// </summary>
        public DataTemplate ToolbarEndContentTemplate
        {
            get => (DataTemplate)GetValue(ToolbarEndContentTemplateProperty);
            set => SetValue(ToolbarEndContentTemplateProperty, value);
        }

        /// <summary>
        /// Gets or sets the toolbar visibility.
        /// </summary>
        public Visibility ToolbarVisibility
        {
            get => GetValue(ToolbarVisibilityProperty) as Visibility? ?? Visibility.Visible;
            set => SetValue(ToolbarVisibilityProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this page is loading something.
        /// </summary>
        public bool IsLoading
        {
            get => GetValue(IsLoadingProperty) as bool? ?? false;
            set => SetValue(IsLoadingProperty, value);
        }

        /// <summary>
        /// Gets or sets the loading text.
        /// </summary>
        public string LoadingText
        {
            get => (string)GetValue(LoadingTextProperty);
            set => SetValue(LoadingTextProperty, value);
        }

        static ExtendedSplitViewContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedSplitViewContent), new FrameworkPropertyMetadata(typeof(ExtendedSplitViewContent)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedSplitViewContent"/> class.
        /// </summary>
        public ExtendedSplitViewContent()
        {
            BindingOperations.SetBinding(this, IsLoadingProperty, new Binding(nameof(SplitViewContentViewModel.IsLoading)));
            BindingOperations.SetBinding(this, LoadingTextProperty, new Binding(nameof(SplitViewContentViewModel.LoadingText)));
            DataContextChanged += OnDataContextChanged;
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _messageIcon = GetTemplateChild("PART_MessageIcon") as IconPresenter;
            _messageText = GetTemplateChild("PART_MessageText") as TextBlock;
            _rootElement = GetTemplateChild("PART_Root") as FrameworkElement;
            Guard.NotNull(_messageIcon, nameof(_messageIcon));
            Guard.NotNull(_messageText, nameof(_messageText));
            Guard.NotNull(_rootElement, nameof(_rootElement));

            _showMessageStoryboard = _rootElement.Resources["ShowMessageStoryboard"] as Storyboard;
            Guard.NotNull(_showMessageStoryboard, nameof(_showMessageStoryboard));
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is SplitViewContentViewModel ovm)
                ovm.NewMessage -= ViewModel_NewMessage;
            if (e.NewValue is SplitViewContentViewModel nvm)
                nvm.NewMessage += ViewModel_NewMessage;
        }

        private void ViewModel_NewMessage(object? sender, Tuple<string, MessageType> e)
        {
            NotifyNewMessage(e.Item1, e.Item2);
        }

        /// <summary>
        /// Shows a new message to the user.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The message type.</param>
        public void NotifyNewMessage(string message, MessageType type)
        {
            Brush brush = new SolidColorBrush(Colors.Black);
            switch (type)
            {
                case MessageType.Success:
                    _messageIcon!.Icon = SuccessMessageIcon;
                    brush = SuccessMessageBrush;
                    break;
                case MessageType.Failure:
                    _messageIcon!.Icon = FailureMessageIcon;
                    brush = FailureMessageBrush;
                    break;
                case MessageType.Warning:
                    _messageIcon!.Icon = WarningMessageIcon;
                    brush = WarningMessageBrush;
                    break;
                case MessageType.Information:
                    _messageIcon!.Icon = InformationMessageIcon;
                    brush = InformationMessageBrush;
                    break;
            }

            _messageIcon!.Foreground = brush;
            _messageText!.Foreground = brush;
            _messageText.Text = message;

            _showMessageStoryboard!.Begin();
        }
    }
}
