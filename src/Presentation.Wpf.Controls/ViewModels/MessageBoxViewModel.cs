using MaSch.Core;
using MaSch.Core.Observable;
using MaSch.Presentation.Wpf.Themes;
using MaSch.Presentation.Wpf.ThemeValues;
using MaSch.Presentation.Wpf.ViewModels.MessageBox;
using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ViewModels;

/// <summary>
/// Observable properties of the <see cref="MessageBoxViewModel"/> class.
/// </summary>
[ObservablePropertyDefinition]
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Property definition interface should be first in file.")]
internal interface IMessageBoxViewModelProps
{
    /// <summary>
    /// Gets or sets the message box text.
    /// </summary>
    string? MessageBoxText { get; set; }

    /// <summary>
    /// Gets or sets the caption of the message box.
    /// </summary>
    string? Caption { get; set; }

    /// <summary>
    /// Gets or sets the button state of the message box.
    /// </summary>
    ButtonVisibilities? Buttons { get; set; }

    /// <summary>
    /// Gets or sets the icon of the message box.
    /// </summary>
    BrushGeometry? Icon { get; set; }
}

/// <summary>
/// View model for the <see cref="Views.MessageBox"/>.
/// </summary>
/// <seealso cref="ObservableObject" />
/// <seealso cref="IMessageBoxViewModelProps" />
public partial class MessageBoxViewModel : ObservableObject, IMessageBoxViewModelProps
{
    private static readonly BrushGeometry EmptyIcon = new();

    private MessageBoxImage _messageBoxImage = MessageBoxImage.None;

    /// <summary>
    /// Gets or sets the message box buttons.
    /// </summary>
    public MessageBoxButton MessageBoxButtons
    {
        get => Buttons?.GetMessageBoxButton() ?? MessageBoxButton.OK;
        set => Buttons = new ButtonVisibilities(value);
    }

    /// <summary>
    /// Gets or sets the message box image.
    /// </summary>
    public MessageBoxImage MessageBoxImage
    {
        get => _messageBoxImage;
        set
        {
            Icon = GetIcon(value);
            _messageBoxImage = value;
        }
    }

    /// <summary>
    /// Gets or sets the default result.
    /// </summary>
    public MessageBoxResult DefaultResult { get; set; }

    private static IThemeManager CurrentThemeManager => ServiceContext.TryGetService<IThemeManager>() ?? ThemeManager.DefaultThemeManager;

    private static BrushGeometry GetIcon(MessageBoxImage image)
    {
        return image switch
        {
            MessageBoxImage.Error => CreateResult(ThemeKey.MessageBoxErrorIcon, ThemeKey.MessageBoxErrorIconBrush),
            MessageBoxImage.Question => CreateResult(ThemeKey.MessageBoxQuestionIcon, ThemeKey.MessageBoxQuestionIconBrush),
            MessageBoxImage.Exclamation => CreateResult(ThemeKey.MessageBoxWarningIcon, ThemeKey.MessageBoxWarningIconBrush),
            MessageBoxImage.Asterisk => CreateResult(ThemeKey.MessageBoxInfoIcon, ThemeKey.MessageBoxInfoIconBrush),
            _ => EmptyIcon,
        };

        static BrushGeometry CreateResult(ThemeKey iconThemeKey, ThemeKey iconBrushKey)
        {
            return new BrushGeometry
            {
                Icon = (CurrentThemeManager.GetValue<Icon>(iconThemeKey) as IconThemeValue)?.Value,
                FillBrush = (CurrentThemeManager.GetValue<SolidColorBrush>(iconBrushKey) as SolidColorBrushThemeValue)?.Value,
            };
        }
    }

    partial void OnMessageBoxTextChanging(string? previous, ref string? value)
    {
        value = value?.Trim();
    }
}
