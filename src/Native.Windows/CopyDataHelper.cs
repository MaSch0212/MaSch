using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MaSch.Native.Windows
{
    /// <summary>
    /// Represents a class for helping with inter process communication over windows api.
    /// </summary>
    /// <typeparam name="T">The data struct.</typeparam>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Will not fix for this project.")]
    public class PostHelper<T>
        where T : struct
    {
        /// <summary>
        /// Occurs when a new message was received from another process.
        /// </summary>
        public event CopyDataMessageReceivedEventHandler<T>? MessageReceived;

        /// <summary>
        /// Sends the secified data to another process.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="targetHwnd">The target window handle.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <exception cref="Exception">
        /// SendMessage(WM_COPYDATA) failed with error 0x[Error].
        /// </exception>
        public void SendData(T data, IntPtr targetHwnd, IntPtr? wParam = null)
        {
            var dataSize = Marshal.SizeOf(data);
            var dataPointer = Marshal.AllocHGlobal(dataSize);
            try
            {
                Marshal.StructureToPtr(data, dataPointer, true);
                var ps = new PostStruct
                {
                    cbData = dataSize,
                    lpData = dataPointer,
                };

                var result = User32.SendMessage(targetHwnd, User32.WmCopyData, wParam ?? IntPtr.Zero, ref ps);

                if (result.ToInt32() > 1)
                    throw new Exception($"SendMessage(WM_COPYDATA) failed with error 0x{result.ToInt32():X}");
            }
            finally
            {
                Marshal.FreeHGlobal(dataPointer);
            }
        }

        /// <summary>
        /// The window process method for registration to window handle to listen for message incomes.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <param name="handled">if set to <c>true</c> this message is handeled.</param>
        /// <returns></returns>
        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == User32.WmCopyData &&
                Marshal.PtrToStructure(lParam, typeof(PostStruct)) is PostStruct ps &&
                ps.cbData == Marshal.SizeOf(typeof(T)) &&
                Marshal.PtrToStructure(ps.lpData, typeof(T)) is T data)
            {
                var e = new CopyDataMessageReceivedEventArgs<T> { Data = data, WParam = wParam };
                MessageReceived?.Invoke(this, e);
                handled = e.Handled;
            }

            return IntPtr.Zero;
        }
    }

    /// <summary>
    /// Represents a method for handling the <see cref="PostHelper{T}.MessageReceived"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="CopyDataMessageReceivedEventArgs{T}"/> instance containing the event data.</param>
    public delegate void CopyDataMessageReceivedEventHandler<T>(object sender, CopyDataMessageReceivedEventArgs<T> e);

    /// <summary>
    /// Represents the event data for the <see cref="PostHelper{T}.MessageReceived"/> event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "Will not fix for this project.")]
    public class CopyDataMessageReceivedEventArgs<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CopyDataMessageReceivedEventArgs{T}"/> is handled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if handled; otherwise, <c>false</c>.
        /// </value>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the w parameter.
        /// </summary>
        /// <value>
        /// The w parameter.
        /// </value>
        public IntPtr WParam { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public T? Data { get; set; }
    }
}
