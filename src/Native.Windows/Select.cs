﻿using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaSch.Native.Windows;

public sealed class Select : ISelect
{
    /// <summary>
    /// Gets or sets folder in which dialog will be open.
    /// </summary>
    public string? InitialFolder { get; set; }

    /// <summary>
    /// Gets or sets directory in which dialog will be open
    /// if there is no recent directory available.
    /// </summary>
    public string? DefaultFolder { get; set; }

    /// <summary>
    /// Gets or sets the selected folder.
    /// </summary>
    public string? Folder { get; set; }

    public DialogResult ShowDialog()
    {
        return ShowDialog(owner: new WindowWrapper(IntPtr.Zero));
    }

    public DialogResult ShowDialog(IWin32Window owner)
    {
        if (Environment.OSVersion.Version.Major >= 6)
        {
            return ShowVistaDialog(owner);
        }
        else
        {
            return ShowLegacyDialog(owner);
        }
    }

    public DialogResult ShowVistaDialog(IWin32Window owner)
    {
        var frm = (NativeMethods.IFileDialog)new NativeMethods.FileOpenDialogRCW();
        _ = frm.GetOptions(out uint options);
        options |= NativeMethods.FOS_PICKFOLDERS |
                   NativeMethods.FOS_FORCEFILESYSTEM |
                   NativeMethods.FOS_NOVALIDATE |
                   NativeMethods.FOS_NOTESTFILECREATE |
                   NativeMethods.FOS_DONTADDTORECENT;
        _ = frm.SetOptions(options);
        if (InitialFolder != null)
        {
            var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); // IShellItem
            if (NativeMethods.SHCreateItemFromParsingName(InitialFolder, IntPtr.Zero, ref riid, out var directoryShellItem) == NativeMethods.S_OK)
            {
                _ = frm.SetFolder(directoryShellItem);
            }
        }

        if (DefaultFolder != null)
        {
            var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); // IShellItem
            if (NativeMethods.SHCreateItemFromParsingName(DefaultFolder, IntPtr.Zero, ref riid, out var directoryShellItem) == NativeMethods.S_OK)
            {
                frm.SetDefaultFolder(directoryShellItem);
            }
        }

        if (frm.Show(owner.Handle) == NativeMethods.S_OK &&
            frm.GetResult(out var shellItem) == NativeMethods.S_OK &&
            shellItem.GetDisplayName(NativeMethods.SIGDN_FILESYSPATH, out var pszString) == NativeMethods.S_OK &&
            pszString != IntPtr.Zero)
        {
            try
            {
                Folder = Marshal.PtrToStringAuto(pszString);
                return DialogResult.OK;
            }
            finally
            {
                Marshal.FreeCoTaskMem(pszString);
            }
        }

        return DialogResult.Cancel;
    }

    public DialogResult ShowLegacyDialog(IWin32Window owner)
    {
        using var frm = new SaveFileDialog
        {
            CheckFileExists = false,
            CheckPathExists = true,
            CreatePrompt = false,
            Filter = "|" + Guid.Empty.ToString(),
            FileName = "any",
            OverwritePrompt = false,
            Title = "Select Folder",
            ValidateNames = false,
        };
        if (InitialFolder != null)
            frm.InitialDirectory = InitialFolder;
        if (frm.ShowDialog(owner) == DialogResult.OK)
        {
            Folder = Path.GetDirectoryName(frm.FileName);
            return DialogResult.OK;
        }
        else
        {
            return DialogResult.Cancel;
        }
    }

    public void Dispose()
    {
        // just to have possibility of Using statement.
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "This class is really only used for the Select-class.")]
public class WindowWrapper : IWin32Window
{
    private readonly IntPtr _hwnd;

    public WindowWrapper(IntPtr handle)
    {
        _hwnd = handle;
    }

    public IntPtr Handle
    {
        get { return _hwnd; }
    }
}

[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:File may only contain a single type", Justification = "This class is really only used for the Select-class.")]
internal static class NativeMethods
{
    #region Constants

    public const uint FOS_PICKFOLDERS = 0x00000020;
    public const uint FOS_FORCEFILESYSTEM = 0x00000040;
    public const uint FOS_NOVALIDATE = 0x00000100;
    public const uint FOS_NOTESTFILECREATE = 0x00010000;
    public const uint FOS_DONTADDTORECENT = 0x02000000;

    public const uint S_OK = 0x0000;

    public const uint SIGDN_FILESYSPATH = 0x80058000;

    #endregion

    #region COM

    [ComImport, Guid("42F85136-DB7E-439C-85F1-E4075D135FC8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IFileDialog
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        [PreserveSig]
        uint Show([In, Optional] IntPtr hwndOwner); // IModalWindow

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetFileTypes([In] uint cFileTypes, [In, MarshalAs(UnmanagedType.LPArray)] IntPtr rgFilterSpec);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetFileTypeIndex([In] uint iFileType);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetFileTypeIndex(out uint piFileType);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint Advise([In, MarshalAs(UnmanagedType.Interface)] IntPtr pfde, out uint pdwCookie);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint Unadvise([In] uint dwCookie);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetOptions([In] uint fos);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetOptions(out uint fos);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, uint fdap);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint Close([MarshalAs(UnmanagedType.Error)] uint hr);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetClientGuid([In] ref Guid guid);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint ClearClientData();

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
    }

    [ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"),
                InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItem
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint BindToHandler([In] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IntPtr ppvOut);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetDisplayName([In] uint sigdnName, out IntPtr ppszName);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
        uint Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

    [ComImport, ClassInterface(ClassInterfaceType.None), TypeLibType(TypeLibTypeFlags.FCanCreate), Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
    internal class FileOpenDialogRCW
    {
    }

    #endregion
}
