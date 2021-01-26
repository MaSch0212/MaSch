using MaSch.Core;
using System;
using System.Drawing;
using System.Globalization;

namespace MaSch.Console.Controls
{
    public class NumberInputControl
    {
        private readonly IConsoleService _console;

        public double Minimum { get; set; } = double.MinValue;
        public double Maximum { get; set; } = double.MaxValue;
        public double? Value { get; set; }
        public bool IsDecimal { get; set; } = false;

        public NumberInputControl(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));
        }

        public void Show()
            => Value = ShowInternal(_console, Value, Minimum, Maximum, IsDecimal);

        public static double Show(IConsoleService console)
            => ShowInternal(console, null, double.MinValue, double.MaxValue, false);
        public static double Show(IConsoleService console, double min, double max)
            => ShowInternal(console, null, min, max, false);
        public static double Show(IConsoleService console, double value)
            => ShowInternal(console, value, double.MinValue, double.MaxValue, false);
        public static double Show(IConsoleService console, double value, double min, double max)
            => ShowInternal(console, value, min, max, false);
        public static double Show(IConsoleService console, double value, double min, double max, bool isDecimal)
            => ShowInternal(console, value, min, max, isDecimal);
        private static double ShowInternal(IConsoleService console, double? value, double min, double max, bool isDecimal)
        {
            using var scope = ConsoleSynchronizer.Scope();

            if (min > max)
                throw new ArgumentException("min cannot be larger than max.");
            var culture = CultureInfo.CurrentUICulture;
            string result = value?.ToString(culture) ?? string.Empty;
            string temp = string.Empty;
            var pos = console.CursorPosition.Point;
            var bs = console.BufferSize;
            int lLeft = pos.X;
            int lTop = pos.Y;
            int strPos;
            ConsoleKeyInfo key;
            var numberStyle = isDecimal ? NumberStyles.Float : NumberStyles.Integer;

            do
            {
                if (lLeft >= bs.Width)
                {
                    lLeft = 0;
                    lTop++;
                }
                else if (lLeft < 0)
                {
                    lLeft = bs.Width - 1;
                    lTop--;
                }

                console.CursorPosition.Point = pos;
                console.Write(new string(' ', temp.Length + 1));
                console.CursorPosition.Point = pos;
                console.Write(result);
                console.CursorPosition.Point = new Point(lLeft, lTop);
                key = console.ReadKey(true);
                strPos = (lTop - pos.Y) * bs.Width + (lLeft - pos.X);
                temp = result;
                if (key.Key == ConsoleKey.Backspace && strPos > 0)
                    temp = temp.Remove(strPos - 1, 1);
                else if (key.Key == ConsoleKey.Delete && strPos < temp.Length)
                {
                    temp = temp.Remove(strPos, 1);
                    lLeft++;
                }
                else if (key.Key == ConsoleKey.RightArrow && strPos < temp.Length - 1)
                    lLeft++;
                else if (key.Key == ConsoleKey.LeftArrow && strPos > 0)
                    lLeft--;
                else
                    temp = temp.Insert(strPos, key.KeyChar.ToString());

                if (double.TryParse(temp, numberStyle, culture, out var tmp1))
                {
                    if (tmp1 < min)
                        temp = min.ToString();
                    if (tmp1 > max)
                        temp = max.ToString();
                    lLeft += temp.Length - result.Length;
                    result = temp;
                }
                else if (temp == "-" || temp == "")
                {
                    lLeft += temp.Length - result.Length;
                    result = temp;
                }
            }
            while (key.Key != ConsoleKey.Enter);

            console.CursorPosition.Point = new Point(0, lTop + 1);
            return double.TryParse(result, numberStyle, culture, out var tmp) ? tmp : min;
        }
    }
}
