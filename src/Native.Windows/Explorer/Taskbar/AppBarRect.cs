using System.Runtime.InteropServices;

namespace MaSch.Native.Windows.Explorer.Taskbar
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AppBarRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
