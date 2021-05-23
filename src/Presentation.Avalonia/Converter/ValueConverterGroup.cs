using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MaSch.Presentation.Avalonia.Converter
{
    /// <summary>
    /// Groups together multiple <see cref="IValueConverter"/>s The value converters are executed in order and the results of each are passed in as parameter to the next.
    /// </summary>
    /// <seealso cref="List{T}" />
    /// <seealso cref="IValueConverter" />
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Reverse<IValueConverter>().Aggregate(value, (current, converter) => converter.ConvertBack(current, targetType, parameter, culture));
        }
    }
}
