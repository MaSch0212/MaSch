using System;

namespace MaSch.Console.Ansi
{
    /// <summary>
    /// Specifies constants that define what should be cleared for the ANSI escape code <c>ESC[#J</c>.
    /// </summary>
    public enum AnsiClearMode
    {
        /// <summary>
        /// Clears everything on screen after the current cursor position.
        /// </summary>
        CurrentToEnd = 0,

        /// <summary>
        /// Clears everything on screen before the current cursor position.
        /// </summary>
        StartToCurrent = 1,

        /// <summary>
        /// Clears the whole screen.
        /// </summary>
        Screen = 2,

        /// <summary>
        /// Clears the scroll buffer of the terminal.
        /// </summary>
        Buffer = 3,
    }

    /// <summary>
    /// Specifies constants that define what should be cleared for the ANSI escape code <c>ESC[#K</c>".
    /// </summary>
    public enum AnsiLineClearMode
    {
        /// <summary>
        /// Clears the line after the current cursor position.
        /// </summary>
        CurrentToEnd = 0,

        /// <summary>
        /// Clears the line before the current cursor position.
        /// </summary>
        StartToCurrent = 1,

        /// <summary>
        /// Clears the entire line.
        /// </summary>
        EntireLine = 2,
    }

    /// <summary>
    /// Specifies constants that define text styles that can be used with ANSI escape codes.
    /// </summary>
    [Flags]
    public enum AnsiTextStyle
    {
        /// <summary>
        /// No style.
        /// </summary>
        None = 0,

        /// <summary>
        /// Bold text.
        /// </summary>
        Bold = 1,

        /// <summary>
        /// Fainted or dimmed text or text with decreased intensity.
        /// </summary>
        Faint = 2,

        /// <summary>
        /// Italic text.
        /// </summary>
        Italic = 4,

        /// <summary>
        /// Underlined text.
        /// </summary>
        Underline = 8,

        /// <summary>
        /// Blinking text.
        /// </summary>
        Blink = 16,

        /// <summary>
        /// Crossed-out text.
        /// </summary>
        CrossedOut = 32,

        /// <summary>
        /// Doubly underlined text.
        /// </summary>
        DoublyUnderlined = 64,

        /// <summary>
        /// Overlined text.
        /// </summary>
        Overlined = 128,

        /// <summary>
        /// Swap foreground and background colors.
        /// </summary>
        Invert = 256,

        /// <summary>
        /// All styles.
        /// </summary>
        All = Bold | Faint | Italic | Underline | Blink | CrossedOut | DoublyUnderlined | Overlined | Invert,
    }

    /// <summary>
    /// Specifies constants that define color codes for the ANSI escape codes <c>ESC[38;5;#m</c> and <c>ESC[48;5;#m</c>".
    /// See <see href="https://en.wikipedia.org/wiki/ANSI_escape_code#8-bit"/> for color table.
    /// </summary>
    public enum AnsiColorCode
    {
        /// <summary>
        /// Black. Actual color specified by the terminal.
        /// </summary>
        Black = 0,

        /// <summary>
        /// Dark red. Actual color specified by the terminal.
        /// </summary>
        DarkRed = 1,

        /// <summary>
        /// Dark green. Actual color specified by the terminal.
        /// </summary>
        DarkGreen = 2,

        /// <summary>
        /// Dark yellow. Actual color specified by the terminal.
        /// </summary>
        DarkYellow = 3,

        /// <summary>
        /// Dark blue. Actual color specified by the terminal.
        /// </summary>
        DarkBlue = 4,

        /// <summary>
        /// Dark magenta. Actual color specified by the terminal.
        /// </summary>
        DarkMagenta = 5,

        /// <summary>
        /// Dark cyan. Actual color specified by the terminal.
        /// </summary>
        DarkCyan = 6,

        /// <summary>
        /// Gray. Actual color specified by the terminal.
        /// </summary>
        Gray = 7,

        /// <summary>
        /// Dark gray. Actual color specified by the terminal.
        /// </summary>
        DarkGray = 8,

        /// <summary>
        /// Red. Actual color specified by the terminal.
        /// </summary>
        Red = 9,

        /// <summary>
        /// Green. Actual color specified by the terminal.
        /// </summary>
        Green = 10,

        /// <summary>
        /// Yellow. Actual color specified by the terminal.
        /// </summary>
        Yellow = 11,

        /// <summary>
        /// Blue. Actual color specified by the terminal.
        /// </summary>
        Blue = 12,

