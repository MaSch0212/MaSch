using System.Runtime.InteropServices;

namespace MaSch.Native.Windows;

public static class Kernel32
{
    private const string Kernel32DllName = "kernel32.dll";

    #region Delegates

    public delegate CopyProgressResult CopyProgressRoutine(
        long totalFileSize,
        long totalBytesTransferred,
        long streamSize,
        long streamBytesTransferred,
        uint dwStreamNumber,
        CopyProgressCallbackReason dwCallbackReason,
        IntPtr hSourceFile,
        IntPtr hDestinationFile,
        IntPtr lpData);

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
    [SuppressMessage("Minor Code Smell", "S2344:Enumeration type names should not have \"Flags\" or \"Enum\" suffixes", Justification = "Won't fix. Would introduce breaking change.")]
    public enum CopyFileFlags : uint
    {
        CopyFileFailIfExists = 1,
        CopyFileRestartable = 2,
        CopyFileOpenSourceForWrite = 4,
        CopyFileAllowDecryptedDestination = 8,
        CopyFileCopySymlink = 2048,
    }

    #endregion

    #region Extern Methods

    [DllImport(Kernel32DllName, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref int pbCancel, CopyFileFlags dwCopyFlags);

    [DllImport(Kernel32DllName)]
    public static extern bool AllocConsole();

    [DllImport(Kernel32DllName)]
    public static extern bool FreeConsole();

    [DllImport(Kernel32DllName)]
    public static extern IntPtr GetConsoleWindow();

    [DllImport(Kernel32DllName)]
    public static extern int GetConsoleOutputCP();

    #endregion
}
