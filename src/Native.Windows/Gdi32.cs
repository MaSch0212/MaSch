using System;
using System.Runtime.InteropServices;
#pragma warning disable 1591

namespace MaSch.Native.Windows
{
    public static class Gdi32
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