        /// <summary>
        /// Magenta. Actual color specified by the terminal.
        /// </summary>
        Magenta = 13,

        /// <summary>
        /// Cyan. Actual color specified by the terminal.
        /// </summary>
        Cyan = 14,

        /// <summary>
        /// White. Actual color specified by the terminal.
        /// </summary>
        White = 15,

        /// <summary>
        /// #000000 - rgb(0, 0, 0).
        /// </summary>
        RGB_000 = 16,

        /// <summary>
        /// #00005f - rgb(0, 0, 95).
        /// </summary>
        RGB_001 = 17,

        /// <summary>
        /// #000087 - rgb(0, 0, 135).
        /// </summary>
        RGB_002 = 18,

        /// <summary>
        /// #0000af - rgb(0, 0, 175).
        /// </summary>
        RGB_003 = 19,

        /// <summary>
        /// #0000d7 - rgb(0, 0, 215).
        /// </summary>
        RGB_004 = 20,

        /// <summary>
        /// #0000ff - rgb(0, 0, 255).
        /// </summary>
        RGB_005 = 21,

        /// <summary>
        /// #005f00 - rgb(0, 95, 0).
        /// </summary>
        RGB_010 = 22,

        /// <summary>
        /// #005f5f - rgb(0, 95, 95).
        /// </summary>
        RGB_011 = 23,

        /// <summary>
        /// #005f87 - rgb(0, 95, 135).
        /// </summary>
        RGB_012 = 24,

        /// <summary>
        /// #005faf - rgb(0, 95, 175).
        /// </summary>
        RGB_013 = 25,

        /// <summary>
        /// #005fd7 - rgb(0, 95, 215).
        /// </summary>
        RGB_014 = 26,

        /// <summary>
        /// #005fff - rgb(0, 95, 255).
        /// </summary>
        RGB_015 = 27,

        /// <summary>
        /// #008700 - rgb(0, 135, 0).
        /// </summary>
        RGB_020 = 28,

        /// <summary>
        /// #00875f - rgb(0, 135, 95).
        /// </summary>
        RGB_021 = 29,

        /// <summary>
        /// #008787 - rgb(0, 135, 135).
        /// </summary>
        RGB_022 = 30,

        /// <summary>
        /// #0087af - rgb(0, 135, 175).
        /// </summary>
        RGB_023 = 31,

        /// <summary>
        /// #0087d7 - rgb(0, 135, 215).
        /// </summary>
        RGB_024 = 32,

        /// <summary>
        /// #0087ff - rgb(0, 135, 255).
        /// </summary>
        RGB_025 = 33,

        /// <summary>
        /// #00af00 - rgb(0, 175, 0).
        /// </summary>
        RGB_030 = 34,

        /// <summary>
        /// #00af5f - rgb(0, 175, 95).
        /// </summary>
        RGB_031 = 35,

        /// <summary>
        /// #00af87 - rgb(0, 175, 135).
        /// </summary>
        RGB_032 = 36,

        /// <summary>
        /// #00afaf - rgb(0, 175, 175).
        /// </summary>
        RGB_033 = 37,

        /// <summary>
        /// #00afd7 - rgb(0, 175, 215).
        /// </summary>
        RGB_034 = 38,

        /// <summary>
        /// #00afff - rgb(0, 175, 255).
        /// </summary>
        RGB_035 = 39,

        /// <summary>
        /// #00d700 - rgb(0, 215, 0).
        /// </summary>
        RGB_040 = 40,

        /// <summary>
        /// #00d75f - rgb(0, 215, 95).
        /// </summary>
        RGB_041 = 41,

        /// <summary>
        /// #00d787 - rgb(0, 215, 135).
        /// </summary>
        RGB_042 = 42,

        /// <summary>
        /// #00d7af - rgb(0, 215, 175).
        /// </summary>
        RGB_043 = 43,

        /// <summary>
        /// #00d7d7 - rgb(0, 215, 215).
        /// </summary>
        RGB_044 = 44,

        /// <summary>
        /// #00d7ff - rgb(0, 215, 255).
        /// </summary>
        RGB_045 = 45,

        /// <summary>
        /// #00ff00 - rgb(0, 255, 0).
        /// </summary>
        RGB_050 = 46,

        /// <summary>
        /// #00ff5f - rgb(0, 255, 95).
        /// </summary>
        RGB_051 = 47,

