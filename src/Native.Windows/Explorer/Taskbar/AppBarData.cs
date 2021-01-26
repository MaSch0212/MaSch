using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MaSch.Native.Windows.Explorer.Taskbar
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
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
