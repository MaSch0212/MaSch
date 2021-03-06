﻿using MaSch.Core;
using MaSch.Core.Attributes;
using MaSch.Core.Observable;
using MaSch.Presentation.Wpf.Themes;
using MaSch.Presentation.Wpf.ViewModels.MessageBox;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ViewModels
{
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
    /// <seealso cref="MaSch.Core.Observable.ObservableObject" />
    /// <seealso cref="MaSch.Presentation.Wpf.ViewModels.IMessageBoxViewModelProps" />
    public partial class MessageBoxViewModel : ObservableObject, IMessageBoxViewModelProps
    {
        private static readonly Dictionary<int, Func<BrushGeometry>> IconDict = new()
        {
            [0] = () => new BrushGeometry(),
            [16] = () => new BrushGeometry
            {
                Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxErrorIcon) as Icon,
                FillBrush = new SolidColorBrush(Colors.Red),
            },
            [32] = () => new BrushGeometry
            {
                Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxQuestionIcon) as Icon,
                FillBrush = new SolidColorBrush(Color.FromArgb(255, 25, 88, 185)),
            },
            [48] = () => new BrushGeometry
            {
                Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxWarningIcon) as Icon,
                FillBrush = new SolidColorBrush(Colors.Orange),
            },
            [64] = () => new BrushGeometry
            {
                Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxInfoIcon) as Icon,
                FillBrush = new SolidColorBrush(Color.FromArgb(255, 25, 88, 185)),
            },
        };

        private static IThemeManager CurrentThemeManager => ServiceContext.TryGetService<IThemeManager>() ?? ThemeManager.DefaultThemeManager;

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
                Icon = IconDict.ContainsKey((int)value) ? IconDict[(int)value]() : IconDict.FirstOrDefault().Value();
                _messageBoxImage = value;
            }
        }

        /// <summary>
        /// Gets or sets the default result.
        /// </summary>
        public MessageBoxResult DefaultResult { get; set; }
    }
}
