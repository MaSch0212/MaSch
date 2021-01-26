using System;
using System.Globalization;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsEqualConverter : IValueConverter
    {
        private static readonly object UnsetValue = new object();

        public object ForcedParameter { get; set; } = UnsetValue;
        public bool CompareStringRepresentation { get; set; } = true;
        public bool CompareExactObject { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var otherValue = parameter;
            if (!ReferenceEquals(ForcedParameter, UnsetValue))
                otherValue = ForcedParameter;
            if (value == null)
                return otherValue == null;
            return CompareExactObject && value.Equals(otherValue) || 
                   CompareStringRepresentation && value.ToString().Equals(otherValue?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
            => throw new NotSupportedException();
    }
}