        /// <summary>
        /// #00ff87 - rgb(0, 255, 135).
        /// </summary>
        RGB_052 = 48,

        /// <summary>
        /// #00ffaf - rgb(0, 255, 175).
        /// </summary>
        RGB_053 = 49,

        /// <summary>
        /// #00ffd7 - rgb(0, 255, 215).
        /// </summary>
        RGB_054 = 50,

        /// <summary>
        /// #00ffff - rgb(0, 255, 255).
        /// </summary>
        RGB_055 = 51,

        /// <summary>
        /// #5f0000 - rgb(95, 0, 0).
        /// </summary>
        RGB_100 = 52,

        /// <summary>
        /// #5f005f - rgb(95, 0, 95).
        /// </summary>
        RGB_101 = 53,

        /// <summary>
        /// #5f0087 - rgb(95, 0, 135).
        /// </summary>
        RGB_102 = 54,

        /// <summary>
        /// #5f00af - rgb(95, 0, 175).
        /// </summary>
        RGB_103 = 55,

        /// <summary>
        /// #5f00d7 - rgb(95, 0, 215).
        /// </summary>
        RGB_104 = 56,

        /// <summary>
        /// #5f00ff - rgb(95, 0, 255).
        /// </summary>
        RGB_105 = 57,

        /// <summary>
        /// #5f5f00 - rgb(95, 95, 0).
        /// </summary>
        RGB_110 = 58,

        /// <summary>
        /// #5f5f5f - rgb(95, 95, 95).
        /// </summary>
        RGB_111 = 59,

        /// <summary>
        /// #5f5f87 - rgb(95, 95, 135).
        /// </summary>
        RGB_112 = 60,

        /// <summary>
        /// #5f5faf - rgb(95, 95, 175).
        /// </summary>
        RGB_113 = 61,

        /// <summary>
        /// #5f5fd7 - rgb(95, 95, 215).
        /// </summary>
        RGB_114 = 62,

        /// <summary>
        /// #5f5fff - rgb(95, 95, 255).
        /// </summary>
        RGB_115 = 63,

        /// <summary>
        /// #5f8700 - rgb(95, 135, 0).
        /// </summary>
        RGB_120 = 64,

        /// <summary>
        /// #5f875f - rgb(95, 135, 95).
        /// </summary>
        RGB_121 = 65,

        /// <summary>
        /// #5f8787 - rgb(95, 135, 135).
        /// </summary>
        RGB_122 = 66,

        /// <summary>
        /// #5f87af - rgb(95, 135, 175).
        /// </summary>
        RGB_123 = 67,

        /// <summary>
        /// #5f87d7 - rgb(95, 135, 215).
        /// </summary>
        RGB_124 = 68,

        /// <summary>
        /// #5f87ff - rgb(95, 135, 255).
        /// </summary>
        RGB_125 = 69,

        /// <summary>
        /// #5faf00 - rgb(95, 175, 0).
        /// </summary>
        RGB_130 = 70,

        /// <summary>
        /// #5faf5f - rgb(95, 175, 95).
        /// </summary>
        RGB_131 = 71,

        /// <summary>
        /// #5faf87 - rgb(95, 175, 135).
        /// </summary>
        RGB_132 = 72,

        /// <summary>
        /// #5fafaf - rgb(95, 175, 175).
        /// </summary>
        RGB_133 = 73,

        /// <summary>
        /// #5fafd7 - rgb(95, 175, 215).
        /// </summary>
        RGB_134 = 74,

        /// <summary>
        /// #5fafff - rgb(95, 175, 255).
        /// </summary>
        RGB_135 = 75,

        /// <summary>
        /// #5fd700 - rgb(95, 215, 0).
        /// </summary>
        RGB_140 = 76,

        /// <summary>
        /// #5fd75f - rgb(95, 215, 95).
        /// </summary>
        RGB_141 = 77,

        /// <summary>
        /// #5fd787 - rgb(95, 215, 135).
        /// </summary>
        RGB_142 = 78,

        /// <summary>
        /// #5fd7af - rgb(95, 215, 175).
        /// </summary>
        RGB_143 = 79,

        /// <summary>
        /// #5fd7d7 - rgb(95, 215, 215).
        /// </summary>
        RGB_144 = 80,

        /// <summary>
        /// #5fd7ff - rgb(95, 215, 255).
        /// </summary>
        RGB_145 = 81,

        /// <summary>
        /// #5fff00 - rgb(95, 255, 0).
        /// </summary>
        RGB_150 = 82,

