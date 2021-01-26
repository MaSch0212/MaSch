using System;
using System.Runtime.InteropServices;

namespace MaSch.Native.Explorer.Taskbar
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AppBarData
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public AppBarEdge uEdge;
        public AppBarRect rc;
        public int lParam;
    }
}
