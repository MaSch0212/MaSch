using MaSch.Core;
using System.Text;

namespace MaSch.Console.Ansi
{
    /// <summary>
    /// Represents a style that can be produced by ANSI control sequences.
    /// </summary>
    public class AnsiStyle
    {
        private AnsiTextStyle _addedStyles;
        private AnsiTextStyle _removedStyles;
        private AnsiColor? _foreground;
        private AnsiColor? _background;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnsiStyle"/> class.
        /// </summary>
        public AnsiStyle()
        {
        }

        /// <summary>
        /// Gets the styles that should be used for this style.
        /// </summary>
        public AnsiTextStyle AddedStyles => _addedStyles;

        /// <summary>
        /// Gets the styles that should not be used for this style.
        /// </summary>
        public AnsiTextStyle RemovedStyles => _removedStyles;

        /// <summary>
        /// Gets the foreground color to use for this style.
        /// </summary>
        public AnsiColor? ForegroundColor => _foreground;

        /// <summary>
        /// Gets the background color to use for this style.
        /// </summary>
        public AnsiColor? BackgroundColor => _background;

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that the specified styles should be used.
        /// </summary>
        /// <param name="styles">The styles to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle WithStyles(AnsiTextStyle styles)
        {
            Guard.NotUndefinedFlagInEnumValue(styles, nameof(styles));
            _addedStyles |= styles;
            _removedStyles ^= styles & _removedStyles;
            return this;
        }

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that the specified styles should not be used.
        /// </summary>
        /// <param name="styles">The styles to not use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle WithoutStyles(AnsiTextStyle styles)
        {
            Guard.NotUndefinedFlagInEnumValue(styles, nameof(styles));
            _addedStyles ^= styles & _addedStyles;
            _removedStyles |= styles;
            return this;
        }

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that the specified styles should be used exclusively.
        /// </summary>
        /// <param name="exactStyles">The exact styles to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle OverrideStyles(AnsiTextStyle exactStyles)
        {
            Guard.NotUndefinedFlagInEnumValue(exactStyles, nameof(exactStyles));
            _addedStyles = exactStyles;
            _removedStyles = AnsiTextStyle.All ^ AnsiTextStyle.Invert ^ exactStyles;
            return this;
        }

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be bold.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Bold() => WithStyles(AnsiTextStyle.Bold);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be fainted, dimmed or have decreased intensity.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Faint() => WithStyles(AnsiTextStyle.Faint);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be italic.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Italic() => WithStyles(AnsiTextStyle.Italic);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be underlined.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Underlined() => WithStyles(AnsiTextStyle.Underline);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should blink.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Blinking() => WithStyles(AnsiTextStyle.Blink);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that foreground and background colors should be swapped.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Inverted() => WithStyles(AnsiTextStyle.Invert);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be crossed-out.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle CrossedOut() => WithStyles(AnsiTextStyle.CrossedOut);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be doubly underlined.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle DoublyUnderlined() => WithStyles(AnsiTextStyle.DoublyUnderlined);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should be overlined.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Overlined() => WithStyles(AnsiTextStyle.Overlined);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be bold, fainted, dimmed or have decreased intensity.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle NotBoldOrFaint() => WithoutStyles(AnsiTextStyle.Bold | AnsiTextStyle.Faint);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be italic.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle NotItalic() => WithoutStyles(AnsiTextStyle.Italic);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be underlined.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle NotUnderlined() => WithoutStyles(AnsiTextStyle.Underline | AnsiTextStyle.DoublyUnderlined);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not blink.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle NotBlinking() => WithoutStyles(AnsiTextStyle.Blink);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be crossed-out.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle NotCrossedOut() => WithoutStyles(AnsiTextStyle.CrossedOut);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should not be overlined.
        /// </summary>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle NotOverlined() => WithoutStyles(AnsiTextStyle.Overlined);

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified foreground color.
        /// </summary>
        /// <param name="color">The foreground color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Foreground(AnsiColor color)
        {
            _foreground = color;
            return this;
        }

        /// <summary>
        /// Adds the information to this <see cref="AnsiStyle"/> instance that text should have the specified background color.
        /// </summary>
        /// <param name="color">The background color to use.</param>
        /// <returns>The same <see cref="AnsiStyle"/> instance this method was called on.</returns>
        public AnsiStyle Background(AnsiColor color)
        {
            _background = color;
            return this;
        }

        /// <summary>
        /// Builds the ANSI control sequence for this <see cref="AnsiStyle"/>.
        /// </summary>
        /// <returns>An ANSI control sequence that will set the text style to the values specified by this <see cref="AnsiStyle"/>.</returns>
        public string BuildAnsiSequence()
        {
            var result = new StringBuilder();
            if (RemovedStyles != AnsiTextStyle.None)
                result.Append(AnsiEscapeUtility.GetRemoveStyle(RemovedStyles));
            if (AddedStyles != AnsiTextStyle.None)
                result.Append(AnsiEscapeUtility.GetAddStyle(AddedStyles));
            if (ForegroundColor.HasValue)
                result.Append(ForegroundColor.Value.ForegroundSequence);
            if (BackgroundColor.HasValue)
                result.Append(BackgroundColor.Value.BackgroundSequence);
            return result.ToString();
        }
    }
}