        /// <summary>
        /// #5fff5f - rgb(95, 255, 95).
        /// </summary>
        RGB_151 = 83,

        /// <summary>
        /// #5fff87 - rgb(95, 255, 135).
        /// </summary>
        RGB_152 = 84,

        /// <summary>
        /// #5fffaf - rgb(95, 255, 175).
        /// </summary>
        RGB_153 = 85,

        /// <summary>
        /// #5fffd7 - rgb(95, 255, 215).
        /// </summary>
        RGB_154 = 86,

        /// <summary>
        /// #5fffff - rgb(95, 255, 255).
        /// </summary>
        RGB_155 = 87,

        /// <summary>
        /// #870000 - rgb(135, 0, 0).
        /// </summary>
        RGB_200 = 88,

        /// <summary>
        /// #87005f - rgb(135, 0, 95).
        /// </summary>
        RGB_201 = 89,

        /// <summary>
        /// #870087 - rgb(135, 0, 135).
        /// </summary>
        RGB_202 = 90,

        /// <summary>
        /// #8700af - rgb(135, 0, 175).
        /// </summary>
        RGB_203 = 91,

        /// <summary>
        /// #8700d7 - rgb(135, 0, 215).
        /// </summary>
        RGB_204 = 92,

        /// <summary>
        /// #8700ff - rgb(135, 0, 255).
        /// </summary>
        RGB_205 = 93,

        /// <summary>
        /// #875f00 - rgb(135, 95, 0).
        /// </summary>
        RGB_210 = 94,

        /// <summary>
        /// #875f5f - rgb(135, 95, 95).
        /// </summary>
        RGB_211 = 95,

        /// <summary>
        /// #875f87 - rgb(135, 95, 135).
        /// </summary>
        RGB_212 = 96,

        /// <summary>
        /// #875faf - rgb(135, 95, 175).
        /// </summary>
        RGB_213 = 97,

        /// <summary>
        /// #875fd7 - rgb(135, 95, 215).
        /// </summary>
        RGB_214 = 98,

        /// <summary>
        /// #875fff - rgb(135, 95, 255).
        /// </summary>
        RGB_215 = 99,

        /// <summary>
        /// #878700 - rgb(135, 135, 0).
        /// </summary>
        RGB_220 = 100,

        /// <summary>
        /// #87875f - rgb(135, 135, 95).
        /// </summary>
        RGB_221 = 101,

        /// <summary>
        /// #878787 - rgb(135, 135, 135).
        /// </summary>
        RGB_222 = 102,

        /// <summary>
        /// #8787af - rgb(135, 135, 175).
        /// </summary>
        RGB_223 = 103,

        /// <summary>
        /// #8787d7 - rgb(135, 135, 215).
        /// </summary>
        RGB_224 = 104,

        /// <summary>
        /// #8787ff - rgb(135, 135, 255).
        /// </summary>
        RGB_225 = 105,

        /// <summary>
        /// #87af00 - rgb(135, 175, 0).
        /// </summary>
        RGB_230 = 106,

        /// <summary>
        /// #87af5f - rgb(135, 175, 95).
        /// </summary>
        RGB_231 = 107,

        /// <summary>
        /// #87af87 - rgb(135, 175, 135).
        /// </summary>
        RGB_232 = 108,

        /// <summary>
        /// #87afaf - rgb(135, 175, 175).
        /// </summary>
        RGB_233 = 109,

        /// <summary>
        /// #87afd7 - rgb(135, 175, 215).
        /// </summary>
        RGB_234 = 110,

        /// <summary>
        /// #87afff - rgb(135, 175, 255).
        /// </summary>
        RGB_235 = 111,

        /// <summary>
        /// #87d700 - rgb(135, 215, 0).
        /// </summary>
        RGB_240 = 112,

        /// <summary>
        /// #87d75f - rgb(135, 215, 95).
        /// </summary>
        RGB_241 = 113,

        /// <summary>
        /// #87d787 - rgb(135, 215, 135).
        /// </summary>
        RGB_242 = 114,

        /// <summary>
        /// #87d7af - rgb(135, 215, 175).
        /// </summary>
        RGB_243 = 115,

        /// <summary>
        /// #87d7d7 - rgb(135, 215, 215).
        /// </summary>
        RGB_244 = 116,

        /// <summary>
        /// #87d7ff - rgb(135, 215, 255).
        /// </summary>
        RGB_245 = 117,

