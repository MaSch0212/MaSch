using MaSch.Common.Extensions;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    public class GridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = value.ConvertTo<double>();
            GridLength gridLength = new GridLength(val);

            return gridLength;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GridLength val = (GridLength)value;

            return val.Value.ConvertTo(targetType);
        }
    }
}
