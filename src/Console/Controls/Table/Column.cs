using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Console.Controls.Table
{
    public class Column
    {
        public string? Header { get; set; }
        public double Width { get; set; } = double.NaN;
        public ColumnWidthMode WidthMode { get; set; } = ColumnWidthMode.Auto;
        public int? MinWidth { get; set; } = null;
        public int? MaxWidth { get; set; } = null;
    }

    public enum ColumnWidthMode
    {
        Auto,
        Fixed,
        Star,
    }
}
