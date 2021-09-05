using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that calculates a new <see cref="CornerRadius"/> from another <see cref="CornerRadius"/> using formula for each corner.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class CornerRadiusConverter : IValueConverter
    {
        private readonly MathConverter _mathConverter = new();

        /// <summary>
        /// Gets or sets the formula to calculate the top left value of the corner radius.
        /// Use the following placeholders to reference the positions from the original <see cref="CornerRadius"/>: TopLeft=TL; TopRight=TR; BottomLeft=BL; BottomRight=BR.
        /// </summary>
        public string TopLeftFormula { get; set; } = "TL";

        /// <summary>
        /// Gets or sets the formula to calculate the top right value of the corner radius.
        /// Use the following placeholders to reference the positions from the original <see cref="CornerRadius"/>: TopLeft=TL; TopRight=TR; BottomLeft=BL; BottomRight=BR.
        /// </summary>
        public string TopRightFormula { get; set; } = "TR";

        /// <summary>
        /// Gets or sets the formula to calculate the bottom left value of the corner radius.
        /// Use the following placeholders to reference the positions from the original <see cref="CornerRadius"/>: TopLeft=TL; TopRight=TR; BottomLeft=BL; BottomRight=BR.
        /// </summary>
        public string BottomLeftFormula { get; set; } = "BL";

        /// <summary>
        /// Gets or sets the formula to calculate the bottom right value of the corner radius.
        /// Use the following placeholders to reference the positions from the original <see cref="CornerRadius"/>: TopLeft=TL; TopRight=TR; BottomLeft=BL; BottomRight=BR.
        /// </summary>
        public string BottomRightFormula { get; set; } = "BR";

        /// <inheritdoc />
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not CornerRadius cornerRadius)
                return null;

            var values = new object[] { cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft };
            return new CornerRadius(
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(TopLeftFormula), culture),
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(TopRightFormula), culture),
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(BottomRightFormula), culture),
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(BottomLeftFormula), culture));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static string ConvertExpression(string expression)
        {
            return expression
                .Replace("BL", "d")
                .Replace("BR", "c")
                .Replace("TR", "b")
                .Replace("TL", "a");
        }
    }
}
