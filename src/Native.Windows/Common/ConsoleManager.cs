using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MaSch.Native.Windows.Common
{
    public static class ConsoleManager
    {
        public static bool HasConsole
        {
            get { return Kernel32.GetConsoleWindow() != IntPtr.Zero; }
        }

        /// <summary>
        /// Creates a new console instance if the process is not attached to a console already.
        /// </summary>
        public static void Show()
        {
            if (!HasConsole)
            {
                _ = Kernel32.AllocConsole();
                InvalidateOutAndError();
            }
        }

        /// <summary>
        /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
        /// </summary>
        public static void Hide()
        {
            if (HasConsole)
            {
                SetOutAndErrorNull();
                _ = Kernel32.FreeConsole();
            }
        }

        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        internal static void InvalidateOutAndError()
        {
            Type type = typeof(Console);

            var @out = type.GetField("_out", BindingFlags.Static | BindingFlags.NonPublic);

            var error = type.GetField("_error", BindingFlags.Static | BindingFlags.NonPublic);

            var initializeStdOutError = type.GetMethod("InitializeStdOutError", BindingFlags.Static | BindingFlags.NonPublic);

            Debug.Assert(@out != null);
            Debug.Assert(error != null);

            Debug.Assert(initializeStdOutError != null);

            @out.SetValue(null, null);
            error.SetValue(null, null);

            _ = initializeStdOutError.Invoke(null, new object[] { true });
        }

        internal static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}
