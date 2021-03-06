using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaSch.Native.Windows
{
    public static class User32
    {
        /// <summary>
        ///     An application sends the WM_COPYDATA message to pass data to another
        ///     application.
        /// </summary>
        public const int WmCopyData = 0x004A;

        public const int HwndBroadcast = 0xFFFF;

        /// <summary>
        ///     The FindWindow function retrieves a handle to the top-level window
        ///     whose class name and window name match the specified strings. This
        ///     function does not search child windows. This function does not
        ///     perform a case-sensitive search.
        /// </summary>
        /// <param name="lpClassName">Class name.</param>
        /// <param name="lpWindowName">Window caption.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("User32.dll")]
        public static extern IntPtr GetCapture();

        [DllImport("user32.dll")]
        public static extern bool GetGUIThreadInfo(uint idThread, ref GuiThreadInfo lpgui);

        /// <summary>
        ///     Sends the specified message to a window or windows. The SendMessage
        ///     function calls the window procedure for the specified window and does
        ///     not return until the window procedure has processed the message.
        /// </summary>
        /// <param name="hWnd">
        ///     Handle to the window whose window procedure will receive the message.
        /// </param>
        /// <param name="msg">Specifies the message to be sent.</param>
        /// <param name="wParam">
        ///     Specifies additional message-specific information.
        /// </param>
        /// <param name="lParam">
        ///     Specifies additional message-specific information.
        /// </param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1625:Element documentation should not be copied and pasted", Justification = "Works in this case.")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref PostStruct lParam);

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
    }
}