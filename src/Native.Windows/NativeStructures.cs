using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace MaSch.Native.Windows
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Will not fix for this project.")]
    public static class Constants
    {
        public const int RmRebootReasonNone = 0;
        public const int CchRmMaxAppName = 255;
        public const int CchRmMaxSvcName = 63;
    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct PostStruct
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }

    [Flags]
    [SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"", Justification = "Won't fix.")]
    public enum Shgfi : uint
    {
        /// <summary>get icon.</summary>
        Icon = 0x000000100,

        /// <summary>get display name.</summary>
        DisplayName = 0x000000200,

        /// <summary>get type name.</summary>
        TypeName = 0x000000400,

        /// <summary>get attributes.</summary>
        Attributes = 0x000000800,

        /// <summary>get icon location.</summary>
        IconLocation = 0x000001000,

        /// <summary>return exe type.</summary>
        ExeType = 0x000002000,

        /// <summary>get system icon index.</summary>
        SysIconIndex = 0x000004000,

        /// <summary>put a link overlay on icon.</summary>
        LinkOverlay = 0x000008000,

        /// <summary>show icon in selected state.</summary>
        Selected = 0x000010000,

        /// <summary>get only specified attributes.</summary>
        AttrSpecified = 0x000020000,

        /// <summary>get large icon.</summary>
        LargeIcon = 0x000000000,

        /// <summary>get small icon.</summary>
        SmallIcon = 0x000000001,

        /// <summary>get open icon.</summary>
        OpenIcon = 0x000000002,

        /// <summary>get shell size icon.</summary>
        ShellIconSize = 0x000000004,

        /// <summary>pszPath is a pidl.</summary>
        Pidl = 0x000000008,

        /// <summary>use passed dwFileAttribute.</summary>
        UseFileAttributes = 0x000000010,

        /// <summary>apply the appropriate overlays.</summary>
        AddOverlays = 0x000000020,

        /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon.</summary>
        OverlayIndex = 0x000000040,
    }

    public enum RmAppType
    {
        RmUnknownApp = 0,
        RmMainWindow = 1,
        RmOtherWindow = 2,
        RmService = 3,
        RmExplorer = 4,
        RmConsole = 5,
        RmCritical = 1000,
    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct ShFileInfo
    {
        public const int NameSize = 80;
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct RmUniqueProcess
    {
        public int dwProcessId;
        public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
    }

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct GuiThreadInfo
    {
        public int cbSize;
        public int flags;
        public IntPtr hwndActive;
        public IntPtr hwndFocus;
        public IntPtr hwndCapture;
        public IntPtr hwndMenuOwner;
        public IntPtr hwndMoveSize;
        public IntPtr hwndCaret;
        public System.Drawing.Rectangle rcCaret;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct RmProcessInfo
    {
        public RmUniqueProcess Process;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.CchRmMaxAppName + 1)]
        public string strAppName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.CchRmMaxSvcName + 1)]
        public string strServiceShortName;

        public RmAppType ApplicationType;
        public uint AppStatus;
        public uint TSSessionId;
        [MarshalAs(UnmanagedType.Bool)]
        public bool bRestartable;
    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SX1309:Field names should begin with underscore", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct Point
    {
        private int x;
        private int y;
    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct ImageListDrawParams
    {
        public int cbSize;
        public IntPtr himl;
        public int i;
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xBitmap;    // x offest from the upperleft of bitmap
        public int yBitmap;    // y offset from the upperleft of bitmap
        public int rgbBk;
        public int rgbFg;
        public int fStyle;
        public int dwRop;
        public int fState;
        public int Frame;
        public int crEffect;
    }

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1307:Accessible fields should begin with upper-case letter", Justification = "I will not mess with names. I dont know if I would break anything.")]
    public struct ImageInfo
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public Rect rcImage;
    }

    [ComImport]
    [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IImageList
    {
        [PreserveSig]
        int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);

        [PreserveSig]
        int ReplaceIcon(int i, IntPtr hicon, ref int pi);

        [PreserveSig]
        int SetOverlayImage(int iImage, int iOverlay);

        [PreserveSig]
        int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);

        [PreserveSig]
        int AddMasked(IntPtr hbmImage, int crMask, ref int pi);

        [PreserveSig]
        int Draw(ref ImageListDrawParams pimldp);

        [PreserveSig]
        int Remove(int i);

        [PreserveSig]
        int GetIcon(int i, int flags, ref IntPtr picon);

        [PreserveSig]
        int GetImageInfo(int i, ref ImageInfo pImageInfo);

        [PreserveSig]
        int Copy(int iDst, IImageList punkSrc, int iSrc, int uFlags);

        [PreserveSig]
        int Merge(int i1, IImageList punk2, int i2, int dx, int dy, ref Guid riid, ref IntPtr ppv);

        [PreserveSig]
        int Clone(ref Guid riid, ref IntPtr ppv);

        [PreserveSig]
        int GetImageRect(int i, ref Rect prc);

        [PreserveSig]
        int GetIconSize(ref int cx, ref int cy);

        [PreserveSig]
        int SetIconSize(int cx, int cy);

        [PreserveSig]
        int GetImageCount(ref int pi);

        [PreserveSig]
        int SetImageCount(int uNewCount);

        [PreserveSig]
        int SetBkColor(int clrBk, ref int pclr);

        [PreserveSig]
        int GetBkColor(ref int pclr);

        [PreserveSig]
        int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);

        [PreserveSig]
        int EndDrag();

        [PreserveSig]
        int DragEnter(IntPtr hwndLock, int x, int y);

        [PreserveSig]
        int DragLeave(IntPtr hwndLock);

        [PreserveSig]
        int DragMove(int x, int y);

        [PreserveSig]
        int SetDragCursorImage(ref IImageList punk, int iDrag, int dxHotspot, int dyHotspot);

        [PreserveSig]
        int DragShowNolock(int fShow);

        [PreserveSig]
        int GetDragImage(ref Point ppt, ref Point pptHotspot, ref Guid riid, ref IntPtr ppv);

        [PreserveSig]
        int GetItemFlags(int i, ref int dwFlags);

        [PreserveSig]
        int GetOverlayImage(int iOverlay, ref int piIndex);
    }
}
