using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MaSch.Core.Extensions;

namespace MaSch.Presentation.Wpf.Common
{
    /// <summary>
    /// Represents the position and size of a Window. This object can be used to store window position and size information into a file.
    /// </summary>
    public class WindowPosition
    {
        /// <summary>
        /// Gets or sets the name of the window class.
        /// </summary>
        public string? WindowClassName { get; set; }

        /// <summary>
        /// Gets or sets the position of the window.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Gets or sets the size of the window.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Gets or sets the state of the window.
        /// </summary>
        public WindowState WindowState { get; set; }

        /// <summary>
        /// Applies the stored information from this <see cref="WindowPosition"/> to a window.
        /// </summary>
        /// <param name="window">The window to apply the information to.</param>
        /// <param name="disableMinimize">if set to <c>true</c> the window will not be minized even if the <see cref="WindowState"/> pproperty is defined as <see cref="WindowState.Minimized"/>.</param>
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
            if (!disableMinimize || WindowState != WindowState.Minimized)
                window.WindowState = WindowState;
        }

        /// <summary>
        /// Retrieves information from a <see cref="Window"/> object and stores these into a new instance of the <see cref="WindowPosition"/> class.
        /// </summary>
        /// <param name="window">The window to retrieve the information from.</param>
        /// <param name="disableMinimize">if set to <c>true</c> the <see cref="WindowState"/> property is set to <see cref="WindowState.Normal"/> if the window is minimized.</param>
        /// <returns>A new instance of the <see cref="WindowPosition"/> class containing the information from the given window.</returns>
        public static WindowPosition? GetFromWindow(Window window, bool disableMinimize = true)
        {
            if (window == null)
                return null;
            return new WindowPosition
            {
                WindowClassName = window.GetType().FullName,
                Position = new Point(window.Left, window.Top),
                WindowState = window.WindowState == WindowState.Minimized && disableMinimize ? WindowState.Normal : window.WindowState,
                Size = new Size(window.ActualWidth, window.ActualHeight),
            };
        }

        /// <summary>
        /// Retrieves information from multiple instances of the <see cref="Window"/> class and stores these into a new <see cref="Array"/> of instances of the <see cref="WindowPosition"/> class.
        /// </summary>
        /// <param name="disableMinimize">if set to <c>true</c> the <see cref="WindowState"/> property is set to <see cref="WindowState.Normal"/> if the window is minimized.</param>
        /// <param name="windows">The windows to retrieve the information from.</param>
        /// <returns>An <see cref="Array"/> of new instances of the <see cref="WindowPosition"/> class containing the information from the given windows.</returns>
        public static WindowPosition[] GetFromWindows(bool disableMinimize, params Window[] windows)
        {
            return windows.Select(x => GetFromWindow(x, disableMinimize)).WhereNotNull().ToArray();
        }

        /// <summary>
        /// Retrieves information from a <see cref="Window"/> object, stores these into a new instance of the <see cref="WindowPosition"/> class and adds that instance to a given <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="list">The list to add the window information to.</param>
        /// <param name="window">The window to retrieve the information from.</param>
        /// <param name="disableMinimize">if set to <c>true</c> the <see cref="WindowState"/> property is set to <see cref="WindowState.Normal"/> if the window is minimized.</param>
        public static void AddWindowToList(IList<WindowPosition> list, Window window, bool disableMinimize = true)
        {
            var index = list.IndexOf(x => x.WindowClassName == window.GetType().FullName);
            var wp = GetFromWindow(window, disableMinimize);
            if (wp != null)
            {
                if (index < 0)
                    list.Add(wp);
                else
                    list[index] = wp;
            }
        }

        /// <summary>
        /// Applies the correct <see cref="WindowPosition"/> from an <see cref="IEnumerable{T}"/> to the given window.
        /// </summary>
        /// <param name="positions">The window informations to apply.</param>
        /// <param name="targetWindow">The target window to apply the information to.</param>
        /// <param name="disableMinimize">if set to <c>true</c> the window will not be minized even if the <see cref="WindowState"/> pproperty is defined as <see cref="WindowState.Minimized"/>.</param>
        /// <returns><c>true</c> if a <see cref="WindowPosition"/> object matching the type name of the <paramref name="targetWindow"/> was found and applied to the window.</returns>
        public static bool ApplyToWindow(IEnumerable<WindowPosition> positions, Window targetWindow, bool disableMinimize = true)
        {
            var pos = positions.FirstOrDefault(x => x.WindowClassName == targetWindow.GetType().FullName);
            pos?.ApplyToWindow(targetWindow, disableMinimize);
            return pos != null;
        }
    }
}
