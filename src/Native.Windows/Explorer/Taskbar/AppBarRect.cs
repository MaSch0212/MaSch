using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MaSch.Native.Windows.Explorer.Taskbar
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct AppBarRect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
