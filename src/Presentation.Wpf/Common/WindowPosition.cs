using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Wpf.Common
{
    public class WindowPosition
    {
        public string WindowClassName { get; set; }
        public Point Position { get; set; }
        public Size Size { get; set; }
        public WindowState WindowState { get; set; }

        public void ApplyToWindow(Window window, bool disableMinimize = true)
        {
            if (window.WindowState == WindowState.Maximized)
                window.WindowState = WindowState.Normal;

            if (WindowState != WindowState.Maximized)
            {
                if (Size.Width > 0)
                    window.Width = Size.Width;
                if (Size.Height > 0)
                    window.Height = Size.Height;
            }
            window.Left = Position.X;
            window.Top = Position.Y;
            if(!disableMinimize || WindowState != WindowState.Minimized)
                window.WindowState = WindowState;
        }

        public static WindowPosition GetFromWindow(Window window, bool disableMinimize = true)
        {
            if (window == null)
                return null;
            return new WindowPosition
            {
                WindowClassName = window.GetType().FullName,
                Position = new Point(window.Left, window.Top),
                WindowState = window.WindowState == WindowState.Minimized && disableMinimize ? WindowState.Normal : window.WindowState,
                Size = new Size(window.ActualWidth, window.ActualHeight)
            };
        }

        public static WindowPosition[] GetFromWindows(bool disableMinimize, params Window[] windows)
        {
            return windows.Select(x => GetFromWindow(x, disableMinimize)).ToArray();
        }

        public static void AddWindowToList(IList<WindowPosition> list, Window window, bool disableMinimize = true)
        {
            var index = list.IndexOf(x => x.WindowClassName == window.GetType().FullName);
            if (index < 0)
                list.Add(GetFromWindow(window, disableMinimize));
            else
                list[index] = GetFromWindow(window, disableMinimize);
        }

        public static bool ApplyToWindow(IEnumerable<WindowPosition> positions, Window targetWindow, bool disableMinimize = true)
        {
            var pos = positions.FirstOrDefault(x => x.WindowClassName == targetWindow.GetType().FullName);
            pos?.ApplyToWindow(targetWindow, disableMinimize);
            return pos != null;
        }
    }
}