        /// <summary>
        /// #87ff00 - rgb(135, 255, 0).
        /// </summary>
        RGB_250 = 118,

        /// <summary>
        /// #87ff5f - rgb(135, 255, 95).
        /// </summary>
        RGB_251 = 119,

        /// <summary>
        /// #87ff87 - rgb(135, 255, 135).
        /// </summary>
        RGB_252 = 120,

        /// <summary>
        /// #87ffaf - rgb(135, 255, 175).
        /// </summary>
        RGB_253 = 121,

        /// <summary>
        /// #87ffd7 - rgb(135, 255, 215).
        /// </summary>
        RGB_254 = 122,

        /// <summary>
        /// #87ffff - rgb(135, 255, 255).
        /// </summary>
        RGB_255 = 123,

        /// <summary>
        /// #af0000 - rgb(175, 0, 0).
        /// </summary>
        RGB_300 = 124,

        /// <summary>
        /// #af005f - rgb(175, 0, 95).
        /// </summary>
        RGB_301 = 125,

        /// <summary>
        /// #af0087 - rgb(175, 0, 135).
        /// </summary>
        RGB_302 = 126,

        /// <summary>
        /// #af00af - rgb(175, 0, 175).
        /// </summary>
        RGB_303 = 127,

        /// <summary>
        /// #af00d7 - rgb(175, 0, 215).
        /// </summary>
        RGB_304 = 128,

        /// <summary>
        /// #af00ff - rgb(175, 0, 255).
        /// </summary>
        RGB_305 = 129,

        /// <summary>
        /// #af5f00 - rgb(175, 95, 0).
        /// </summary>
        RGB_310 = 130,

        /// <summary>
        /// #af5f5f - rgb(175, 95, 95).
        /// </summary>
        RGB_311 = 131,

        /// <summary>
        /// #af5f87 - rgb(175, 95, 135).
        /// </summary>
        RGB_312 = 132,

        /// <summary>
        /// #af5faf - rgb(175, 95, 175).
        /// </summary>
        RGB_313 = 133,

        /// <summary>
        /// #af5fd7 - rgb(175, 95, 215).
        /// </summary>
        RGB_314 = 134,

        /// <summary>
        /// #af5fff - rgb(175, 95, 255).
        /// </summary>
        RGB_315 = 135,

        /// <summary>
        /// #af8700 - rgb(175, 135, 0).
        /// </summary>
        RGB_320 = 136,

        /// <summary>
        /// #af875f - rgb(175, 135, 95).
        /// </summary>
        RGB_321 = 137,

        /// <summary>
        /// #af8787 - rgb(175, 135, 135).
        /// </summary>
        RGB_322 = 138,

        /// <summary>
        /// #af87af - rgb(175, 135, 175).
        /// </summary>
        RGB_323 = 139,

        /// <summary>
        /// #af87d7 - rgb(175, 135, 215).
        /// </summary>
        RGB_324 = 140,

        /// <summary>
        /// #af87ff - rgb(175, 135, 255).
        /// </summary>
        RGB_325 = 141,

        /// <summary>
        /// #afaf00 - rgb(175, 175, 0).
        /// </summary>
        RGB_330 = 142,

        /// <summary>
        /// #afaf5f - rgb(175, 175, 95).
        /// </summary>
        RGB_331 = 143,

        /// <summary>
        /// #afaf87 - rgb(175, 175, 135).
        /// </summary>
        RGB_332 = 144,

        /// <summary>
        /// #afafaf - rgb(175, 175, 175).
        /// </summary>
        RGB_333 = 145,

        /// <summary>
        /// #afafd7 - rgb(175, 175, 215).
        /// </summary>
        RGB_334 = 146,

        /// <summary>
        /// #afafff - rgb(175, 175, 255).
        /// </summary>
        RGB_335 = 147,

        /// <summary>
        /// #afd700 - rgb(175, 215, 0).
        /// </summary>
        RGB_340 = 148,

        /// <summary>
        /// #afd75f - rgb(175, 215, 95).
        /// </summary>
        RGB_341 = 149,

        /// <summary>
        /// #afd787 - rgb(175, 215, 135).
        /// </summary>
        RGB_342 = 150,

        /// <summary>
        /// #afd7af - rgb(175, 215, 175).
        /// </summary>
        RGB_343 = 151,

        /// <summary>
        /// #afd7d7 - rgb(175, 215, 215).
        /// </summary>
        RGB_344 = 152,

