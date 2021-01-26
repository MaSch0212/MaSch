using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.Native
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Kernel32
    {
        private const string Kernel32_DllName = "kernel32.dll";

        #region Delegates
        public delegate CopyProgressResult CopyProgressRoutine(long totalFileSize, long totalBytesTransferred, long streamSize,
            long streamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason, IntPtr hSourceFile,
            IntPtr hDestinationFile, IntPtr lpData);
        #endregion

        #region Extern Methods
        [DllImport(Kernel32_DllName, CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref int pbCancel, CopyFileFlags dwCopyFlags);

        [DllImport(Kernel32_DllName)]
        public static extern bool AllocConsole();

        [DllImport(Kernel32_DllName)]
        public static extern bool FreeConsole();

        [DllImport(Kernel32_DllName)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32_DllName)]
        public static extern int GetConsoleOutputCP();
        #endregion

        #region Enums
        public enum CopyProgressResult : uint
        {
            ProgressContinue,
            ProgressCancel,
            ProgressStop,
            ProgressQuiet,
        }
        
        public enum CopyProgressCallbackReason : uint
        {
            CallbackChunkFinished,
            CallbackStreamSwitch,
        }

        [Flags]
        public enum CopyFileFlags : uint
        {
            CopyFileFailIfExists = 1,
            CopyFileRestartable = 2,
            CopyFileOpenSourceForWrite = 4,
            CopyFileAllowDecryptedDestination = 8,
            CopyFileCopySymlink = 2048,
        }
        #endregion

    }
}
