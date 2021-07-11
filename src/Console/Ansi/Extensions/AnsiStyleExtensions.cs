using System;
using System.Drawing;

namespace MaSch.Console.Ansi
{
    /// <summary>
    /// Provides extension methods for the <see cref="AnsiStyle"/> class.
    /// </summary>
    public static class AnsiStyleExtensions
    {
        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the default foreground color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle DefaultForeground(this AnsiStyle style)
            => style.Foreground(AnsiColor.Default);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified foreground color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="colorCode">The foreground color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Foreground(this AnsiStyle style, AnsiColorCode colorCode)
            => style.Foreground(AnsiColor.FromColorCode(colorCode));

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified foreground color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="red">The red channel of the color to use.</param>
        /// <param name="green">The green channel of the color to use.</param>
        /// <param name="blue">The blue channel of the color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Foreground(this AnsiStyle style, int red, int green, int blue)
            => style.Foreground(AnsiColor.FromRgb(red, green, blue));

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified foreground color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="color">The foreground color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Foreground(this AnsiStyle style, ConsoleColor color)
            => style.Foreground(AnsiColor.FromConsoleColor(color));

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified foreground color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="color">The foreground color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Foreground(this AnsiStyle style, Color color)
            => style.Foreground(AnsiColor.FromColor(color));

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the default background color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle DefaultBackground(this AnsiStyle style)
            => style.Background(AnsiColor.Default);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified background color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="colorCode">The background color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Background(this AnsiStyle style, AnsiColorCode colorCode)
            => style.Background(AnsiColor.FromColorCode(colorCode));

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified background color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="red">The red channel of the color to use.</param>
        /// <param name="green">The green channel of the color to use.</param>
        /// <param name="blue">The blue channel of the color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Background(this AnsiStyle style, int red, int green, int blue)
            => style.Background(AnsiColor.FromRgb(red, green, blue));

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified background color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="color">The background color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Background(this AnsiStyle style, ConsoleColor color)
            => style.Background(AnsiColor.FromConsoleColor(color));

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified background color.
        /// </summary>
        /// <param name="style">The style to add the information to.</param>
        /// <param name="color">The background color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public static AnsiStyle Background(this AnsiStyle style, Color color)
            => style.Background(AnsiColor.FromColor(color));
    }
}