        /// <summary>
        /// #afd7ff - rgb(175, 215, 255).
        /// </summary>
        RGB_345 = 153,

        /// <summary>
        /// #afff00 - rgb(175, 255, 0).
        /// </summary>
        RGB_350 = 154,

        /// <summary>
        /// #afff5f - rgb(175, 255, 95).
        /// </summary>
        RGB_351 = 155,

        /// <summary>
        /// #afff87 - rgb(175, 255, 135).
        /// </summary>
        RGB_352 = 156,

        /// <summary>
        /// #afffaf - rgb(175, 255, 175).
        /// </summary>
        RGB_353 = 157,

        /// <summary>
        /// #afffd7 - rgb(175, 255, 215).
        /// </summary>
        RGB_354 = 158,

        /// <summary>
        /// #afffff - rgb(175, 255, 255).
        /// </summary>
        RGB_355 = 159,

        /// <summary>
        /// #d70000 - rgb(215, 0, 0).
        /// </summary>
        RGB_400 = 160,

        /// <summary>
        /// #d7005f - rgb(215, 0, 95).
        /// </summary>
        RGB_401 = 161,

        /// <summary>
        /// #d70087 - rgb(215, 0, 135).
        /// </summary>
        RGB_402 = 162,

        /// <summary>
        /// #d700af - rgb(215, 0, 175).
        /// </summary>
        RGB_403 = 163,

        /// <summary>
        /// #d700d7 - rgb(215, 0, 215).
        /// </summary>
        RGB_404 = 164,

        /// <summary>
        /// #d700ff - rgb(215, 0, 255).
        /// </summary>
        RGB_405 = 165,

        /// <summary>
        /// #d75f00 - rgb(215, 95, 0).
        /// </summary>
        RGB_410 = 166,

        /// <summary>
        /// #d75f5f - rgb(215, 95, 95).
        /// </summary>
        RGB_411 = 167,

        /// <summary>
        /// #d75f87 - rgb(215, 95, 135).
        /// </summary>
        RGB_412 = 168,

        /// <summary>
        /// #d75faf - rgb(215, 95, 175).
        /// </summary>
        RGB_413 = 169,

        /// <summary>
        /// #d75fd7 - rgb(215, 95, 215).
        /// </summary>
        RGB_414 = 170,

        /// <summary>
        /// #d75fff - rgb(215, 95, 255).
        /// </summary>
        RGB_415 = 171,

        /// <summary>
        /// #d78700 - rgb(215, 135, 0).
        /// </summary>
        RGB_420 = 172,

        /// <summary>
        /// #d7875f - rgb(215, 135, 95).
        /// </summary>
        RGB_421 = 173,

        /// <summary>
        /// #d78787 - rgb(215, 135, 135).
        /// </summary>
        RGB_422 = 174,

        /// <summary>
        /// #d787af - rgb(215, 135, 175).
        /// </summary>
        RGB_423 = 175,

        /// <summary>
        /// #d787d7 - rgb(215, 135, 215).
        /// </summary>
        RGB_424 = 176,

        /// <summary>
        /// #d787ff - rgb(215, 135, 255).
        /// </summary>
        RGB_425 = 177,

        /// <summary>
        /// #d7af00 - rgb(215, 175, 0).
        /// </summary>
        RGB_430 = 178,

        /// <summary>
        /// #d7af5f - rgb(215, 175, 95).
        /// </summary>
        RGB_431 = 179,

        /// <summary>
        /// #d7af87 - rgb(215, 175, 135).
        /// </summary>
        RGB_432 = 180,

        /// <summary>
        /// #d7afaf - rgb(215, 175, 175).
        /// </summary>
        RGB_433 = 181,

        /// <summary>
        /// #d7afd7 - rgb(215, 175, 215).
        /// </summary>
        RGB_434 = 182,

        /// <summary>
        /// #d7afff - rgb(215, 175, 255).
        /// </summary>
        RGB_435 = 183,

        /// <summary>
        /// #d7d700 - rgb(215, 215, 0).
        /// </summary>
        RGB_440 = 184,

        /// <summary>
        /// #d7d75f - rgb(215, 215, 95).
        /// </summary>
        RGB_441 = 185,

        /// <summary>
        /// #d7d787 - rgb(215, 215, 135).
        /// </summary>
        RGB_442 = 186,

        /// <summary>
        /// #d7d7af - rgb(215, 215, 175).
        /// </summary>
        RGB_443 = 187,

