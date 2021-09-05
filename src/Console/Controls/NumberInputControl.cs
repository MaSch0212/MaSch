using MaSch.Core;
using System;
using System.Drawing;
using System.Globalization;

namespace MaSch.Console.Controls
{
    /// <summary>
    /// Control for a <see cref="IConsoleService"/> with which a number can be requested by the user.
    /// </summary>
    public class NumberInputControl
    {
        private readonly IConsoleService _console;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberInputControl"/> class.
        /// </summary>
        /// <param name="console">The console that is used to request input.</param>
        public NumberInputControl(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));
        }

        /// <summary>
        /// Gets or sets the minimum value that is allowed.
        /// </summary>
        public double Minimum { get; set; } = double.MinValue;

        /// <summary>
        /// Gets or sets the maximum value that is allowed.
        /// </summary>
        public double Maximum { get; set; } = double.MaxValue;

        /// <summary>
        /// Gets or sets the last input value from the user.
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can type in decimal numbers.
        /// </summary>
        public bool IsDecimal { get; set; } = false;

        /// <summary>
        /// Requests a number from the user.
        /// </summary>
        /// <param name="console">The console that is used to request input.</param>
        /// <returns>The typed in value by the user.</returns>
        public static double Show(IConsoleService console)
        {
            return ShowInternal(console, null, double.MinValue, double.MaxValue, false);
        }

        /// <summary>
        /// Requests a number from the user.
        /// </summary>
        /// <param name="console">The console that is used to request input.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The typed in value by the user.</returns>
        public static double Show(IConsoleService console, double min, double max)
        {
            return ShowInternal(console, null, min, max, false);
        }

        /// <summary>
        /// Requests a number from the user.
        /// </summary>
        /// <param name="console">The console that is used to request input.</param>
        /// <param name="value">The value the user can edit.</param>
        /// <returns>The typed in value by the user.</returns>
        public static double Show(IConsoleService console, double value)
        {
            return ShowInternal(console, value, double.MinValue, double.MaxValue, false);
        }

        /// <summary>
        /// Requests a number from the user.
        /// </summary>
        /// <param name="console">The console that is used to request input.</param>
        /// <param name="value">The value the user can edit.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <returns>The typed in value by the user.</returns>
        public static double Show(IConsoleService console, double value, double min, double max)
        {
            return ShowInternal(console, value, min, max, false);
        }

        /// <summary>
        /// Requests a number from the user.
        /// </summary>
        /// <param name="console">The console that is used to request input.</param>
        /// <param name="value">The value the user can edit.</param>
        /// <param name="min">The minimum allowed value.</param>
        /// <param name="max">The maximum allowed value.</param>
        /// <param name="isDecimal">if set to <c>true</c> the user can input decimal numbers.</param>
        /// <returns>The typed in value by the user.</returns>
        public static double Show(IConsoleService console, double value, double min, double max, bool isDecimal)
        {
            return ShowInternal(console, value, min, max, isDecimal);
        }

        /// <summary>
        /// Requests a number from the user.
        /// </summary>
        public void Show()
        {
            Value = ShowInternal(_console, Value, Minimum, Maximum, IsDecimal);
        }

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
            int left = pos.X;
            int top = pos.Y;
            int strPos;
            ConsoleKeyInfo key;
            var numberStyle = isDecimal ? NumberStyles.Float : NumberStyles.Integer;

            do
            {
                if (left >= bs.Width)
                {
                    left = 0;
                    top++;
                }
                else if (left < 0)
                {
                    left = bs.Width - 1;
                    top--;
                }

                console.CursorPosition.Point = pos;
                console.Write(new string(' ', temp.Length + 1));
                console.CursorPosition.Point = pos;
                console.Write(result);
                console.CursorPosition.Point = new Point(left, top);
                key = console.ReadKey(true);
                strPos = ((top - pos.Y) * bs.Width) + (left - pos.X);
                temp = result;
                if (key.Key == ConsoleKey.Backspace && strPos > 0)
                {
                    temp = temp.Remove(strPos - 1, 1);
                }
                else if (key.Key == ConsoleKey.Delete && strPos < temp.Length)
                {
                    temp = temp.Remove(strPos, 1);
                    left++;
                }
                else if (key.Key == ConsoleKey.RightArrow && strPos < temp.Length - 1)
                {
                    left++;
                }
                else if (key.Key == ConsoleKey.LeftArrow && strPos > 0)
                {
                    left--;
                }
                else
                {
                    temp = temp.Insert(strPos, key.KeyChar.ToString());
                }

                if (double.TryParse(temp, numberStyle, culture, out var tmp1))
                {
                    if (tmp1 < min)
                        temp = min.ToString();
                    if (tmp1 > max)
                        temp = max.ToString();
                    left += temp.Length - result.Length;
                    result = temp;
                }
                else if (temp == "-" || temp == string.Empty)
                {
                    left += temp.Length - result.Length;
                    result = temp;
                }
            }
            while (key.Key != ConsoleKey.Enter);

            console.CursorPosition.Point = new Point(0, top + 1);
            return double.TryParse(result, numberStyle, culture, out var tmp) ? tmp : min;
        }
    }
}
