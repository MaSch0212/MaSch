﻿using MaSch.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MaSch.Console.Controls
{
    public enum TextWrap
    {
        NoWrap,
        CharacterWrap,
        WordWrap
    }

    public enum TextEllipsis
    {
        None,
        EndCharacter,
        StartCharacter,
        CenterCharacter,
        EndWord,
        StartWord,
        CenterWord
    }

    public enum TextAlignment
    {
        Left,
        Center,
        Right,
        Block
    }

    public partial class TextBlockControl
    {
        private readonly IConsoleService _console;

        public int X { get; set; } = 0;
        public int? Y { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

        public TextWrap? TextWrap { get; set; }
        public TextEllipsis TextEllipsis { get; set; } = TextEllipsis.EndCharacter;
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;

        public string? Text { get; set; }
        public ConsoleColor? ForegroundColor { get; set; }
        public ConsoleColor? BackgroundColor { get; set; }

        public int ActualWidth => Math.Min(Width ?? int.MaxValue, _console.BufferSize.Width - X - 1);

        public TextBlockControl(IConsoleService console)
        {
            _console = console;
        }

        public void Render()
        {
            if (Height < 1)
                return;

            if (Y.HasValue)
                _console.CursorPosition.Y = Y.Value;

            var lines = GetTextLines();
            for (int i = 0; i < lines.Length; i++)
            {
                RenderLineImpl(lines[i]);
                if (i != lines.Length - 1)
                    _console.WriteLine();
            }
        }

        public void RenderLine(string line, bool isLastLine)
        {
            var maxWidth = ActualWidth;
            RenderLineImpl(AlignText(TrimText(line, maxWidth, TextEllipsis), maxWidth, TextAlignment, isLastLine));
        }

        private void RenderLineImpl(string line)
        {
            _console.CursorPosition.X = X;
            _console.WriteWithColor(line, ForegroundColor ?? _console.ForegroundColor, BackgroundColor ?? _console.BackgroundColor);
        }

        public string[] GetTextLines()
        {
            if (Height < 1)
                return Array.Empty<string>();

            var wrap = TextWrap ?? (Height == 1 ? Controls.TextWrap.NoWrap : Controls.TextWrap.WordWrap);
            var ellipsis = TextEllipsis;
            var maxLength = ActualWidth;

            if (string.IsNullOrEmpty(Text))
                return Array.Empty<string>();

            if ((!Height.HasValue || Height > 1) && wrap != Controls.TextWrap.NoWrap)
                return FormatAndTrimLines(WrapText(Text, maxLength, wrap), maxLength, Height ?? int.MaxValue, ellipsis, TextAlignment);
            else
                return new[] { Text.Length > maxLength ? TrimText(Text, maxLength, ellipsis) : AlignText(Text, maxLength, TextAlignment, false) };
        }

        private static string[] WrapText(string text, int maxLineLength, TextWrap wrap)
        {
            var lines = text.Replace("\r", "").Split(new[] { '\n' }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = "\n" + lines[i];
            return wrap switch
            {
                Controls.TextWrap.CharacterWrap => lines.SelectMany(x => WrapCharacter(x, maxLineLength)).ToArray(),
                Controls.TextWrap.WordWrap => lines.SelectMany(x => WrapWord(x, maxLineLength)).ToArray(),
                _ => new[] { text }
            };

            static IEnumerable<string> WrapCharacter(string text, int maxLineLength)
            {
                for (int i = 0; i < text.Length; i += maxLineLength)
                {
                    var extra = 0;
                    if (i > 0)
                    {
                        for (; text[i + extra] == ' ' && (i + extra) < text.Length; extra++) ;
                        if ((i + extra) >= text.Length)
                        {
                            yield return text[i..];
                            break;
                        }
                    }
                    yield return text.Substring(i, Math.Min(maxLineLength, text.Length - i));
                }
            }

            static IEnumerable<string> WrapWord(string text, int maxLineLength)
            {
                var matches = Regex.Matches(text, @"\W");

                var i = 0;
                foreach (var (cidx, nidx) in matches.OfType<Match>().Select(x => x.Index).Append(text.Length - 1).Distinct().WithNext())
                {
                    var ridx = text[cidx] == ' ' ? cidx - 1 : cidx;
                    var rnidx = nidx > 0 && text[nidx] == ' ' ? nidx - 1 : nidx;

                    while ((ridx - i) >= maxLineLength)
                    {
                        yield return text.Substring(i, maxLineLength);
                        i += maxLineLength;
                    }

                    if (nidx == 0 || (rnidx - i) >= maxLineLength)
                    {
                        yield return text[i..(cidx + 1)];
                        i = cidx + 1;
                    }
                }
            }
        }

        private static string AlignText(string text, int maxLength, TextAlignment alignment, bool isLastLine)
        {
            if (text.Length == maxLength)
                return text;
            if (isLastLine && alignment == TextAlignment.Block)
                alignment = TextAlignment.Left;

            return alignment switch
            {
                TextAlignment.Right => text.PadLeft(maxLength, ' '),
                TextAlignment.Center => text.PadBoth(maxLength, ' '),
                TextAlignment.Block => BlockAlign(text, maxLength),
                _ => text.PadRight(maxLength, ' ')
            };

            static string BlockAlign(string text, int maxLength)
            {
                var matches = Regex.Matches(text, @"\s+");
                if (matches.Count == 0)
                    return text.PadBoth(maxLength, ' ');

                var startMatches = new Stack<Match>(matches.OfType<Match>().Where(x => x.Index <= text.Length / 2D));
                var endMatches = new Stack<Match>(matches.OfType<Match>().Where(x => x.Index > text.Length / 2D).Reverse());
                var cycle = new LinkedList<(Match match, int count)>();

                while (startMatches.Any() && endMatches.Any())
                {
                    var iS = startMatches.Peek().Index;
                    var iE = text.Length - (endMatches.Peek().Index + endMatches.Peek().Length);
                    cycle.AddLast(((iS < iE ? endMatches : startMatches).Pop(), 1));
                }
                while (endMatches.Count > 0)
                    cycle.AddLast((endMatches.Pop(), 1));
                while (startMatches.Count > 0)
                    cycle.AddLast((startMatches.Pop(), 1));

                var current = cycle.First;
                for (int i = 0; i < maxLength - text.Length; i++)
                {
                    current!.Value = (current.Value.match, current.Value.count + 1);
                    current = current.Next ?? cycle.First;
                }

                var result = new StringBuilder();
                int lastIndex = 0;
                foreach ((Match match, int count) in cycle.OrderBy(x => x.match.Index))
                {
                    result.Append(text[lastIndex..match.Index]).Append(new string(' ', count));
                    lastIndex = match.Index + match.Length;
                }
                result.Append(text[lastIndex..]);
                return result.ToString();
            }
        }
        
        private static string[] FormatAndTrimLines(string[] lines, int maxWidth, int maxHeight, TextEllipsis ellipsis, TextAlignment alignment)
        {
            var result = ellipsis switch
            {
                _ when lines.Length <= maxHeight => lines,
                TextEllipsis.EndCharacter or TextEllipsis.EndWord => EndTrimming(),
                TextEllipsis.StartCharacter or TextEllipsis.StartWord => StartTrimming(),
                TextEllipsis.CenterCharacter or TextEllipsis.CenterWord => CenterTrimming(),
                _ => lines[..maxHeight]
            };

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = AlignText(result[i][0] == '\n' ? result[i][1..].TrimEnd() : result[i].Trim(), maxWidth, alignment, i == result.Length - 1);
            }

            return result;

            string[] EndTrimming()
            {
                var r = lines[..maxHeight];
                r[^1] = Trim(r[^1] + lines[maxHeight]);
                return r;
            }

            string[] StartTrimming()
            {
                var r = lines[^maxHeight..];
                r[0] = Trim(lines[^(maxHeight - 1)] + r[0]);
                return r;
            }

            string[] CenterTrimming()
            {
                var n = maxHeight / 2;
                var r1 = lines[..n];
                var r2 = lines[^n..];

                if (maxHeight % 2 == 0)
                {
                    r1[^1] = Trim(r1[^1] + lines[n], ellipsis == TextEllipsis.CenterCharacter ? TextEllipsis.EndCharacter : TextEllipsis.EndWord);
                    r2[0] = Trim(lines[^(n + 1)] + r2[0], ellipsis == TextEllipsis.CenterCharacter ? TextEllipsis.StartCharacter : TextEllipsis.StartWord);
                    return r1.Concat(r2).ToArray();
                }
                else
                {
                    var middle = Trim(lines[n] + lines[^(n + 1)]);
                    return r1.Append(middle).Concat(r2).ToArray();
                }
            }

            string Trim(string s, TextEllipsis? e = null) => TrimText(s[0] == '\n' ? s.TrimEnd() : s.Trim(), maxWidth, e ?? ellipsis);
        }

        private static string TrimText(string text, int maxLength, TextEllipsis ellipsis)
        {
            if (text.Length <= maxLength)
                return text;
            if (maxLength < 3)
                return string.Empty;
            var result = ellipsis switch
            {
                TextEllipsis.EndCharacter => TrimEndCharacter(text, maxLength),
                TextEllipsis.StartCharacter => TrimStartCharacter(text, maxLength),
                TextEllipsis.CenterCharacter => TrimCenterCharacter(text, maxLength),
                TextEllipsis.EndWord => TrimEndWords(text, maxLength),
                TextEllipsis.StartWord => TrimStartWords(text, maxLength),
                TextEllipsis.CenterWord => TrimCenterWords(text, maxLength),
                _ => text[..maxLength]
            };
            return result.Length > maxLength ? result[..maxLength] : result;

            static string TrimEndCharacter(string text, int maxLength) => text[..(maxLength - 3)] + "...";
            static string TrimStartCharacter(string text, int maxLength) => "..." + text[^(maxLength - 3)..];
            static string TrimCenterCharacter(string text, int maxLength) => text[..((int)Math.Ceiling(maxLength / 2D) - 2)] + "..." + text[^((int)Math.Floor(maxLength / 2D) - 1)..];

            static string TrimEndWords(string text, int maxLength)
            {
                var matches = Regex.Matches(text, @"\W");
                string? result = null;
                foreach (Match match in matches.OfType<Match>().Reverse())
                {
                    if (match.Index <= (maxLength - 3))
                    {
                        result = text[..match.Index];
                        break;
                    }
                }
                return result == null ? TrimEndCharacter(text, maxLength) : result + "...";
            }

            static string TrimStartWords(string text, int maxLength)
            {
                var matches = Regex.Matches(text, @"\W");
                string? result = null;
                foreach (Match match in matches)
                {
                    var i = text.Length - (match.Index + match.Length);
                    if (i <= (maxLength - 3))
                    {
                        result = text[^i..];
                        break;
                    }
                }
                return result == null ? TrimStartWords(text, maxLength) : "..." + result;
            }

            static string TrimCenterWords(string text, int maxLength)
            {
                var matches = Regex.Matches(text, @"\W");
                var startMatches = new Stack<Match>(matches.OfType<Match>().Where(x => x.Index <= text.Length / 2D));
                var endMatches = new Stack<Match>(matches.OfType<Match>().Where(x => x.Index > text.Length / 2D).Reverse());

                string? start = null;
                string? end = null;
                while (startMatches.Any() && endMatches.Any())
                {
                    var iS = startMatches.Peek().Index;
                    var iE = text.Length - (endMatches.Peek().Index + endMatches.Peek().Length);
                    if (iS + iE <= maxLength - 3)
                    {
                        start = text[..iS];
                        end = text[^iE..];
                        break;
                    }
                    (iS < iE ? endMatches : startMatches).Pop();
                }
                return start == null && end == null ? TrimEndWords(text, maxLength) : start + "..." + end;
            }
        }
    }
}