        /// <summary>
        /// #d7d7d7 - rgb(215, 215, 215).
        /// </summary>
        RGB_444 = 188,

        /// <summary>
        /// #d7d7ff - rgb(215, 215, 255).
        /// </summary>
        RGB_445 = 189,

        /// <summary>
        /// #d7ff00 - rgb(215, 255, 0).
        /// </summary>
        RGB_450 = 190,

        /// <summary>
        /// #d7ff5f - rgb(215, 255, 95).
        /// </summary>
        RGB_451 = 191,

        /// <summary>
        /// #d7ff87 - rgb(215, 255, 135).
        /// </summary>
        RGB_452 = 192,

        /// <summary>
        /// #d7ffaf - rgb(215, 255, 175).
        /// </summary>
        RGB_453 = 193,

        /// <summary>
        /// #d7ffd7 - rgb(215, 255, 215).
        /// </summary>
        RGB_454 = 194,

        /// <summary>
        /// #d7ffff - rgb(215, 255, 255).
        /// </summary>
        RGB_455 = 195,

        /// <summary>
        /// #ff0000 - rgb(255, 0, 0).
        /// </summary>
        RGB_500 = 196,

        /// <summary>
        /// #ff005f - rgb(255, 0, 95).
        /// </summary>
        RGB_501 = 197,

        /// <summary>
        /// #ff0087 - rgb(255, 0, 135).
        /// </summary>
        RGB_502 = 198,

        /// <summary>
        /// #ff00af - rgb(255, 0, 175).
        /// </summary>
        RGB_503 = 199,

        /// <summary>
        /// #ff00d7 - rgb(255, 0, 215).
        /// </summary>
        RGB_504 = 200,

        /// <summary>
        /// #ff00ff - rgb(255, 0, 255).
        /// </summary>
        RGB_505 = 201,

        /// <summary>
        /// #ff5f00 - rgb(255, 95, 0).
        /// </summary>
        RGB_510 = 202,

        /// <summary>
        /// #ff5f5f - rgb(255, 95, 95).
        /// </summary>
        RGB_511 = 203,

        /// <summary>
        /// #ff5f87 - rgb(255, 95, 135).
        /// </summary>
        RGB_512 = 204,

        /// <summary>
        /// #ff5faf - rgb(255, 95, 175).
        /// </summary>
        RGB_513 = 205,

        /// <summary>
        /// #ff5fd7 - rgb(255, 95, 215).
        /// </summary>
        RGB_514 = 206,

        /// <summary>
        /// #ff5fff - rgb(255, 95, 255).
        /// </summary>
        RGB_515 = 207,

        /// <summary>
        /// #ff8700 - rgb(255, 135, 0).
        /// </summary>
        RGB_520 = 208,

        /// <summary>
        /// #ff875f - rgb(255, 135, 95).
        /// </summary>
        RGB_521 = 209,

        /// <summary>
        /// #ff8787 - rgb(255, 135, 135).
        /// </summary>
        RGB_522 = 210,

        /// <summary>
        /// #ff87af - rgb(255, 135, 175).
        /// </summary>
        RGB_523 = 211,

        /// <summary>
        /// #ff87d7 - rgb(255, 135, 215).
        /// </summary>
        RGB_524 = 212,

        /// <summary>
        /// #ff87ff - rgb(255, 135, 255).
        /// </summary>
        RGB_525 = 213,

        /// <summary>
        /// #ffaf00 - rgb(255, 175, 0).
        /// </summary>
        RGB_530 = 214,

        /// <summary>
        /// #ffaf5f - rgb(255, 175, 95).
        /// </summary>
        RGB_531 = 215,

        /// <summary>
        /// #ffaf87 - rgb(255, 175, 135).
        /// </summary>
        RGB_532 = 216,

        /// <summary>
        /// #ffafaf - rgb(255, 175, 175).
        /// </summary>
        RGB_533 = 217,

        /// <summary>
        /// #ffafd7 - rgb(255, 175, 215).
        /// </summary>
        RGB_534 = 218,

        /// <summary>
        /// #ffafff - rgb(255, 175, 255).
        /// </summary>
        RGB_535 = 219,

        /// <summary>
        /// #ffd700 - rgb(255, 215, 0).
        /// </summary>
        RGB_540 = 220,

        /// <summary>
        /// #ffd75f - rgb(255, 215, 95).
        /// </summary>
        RGB_541 = 221,

