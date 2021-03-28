﻿using MaSch.Console.Controls.Table;
using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MaSch.Console.Controls
{
    /// <summary>
    /// Control for a <see cref="IConsoleService"/> that displays a table.
    /// </summary>
    public class TableControl
    {
        private readonly IConsoleService _console;

        /// <summary>
        /// Gets or sets the width of this control.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the space between columns.
        /// </summary>
        public int ColumnGutter { get; set; } = 3;

        /// <summary>
        /// Gets or sets a value indicating whether column headers should be shown.
        /// </summary>
        public bool ShowColumnHeaders { get; set; } = true;

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        public IList<Column> Columns { get; set; } = new List<Column>();

        /// <summary>
        /// Gets or sets the rows.
        /// </summary>
        public IList<Row> Rows { get; set; } = new List<Row>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TableControl"/> class.
        /// </summary>
        /// <param name="console">The console to use.</param>
        public TableControl(IConsoleService console)
        {
            _console = Guard.NotNull(console, nameof(console));
        }

        /// <summary>
        /// Renders this control.
        /// </summary>
        public void Render()
        {
            var width = _console.IsOutputRedirected ? Width ?? 240 : Math.Min(Width ?? (_console.BufferSize.Width - 1), _console.BufferSize.Width);
            var widths = GetColumnWidths(width);

            if (ShowColumnHeaders)
            {
                var headers = (from c in Columns.WithIndex()
                               select new TextBlockControl(_console)
                               {
                                   Text = c.Item.Header,
                                   Width = widths[c.Index],
                               }).ToArray();
                RenderRow(headers);
                RenderSeparator(widths);
            }

            foreach (var row in Rows)
            {
                if (row.IsSeperator)
                {
                    RenderSeparator(widths);
                    continue;
                }

                var textBlocks = (from c in row.Values.WithIndex()
                                  select new TextBlockControl(_console)
                                  {
                                      Text = c.Item,
                                      Width = widths[c.Index],
                                  }).ToArray();
                RenderRow(textBlocks);
            }
        }

        private void RenderSeparator(int[] widths)
        {
            bool isFirst = true;
            foreach (var w in widths)
            {
                if (!isFirst)
                    _console.Write(new string(' ', ColumnGutter));
                _console.Write(new string('-', w));
                isFirst = false;
            }

            if (_console.IsOutputRedirected || widths.Sum() + ((widths.Length - 1) * ColumnGutter) < _console.BufferSize.Width)
                _console.WriteLine();
        }

        private void RenderRow(IList<TextBlockControl> textBlocks)
        {
            var lines = textBlocks.Select(x => x.GetTextLines()).ToArray();
            var maxLineHeight = lines.Max(x => x.Length);
            var totalWidth = textBlocks.Sum(x => x.Width) + ((textBlocks.Count - 1) * ColumnGutter);
            for (int l = 0; l < maxLineHeight; l++)
            {
                bool isFirst = true;
                for (int c = 0; c < lines.Length; c++)
                {
                    if (!isFirst)
                        _console.Write(new string(' ', ColumnGutter));
                    RenderTextLine(textBlocks[c], lines[c].Length > l ? lines[c][l] : new string(' ', lines[c][0].Length));
                    isFirst = false;
                }

                if (_console.IsOutputRedirected || totalWidth < _console.BufferSize.Width)
                    _console.WriteLine();
            }
        }

        private void RenderTextLine(TextBlockControl textBlock, string? text)
        {
            _console.WriteWithColor(text, textBlock.ForegroundColor ?? _console.ForegroundColor, textBlock.BackgroundColor ?? _console.BackgroundColor);
        }

        private int[] GetColumnWidths(int width)
        {
            var columns = (from x in Columns.WithIndex()
                           let c = x.Item
                           select new ColumnWidthData
                           {
                               Index = x.Index,
                               Width = c.WidthMode switch
                               {
                                   ColumnWidthMode.Fixed => double.IsNaN(c.Width) ? 0 : (int)c.Width,
                                   ColumnWidthMode.Auto => Math.Max(ShowColumnHeaders ? c.Header?.Length ?? 0 : 0, Rows.Max(y => y.Values?.ElementAtOrDefault(x.Index)?.Length ?? 0)),
                                   _ => 0,
                               },
                               MinWidth = c.MinWidth ?? 0,
                               MaxWidth = c.MaxWidth ?? int.MaxValue,
                               StarWidth = c.WidthMode == ColumnWidthMode.Star ? c.Width : 0D,
                           }).ToArray();
            var result = new int[columns.Length];
            width -= (result.Length - 1) * ColumnGutter;

            foreach (var c in columns)
            {
                result[c.Index] = Math.Min(Math.Max(c.Width, c.MinWidth), c.MaxWidth);
                if (result[c.Index] >= c.MaxWidth)
                    c.StarWidth = 0D;
                if (c.StarWidth <= 0.001D)
                    width -= result[c.Index];
            }

            var totalStar = columns.Sum(x => x.StarWidth);
            foreach (var c in columns.Where(x => x.StarWidth > 0.001D))
            {
                result[c.Index] = Math.Min(Math.Max(result[c.Index], (int)(c.StarWidth / totalStar * width)), c.MaxWidth);
                totalStar -= c.StarWidth;
                width -= result[c.Index];
            }

            return result;
        }

        private class ColumnWidthData
        {
            public int Index { get; set; }
            public int Width { get; set; }
            public int MinWidth { get; set; }
            public int MaxWidth { get; set; }
            public double StarWidth { get; set; }
        }
    }
}
