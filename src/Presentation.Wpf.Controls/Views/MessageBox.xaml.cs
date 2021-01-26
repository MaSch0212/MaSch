using System;
using System.Media;
using System.Windows;
using MaSch.Presentation.Wpf.ViewModels;

namespace MaSch.Presentation.Wpf.Views
{
    internal partial class MessageBox
    {
        public MessageBoxResult MessageBoxResult { get; set; }
        private readonly double[] _steps = { 3D, 2D, 1.5D, 1.1D };
        private int _stepIndex;

        public MessageBox()
        {
            InitializeComponent();
            IsVisibleChanged += MessageBox_IsVisibleChanged;
            MessageBoxResult = MessageBoxResult.None;
            MaxHeight = SystemParameters.PrimaryScreenHeight * 0.9D;
            MaxWidth = SystemParameters.PrimaryScreenWidth / 4D;
            OneLineMessageBoxText.SizeChanged += MessageBoxContent_SizeChanged;
            Loaded += (s, e) =>
            {
                WindowState = WindowState.Normal;
                Activate();
            };
        }

        private void MessageBoxContent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Width = Math.Ceiling(30D + OneLineMessageBoxText.ActualWidth + 
                (IconPresenter.Visibility == Visibility.Visible ? (IconPresenter.ActualWidth + 50) : 0D));
            Height = Math.Ceiling(60D + ButtonRow.ActualHeight + Math.Max(MessageBoxContent.ActualHeight,
                (IconPresenter.Visibility == Visibility.Visible ? IconPresenter.ActualHeight : 0)));
            if(Height >= MaxHeight && _stepIndex < _steps.Length - 1)
            {
                MaxWidth = SystemParameters.PrimaryScreenWidth / _steps[++_stepIndex];
                MessageBoxContent_SizeChanged(sender, e);
            }
        }

        private void MessageBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if((bool)e.NewValue && DataContext is MessageBoxViewModel vm)
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
                        throw new ArgumentOutOfRangeException();
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
}
