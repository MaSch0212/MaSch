using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Drawing;

namespace MaSch.Console.Ansi
{
    /// <summary>
    /// Represents a color settable using an ANSI escape sequence.
    /// </summary>
    public struct AnsiColor
    {
        private readonly bool _isDefaultColor;
        private readonly AnsiColorCode? _colorCode;
        private readonly byte _red;
        private readonly byte _green;
        private readonly byte _blue;

        private AnsiColor(AnsiColorCode? color)
        {
            _isDefaultColor = !color.HasValue;
            _colorCode = color;
            _red = _green = _blue = 0;
        }

        private AnsiColor(int red, int green, int blue)
        {
            _isDefaultColor = false;
            _colorCode = null;
            _red = (byte)Guard.NotOutOfRange(red, nameof(red), 0, 255);
            _green = (byte)Guard.NotOutOfRange(green, nameof(green), 0, 255);
            _blue = (byte)Guard.NotOutOfRange(blue, nameof(blue), 0, 255);
        }

        /// <summary>
        /// Gets the default <see cref="AnsiColor"/>.
        /// </summary>
        public static AnsiColor Default { get; } = new AnsiColor(null);

        /// <summary>
        /// Gets the ANSI escape sequence for setting the foreground to the color represented by this <see cref="AnsiColor"/>.
        /// </summary>
        public string ForegroundSequence =>
            _isDefaultColor
                ? AnsiEscapeUtility.GetResetForegroundColor()
                : _colorCode.HasValue
                    ? AnsiEscapeUtility.GetSetForegroundColor(_colorCode.Value)
                    : AnsiEscapeUtility.GetSetForegroundColor(_red, _green, _blue);

        /// <summary>
        /// Gets the ANSI escape sequence for setting the background to the color represented by this <see cref="AnsiColor"/>.
        /// </summary>
        public string BackgroundSequence =>
            _isDefaultColor
                ? AnsiEscapeUtility.GetResetBackgroundColor()
                : _colorCode.HasValue
                    ? AnsiEscapeUtility.GetSetBackgroundColor(_colorCode.Value)
                    : AnsiEscapeUtility.GetSetBackgroundColor(_red, _green, _blue);

        /// <summary>
        /// Creates an <see cref="AnsiColor"/> using a specified <see cref="AnsiColorCode"/>.
        /// </summary>
        /// <param name="colorCode">The <see cref="AnsiColorCode"/> to use.</param>
        /// <returns>A <see cref="AnsiColor"/> instance that represents the <paramref name="colorCode"/>.</returns>
        public static AnsiColor FromColorCode(AnsiColorCode colorCode)
            => new(Guard.NotUndefinedEnumMember(colorCode, nameof(colorCode)));

        /// <summary>
        /// Creates an <see cref="AnsiColor"/> using specified RGB values.
        /// </summary>
        /// <param name="red">The red channel (0-255).</param>
        /// <param name="green">The green channel (0-255).</param>
        /// <param name="blue">The blue channel (0-255).</param>
        /// <returns>A <see cref="AnsiColor"/> instance that represents the given RGB values.</returns>
        public static AnsiColor FromRgb(int red, int green, int blue)
            => new(red, green, blue);

        /// <summary>
        /// Creates an <see cref="AnsiColor"/> using a specified <see cref="Color"/> instance.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to use.</param>
        /// <returns>A <see cref="AnsiColor"/> instance that represents the <paramref name="color"/>.</returns>
        public static AnsiColor FromColor(Color color)
            => new(color.R, color.G, color.B);

        /// <summary>
        /// Creates an <see cref="AnsiColor"/> using a specified <see cref="ConsoleColor"/>.
        /// </summary>
        /// <param name="color">The <see cref="ConsoleColor"/> to use.</param>
        /// <returns>A <see cref="AnsiColor"/> instance that represents the <paramref name="color"/>.</returns>
        public static AnsiColor FromConsoleColor(ConsoleColor color)
            => new(color switch
            {
                ConsoleColor.Black => AnsiColorCode.Black,
                ConsoleColor.DarkBlue => AnsiColorCode.DarkBlue,
                ConsoleColor.DarkGreen => AnsiColorCode.DarkGreen,
                ConsoleColor.DarkCyan => AnsiColorCode.DarkCyan,
                ConsoleColor.DarkRed => AnsiColorCode.DarkRed,
                ConsoleColor.DarkMagenta => AnsiColorCode.DarkMagenta,
                ConsoleColor.DarkYellow => AnsiColorCode.DarkYellow,
                ConsoleColor.Gray => AnsiColorCode.Gray,
                ConsoleColor.DarkGray => AnsiColorCode.DarkGray,
                ConsoleColor.Blue => AnsiColorCode.Blue,
                ConsoleColor.Green => AnsiColorCode.Green,
                ConsoleColor.Cyan => AnsiColorCode.Cyan,
                ConsoleColor.Red => AnsiColorCode.Red,
                ConsoleColor.Magenta => AnsiColorCode.Magenta,
                ConsoleColor.Yellow => AnsiColorCode.Yellow,
                ConsoleColor.White => AnsiColorCode.White,
                _ => throw new ArgumentOutOfRangeException(nameof(color)),
            });

        /// <summary>
        /// Returns the string representation of the color represented by this struct.
        /// </summary>
        /// <returns>The string representation of the color.</returns>
        public override string ToString()
        {
            return _isDefaultColor
                ? "Default"
                : _colorCode.HasValue
                    ? Enum.GetName(typeof(AnsiColorCode), _colorCode.Value)!
                    : $"#{_red.ToHexString()}{_green.ToHexString()}{_blue.ToHexString()} - rgb({_red}, {_green}, {_blue})";
        }
    }
}
