using MaSch.Core;
using MaSch.Presentation.Wpf.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MaSch.Presentation.Wpf.ViewModels
{
    public class MessageBoxViewModel : INotifyPropertyChanged
    {
        private string _messageBoxText;
        public string MessageBoxText
        {
            get => _messageBoxText;
            set
            {
                _messageBoxText = value;
                NotifyPropertyChanged();
            }
        }

        private string _caption;
        public string Caption
        {
            get => _caption;
            set
            {
                _caption = value;
                NotifyPropertyChanged();
            }
        }

        private ButtonVisibilities _buttons;
        public ButtonVisibilities Buttons
        {
            get => _buttons;
            set
            {
                _buttons = value;
                NotifyPropertyChanged();
            }
        }
        public MessageBoxButton MessageBoxButtons
        {
            get => Buttons.GetMessageBoxButton() ?? MessageBoxButton.OK;
            set => Buttons = new ButtonVisibilities(value);
        }

        public static IThemeManager CurrentThemeManager { get; }
        public Dictionary<int, BrushGeometry> IconDict = new Dictionary<int, BrushGeometry>
        {
            { 0, new BrushGeometry() },
            { 16, new BrushGeometry{ Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxErrorIcon) as Icon, FillBrush = new SolidColorBrush(Colors.Red) } },
            { 32, new BrushGeometry{ Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxQuestionIcon) as Icon, FillBrush = new SolidColorBrush(Color.FromArgb(255, 25, 88, 185 )) } },
            { 48, new BrushGeometry{ Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxWarningIcon) as Icon, FillBrush = new SolidColorBrush(Colors.Orange) } },
            { 64, new BrushGeometry{ Icon = CurrentThemeManager.GetValue<Icon>(ThemeKey.MessageBoxInfoIcon) as Icon, FillBrush = new SolidColorBrush(Color.FromArgb(255, 25, 88, 185 )) } },
        };
        private BrushGeometry _icon;
        public BrushGeometry Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                NotifyPropertyChanged();
            }
        }
        public MessageBoxImage MessageBoxImage
        {
            get
            {
                if (!IconDict.ContainsValue(Icon))
                    return MessageBoxImage.None;
                else
                    return (MessageBoxImage)IconDict.FirstOrDefault(i => i.Value.Equals(Icon)).Key;
            }
            set => Icon = IconDict.ContainsKey((int)value) ? IconDict[(int)value] : IconDict.FirstOrDefault().Value;
        }

        public MessageBoxResult DefaultResult { get; set; }

        static MessageBoxViewModel()
        {
            CurrentThemeManager = ServiceContext.TryGetService<IThemeManager>() ?? ThemeManager.DefaultThemeManager;
        }

        public class BrushGeometry : INotifyPropertyChanged
        {
            private Brush _fillbrush = new SolidColorBrush(Colors.Black);
            public Brush FillBrush
            {
                get => _fillbrush;
                set
                {
                    _fillbrush = value;
                    NotifyPropertyChanged();
                }
            }

            private Brush _strokeBrush = new SolidColorBrush(Colors.Transparent);
            public Brush StrokeBrush
            {
                get => _strokeBrush;
                set
                {
                    _strokeBrush = value;
                    NotifyPropertyChanged();
                }
            }
            
            private Icon _icon;
            public Icon Icon
            {
                get => _icon;
                set
                {
                    _icon = value;
                    NotifyPropertyChanged();
                }
            }

            #region INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }

        public class ButtonVisibilities : INotifyPropertyChanged
        {
            private Visibility _ok = Visibility.Collapsed;
            public Visibility Ok
            {
                get => _ok;
                set
                {
                    _ok = value;
                    NotifyPropertyChanged();
                }
            }

            private Visibility _cancel = Visibility.Collapsed;
            public Visibility Cancel
            {
                get => _cancel;
                set
                {
                    _cancel = value;
                    NotifyPropertyChanged();
                }
            }

            private Visibility _yes = Visibility.Collapsed;
            public Visibility Yes
            {
                get => _yes;
                set
                {
                    _yes = value;
                    NotifyPropertyChanged();
                }
            }

            private Visibility _no = Visibility.Collapsed;
            public Visibility No
            {
                get => _no;
                set
                {
                    _no = value;
                    NotifyPropertyChanged();
                }
            }

            public MessageBoxButton? GetMessageBoxButton()
            {
                Visibility[] visibilities = { Ok, Cancel, Yes, No };
                StringBuilder sb = new StringBuilder(4);
                foreach (var vis in visibilities)
                    sb.Append(vis == Visibility.Visible ? "1" : "0");
                string s = sb.ToString();
                if (s == "1000")
                    return MessageBoxButton.OK;
                if (s == "1100")
                    return MessageBoxButton.OKCancel;
                if (s == "0011")
                    return MessageBoxButton.YesNo;
                if (s == "0111")
                    return MessageBoxButton.YesNoCancel;
                return null;
            }

            public ButtonVisibilities(MessageBoxButton msgButton)
            {
                switch (msgButton)
                {
                    case MessageBoxButton.OK:
                        Ok = Visibility.Visible;
                        break;
                    case MessageBoxButton.OKCancel:
                        Ok = Visibility.Visible;
                        Cancel = Visibility.Visible;
                        break;
                    case MessageBoxButton.YesNo:
                        Yes = Visibility.Visible;
                        No = Visibility.Visible;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        Yes = Visibility.Visible;
                        No = Visibility.Visible;
                        Cancel = Visibility.Visible;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(msgButton), msgButton, null);
                }
            }

            #region INotifyPropertyChanged

            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
