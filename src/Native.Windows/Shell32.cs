using System;
using System.Runtime.InteropServices;
using MaSch.Native.Windows.Explorer.Taskbar;

namespace MaSch.Native.Windows
{
    internal static class Shell32
    {
        public const string IidIImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950";
        public const string IidIImageList2 = "192B9D83-50FC-457B-90A0-2B82A8B5DAE1";

        public const int ShilLarge = 0x0;
        public const int ShilSmall = 0x1;
        public const int ShilExtralarge = 0x2;
        public const int ShilSyssmall = 0x3;
        public const int ShilJumbo = 0x4;
        public const int ShilLast = 0x4;

        public const int IldTransparent = 0x00000001;
        public const int IldImage = 0x00000020;

        public const uint FileAttributeDirectory = 0x00000010;

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern IntPtr SHAppBarMessage(AppBarMessage dwMessage, [In] ref AppBarData pData);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        public static extern int SHGetImageList(int iImageList, ref Guid riid, ref IImageList? ppv);

        [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
        public static extern int DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll")]
        public static extern uint SHGetIDListFromObject([MarshalAs(UnmanagedType.IUnknown)] object iUnknown, out IntPtr ppidl);

        [DllImport("Shell32.dll")]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref ShFileInfo psfi,
            uint cbFileInfo,
            uint uFlags);

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}