        /// <summary>
        /// #ffd787 - rgb(255, 215, 135).
        /// </summary>
        RGB_542 = 222,

        /// <summary>
        /// #ffd7af - rgb(255, 215, 175).
        /// </summary>
        RGB_543 = 223,

        /// <summary>
        /// #ffd7d7 - rgb(255, 215, 215).
        /// </summary>
        RGB_544 = 224,

        /// <summary>
        /// #ffd7ff - rgb(255, 215, 255).
        /// </summary>
        RGB_545 = 225,

        /// <summary>
        /// #ffff00 - rgb(255, 255, 0).
        /// </summary>
        RGB_550 = 226,

        /// <summary>
        /// #ffff5f - rgb(255, 255, 95).
        /// </summary>
        RGB_551 = 227,

        /// <summary>
        /// #ffff87 - rgb(255, 255, 135).
        /// </summary>
        RGB_552 = 228,

        /// <summary>
        /// #ffffaf - rgb(255, 255, 175).
        /// </summary>
        RGB_553 = 229,

        /// <summary>
        /// #ffffd7 - rgb(255, 255, 215).
        /// </summary>
        RGB_554 = 230,

        /// <summary>
        /// #ffffff - rgb(255, 255, 255).
        /// </summary>
        RGB_555 = 231,

        /// <summary>
        /// #080808 - rgb(8, 8, 8).
        /// </summary>
        Grayscale_00 = 232,

        /// <summary>
        /// #121212 - rgb(18, 18, 18).
        /// </summary>
        Grayscale_01 = 233,

        /// <summary>
        /// #1c1c1c - rgb(28, 28, 28).
        /// </summary>
        Grayscale_02 = 234,

        /// <summary>
        /// #262626 - rgb(38, 38, 38).
        /// </summary>
        Grayscale_03 = 235,

        /// <summary>
        /// #303030 - rgb(48, 48, 48).
        /// </summary>
        Grayscale_04 = 236,

        /// <summary>
        /// #3a3a3a - rgb(58, 58, 58).
        /// </summary>
        Grayscale_05 = 237,

        /// <summary>
        /// #444444 - rgb(68, 68, 68).
        /// </summary>
        Grayscale_06 = 238,

        /// <summary>
        /// #4e4e4e - rgb(78, 78, 78).
        /// </summary>
        Grayscale_07 = 239,

        /// <summary>
        /// #585858 - rgb(88, 88, 88).
        /// </summary>
        Grayscale_08 = 240,

        /// <summary>
        /// #626262 - rgb(98, 98, 98).
        /// </summary>
        Grayscale_09 = 241,

        /// <summary>
        /// #6c6c6c - rgb(108, 108, 108).
        /// </summary>
        Grayscale_10 = 242,

        /// <summary>
        /// #767676 - rgb(118, 118, 118).
        /// </summary>
        Grayscale_11 = 243,

        /// <summary>
        /// #808080 - rgb(128, 128, 128).
        /// </summary>
        Grayscale_12 = 244,

        /// <summary>
        /// #8a8a8a - rgb(138, 138, 138).
        /// </summary>
        Grayscale_13 = 245,

        /// <summary>
        /// #949494 - rgb(148, 148, 148).
        /// </summary>
        Grayscale_14 = 246,

        /// <summary>
        /// #9e9e9e - rgb(158, 158, 158).
        /// </summary>
        Grayscale_15 = 247,

        /// <summary>
        /// #a8a8a8 - rgb(168, 168, 168).
        /// </summary>
        Grayscale_16 = 248,

        /// <summary>
        /// #b2b2b2 - rgb(178, 178, 178).
        /// </summary>
        Grayscale_17 = 249,

        /// <summary>
        /// #bcbcbc - rgb(188, 188, 188).
        /// </summary>
        Grayscale_18 = 250,

        /// <summary>
        /// #c6c6c6 - rgb(198, 198, 198).
        /// </summary>
        Grayscale_19 = 251,

        /// <summary>
        /// #d0d0d0 - rgb(208, 208, 208).
        /// </summary>
        Grayscale_20 = 252,

        /// <summary>
        /// #dadada - rgb(218, 218, 218).
        /// </summary>
        Grayscale_21 = 253,

        /// <summary>
        /// #e4e4e4 - rgb(228, 228, 228).
        /// </summary>
        Grayscale_22 = 254,

        /// <summary>
        /// #eeeeee - rgb(238, 238, 238).
        /// </summary>
        Grayscale_23 = 255,
    }
}
