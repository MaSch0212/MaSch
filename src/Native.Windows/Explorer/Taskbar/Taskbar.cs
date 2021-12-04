using System.Drawing;
using System.Runtime.InteropServices;

namespace MaSch.Native.Windows.Explorer.Taskbar;

public sealed class Taskbar
{
    private const string ClassName = "Shell_TrayWnd";

    public Taskbar()
    {
        var taskBarHandle = User32.FindWindow(ClassName, null);

        var data = new AppBarData
        {
            cbSize = (uint)Marshal.SizeOf(typeof(AppBarData)),
            hWnd = taskBarHandle,
        };
        var result = Shell32.SHAppBarMessage(AppBarMessage.GetTaskbarPos, ref data);
        if (result == IntPtr.Zero)
            throw new InvalidOperationException();

        Position = (TaskbarPosition)data.uEdge;
        Bounds = Rectangle.FromLTRB(data.rc.left, data.rc.top, data.rc.right, data.rc.bottom);
        Size = Bounds.Size;
        Location = Bounds.Location;

        data.cbSize = (uint)Marshal.SizeOf(typeof(AppBarData));
        result = Shell32.SHAppBarMessage(AppBarMessage.GetState, ref data);
        var state = result.ToInt32();
        AlwaysOnTop = (state & AppBarState.AlwaysOnTop) == AppBarState.AlwaysOnTop;
        AutoHide = (state & AppBarState.Autohide) == AppBarState.Autohide;
    }

    public Rectangle Bounds { get; }
    public TaskbarPosition Position { get; }
    public System.Drawing.Point Location { get; }
    public Size Size { get; }
    public bool AlwaysOnTop { get; }
    public bool AutoHide { get; }
}
