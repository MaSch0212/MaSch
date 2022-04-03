using MaSch.Presentation.Wpf.ViewModels;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.Views;

/// <summary>
/// MessageBox that is styled using MaSch.Presentation.Wpf.Themes.
/// </summary>
/// <seealso cref="Controls.Window" />
/// <seealso cref="System.Windows.Markup.IComponentConnector" />
internal partial class MessageBox
{
    private readonly double[] _steps = { 4D, 3D, 2D, 1.5D, 1.1D };

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBox"/> class.
    /// </summary>
    public MessageBox(MessageBoxViewModel viewModel)
    {
        InitializeComponent();
        IsVisibleChanged += MessageBox_IsVisibleChanged;
        MessageBoxResult = MessageBoxResult.None;
        MaxHeight = SystemParameters.PrimaryScreenHeight * 0.9D;
        MaxWidth = SystemParameters.PrimaryScreenWidth / 4D;
        DataContext = viewModel;
        Loaded += (s, e) =>
        {
            UpdateSize(true);
            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MessageBoxViewModel.Icon) || e.PropertyName == nameof(MessageBoxViewModel.MessageBoxText))
                    UpdateSize(false);
            };

            WindowState = WindowState.Normal;
            _ = Activate();
        };
    }

    /// <summary>
    /// Gets or sets the message box result.
    /// </summary>
    public MessageBoxResult MessageBoxResult { get; set; }

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:Field names should not use Hungarian notation", Justification = "x/y is fine.")]
    private void UpdateSize(bool updateLocation)
    {
        var formatted = new FormattedText(
            ((MessageBoxViewModel)DataContext!).MessageBoxText,
            CultureInfo.CurrentUICulture,
            MessageBoxContent.FlowDirection,
            new Typeface(MessageBoxContent.FontFamily, MessageBoxContent.FontStyle, MessageBoxContent.FontWeight, MessageBoxContent.FontStretch),
            MessageBoxContent.FontSize,
            MessageBoxContent.Foreground,
            VisualTreeHelper.GetDpi(MessageBoxContent).PixelsPerDip);

        double maxWidth = MaxWidth;
        var xMargin = ActualWidth - MessageBoxContent.ActualWidth + MessageBoxContent.Padding.Left + MessageBoxContent.Padding.Right;
        var yMargin = ActualHeight - MessageBoxContent.ActualHeight + MessageBoxContent.Padding.Top + MessageBoxContent.Padding.Bottom;
        var maxTextHeight = MaxHeight - yMargin;
        var rawTextWidth = formatted.Width;

        int stepIndex;
        for (stepIndex = 0; stepIndex < _steps.Length; stepIndex++)
        {
            maxWidth = SystemParameters.PrimaryScreenWidth / _steps[stepIndex];
            formatted.MaxTextWidth = maxWidth - xMargin;
            if (formatted.Height <= maxTextHeight && (formatted.Width > formatted.Height || formatted.Width == rawTextWidth))
                break;
        }

        MaxWidth = Math.Ceiling(maxWidth);
        Width = Math.Ceiling(formatted.Width + xMargin);
        Height = Math.Ceiling(formatted.Height + yMargin);

        MessageBoxContent.VerticalScrollBarVisibility = stepIndex >= _steps.Length ? ScrollBarVisibility.Auto : ScrollBarVisibility.Disabled;

        if (updateLocation)
        {
            var currentScreen = Screen.FromHandle(new WindowInteropHelper(this).Handle);
            Top = (currentScreen.WorkingArea.Height - Height) / 2;
            Left = (currentScreen.WorkingArea.Width - Width) / 2;
        }
    }

    private void MessageBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue && DataContext is MessageBoxViewModel vm)
        {
            switch (vm.MessageBoxImage)
            {
                case MessageBoxImage.Error: // Hand and Stop, too
                    SystemSounds.Hand.Play();
                    break;
                case MessageBoxImage.Question:
                    SystemSounds.Question.Play();
                    break;
                case MessageBoxImage.Exclamation: // Warning, too
                    SystemSounds.Exclamation.Play();
                    break;
                case MessageBoxImage.Information: // Asterisk, too
                    SystemSounds.Asterisk.Play();
                    break;
                case MessageBoxImage.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"The message box image \"{vm.MessageBoxImage}\" is unknown.");
            }
        }
    }

    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult = MessageBoxResult.OK;
        Close();
    }

    private void YesButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult = MessageBoxResult.Yes;
        Close();
    }

    private void NoButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult = MessageBoxResult.No;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult = MessageBoxResult.Cancel;
        Close();
    }
}
