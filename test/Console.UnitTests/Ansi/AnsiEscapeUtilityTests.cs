using MaSch.Console.Ansi;
using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MaSch.Console.UnitTests.Ansi
{
    [TestClass]
    public class AnsiEscapeUtilityTests : TestClassBase
    {
        [TestMethod]
        public void Esc()
        {
            Assert.AreEqual('\u001b', AnsiEscapeUtility.ESC);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetCursorUp_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorUp(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0A")]
        [DataRow(2, "\u001b[2A")]
        [DataRow(1000, "\u001b[1000A")]
        public void GetCursorUp_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorUp(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetCursorDown_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorDown(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0B")]
        [DataRow(2, "\u001b[2B")]
        [DataRow(1000, "\u001b[1000B")]
        public void GetCursorDown_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorDown(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetCursorBack_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorBack(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0D")]
        [DataRow(2, "\u001b[2D")]
        [DataRow(1000, "\u001b[1000D")]
        public void GetCursorBack_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorBack(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetCursorForward_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorForward(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0C")]
        [DataRow(2, "\u001b[2C")]
        [DataRow(1000, "\u001b[1000C")]
        public void GetCursorForward_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorForward(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetCursorNextLine_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorNextLine(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0E")]
        [DataRow(2, "\u001b[2E")]
        [DataRow(1000, "\u001b[1000E")]
        public void GetCursorNextLine_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorNextLine(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetCursorPreviousLine_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorPreviousLine(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0F")]
        [DataRow(2, "\u001b[2F")]
        [DataRow(1000, "\u001b[1000F")]
        public void GetCursorPreviousLine_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorPreviousLine(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetCursorToColumn_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorToColumn(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0G")]
        [DataRow(2, "\u001b[2G")]
        [DataRow(1000, "\u001b[1000G")]
        public void GetCursorToColumn_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorToColumn(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000, 0)]
        [DataRow(-1, 0)]
        [DataRow(0, -1000)]
        [DataRow(0, -1)]
        public void GetCursorToPosition_ArgumentOutOfRangeException(int row, int column)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetCursorToPosition(row, column));
        }

        [TestMethod]
        [DataRow(0, 2, "\u001b[0;2H")]
        [DataRow(2, 0, "\u001b[2;0H")]
        [DataRow(1000, 2000, "\u001b[1000;2000H")]
        public void GetCursorToPosition_Success(int row, int column, string expected)
        {
            var result = AnsiEscapeUtility.GetCursorToPosition(row, column);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(4)]
        public void GetEraseScreen_ArgumentException(AnsiClearMode mode)
        {
            Assert.ThrowsException<ArgumentException>(() => AnsiEscapeUtility.GetEraseScreen(mode));
        }

        [TestMethod]
        [DataRow(AnsiClearMode.CurrentToEnd, "\u001b[0J")]
        [DataRow(AnsiClearMode.StartToCurrent, "\u001b[1J")]
        [DataRow(AnsiClearMode.Screen, "\u001b[2J")]
        [DataRow(AnsiClearMode.Buffer, "\u001b[3J")]
        public void GetEraseScreen_Success(AnsiClearMode mode, string expected)
        {
            var result = AnsiEscapeUtility.GetEraseScreen(mode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(3)]
        public void GetEraseLine_ArgumentException(AnsiLineClearMode mode)
        {
            Assert.ThrowsException<ArgumentException>(() => AnsiEscapeUtility.GetEraseLine(mode));
        }

        [TestMethod]
        [DataRow(AnsiLineClearMode.CurrentToEnd, "\u001b[0K")]
        [DataRow(AnsiLineClearMode.StartToCurrent, "\u001b[1K")]
        [DataRow(AnsiLineClearMode.EntireLine, "\u001b[2K")]
        public void GetEraseLine_Success(AnsiLineClearMode mode, string expected)
        {
            var result = AnsiEscapeUtility.GetEraseLine(mode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetScrollUp_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetScrollUp(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0S")]
        [DataRow(2, "\u001b[2S")]
        [DataRow(1000, "\u001b[1000S")]
        public void GetScrollUp_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetScrollUp(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetScrollDown_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetScrollDown(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0T")]
        [DataRow(2, "\u001b[2T")]
        [DataRow(1000, "\u001b[1000T")]
        public void GetScrollDown_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetScrollDown(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetDeleteLines_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetDeleteLines(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0M")]
        [DataRow(2, "\u001b[2M")]
        [DataRow(1000, "\u001b[1000M")]
        public void GetDeleteLines_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetDeleteLines(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetInsertLines_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetInsertLines(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0L")]
        [DataRow(2, "\u001b[2L")]
        [DataRow(1000, "\u001b[1000L")]
        public void GetInsertLines_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetInsertLines(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetEraseCharacters_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetEraseCharacters(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0X")]
        [DataRow(2, "\u001b[2X")]
        [DataRow(1000, "\u001b[1000X")]
        public void GetEraseCharacters_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetEraseCharacters(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetDeleteCharacters_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetDeleteCharacters(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0P")]
        [DataRow(2, "\u001b[2P")]
        [DataRow(1000, "\u001b[1000P")]
        public void GetDeleteCharacters_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetDeleteCharacters(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1000)]
        [DataRow(-1)]
        public void GetInsertCharacters_ArgumentOutOfRangeException(int n)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => AnsiEscapeUtility.GetInsertCharacters(n));
        }

        [TestMethod]
        [DataRow(0, "\u001b[0@")]
        [DataRow(2, "\u001b[2@")]
        [DataRow(1000, "\u001b[1000@")]
        public void GetInsertCharacters_Success(int n, string expected)
        {
            var result = AnsiEscapeUtility.GetInsertCharacters(n);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetSaveCursor_Success()
        {
            var result = AnsiEscapeUtility.GetSaveCursor();
            Assert.AreEqual("\u001b7", result);
        }

        [TestMethod]
        public void GetRestoreCursor_Success()
        {
            var result = AnsiEscapeUtility.GetRestoreCursor();
            Assert.AreEqual("\u001b8", result);
        }

        [TestMethod]
        public void GetHideCursor_Success()
        {
            var result = AnsiEscapeUtility.GetHideCursor();
            Assert.AreEqual("\u001b[?25l", result);
        }

        [TestMethod]
        public void GetShowCursor_Success()
        {
            var result = AnsiEscapeUtility.GetShowCursor();
            Assert.AreEqual("\u001b[?25h", result);
        }

        [TestMethod]
        public void GetResetStyle_Success()
        {
            var result = AnsiEscapeUtility.GetResetStyle();
            Assert.AreEqual("\u001b[0m", result);
        }

        [TestMethod]
        [DataRow((AnsiTextStyle)(-1))]
        [DataRow((AnsiTextStyle)512)]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Faint | (AnsiTextStyle)512)]
        public void GetAddStyle_ArgumentException(AnsiTextStyle style)
        {
            Assert.ThrowsException<ArgumentException>(() => AnsiEscapeUtility.GetAddStyle(style));
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
        public void GetAddStyle_Success(AnsiTextStyle style, string expected)
        {
            var result = AnsiEscapeUtility.GetAddStyle(style);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow((AnsiTextStyle)(-1))]
        [DataRow((AnsiTextStyle)512)]
        [DataRow(AnsiTextStyle.Bold | AnsiTextStyle.Faint | (AnsiTextStyle)512)]
        public void GetRemoveStyle_ArgumentException(AnsiTextStyle style)
        {
            Assert.ThrowsException<ArgumentException>(() => AnsiEscapeUtility.GetRemoveStyle(style));
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
        public void GetRemoveStyle_Success(AnsiTextStyle style, string expected)
        {
            var result = AnsiEscapeUtility.GetRemoveStyle(style);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(256)]
        public void GetSetForegroundColor_ColorCode_ArgumentException(AnsiColorCode colorCode)
        {
            Assert.ThrowsException<ArgumentException>(() => AnsiEscapeUtility.GetSetForegroundColor(colorCode));
        }

        [TestMethod]
        [DataRow(AnsiColorCode.Black, "\u001b[38;5;0m")]
        [DataRow(AnsiColorCode.DarkRed, "\u001b[38;5;1m")]
        [DataRow(AnsiColorCode.DarkGreen, "\u001b[38;5;2m")]
        [DataRow(AnsiColorCode.DarkYellow, "\u001b[38;5;3m")]
        [DataRow(AnsiColorCode.DarkBlue, "\u001b[38;5;4m")]
        [DataRow(AnsiColorCode.DarkMagenta, "\u001b[38;5;5m")]
        [DataRow(AnsiColorCode.DarkCyan, "\u001b[38;5;6m")]
        [DataRow(AnsiColorCode.Gray, "\u001b[38;5;7m")]
        [DataRow(AnsiColorCode.DarkGray, "\u001b[38;5;8m")]
        [DataRow(AnsiColorCode.Red, "\u001b[38;5;9m")]
        [DataRow(AnsiColorCode.Green, "\u001b[38;5;10m")]
        [DataRow(AnsiColorCode.Yellow, "\u001b[38;5;11m")]
        [DataRow(AnsiColorCode.Blue, "\u001b[38;5;12m")]
        [DataRow(AnsiColorCode.Magenta, "\u001b[38;5;13m")]
        [DataRow(AnsiColorCode.Cyan, "\u001b[38;5;14m")]
        [DataRow(AnsiColorCode.White, "\u001b[38;5;15m")]
        [DataRow(AnsiColorCode.RGB_000, "\u001b[38;5;16m")]
        [DataRow(AnsiColorCode.RGB_001, "\u001b[38;5;17m")]
        [DataRow(AnsiColorCode.RGB_002, "\u001b[38;5;18m")]
        [DataRow(AnsiColorCode.RGB_003, "\u001b[38;5;19m")]
        [DataRow(AnsiColorCode.RGB_004, "\u001b[38;5;20m")]
        [DataRow(AnsiColorCode.RGB_005, "\u001b[38;5;21m")]
        [DataRow(AnsiColorCode.RGB_010, "\u001b[38;5;22m")]
        [DataRow(AnsiColorCode.RGB_011, "\u001b[38;5;23m")]
        [DataRow(AnsiColorCode.RGB_012, "\u001b[38;5;24m")]
        [DataRow(AnsiColorCode.RGB_013, "\u001b[38;5;25m")]
        [DataRow(AnsiColorCode.RGB_014, "\u001b[38;5;26m")]
        [DataRow(AnsiColorCode.RGB_015, "\u001b[38;5;27m")]
        [DataRow(AnsiColorCode.RGB_020, "\u001b[38;5;28m")]
        [DataRow(AnsiColorCode.RGB_021, "\u001b[38;5;29m")]
        [DataRow(AnsiColorCode.RGB_022, "\u001b[38;5;30m")]
        [DataRow(AnsiColorCode.RGB_023, "\u001b[38;5;31m")]
        [DataRow(AnsiColorCode.RGB_024, "\u001b[38;5;32m")]
        [DataRow(AnsiColorCode.RGB_025, "\u001b[38;5;33m")]
        [DataRow(AnsiColorCode.RGB_030, "\u001b[38;5;34m")]
        [DataRow(AnsiColorCode.RGB_031, "\u001b[38;5;35m")]
        [DataRow(AnsiColorCode.RGB_032, "\u001b[38;5;36m")]
        [DataRow(AnsiColorCode.RGB_033, "\u001b[38;5;37m")]
        [DataRow(AnsiColorCode.RGB_034, "\u001b[38;5;38m")]
        [DataRow(AnsiColorCode.RGB_035, "\u001b[38;5;39m")]
        [DataRow(AnsiColorCode.RGB_040, "\u001b[38;5;40m")]
        [DataRow(AnsiColorCode.RGB_041, "\u001b[38;5;41m")]
        [DataRow(AnsiColorCode.RGB_042, "\u001b[38;5;42m")]
        [DataRow(AnsiColorCode.RGB_043, "\u001b[38;5;43m")]
        [DataRow(AnsiColorCode.RGB_044, "\u001b[38;5;44m")]
        [DataRow(AnsiColorCode.RGB_045, "\u001b[38;5;45m")]
        [DataRow(AnsiColorCode.RGB_050, "\u001b[38;5;46m")]
        [DataRow(AnsiColorCode.RGB_051, "\u001b[38;5;47m")]
        [DataRow(AnsiColorCode.RGB_052, "\u001b[38;5;48m")]
        [DataRow(AnsiColorCode.RGB_053, "\u001b[38;5;49m")]
        [DataRow(AnsiColorCode.RGB_054, "\u001b[38;5;50m")]
        [DataRow(AnsiColorCode.RGB_055, "\u001b[38;5;51m")]
        [DataRow(AnsiColorCode.RGB_100, "\u001b[38;5;52m")]
        [DataRow(AnsiColorCode.RGB_101, "\u001b[38;5;53m")]
        [DataRow(AnsiColorCode.RGB_102, "\u001b[38;5;54m")]
        [DataRow(AnsiColorCode.RGB_103, "\u001b[38;5;55m")]
        [DataRow(AnsiColorCode.RGB_104, "\u001b[38;5;56m")]
        [DataRow(AnsiColorCode.RGB_105, "\u001b[38;5;57m")]
        [DataRow(AnsiColorCode.RGB_110, "\u001b[38;5;58m")]
        [DataRow(AnsiColorCode.RGB_111, "\u001b[38;5;59m")]
        [DataRow(AnsiColorCode.RGB_112, "\u001b[38;5;60m")]
        [DataRow(AnsiColorCode.RGB_113, "\u001b[38;5;61m")]
        [DataRow(AnsiColorCode.RGB_114, "\u001b[38;5;62m")]
        [DataRow(AnsiColorCode.RGB_115, "\u001b[38;5;63m")]
        [DataRow(AnsiColorCode.RGB_120, "\u001b[38;5;64m")]
        [DataRow(AnsiColorCode.RGB_121, "\u001b[38;5;65m")]
        [DataRow(AnsiColorCode.RGB_122, "\u001b[38;5;66m")]
        [DataRow(AnsiColorCode.RGB_123, "\u001b[38;5;67m")]
        [DataRow(AnsiColorCode.RGB_124, "\u001b[38;5;68m")]
        [DataRow(AnsiColorCode.RGB_125, "\u001b[38;5;69m")]
        [DataRow(AnsiColorCode.RGB_130, "\u001b[38;5;70m")]
        [DataRow(AnsiColorCode.RGB_131, "\u001b[38;5;71m")]
        [DataRow(AnsiColorCode.RGB_132, "\u001b[38;5;72m")]
        [DataRow(AnsiColorCode.RGB_133, "\u001b[38;5;73m")]
        [DataRow(AnsiColorCode.RGB_134, "\u001b[38;5;74m")]
        [DataRow(AnsiColorCode.RGB_135, "\u001b[38;5;75m")]
        [DataRow(AnsiColorCode.RGB_140, "\u001b[38;5;76m")]
        [DataRow(AnsiColorCode.RGB_141, "\u001b[38;5;77m")]
        [DataRow(AnsiColorCode.RGB_142, "\u001b[38;5;78m")]
        [DataRow(AnsiColorCode.RGB_143, "\u001b[38;5;79m")]
        [DataRow(AnsiColorCode.RGB_144, "\u001b[38;5;80m")]
        [DataRow(AnsiColorCode.RGB_145, "\u001b[38;5;81m")]
        [DataRow(AnsiColorCode.RGB_150, "\u001b[38;5;82m")]
        [DataRow(AnsiColorCode.RGB_151, "\u001b[38;5;83m")]
        [DataRow(AnsiColorCode.RGB_152, "\u001b[38;5;84m")]
        [DataRow(AnsiColorCode.RGB_153, "\u001b[38;5;85m")]
        [DataRow(AnsiColorCode.RGB_154, "\u001b[38;5;86m")]
        [DataRow(AnsiColorCode.RGB_155, "\u001b[38;5;87m")]
        [DataRow(AnsiColorCode.RGB_200, "\u001b[38;5;88m")]
        [DataRow(AnsiColorCode.RGB_201, "\u001b[38;5;89m")]
        [DataRow(AnsiColorCode.RGB_202, "\u001b[38;5;90m")]
        [DataRow(AnsiColorCode.RGB_203, "\u001b[38;5;91m")]
        [DataRow(AnsiColorCode.RGB_204, "\u001b[38;5;92m")]
        [DataRow(AnsiColorCode.RGB_205, "\u001b[38;5;93m")]
        [DataRow(AnsiColorCode.RGB_210, "\u001b[38;5;94m")]
        [DataRow(AnsiColorCode.RGB_211, "\u001b[38;5;95m")]
        [DataRow(AnsiColorCode.RGB_212, "\u001b[38;5;96m")]
        [DataRow(AnsiColorCode.RGB_213, "\u001b[38;5;97m")]
        [DataRow(AnsiColorCode.RGB_214, "\u001b[38;5;98m")]
        [DataRow(AnsiColorCode.RGB_215, "\u001b[38;5;99m")]
        [DataRow(AnsiColorCode.RGB_220, "\u001b[38;5;100m")]
        [DataRow(AnsiColorCode.RGB_221, "\u001b[38;5;101m")]
        [DataRow(AnsiColorCode.RGB_222, "\u001b[38;5;102m")]
        [DataRow(AnsiColorCode.RGB_223, "\u001b[38;5;103m")]
        [DataRow(AnsiColorCode.RGB_224, "\u001b[38;5;104m")]
        [DataRow(AnsiColorCode.RGB_225, "\u001b[38;5;105m")]
        [DataRow(AnsiColorCode.RGB_230, "\u001b[38;5;106m")]
        [DataRow(AnsiColorCode.RGB_231, "\u001b[38;5;107m")]
        [DataRow(AnsiColorCode.RGB_232, "\u001b[38;5;108m")]
        [DataRow(AnsiColorCode.RGB_233, "\u001b[38;5;109m")]
        [DataRow(AnsiColorCode.RGB_234, "\u001b[38;5;110m")]
        [DataRow(AnsiColorCode.RGB_235, "\u001b[38;5;111m")]
        [DataRow(AnsiColorCode.RGB_240, "\u001b[38;5;112m")]
        [DataRow(AnsiColorCode.RGB_241, "\u001b[38;5;113m")]
        [DataRow(AnsiColorCode.RGB_242, "\u001b[38;5;114m")]
        [DataRow(AnsiColorCode.RGB_243, "\u001b[38;5;115m")]
        [DataRow(AnsiColorCode.RGB_244, "\u001b[38;5;116m")]
        [DataRow(AnsiColorCode.RGB_245, "\u001b[38;5;117m")]
        [DataRow(AnsiColorCode.RGB_250, "\u001b[38;5;118m")]
        [DataRow(AnsiColorCode.RGB_251, "\u001b[38;5;119m")]
        [DataRow(AnsiColorCode.RGB_252, "\u001b[38;5;120m")]
        [DataRow(AnsiColorCode.RGB_253, "\u001b[38;5;121m")]
        [DataRow(AnsiColorCode.RGB_254, "\u001b[38;5;122m")]
        [DataRow(AnsiColorCode.RGB_255, "\u001b[38;5;123m")]
        [DataRow(AnsiColorCode.RGB_300, "\u001b[38;5;124m")]
        [DataRow(AnsiColorCode.RGB_301, "\u001b[38;5;125m")]
        [DataRow(AnsiColorCode.RGB_302, "\u001b[38;5;126m")]
        [DataRow(AnsiColorCode.RGB_303, "\u001b[38;5;127m")]
        [DataRow(AnsiColorCode.RGB_304, "\u001b[38;5;128m")]
        [DataRow(AnsiColorCode.RGB_305, "\u001b[38;5;129m")]
        [DataRow(AnsiColorCode.RGB_310, "\u001b[38;5;130m")]
        [DataRow(AnsiColorCode.RGB_311, "\u001b[38;5;131m")]
        [DataRow(AnsiColorCode.RGB_312, "\u001b[38;5;132m")]
        [DataRow(AnsiColorCode.RGB_313, "\u001b[38;5;133m")]
        [DataRow(AnsiColorCode.RGB_314, "\u001b[38;5;134m")]
        [DataRow(AnsiColorCode.RGB_315, "\u001b[38;5;135m")]
        [DataRow(AnsiColorCode.RGB_320, "\u001b[38;5;136m")]
        [DataRow(AnsiColorCode.RGB_321, "\u001b[38;5;137m")]
        [DataRow(AnsiColorCode.RGB_322, "\u001b[38;5;138m")]
        [DataRow(AnsiColorCode.RGB_323, "\u001b[38;5;139m")]
        [DataRow(AnsiColorCode.RGB_324, "\u001b[38;5;140m")]
        [DataRow(AnsiColorCode.RGB_325, "\u001b[38;5;141m")]
        [DataRow(AnsiColorCode.RGB_330, "\u001b[38;5;142m")]
        [DataRow(AnsiColorCode.RGB_331, "\u001b[38;5;143m")]
        [DataRow(AnsiColorCode.RGB_332, "\u001b[38;5;144m")]
        [DataRow(AnsiColorCode.RGB_333, "\u001b[38;5;145m")]
        [DataRow(AnsiColorCode.RGB_334, "\u001b[38;5;146m")]
        [DataRow(AnsiColorCode.RGB_335, "\u001b[38;5;147m")]
        [DataRow(AnsiColorCode.RGB_340, "\u001b[38;5;148m")]
        [DataRow(AnsiColorCode.RGB_341, "\u001b[38;5;149m")]
        [DataRow(AnsiColorCode.RGB_342, "\u001b[38;5;150m")]
        [DataRow(AnsiColorCode.RGB_343, "\u001b[38;5;151m")]
        [DataRow(AnsiColorCode.RGB_344, "\u001b[38;5;152m")]
        [DataRow(AnsiColorCode.RGB_345, "\u001b[38;5;153m")]
        [DataRow(AnsiColorCode.RGB_350, "\u001b[38;5;154m")]
        [DataRow(AnsiColorCode.RGB_351, "\u001b[38;5;155m")]
        [DataRow(AnsiColorCode.RGB_352, "\u001b[38;5;156m")]
        [DataRow(AnsiColorCode.RGB_353, "\u001b[38;5;157m")]
        [DataRow(AnsiColorCode.RGB_354, "\u001b[38;5;158m")]
        [DataRow(AnsiColorCode.RGB_355, "\u001b[38;5;159m")]
        [DataRow(AnsiColorCode.RGB_400, "\u001b[38;5;160m")]
        [DataRow(AnsiColorCode.RGB_401, "\u001b[38;5;161m")]
        [DataRow(AnsiColorCode.RGB_402, "\u001b[38;5;162m")]
        [DataRow(AnsiColorCode.RGB_403, "\u001b[38;5;163m")]
        [DataRow(AnsiColorCode.RGB_404, "\u001b[38;5;164m")]
        [DataRow(AnsiColorCode.RGB_405, "\u001b[38;5;165m")]
        [DataRow(AnsiColorCode.RGB_410, "\u001b[38;5;166m")]
        [DataRow(AnsiColorCode.RGB_411, "\u001b[38;5;167m")]
        [DataRow(AnsiColorCode.RGB_412, "\u001b[38;5;168m")]
        [DataRow(AnsiColorCode.RGB_413, "\u001b[38;5;169m")]
        [DataRow(AnsiColorCode.RGB_414, "\u001b[38;5;170m")]
        [DataRow(AnsiColorCode.RGB_415, "\u001b[38;5;171m")]
        [DataRow(AnsiColorCode.RGB_420, "\u001b[38;5;172m")]
        [DataRow(AnsiColorCode.RGB_421, "\u001b[38;5;173m")]
        [DataRow(AnsiColorCode.RGB_422, "\u001b[38;5;174m")]
        [DataRow(AnsiColorCode.RGB_423, "\u001b[38;5;175m")]
        [DataRow(AnsiColorCode.RGB_424, "\u001b[38;5;176m")]
        [DataRow(AnsiColorCode.RGB_425, "\u001b[38;5;177m")]
        [DataRow(AnsiColorCode.RGB_430, "\u001b[38;5;178m")]
        [DataRow(AnsiColorCode.RGB_431, "\u001b[38;5;179m")]
        [DataRow(AnsiColorCode.RGB_432, "\u001b[38;5;180m")]
        [DataRow(AnsiColorCode.RGB_433, "\u001b[38;5;181m")]
        [DataRow(AnsiColorCode.RGB_434, "\u001b[38;5;182m")]
        [DataRow(AnsiColorCode.RGB_435, "\u001b[38;5;183m")]
        [DataRow(AnsiColorCode.RGB_440, "\u001b[38;5;184m")]
        [DataRow(AnsiColorCode.RGB_441, "\u001b[38;5;185m")]
        [DataRow(AnsiColorCode.RGB_442, "\u001b[38;5;186m")]
        [DataRow(AnsiColorCode.RGB_443, "\u001b[38;5;187m")]
        [DataRow(AnsiColorCode.RGB_444, "\u001b[38;5;188m")]
        [DataRow(AnsiColorCode.RGB_445, "\u001b[38;5;189m")]
        [DataRow(AnsiColorCode.RGB_450, "\u001b[38;5;190m")]
        [DataRow(AnsiColorCode.RGB_451, "\u001b[38;5;191m")]
        [DataRow(AnsiColorCode.RGB_452, "\u001b[38;5;192m")]
        [DataRow(AnsiColorCode.RGB_453, "\u001b[38;5;193m")]
        [DataRow(AnsiColorCode.RGB_454, "\u001b[38;5;194m")]
        [DataRow(AnsiColorCode.RGB_455, "\u001b[38;5;195m")]
        [DataRow(AnsiColorCode.RGB_500, "\u001b[38;5;196m")]
        [DataRow(AnsiColorCode.RGB_501, "\u001b[38;5;197m")]
        [DataRow(AnsiColorCode.RGB_502, "\u001b[38;5;198m")]
        [DataRow(AnsiColorCode.RGB_503, "\u001b[38;5;199m")]
        [DataRow(AnsiColorCode.RGB_504, "\u001b[38;5;200m")]
        [DataRow(AnsiColorCode.RGB_505, "\u001b[38;5;201m")]
        [DataRow(AnsiColorCode.RGB_510, "\u001b[38;5;202m")]
        [DataRow(AnsiColorCode.RGB_511, "\u001b[38;5;203m")]
        [DataRow(AnsiColorCode.RGB_512, "\u001b[38;5;204m")]
        [DataRow(AnsiColorCode.RGB_513, "\u001b[38;5;205m")]
        [DataRow(AnsiColorCode.RGB_514, "\u001b[38;5;206m")]
        [DataRow(AnsiColorCode.RGB_515, "\u001b[38;5;207m")]
        [DataRow(AnsiColorCode.RGB_520, "\u001b[38;5;208m")]
        [DataRow(AnsiColorCode.RGB_521, "\u001b[38;5;209m")]
        [DataRow(AnsiColorCode.RGB_522, "\u001b[38;5;210m")]
        [DataRow(AnsiColorCode.RGB_523, "\u001b[38;5;211m")]
        [DataRow(AnsiColorCode.RGB_524, "\u001b[38;5;212m")]
        [DataRow(AnsiColorCode.RGB_525, "\u001b[38;5;213m")]
        [DataRow(AnsiColorCode.RGB_530, "\u001b[38;5;214m")]
        [DataRow(AnsiColorCode.RGB_531, "\u001b[38;5;215m")]
        [DataRow(AnsiColorCode.RGB_532, "\u001b[38;5;216m")]
        [DataRow(AnsiColorCode.RGB_533, "\u001b[38;5;217m")]
        [DataRow(AnsiColorCode.RGB_534, "\u001b[38;5;218m")]
        [DataRow(AnsiColorCode.RGB_535, "\u001b[38;5;219m")]
        [DataRow(AnsiColorCode.RGB_540, "\u001b[38;5;220m")]
        [DataRow(AnsiColorCode.RGB_541, "\u001b[38;5;221m")]
        [DataRow(AnsiColorCode.RGB_542, "\u001b[38;5;222m")]
        [DataRow(AnsiColorCode.RGB_543, "\u001b[38;5;223m")]
        [DataRow(AnsiColorCode.RGB_544, "\u001b[38;5;224m")]
        [DataRow(AnsiColorCode.RGB_545, "\u001b[38;5;225m")]
        [DataRow(AnsiColorCode.RGB_550, "\u001b[38;5;226m")]
        [DataRow(AnsiColorCode.RGB_551, "\u001b[38;5;227m")]
        [DataRow(AnsiColorCode.RGB_552, "\u001b[38;5;228m")]
        [DataRow(AnsiColorCode.RGB_553, "\u001b[38;5;229m")]
        [DataRow(AnsiColorCode.RGB_554, "\u001b[38;5;230m")]
        [DataRow(AnsiColorCode.RGB_555, "\u001b[38;5;231m")]
        [DataRow(AnsiColorCode.Grayscale_00, "\u001b[38;5;232m")]
        [DataRow(AnsiColorCode.Grayscale_01, "\u001b[38;5;233m")]
        [DataRow(AnsiColorCode.Grayscale_02, "\u001b[38;5;234m")]
        [DataRow(AnsiColorCode.Grayscale_03, "\u001b[38;5;235m")]
        [DataRow(AnsiColorCode.Grayscale_04, "\u001b[38;5;236m")]
        [DataRow(AnsiColorCode.Grayscale_05, "\u001b[38;5;237m")]
        [DataRow(AnsiColorCode.Grayscale_06, "\u001b[38;5;238m")]
        [DataRow(AnsiColorCode.Grayscale_07, "\u001b[38;5;239m")]
        [DataRow(AnsiColorCode.Grayscale_08, "\u001b[38;5;240m")]
        [DataRow(AnsiColorCode.Grayscale_09, "\u001b[38;5;241m")]
        [DataRow(AnsiColorCode.Grayscale_10, "\u001b[38;5;242m")]
        [DataRow(AnsiColorCode.Grayscale_11, "\u001b[38;5;243m")]
        [DataRow(AnsiColorCode.Grayscale_12, "\u001b[38;5;244m")]
        [DataRow(AnsiColorCode.Grayscale_13, "\u001b[38;5;245m")]
        [DataRow(AnsiColorCode.Grayscale_14, "\u001b[38;5;246m")]
        [DataRow(AnsiColorCode.Grayscale_15, "\u001b[38;5;247m")]
        [DataRow(AnsiColorCode.Grayscale_16, "\u001b[38;5;248m")]
        [DataRow(AnsiColorCode.Grayscale_17, "\u001b[38;5;249m")]
        [DataRow(AnsiColorCode.Grayscale_18, "\u001b[38;5;250m")]
        [DataRow(AnsiColorCode.Grayscale_19, "\u001b[38;5;251m")]
        [DataRow(AnsiColorCode.Grayscale_20, "\u001b[38;5;252m")]
        [DataRow(AnsiColorCode.Grayscale_21, "\u001b[38;5;253m")]
        [DataRow(AnsiColorCode.Grayscale_22, "\u001b[38;5;254m")]
        [DataRow(AnsiColorCode.Grayscale_23, "\u001b[38;5;255m")]
        public void GetSetForegroundColor_ColorCode_Success(AnsiColorCode colorCode, string expected)
        {
            var result = AnsiEscapeUtility.GetSetForegroundColor(colorCode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow((byte)128, (byte)0, (byte)64, "\u001b[38;2;128;0;64m")]
        [DataRow((byte)0, (byte)34, (byte)156, "\u001b[38;2;0;34;156m")]
        [DataRow((byte)42, (byte)136, (byte)0, "\u001b[38;2;42;136;0m")]
        public void GetSetForegroundColor_RGB_Success(byte red, byte green, byte blue, string expected)
        {
            var result = AnsiEscapeUtility.GetSetForegroundColor(red, green, blue);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetResetForegroundColor_Success()
        {
            var result = AnsiEscapeUtility.GetResetForegroundColor();
            Assert.AreEqual("\u001b[39m", result);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(256)]
        public void GetSetBackgroundColor_ColorCode_ArgumentException(AnsiColorCode colorCode)
        {
            Assert.ThrowsException<ArgumentException>(() => AnsiEscapeUtility.GetSetBackgroundColor(colorCode));
        }

        [TestMethod]
        [DataRow(AnsiColorCode.Black, "\u001b[48;5;0m")]
        [DataRow(AnsiColorCode.DarkRed, "\u001b[48;5;1m")]
        [DataRow(AnsiColorCode.DarkGreen, "\u001b[48;5;2m")]
        [DataRow(AnsiColorCode.DarkYellow, "\u001b[48;5;3m")]
        [DataRow(AnsiColorCode.DarkBlue, "\u001b[48;5;4m")]
        [DataRow(AnsiColorCode.DarkMagenta, "\u001b[48;5;5m")]
        [DataRow(AnsiColorCode.DarkCyan, "\u001b[48;5;6m")]
        [DataRow(AnsiColorCode.Gray, "\u001b[48;5;7m")]
        [DataRow(AnsiColorCode.DarkGray, "\u001b[48;5;8m")]
        [DataRow(AnsiColorCode.Red, "\u001b[48;5;9m")]
        [DataRow(AnsiColorCode.Green, "\u001b[48;5;10m")]
        [DataRow(AnsiColorCode.Yellow, "\u001b[48;5;11m")]
        [DataRow(AnsiColorCode.Blue, "\u001b[48;5;12m")]
        [DataRow(AnsiColorCode.Magenta, "\u001b[48;5;13m")]
        [DataRow(AnsiColorCode.Cyan, "\u001b[48;5;14m")]
        [DataRow(AnsiColorCode.White, "\u001b[48;5;15m")]
        [DataRow(AnsiColorCode.RGB_000, "\u001b[48;5;16m")]
        [DataRow(AnsiColorCode.RGB_001, "\u001b[48;5;17m")]
        [DataRow(AnsiColorCode.RGB_002, "\u001b[48;5;18m")]
        [DataRow(AnsiColorCode.RGB_003, "\u001b[48;5;19m")]
        [DataRow(AnsiColorCode.RGB_004, "\u001b[48;5;20m")]
        [DataRow(AnsiColorCode.RGB_005, "\u001b[48;5;21m")]
        [DataRow(AnsiColorCode.RGB_010, "\u001b[48;5;22m")]
        [DataRow(AnsiColorCode.RGB_011, "\u001b[48;5;23m")]
        [DataRow(AnsiColorCode.RGB_012, "\u001b[48;5;24m")]
        [DataRow(AnsiColorCode.RGB_013, "\u001b[48;5;25m")]
        [DataRow(AnsiColorCode.RGB_014, "\u001b[48;5;26m")]
        [DataRow(AnsiColorCode.RGB_015, "\u001b[48;5;27m")]
        [DataRow(AnsiColorCode.RGB_020, "\u001b[48;5;28m")]
        [DataRow(AnsiColorCode.RGB_021, "\u001b[48;5;29m")]
        [DataRow(AnsiColorCode.RGB_022, "\u001b[48;5;30m")]
        [DataRow(AnsiColorCode.RGB_023, "\u001b[48;5;31m")]
        [DataRow(AnsiColorCode.RGB_024, "\u001b[48;5;32m")]
        [DataRow(AnsiColorCode.RGB_025, "\u001b[48;5;33m")]
        [DataRow(AnsiColorCode.RGB_030, "\u001b[48;5;34m")]
        [DataRow(AnsiColorCode.RGB_031, "\u001b[48;5;35m")]
        [DataRow(AnsiColorCode.RGB_032, "\u001b[48;5;36m")]
        [DataRow(AnsiColorCode.RGB_033, "\u001b[48;5;37m")]
        [DataRow(AnsiColorCode.RGB_034, "\u001b[48;5;38m")]
        [DataRow(AnsiColorCode.RGB_035, "\u001b[48;5;39m")]
        [DataRow(AnsiColorCode.RGB_040, "\u001b[48;5;40m")]
        [DataRow(AnsiColorCode.RGB_041, "\u001b[48;5;41m")]
        [DataRow(AnsiColorCode.RGB_042, "\u001b[48;5;42m")]
        [DataRow(AnsiColorCode.RGB_043, "\u001b[48;5;43m")]
        [DataRow(AnsiColorCode.RGB_044, "\u001b[48;5;44m")]
        [DataRow(AnsiColorCode.RGB_045, "\u001b[48;5;45m")]
        [DataRow(AnsiColorCode.RGB_050, "\u001b[48;5;46m")]
        [DataRow(AnsiColorCode.RGB_051, "\u001b[48;5;47m")]
        [DataRow(AnsiColorCode.RGB_052, "\u001b[48;5;48m")]
        [DataRow(AnsiColorCode.RGB_053, "\u001b[48;5;49m")]
        [DataRow(AnsiColorCode.RGB_054, "\u001b[48;5;50m")]
        [DataRow(AnsiColorCode.RGB_055, "\u001b[48;5;51m")]
        [DataRow(AnsiColorCode.RGB_100, "\u001b[48;5;52m")]
        [DataRow(AnsiColorCode.RGB_101, "\u001b[48;5;53m")]
        [DataRow(AnsiColorCode.RGB_102, "\u001b[48;5;54m")]
        [DataRow(AnsiColorCode.RGB_103, "\u001b[48;5;55m")]
        [DataRow(AnsiColorCode.RGB_104, "\u001b[48;5;56m")]
        [DataRow(AnsiColorCode.RGB_105, "\u001b[48;5;57m")]
        [DataRow(AnsiColorCode.RGB_110, "\u001b[48;5;58m")]
        [DataRow(AnsiColorCode.RGB_111, "\u001b[48;5;59m")]
        [DataRow(AnsiColorCode.RGB_112, "\u001b[48;5;60m")]
        [DataRow(AnsiColorCode.RGB_113, "\u001b[48;5;61m")]
        [DataRow(AnsiColorCode.RGB_114, "\u001b[48;5;62m")]
        [DataRow(AnsiColorCode.RGB_115, "\u001b[48;5;63m")]
        [DataRow(AnsiColorCode.RGB_120, "\u001b[48;5;64m")]
        [DataRow(AnsiColorCode.RGB_121, "\u001b[48;5;65m")]
        [DataRow(AnsiColorCode.RGB_122, "\u001b[48;5;66m")]
        [DataRow(AnsiColorCode.RGB_123, "\u001b[48;5;67m")]
        [DataRow(AnsiColorCode.RGB_124, "\u001b[48;5;68m")]
        [DataRow(AnsiColorCode.RGB_125, "\u001b[48;5;69m")]
        [DataRow(AnsiColorCode.RGB_130, "\u001b[48;5;70m")]
        [DataRow(AnsiColorCode.RGB_131, "\u001b[48;5;71m")]
        [DataRow(AnsiColorCode.RGB_132, "\u001b[48;5;72m")]
        [DataRow(AnsiColorCode.RGB_133, "\u001b[48;5;73m")]
        [DataRow(AnsiColorCode.RGB_134, "\u001b[48;5;74m")]
        [DataRow(AnsiColorCode.RGB_135, "\u001b[48;5;75m")]
        [DataRow(AnsiColorCode.RGB_140, "\u001b[48;5;76m")]
        [DataRow(AnsiColorCode.RGB_141, "\u001b[48;5;77m")]
        [DataRow(AnsiColorCode.RGB_142, "\u001b[48;5;78m")]
        [DataRow(AnsiColorCode.RGB_143, "\u001b[48;5;79m")]
        [DataRow(AnsiColorCode.RGB_144, "\u001b[48;5;80m")]
        [DataRow(AnsiColorCode.RGB_145, "\u001b[48;5;81m")]
        [DataRow(AnsiColorCode.RGB_150, "\u001b[48;5;82m")]
        [DataRow(AnsiColorCode.RGB_151, "\u001b[48;5;83m")]
        [DataRow(AnsiColorCode.RGB_152, "\u001b[48;5;84m")]
        [DataRow(AnsiColorCode.RGB_153, "\u001b[48;5;85m")]
        [DataRow(AnsiColorCode.RGB_154, "\u001b[48;5;86m")]
        [DataRow(AnsiColorCode.RGB_155, "\u001b[48;5;87m")]
        [DataRow(AnsiColorCode.RGB_200, "\u001b[48;5;88m")]
        [DataRow(AnsiColorCode.RGB_201, "\u001b[48;5;89m")]
        [DataRow(AnsiColorCode.RGB_202, "\u001b[48;5;90m")]
        [DataRow(AnsiColorCode.RGB_203, "\u001b[48;5;91m")]
        [DataRow(AnsiColorCode.RGB_204, "\u001b[48;5;92m")]
        [DataRow(AnsiColorCode.RGB_205, "\u001b[48;5;93m")]
        [DataRow(AnsiColorCode.RGB_210, "\u001b[48;5;94m")]
        [DataRow(AnsiColorCode.RGB_211, "\u001b[48;5;95m")]
        [DataRow(AnsiColorCode.RGB_212, "\u001b[48;5;96m")]
        [DataRow(AnsiColorCode.RGB_213, "\u001b[48;5;97m")]
        [DataRow(AnsiColorCode.RGB_214, "\u001b[48;5;98m")]
        [DataRow(AnsiColorCode.RGB_215, "\u001b[48;5;99m")]
        [DataRow(AnsiColorCode.RGB_220, "\u001b[48;5;100m")]
        [DataRow(AnsiColorCode.RGB_221, "\u001b[48;5;101m")]
        [DataRow(AnsiColorCode.RGB_222, "\u001b[48;5;102m")]
        [DataRow(AnsiColorCode.RGB_223, "\u001b[48;5;103m")]
        [DataRow(AnsiColorCode.RGB_224, "\u001b[48;5;104m")]
        [DataRow(AnsiColorCode.RGB_225, "\u001b[48;5;105m")]
        [DataRow(AnsiColorCode.RGB_230, "\u001b[48;5;106m")]
        [DataRow(AnsiColorCode.RGB_231, "\u001b[48;5;107m")]
        [DataRow(AnsiColorCode.RGB_232, "\u001b[48;5;108m")]
        [DataRow(AnsiColorCode.RGB_233, "\u001b[48;5;109m")]
        [DataRow(AnsiColorCode.RGB_234, "\u001b[48;5;110m")]
        [DataRow(AnsiColorCode.RGB_235, "\u001b[48;5;111m")]
        [DataRow(AnsiColorCode.RGB_240, "\u001b[48;5;112m")]
        [DataRow(AnsiColorCode.RGB_241, "\u001b[48;5;113m")]
        [DataRow(AnsiColorCode.RGB_242, "\u001b[48;5;114m")]
        [DataRow(AnsiColorCode.RGB_243, "\u001b[48;5;115m")]
        [DataRow(AnsiColorCode.RGB_244, "\u001b[48;5;116m")]
        [DataRow(AnsiColorCode.RGB_245, "\u001b[48;5;117m")]
        [DataRow(AnsiColorCode.RGB_250, "\u001b[48;5;118m")]
        [DataRow(AnsiColorCode.RGB_251, "\u001b[48;5;119m")]
        [DataRow(AnsiColorCode.RGB_252, "\u001b[48;5;120m")]
        [DataRow(AnsiColorCode.RGB_253, "\u001b[48;5;121m")]
        [DataRow(AnsiColorCode.RGB_254, "\u001b[48;5;122m")]
        [DataRow(AnsiColorCode.RGB_255, "\u001b[48;5;123m")]
        [DataRow(AnsiColorCode.RGB_300, "\u001b[48;5;124m")]
        [DataRow(AnsiColorCode.RGB_301, "\u001b[48;5;125m")]
        [DataRow(AnsiColorCode.RGB_302, "\u001b[48;5;126m")]
        [DataRow(AnsiColorCode.RGB_303, "\u001b[48;5;127m")]
        [DataRow(AnsiColorCode.RGB_304, "\u001b[48;5;128m")]
        [DataRow(AnsiColorCode.RGB_305, "\u001b[48;5;129m")]
        [DataRow(AnsiColorCode.RGB_310, "\u001b[48;5;130m")]
        [DataRow(AnsiColorCode.RGB_311, "\u001b[48;5;131m")]
        [DataRow(AnsiColorCode.RGB_312, "\u001b[48;5;132m")]
        [DataRow(AnsiColorCode.RGB_313, "\u001b[48;5;133m")]
        [DataRow(AnsiColorCode.RGB_314, "\u001b[48;5;134m")]
        [DataRow(AnsiColorCode.RGB_315, "\u001b[48;5;135m")]
        [DataRow(AnsiColorCode.RGB_320, "\u001b[48;5;136m")]
        [DataRow(AnsiColorCode.RGB_321, "\u001b[48;5;137m")]
        [DataRow(AnsiColorCode.RGB_322, "\u001b[48;5;138m")]
        [DataRow(AnsiColorCode.RGB_323, "\u001b[48;5;139m")]
        [DataRow(AnsiColorCode.RGB_324, "\u001b[48;5;140m")]
        [DataRow(AnsiColorCode.RGB_325, "\u001b[48;5;141m")]
        [DataRow(AnsiColorCode.RGB_330, "\u001b[48;5;142m")]
        [DataRow(AnsiColorCode.RGB_331, "\u001b[48;5;143m")]
        [DataRow(AnsiColorCode.RGB_332, "\u001b[48;5;144m")]
        [DataRow(AnsiColorCode.RGB_333, "\u001b[48;5;145m")]
        [DataRow(AnsiColorCode.RGB_334, "\u001b[48;5;146m")]
        [DataRow(AnsiColorCode.RGB_335, "\u001b[48;5;147m")]
        [DataRow(AnsiColorCode.RGB_340, "\u001b[48;5;148m")]
        [DataRow(AnsiColorCode.RGB_341, "\u001b[48;5;149m")]
        [DataRow(AnsiColorCode.RGB_342, "\u001b[48;5;150m")]
        [DataRow(AnsiColorCode.RGB_343, "\u001b[48;5;151m")]
        [DataRow(AnsiColorCode.RGB_344, "\u001b[48;5;152m")]
        [DataRow(AnsiColorCode.RGB_345, "\u001b[48;5;153m")]
        [DataRow(AnsiColorCode.RGB_350, "\u001b[48;5;154m")]
        [DataRow(AnsiColorCode.RGB_351, "\u001b[48;5;155m")]
        [DataRow(AnsiColorCode.RGB_352, "\u001b[48;5;156m")]
        [DataRow(AnsiColorCode.RGB_353, "\u001b[48;5;157m")]
        [DataRow(AnsiColorCode.RGB_354, "\u001b[48;5;158m")]
        [DataRow(AnsiColorCode.RGB_355, "\u001b[48;5;159m")]
        [DataRow(AnsiColorCode.RGB_400, "\u001b[48;5;160m")]
        [DataRow(AnsiColorCode.RGB_401, "\u001b[48;5;161m")]
        [DataRow(AnsiColorCode.RGB_402, "\u001b[48;5;162m")]
        [DataRow(AnsiColorCode.RGB_403, "\u001b[48;5;163m")]
        [DataRow(AnsiColorCode.RGB_404, "\u001b[48;5;164m")]
        [DataRow(AnsiColorCode.RGB_405, "\u001b[48;5;165m")]
        [DataRow(AnsiColorCode.RGB_410, "\u001b[48;5;166m")]
        [DataRow(AnsiColorCode.RGB_411, "\u001b[48;5;167m")]
        [DataRow(AnsiColorCode.RGB_412, "\u001b[48;5;168m")]
        [DataRow(AnsiColorCode.RGB_413, "\u001b[48;5;169m")]
        [DataRow(AnsiColorCode.RGB_414, "\u001b[48;5;170m")]
        [DataRow(AnsiColorCode.RGB_415, "\u001b[48;5;171m")]
        [DataRow(AnsiColorCode.RGB_420, "\u001b[48;5;172m")]
        [DataRow(AnsiColorCode.RGB_421, "\u001b[48;5;173m")]
        [DataRow(AnsiColorCode.RGB_422, "\u001b[48;5;174m")]
        [DataRow(AnsiColorCode.RGB_423, "\u001b[48;5;175m")]
        [DataRow(AnsiColorCode.RGB_424, "\u001b[48;5;176m")]
        [DataRow(AnsiColorCode.RGB_425, "\u001b[48;5;177m")]
        [DataRow(AnsiColorCode.RGB_430, "\u001b[48;5;178m")]
        [DataRow(AnsiColorCode.RGB_431, "\u001b[48;5;179m")]
        [DataRow(AnsiColorCode.RGB_432, "\u001b[48;5;180m")]
        [DataRow(AnsiColorCode.RGB_433, "\u001b[48;5;181m")]
        [DataRow(AnsiColorCode.RGB_434, "\u001b[48;5;182m")]
        [DataRow(AnsiColorCode.RGB_435, "\u001b[48;5;183m")]
        [DataRow(AnsiColorCode.RGB_440, "\u001b[48;5;184m")]
        [DataRow(AnsiColorCode.RGB_441, "\u001b[48;5;185m")]
        [DataRow(AnsiColorCode.RGB_442, "\u001b[48;5;186m")]
        [DataRow(AnsiColorCode.RGB_443, "\u001b[48;5;187m")]
        [DataRow(AnsiColorCode.RGB_444, "\u001b[48;5;188m")]
        [DataRow(AnsiColorCode.RGB_445, "\u001b[48;5;189m")]
        [DataRow(AnsiColorCode.RGB_450, "\u001b[48;5;190m")]
        [DataRow(AnsiColorCode.RGB_451, "\u001b[48;5;191m")]
        [DataRow(AnsiColorCode.RGB_452, "\u001b[48;5;192m")]
        [DataRow(AnsiColorCode.RGB_453, "\u001b[48;5;193m")]
        [DataRow(AnsiColorCode.RGB_454, "\u001b[48;5;194m")]
        [DataRow(AnsiColorCode.RGB_455, "\u001b[48;5;195m")]
        [DataRow(AnsiColorCode.RGB_500, "\u001b[48;5;196m")]
        [DataRow(AnsiColorCode.RGB_501, "\u001b[48;5;197m")]
        [DataRow(AnsiColorCode.RGB_502, "\u001b[48;5;198m")]
        [DataRow(AnsiColorCode.RGB_503, "\u001b[48;5;199m")]
        [DataRow(AnsiColorCode.RGB_504, "\u001b[48;5;200m")]
        [DataRow(AnsiColorCode.RGB_505, "\u001b[48;5;201m")]
        [DataRow(AnsiColorCode.RGB_510, "\u001b[48;5;202m")]
        [DataRow(AnsiColorCode.RGB_511, "\u001b[48;5;203m")]
        [DataRow(AnsiColorCode.RGB_512, "\u001b[48;5;204m")]
        [DataRow(AnsiColorCode.RGB_513, "\u001b[48;5;205m")]
        [DataRow(AnsiColorCode.RGB_514, "\u001b[48;5;206m")]
        [DataRow(AnsiColorCode.RGB_515, "\u001b[48;5;207m")]
        [DataRow(AnsiColorCode.RGB_520, "\u001b[48;5;208m")]
        [DataRow(AnsiColorCode.RGB_521, "\u001b[48;5;209m")]
        [DataRow(AnsiColorCode.RGB_522, "\u001b[48;5;210m")]
        [DataRow(AnsiColorCode.RGB_523, "\u001b[48;5;211m")]
        [DataRow(AnsiColorCode.RGB_524, "\u001b[48;5;212m")]
        [DataRow(AnsiColorCode.RGB_525, "\u001b[48;5;213m")]
        [DataRow(AnsiColorCode.RGB_530, "\u001b[48;5;214m")]
        [DataRow(AnsiColorCode.RGB_531, "\u001b[48;5;215m")]
        [DataRow(AnsiColorCode.RGB_532, "\u001b[48;5;216m")]
        [DataRow(AnsiColorCode.RGB_533, "\u001b[48;5;217m")]
        [DataRow(AnsiColorCode.RGB_534, "\u001b[48;5;218m")]
        [DataRow(AnsiColorCode.RGB_535, "\u001b[48;5;219m")]
        [DataRow(AnsiColorCode.RGB_540, "\u001b[48;5;220m")]
        [DataRow(AnsiColorCode.RGB_541, "\u001b[48;5;221m")]
        [DataRow(AnsiColorCode.RGB_542, "\u001b[48;5;222m")]
        [DataRow(AnsiColorCode.RGB_543, "\u001b[48;5;223m")]
        [DataRow(AnsiColorCode.RGB_544, "\u001b[48;5;224m")]
        [DataRow(AnsiColorCode.RGB_545, "\u001b[48;5;225m")]
        [DataRow(AnsiColorCode.RGB_550, "\u001b[48;5;226m")]
        [DataRow(AnsiColorCode.RGB_551, "\u001b[48;5;227m")]
        [DataRow(AnsiColorCode.RGB_552, "\u001b[48;5;228m")]
        [DataRow(AnsiColorCode.RGB_553, "\u001b[48;5;229m")]
        [DataRow(AnsiColorCode.RGB_554, "\u001b[48;5;230m")]
        [DataRow(AnsiColorCode.RGB_555, "\u001b[48;5;231m")]
        [DataRow(AnsiColorCode.Grayscale_00, "\u001b[48;5;232m")]
        [DataRow(AnsiColorCode.Grayscale_01, "\u001b[48;5;233m")]
        [DataRow(AnsiColorCode.Grayscale_02, "\u001b[48;5;234m")]
        [DataRow(AnsiColorCode.Grayscale_03, "\u001b[48;5;235m")]
        [DataRow(AnsiColorCode.Grayscale_04, "\u001b[48;5;236m")]
        [DataRow(AnsiColorCode.Grayscale_05, "\u001b[48;5;237m")]
        [DataRow(AnsiColorCode.Grayscale_06, "\u001b[48;5;238m")]
        [DataRow(AnsiColorCode.Grayscale_07, "\u001b[48;5;239m")]
        [DataRow(AnsiColorCode.Grayscale_08, "\u001b[48;5;240m")]
        [DataRow(AnsiColorCode.Grayscale_09, "\u001b[48;5;241m")]
        [DataRow(AnsiColorCode.Grayscale_10, "\u001b[48;5;242m")]
        [DataRow(AnsiColorCode.Grayscale_11, "\u001b[48;5;243m")]
        [DataRow(AnsiColorCode.Grayscale_12, "\u001b[48;5;244m")]
        [DataRow(AnsiColorCode.Grayscale_13, "\u001b[48;5;245m")]
        [DataRow(AnsiColorCode.Grayscale_14, "\u001b[48;5;246m")]
        [DataRow(AnsiColorCode.Grayscale_15, "\u001b[48;5;247m")]
        [DataRow(AnsiColorCode.Grayscale_16, "\u001b[48;5;248m")]
        [DataRow(AnsiColorCode.Grayscale_17, "\u001b[48;5;249m")]
        [DataRow(AnsiColorCode.Grayscale_18, "\u001b[48;5;250m")]
        [DataRow(AnsiColorCode.Grayscale_19, "\u001b[48;5;251m")]
        [DataRow(AnsiColorCode.Grayscale_20, "\u001b[48;5;252m")]
        [DataRow(AnsiColorCode.Grayscale_21, "\u001b[48;5;253m")]
        [DataRow(AnsiColorCode.Grayscale_22, "\u001b[48;5;254m")]
        [DataRow(AnsiColorCode.Grayscale_23, "\u001b[48;5;255m")]
        public void GetSetBackgroundColor_ColorCode_Success(AnsiColorCode colorCode, string expected)
        {
            var result = AnsiEscapeUtility.GetSetBackgroundColor(colorCode);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow((byte)128, (byte)0, (byte)64, "\u001b[48;2;128;0;64m")]
        [DataRow((byte)0, (byte)34, (byte)156, "\u001b[48;2;0;34;156m")]
        [DataRow((byte)42, (byte)136, (byte)0, "\u001b[48;2;42;136;0m")]
        public void GetSetBackgroundColor_RGB_Success(byte red, byte green, byte blue, string expected)
        {
            var result = AnsiEscapeUtility.GetSetBackgroundColor(red, green, blue);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetResetBackgroundColor_Success()
        {
            var result = AnsiEscapeUtility.GetResetBackgroundColor();
            Assert.AreEqual("\u001b[49m", result);
        }
    }
}
