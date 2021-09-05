using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MaSch.Presentation.Wpf.ColorPicker
{
    /// <summary>
    /// A control to select a color spectrum.
    /// </summary>
    /// <seealso cref="Slider" />
    public class SpectrumSlider : Slider
    {
        /// <summary>
        /// Dependency property. Gets or sets the selected color.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                "SelectedColor",
                typeof(Color),
                typeof(SpectrumSlider),
                new PropertyMetadata(Colors.Red));

        private const string SpectrumDisplayName = "PART_SpectrumDisplay";
        private Rectangle? _spectrumDisplay;

        static SpectrumSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpectrumSlider), new FrameworkPropertyMetadata(typeof(SpectrumSlider)));
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        public Color SelectedColor
        {
            get => GetValue(SelectedColorProperty) as Color? ?? Colors.Red;
            set => SetValue(SelectedColorProperty, value);
        }

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _spectrumDisplay = GetTemplateChild(SpectrumDisplayName) as Rectangle ?? throw new InvalidOperationException($"Control with name \"{SpectrumDisplayName}\" was not found.");
            UpdateColorSpectrum();
            OnValueChanged(double.NaN, Value);
        }

        /// <inheritdoc/>
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Mouse.Captured == null)
                    _ = Mouse.Capture(this);
                OnPreviewMouseLeftButtonDown(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left)
                {
                    RoutedEvent = e.RoutedEvent,
                });
            }
            else if (Mouse.Captured != null)
            {
                _ = Mouse.Capture(null);
            }
        }

        /// <inheritdoc/>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            var theColor = ColorUtilities.ConvertHsvToRgb(360 - newValue, 1, 1);
            SetValue(SelectedColorProperty, theColor);
        }

        private void UpdateColorSpectrum()
        {
            if (_spectrumDisplay != null)
            {
                CreateSpectrum();
            }
        }

        private void CreateSpectrum()
        {
            var pickerBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
                ColorInterpolationMode = ColorInterpolationMode.SRgbLinearInterpolation,
            };

            var colorsList = ColorUtilities.GenerateHsvSpectrum();
            var stopIncrement = 1D / colorsList.Count;

            int i;
            for (i = 0; i < colorsList.Count; i++)
            {
                pickerBrush.GradientStops.Add(new GradientStop(colorsList[i], i * stopIncrement));
            }

            pickerBrush.GradientStops[i - 1].Offset = 1.0;
            _spectrumDisplay!.Fill = pickerBrush;
        }
    }
}
