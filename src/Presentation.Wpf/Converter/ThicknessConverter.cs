using MaSch.Core;
using MaSch.Core.Extensions;
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
    public class ThicknessConverter : IValueConverter, IMultiValueConverter
    {
        private readonly MathConverter _mathConverter = new MathConverter();

        public string TopFormula { get; set; } = "T";
        public string BottomFormula { get; set; } = "B";
        public string LeftFormula { get; set; } = "L";
        public string RightFormula { get; set; } = "R";

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var (lf, tf, rf, bf) = GetFormulas(parameter);
            var v = new List<object>();
            if (ObjectExtensions.ConvertManager.TryConvert(value, CultureInfo.InvariantCulture, out double d))
            {
                ConvertExpressions(ref lf, ref tf, ref rf, ref bf, "", 0);
                v.Add(d);
            }
            else if (value is Thickness t)
            {
                ConvertExpressions(ref lf, ref tf, ref rf, ref bf, "", 0);
                v.Add(t.Left, t.Top, t.Right, t.Bottom);
            }
            if (v.Count == 0)
                return null;

            var vArr = v.ToArray();
            return new Thickness(
                (double)_mathConverter.Convert(vArr, typeof(double), lf, culture),
                (double)_mathConverter.Convert(vArr, typeof(double), tf, culture),
                (double)_mathConverter.Convert(vArr, typeof(double), rf, culture),
                (double)_mathConverter.Convert(vArr, typeof(double), bf, culture));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.IsNullOrEmpty())
                return null;

            var (lf, tf, rf, bf) = GetFormulas(parameter);

            var v = new List<object>();
            int rCount = 0;
            foreach (var p in values)
            {
                if (ObjectExtensions.ConvertManager.TryConvert(p, CultureInfo.InvariantCulture, out double d))
                {
                    ConvertExpressions(ref lf, ref tf, ref rf, ref bf, (rCount++).ToString(), v.Count);
                    v.Add(d);
                }
                else if (p is Thickness t)
                {
                    ConvertExpressions(ref lf, ref tf, ref rf, ref bf, (rCount++).ToString(), v.Count);
                    v.Add(t.Left, t.Top, t.Right, t.Bottom);
                }
            }
            var vArr = v.ToArray();
            return new Thickness(
                (double)_mathConverter.Convert(vArr, typeof(double), lf, culture),
                (double)_mathConverter.Convert(vArr, typeof(double), tf, culture),
                (double)_mathConverter.Convert(vArr, typeof(double), rf, culture),
                (double)_mathConverter.Convert(vArr, typeof(double), bf, culture));
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private (string left, string top, string right, string bottom) GetFormulas(object parameter)
        {
            string lf = LeftFormula, tf = TopFormula, rf = RightFormula, bf = BottomFormula;
            if (parameter is string f)
            {
                var fSplit = f.Split(';');
                if (fSplit.Length == 4)
                {
                    lf = fSplit[0];
                    tf = fSplit[1];
                    rf = fSplit[2];
                    bf = fSplit[3];
                }
            }
            return (lf, tf, rf, bf);
        }

        private static void ConvertExpressions(ref string leftExpression, ref string topExpression, ref string rightExpression, ref string bottomExpression, string suffix, int startIndex)
        {
            leftExpression = ConvertExpression(leftExpression, suffix, startIndex);
            topExpression = ConvertExpression(topExpression, suffix, startIndex);
            rightExpression = ConvertExpression(rightExpression, suffix, startIndex); 
            bottomExpression = ConvertExpression(bottomExpression, suffix, startIndex);
        }

        private static string ConvertExpression(string expression, string suffix, int startIndex)
        {
            return expression.ToLower()
                .Replace("l" + suffix, $"{{{startIndex}}}")
                .Replace("t" + suffix, $"{{{startIndex + 1}}}")
                .Replace("r" + suffix, $"{{{startIndex + 2}}}")
                .Replace("b" + suffix, $"{{{startIndex + 3}}}")
                .Replace("x" + suffix, $"{{{startIndex}}}");
        }
    }
}
