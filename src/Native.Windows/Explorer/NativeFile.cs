using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MaSch.Native.Windows.Explorer;

public static class NativeFile
{
    public static bool CopyFile(string fileToCopy, string destinationFile, CancellationToken token)
    {
        bool canceled = false;

        Kernel32.CopyProgressResult ProgressRoutine(
            long size,
            long transferred,
            long streamSize,
            long bytesTransferred,
            uint number,
            Kernel32.CopyProgressCallbackReason reason,
            IntPtr file,
            IntPtr destFile,
            IntPtr data)
        {
            if (token.IsCancellationRequested)
            {
                canceled = true;
                return Kernel32.CopyProgressResult.ProgressCancel;
            }

            return Kernel32.CopyProgressResult.ProgressContinue;
        }

        int pbCancel = 0;
        if (!Kernel32.CopyFileEx(fileToCopy, destinationFile, ProgressRoutine, IntPtr.Zero, ref pbCancel, Kernel32.CopyFileFlags.CopyFileRestartable))
            throw new Win32Exception(Marshal.GetLastWin32Error());

        return !canceled;
    }
}
