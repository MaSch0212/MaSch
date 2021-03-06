using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaSch.Presentation.Wpf.Converter
{
    /// <summary>
    /// A <see cref="IValueConverter"/> that calculates a new <see cref="Thickness"/> from another <see cref="Thickness"/> using formula for each direction.
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class ThicknessConverter : IValueConverter, IMultiValueConverter
    {
        private readonly MathConverter _mathConverter = new MathConverter();

        /// <summary>
        /// Gets or sets the formula to calculate the top value of the thickness.
        /// Use the following placeholders to reference the positions from the original <see cref="Thickness"/>: Top=T; Bottom=B; Left=L; Right=R.
        /// </summary>
        public string TopFormula { get; set; } = "T";

        /// <summary>
        /// Gets or sets the formula to calculate the bottom value of the thickness.
        /// Use the following placeholders to reference the positions from the original <see cref="Thickness"/>: Top=T; Bottom=B; Left=L; Right=R.
        /// </summary>
        public string BottomFormula { get; set; } = "B";

        /// <summary>
        /// Gets or sets the formula to calculate the left value of the thickness.
        /// Use the following placeholders to reference the positions from the original <see cref="Thickness"/>: Top=T; Bottom=B; Left=L; Right=R.
        /// </summary>
        public string LeftFormula { get; set; } = "L";

        /// <summary>
        /// Gets or sets the formula to calculate the right value of the thickness.
        /// Use the following placeholders to reference the positions from the original <see cref="Thickness"/>: Top=T; Bottom=B; Left=L; Right=R.
        /// </summary>
        public string RightFormula { get; set; } = "R";

        /// <inheritdoc />
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var (lf, tf, rf, bf) = GetFormulas(parameter);
            var v = new List<object>();
            if (ObjectExtensions.ConvertManager.TryConvert(value, CultureInfo.InvariantCulture, out double d))
            {
                ConvertExpressions(ref lf, ref tf, ref rf, ref bf, string.Empty, 0);
                v.Add(d);
            }
            else if (value is Thickness t)
            {
                ConvertExpressions(ref lf, ref tf, ref rf, ref bf, string.Empty, 0);
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
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
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

        private (string Left, string Top, string Right, string Bottom) GetFormulas(object parameter)
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
