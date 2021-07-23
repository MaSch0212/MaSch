using MaSch.Console.Ansi;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Console.UnitTests.Ansi
{
    [TestClass]
    public class AnsiStyleTests : TestClassBase
    {
        private AnsiStyle Style => Cache.GetValue(() => new AnsiStyle())!;

        [TestMethod]
        [DataRow((AnsiTextStyle)(-1))]
        [DataRow((AnsiTextStyle)512)]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Blink | (AnsiTextStyle)512)]
        public void WithStyles_ArgumentException(AnsiTextStyle styles)
        {
            Assert.ThrowsException<ArgumentException>(() => Style.WithStyles(styles));
        }

        [TestMethod]
        public void WithStyles_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Bold | AnsiTextStyle.Blink);
            var result = Style.WithStyles(AnsiTextStyle.Bold | AnsiTextStyle.Italic);

            Assert.AreEqual(AnsiTextStyle.Bold | AnsiTextStyle.Italic, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Blink, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        [DataRow((AnsiTextStyle)(-1))]
        [DataRow((AnsiTextStyle)512)]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Blink | (AnsiTextStyle)512)]
        public void WithoutStyles_ArgumentException(AnsiTextStyle styles)
        {
            Assert.ThrowsException<ArgumentException>(() => Style.WithoutStyles(styles));
        }

        [TestMethod]
        public void WithoutStyles_Success()
        {
            Style.WithStyles(AnsiTextStyle.Bold | AnsiTextStyle.Blink);
            var result = Style.WithoutStyles(AnsiTextStyle.Bold | AnsiTextStyle.Italic);

            Assert.AreEqual(AnsiTextStyle.Blink, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Bold | AnsiTextStyle.Italic, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        [DataRow((AnsiTextStyle)(-1))]
        [DataRow((AnsiTextStyle)512)]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Blink | (AnsiTextStyle)512)]
        public void OverrideStyles_ArgumentException(AnsiTextStyle styles)
        {
            Assert.ThrowsException<ArgumentException>(() => Style.OverrideStyles(styles));
        }

        [TestMethod]
        public void OverrideStyles_Success()
        {
            Style.WithStyles(AnsiTextStyle.Bold | AnsiTextStyle.Blink);
            Style.WithoutStyles(AnsiTextStyle.Bold | AnsiTextStyle.Italic);
            var result = Style.OverrideStyles(AnsiTextStyle.Faint | AnsiTextStyle.Overlined);

            Assert.AreEqual(AnsiTextStyle.Faint | AnsiTextStyle.Overlined, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Bold | AnsiTextStyle.Italic | AnsiTextStyle.Underline | AnsiTextStyle.Blink | AnsiTextStyle.CrossedOut | AnsiTextStyle.DoublyUnderlined, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Bold_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Bold);
            var result = Style.Bold();

            Assert.AreEqual(AnsiTextStyle.Bold, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Faint_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Faint);
            var result = Style.Faint();

            Assert.AreEqual(AnsiTextStyle.Faint, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Italic_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Italic);
            var result = Style.Italic();

            Assert.AreEqual(AnsiTextStyle.Italic, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Underlined_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Underline);
            var result = Style.Underlined();

            Assert.AreEqual(AnsiTextStyle.Underline, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Blinking_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Blink);
            var result = Style.Blinking();

            Assert.AreEqual(AnsiTextStyle.Blink, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Inverted_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Invert);
            var result = Style.Inverted();

            Assert.AreEqual(AnsiTextStyle.Invert, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void CrossedOut_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.CrossedOut);
            var result = Style.CrossedOut();

            Assert.AreEqual(AnsiTextStyle.CrossedOut, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void DoublyUnderlined_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.DoublyUnderlined);
            var result = Style.DoublyUnderlined();

            Assert.AreEqual(AnsiTextStyle.DoublyUnderlined, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Overlined_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Overlined);
            var result = Style.Overlined();

            Assert.AreEqual(AnsiTextStyle.Overlined, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.None, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void NotBoldOrFaint_Success()
        {
            Style.WithStyles(AnsiTextStyle.Bold | AnsiTextStyle.Faint);
            var result = Style.NotBoldOrFaint();

            Assert.AreEqual(AnsiTextStyle.None, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Bold | AnsiTextStyle.Faint, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void NotItalic_Success()
        {
            Style.WithStyles(AnsiTextStyle.Italic);
            var result = Style.NotItalic();

            Assert.AreEqual(AnsiTextStyle.None, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Italic, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void NotUnderlined_Success()
        {
            Style.WithStyles(AnsiTextStyle.Underline | AnsiTextStyle.DoublyUnderlined);
            var result = Style.NotUnderlined();

            Assert.AreEqual(AnsiTextStyle.None, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Underline | AnsiTextStyle.DoublyUnderlined, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void NotBlinking_Success()
        {
            Style.WithStyles(AnsiTextStyle.Blink);
            var result = Style.NotBlinking();

            Assert.AreEqual(AnsiTextStyle.None, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Blink, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void NotCrossedOut_Success()
        {
            Style.WithStyles(AnsiTextStyle.CrossedOut);
            var result = Style.NotCrossedOut();

            Assert.AreEqual(AnsiTextStyle.None, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.CrossedOut, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void NotOverlined_Success()
        {
            Style.WithStyles(AnsiTextStyle.Overlined);
            var result = Style.NotOverlined();

            Assert.AreEqual(AnsiTextStyle.None, Style.AddedStyles);
            Assert.AreEqual(AnsiTextStyle.Overlined, Style.RemovedStyles);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Foreground_Success()
        {
            var color = AnsiColor.FromRgb(45, 23, 68);
            var result = Style.Foreground(color);

            Assert.AreEqual(color, Style.ForegroundColor);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        public void Background_Success()
        {
            var color = AnsiColor.FromRgb(45, 23, 68);
            var result = Style.Background(color);

            Assert.AreEqual(color, Style.BackgroundColor);
            Assert.AreSame(Style, result);
        }

        [TestMethod]
        [DataRow(AnsiTextStyle.Bold, "\u001b[22m")]
        [DataRow(AnsiTextStyle.Faint, "\u001b[22m")]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Faint, "\u001b[22m")]
        [DataRow(AnsiTextStyle.Italic, "\u001b[23m")]
        [DataRow(AnsiTextStyle.Underline, "\u001b[24m")]
        [DataRow(AnsiTextStyle.Underline | AnsiTextStyle.DoublyUnderlined, "\u001b[24m")]
        [DataRow(AnsiTextStyle.Blink, "\u001b[25m")]
        [DataRow(AnsiTextStyle.Invert, "\u001b[7m")]
        [DataRow(AnsiTextStyle.CrossedOut, "\u001b[29m")]
        [DataRow(AnsiTextStyle.DoublyUnderlined, "\u001b[24m")]
        [DataRow(AnsiTextStyle.Overlined, "\u001b[55m")]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Faint | AnsiTextStyle.Italic, "\u001b[22m\u001b[23m")]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Overlined | AnsiTextStyle.Italic, "\u001b[22m\u001b[23m\u001b[55m")]
        public void BuildAnsiSequence_RemovedStyles_Success(AnsiTextStyle styles, string expected)
        {
            Style.WithoutStyles(styles);
            var result = Style.BuildAnsiSequence();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(AnsiTextStyle.Bold, "\u001b[1m")]
        [DataRow(AnsiTextStyle.Faint, "\u001b[2m")]
        [DataRow(AnsiTextStyle.Italic, "\u001b[3m")]
        [DataRow(AnsiTextStyle.Underline, "\u001b[4m")]
        [DataRow(AnsiTextStyle.Blink, "\u001b[5m")]
        [DataRow(AnsiTextStyle.Invert, "\u001b[7m")]
        [DataRow(AnsiTextStyle.CrossedOut, "\u001b[9m")]
        [DataRow(AnsiTextStyle.DoublyUnderlined, "\u001b[21m")]
        [DataRow(AnsiTextStyle.Overlined, "\u001b[53m")]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Italic, "\u001b[1m\u001b[3m")]
        [DataRow(AnsiTextStyle.Blink | AnsiTextStyle.Invert | AnsiTextStyle.Overlined, "\u001b[5m\u001b[7m\u001b[53m")]
        public void BuildAnsiSequence_AddedStyles_Success(AnsiTextStyle styles, string expected)
        {
            Style.WithStyles(styles);
            var result = Style.BuildAnsiSequence();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(null, "\u001b[39m")]
        [DataRow(AnsiColorCode.RGB_003, "\u001b[38;5;19m")]
        [DataRow("64,52,124", "\u001b[38;2;64;52;124m")]
        public void BuildAnsiSequence_Foreground_Success(object? color, string expected)
        {
            Style.Foreground(GetColorFromObject(color));
            var result = Style.BuildAnsiSequence();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(null, "\u001b[49m")]
        [DataRow(AnsiColorCode.RGB_003, "\u001b[48;5;19m")]
        [DataRow("64,52,124", "\u001b[48;2;64;52;124m")]
        public void BuildAnsiSequence_Background_Success(object? color, string expected)
        {
            Style.Background(GetColorFromObject(color));
            var result = Style.BuildAnsiSequence();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void BuildAnsiSequence_Combined_Success()
        {
            Style.WithoutStyles(AnsiTextStyle.Bold);
            Style.WithStyles(AnsiTextStyle.Italic);
            Style.Foreground(AnsiColor.FromColorCode(AnsiColorCode.Cyan));
            Style.Background(AnsiColor.FromRgb(45, 46, 47));
            var result = Style.BuildAnsiSequence();

            Assert.AreEqual("\u001b[22m\u001b[3m\u001b[38;5;14m\u001b[48;2;45;46;47m", result);
        }

        private static AnsiColor GetColorFromObject(object? color)
        {
            return color switch
            {
                null => AnsiColor.Default,
                AnsiColorCode code => AnsiColor.FromColorCode(code),
                _ => GetRgbFromString(color!.ToString()!),
            };
        }

        private static AnsiColor GetRgbFromString(string rgb)
        {
            var split = rgb.ToString().Split(',');
            return AnsiColor.FromRgb(int.Parse(split![0]), int.Parse(split[1]), int.Parse(split[2]));
        }
    }
}
