using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace MaSch.Presentation.Wpf.Common
{
    public class WpfScreen
    {
        private readonly Screen _screen;

        public bool IsPrimary => _screen.Primary;
        public string DeviceName => _screen.DeviceName;
        public Rect DeviceBounds => GetRect(_screen.Bounds);
        public Rect WorkingArea => GetRect(_screen.WorkingArea);

        internal WpfScreen(Screen screen) => _screen = screen;
        
        #region Static Members
        public static WpfScreen Primary => new WpfScreen(Screen.PrimaryScreen);

        public static IEnumerable<WpfScreen> AllScreens()
            => Screen.AllScreens.Select(screen => new WpfScreen(screen));

        public static WpfScreen GetScreenFrom(Window window)
        {
            var windowInteropHelper = new WindowInteropHelper(window);
            var screen = Screen.FromHandle(windowInteropHelper.Handle);
            var wpfScreen = new WpfScreen(screen);
            return wpfScreen;
        }

        public static WpfScreen GetScreenFrom(System.Windows.Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            // are x,y device-independent-pixels ??
            var drawingPoint = new System.Drawing.Point(x, y);
            var screen = Screen.FromPoint(drawingPoint);
            var wpfScreen = new WpfScreen(screen);

            return wpfScreen;
        }
        
        private static Rect GetRect(Rectangle value)
        {
            // should x, y, width, height be device-independent-pixels ??
            return new Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }
        #endregion
    }
}
