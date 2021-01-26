using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    public class CornerRadiusConverter : IValueConverter
    {
        private readonly MathConverter _mathConverter = new MathConverter();

        public string TopLeftFormula { get; set; } = "TL";
        public string TopRightFormula { get; set; } = "TR";
        public string BottomLeftFormula { get; set; } = "BL";
        public string BottomRightFormula { get; set; } = "BR";
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is CornerRadius cornerRadius))
                return null;

            var values = new object[] { cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft };
            return new CornerRadius(
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(TopLeftFormula), culture),
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(TopRightFormula), culture),
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(BottomRightFormula), culture),
                (double)_mathConverter.Convert(values, typeof(double), ConvertExpression(BottomLeftFormula), culture));
        }

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
