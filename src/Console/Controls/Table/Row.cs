using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Console.Controls.Table
{
    public class Row
    {
        public bool IsSeperator { get; set; } = false;
        public IList<string> Values { get; set; } = Array.Empty<string>();
        public int? Height { get; set; } = null;
        public RowHeightMode HeightMode { get; set; } = RowHeightMode.Auto;
    }

    public enum RowHeightMode
    {
        Auto,
        Fixed,
    }
}
