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
    public class ExtendedSplitViewContent : SplitViewContent
    {
        #region Fields
        private Storyboard _showMessageStoryboard;
        private IconPresenter _messageIcon;
        private TextBlock _messageText;
        private FrameworkElement _rootElement;
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty SuccessMessageIconProperty =
            DependencyProperty.Register("SuccessMessageIcon", typeof(Icon), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty FailureMessageIconProperty =
            DependencyProperty.Register("FailureMessageIcon", typeof(Icon), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty WarningMessageIconProperty =
            DependencyProperty.Register("WarningMessageIcon", typeof(Icon), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty InformationMessageIconProperty =
            DependencyProperty.Register("InformationMessageIcon", typeof(Icon), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty SuccessMessageBrushProperty =
            DependencyProperty.Register("SuccessMessageBrush", typeof(Brush), typeof(ExtendedSplitViewContent), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 192, 0))));
        public static readonly DependencyProperty FailureMessageBrushProperty =
            DependencyProperty.Register("FailureMessageBrush", typeof(Brush), typeof(ExtendedSplitViewContent), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(192, 0, 0))));
        public static readonly DependencyProperty WarningMessageBrushProperty =
            DependencyProperty.Register("WarningMessageBrush", typeof(Brush), typeof(ExtendedSplitViewContent), new PropertyMetadata(new SolidColorBrush(Colors.Orange)));
        public static readonly DependencyProperty InformationMessageBrushProperty =
            DependencyProperty.Register("InformationMessageBrush", typeof(Brush), typeof(ExtendedSplitViewContent), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 128, 255))));
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty ToolbarContentProperty =
            DependencyProperty.Register("ToolbarContent", typeof(object), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty ToolbarContentTemplateProperty =
            DependencyProperty.Register("ToolbarContentTemplate", typeof(DataTemplate), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty ToolbarEndContentProperty =
            DependencyProperty.Register("ToolbarEndContent", typeof(object), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty ToolbarEndContentTemplateProperty =
            DependencyProperty.Register("ToolbarEndContentTemplate", typeof(DataTemplate), typeof(ExtendedSplitViewContent), new PropertyMetadata(null));
        public static readonly DependencyProperty ToolbarVisibilityProperty =
            DependencyProperty.Register("ToolbarVisibility", typeof(Visibility), typeof(ExtendedSplitViewContent), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(ExtendedSplitViewContent), new PropertyMetadata(false));
        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register("LoadingText", typeof(string), typeof(ExtendedSplitViewContent), new PropertyMetadata(""));
        #endregion
        
        #region Properties
        public Icon SuccessMessageIcon
        {
            get => (Icon)GetValue(SuccessMessageIconProperty);
            set => SetValue(SuccessMessageIconProperty, value);
        }
        public Icon FailureMessageIcon
        {
            get => (Icon)GetValue(FailureMessageIconProperty);
            set => SetValue(FailureMessageIconProperty, value);
        }
        public Icon WarningMessageIcon
        {
            get => (Icon)GetValue(WarningMessageIconProperty);
            set => SetValue(WarningMessageIconProperty, value);
        }
        public Icon InformationMessageIcon
        {
            get => (Icon)GetValue(InformationMessageIconProperty);
            set => SetValue(InformationMessageIconProperty, value);
        }
        public Brush SuccessMessageBrush
        {
            get => (Brush)GetValue(SuccessMessageBrushProperty);
            set => SetValue(SuccessMessageBrushProperty, value);
        }
        public Brush FailureMessageBrush
        {
            get => (Brush)GetValue(FailureMessageBrushProperty);
            set => SetValue(FailureMessageBrushProperty, value);
        }
        public Brush WarningMessageBrush
        {
            get => (Brush)GetValue(WarningMessageBrushProperty);
            set => SetValue(WarningMessageBrushProperty, value);
        }
        public Brush InformationMessageBrush
        {
            get => (Brush)GetValue(InformationMessageBrushProperty);
            set => SetValue(InformationMessageBrushProperty, value);
        }
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public object ToolbarContent
        {
            get => GetValue(ToolbarContentProperty);
            set => SetValue(ToolbarContentProperty, value);
        }
        public DataTemplate ToolbarContentTemplate
        {
            get => (DataTemplate)GetValue(ToolbarContentTemplateProperty);
            set => SetValue(ToolbarContentTemplateProperty, value);
        }
        public object ToolbarEndContent
        {
            get => GetValue(ToolbarEndContentProperty);
            set => SetValue(ToolbarEndContentProperty, value);
        }
        public DataTemplate ToolbarEndContentTemplate
        {
            get => (DataTemplate)GetValue(ToolbarEndContentTemplateProperty);
            set => SetValue(ToolbarEndContentTemplateProperty, value);
        }
        public Visibility ToolbarVisibility
        {
            get => GetValue(ToolbarVisibilityProperty) as Visibility? ?? Visibility.Visible;
            set => SetValue(ToolbarVisibilityProperty, value);
        }
        public bool IsLoading
        {
            get => GetValue(IsLoadingProperty) as bool? ?? false;
            set => SetValue(IsLoadingProperty, value);
        }
        public string LoadingText
        {
            get => (string)GetValue(LoadingTextProperty);
            set => SetValue(LoadingTextProperty, value);
        }
        #endregion

        static ExtendedSplitViewContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedSplitViewContent), new FrameworkPropertyMetadata(typeof(ExtendedSplitViewContent)));
        }

        public ExtendedSplitViewContent()
        {
            BindingOperations.SetBinding(this, IsLoadingProperty, new Binding(nameof(SplitViewContentViewModel.IsLoading)));
            BindingOperations.SetBinding(this, LoadingTextProperty, new Binding(nameof(SplitViewContentViewModel.LoadingText)));
            DataContextChanged += OnDataContextChanged;
        }

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

        private void ViewModel_NewMessage(object sender, Tuple<string, MessageType> e)
        {
            NotifyNewMessage(e.Item1, e.Item2);
        }

        public void NotifyNewMessage(string message, MessageType type)
        {
            Brush brush = new SolidColorBrush(Colors.Black);
            switch (type)
            {
                case MessageType.Success:
                    _messageIcon.Icon = SuccessMessageIcon;
                    brush = SuccessMessageBrush;
                    break;
                case MessageType.Failure:
                    _messageIcon.Icon = FailureMessageIcon;
                    brush = FailureMessageBrush;
                    break;
                case MessageType.Warning:
                    _messageIcon.Icon = WarningMessageIcon;
                    brush = WarningMessageBrush;
                    break;
                case MessageType.Information:
                    _messageIcon.Icon = InformationMessageIcon;
                    brush = InformationMessageBrush;
                    break;
            }
            _messageIcon.Foreground = brush;
            _messageText.Foreground = brush;
            _messageText.Text = message;

            _showMessageStoryboard.Begin();
        }
    }
}
