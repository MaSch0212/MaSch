using System;
using System.Globalization;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that checks an object for equality.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsEqualConverter : IValueConverter
    {
        private static readonly object UnsetValue = new object();

        /// <summary>
        /// Gets or sets the forced parameter that is always used regardless of which parameter is given to this <see cref="IsEqualConverter"/>.
        /// </summary>
        public object ForcedParameter { get; set; } = UnsetValue;

        /// <summary>
        /// Gets or sets a value indicating whether both objects should be converted to strings before comparing them.
        /// </summary>
        public bool CompareStringRepresentation { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the reference should be compared. If set to <c>true</c> the <see cref="object.ReferenceEquals(object?, object?)"/> method is used instead of <see cref="object.Equals(object?)"/>.
        /// </summary>
        public bool CompareExactObject { get; set; } = true;

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var otherValue = parameter;
            if (!ReferenceEquals(ForcedParameter, UnsetValue))
                otherValue = ForcedParameter;
            if (value == null)
                return otherValue == null;
            return (CompareExactObject && value.Equals(otherValue)) ||
                   (CompareStringRepresentation && string.Equals(value.ToString(), otherValue?.ToString()));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
