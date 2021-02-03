using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MaSch.Native.Windows.Explorer
{
    public static class FileInfo
    {
        private static int GetFileIconIndex(string pszFile)
        {
            var sfi = default(ShFileInfo);
            Shell32.SHGetFileInfo(
                pszFile,
                0,
                ref sfi,
                (uint)Marshal.SizeOf(sfi),
                (uint)(Shgfi.SysIconIndex | Shgfi.LargeIcon | Shgfi.UseFileAttributes));
            return sfi.iIcon;
        }

        private static int GetDirectoryIconIndex(string pszFile)
        {
            var sfi = default(ShFileInfo);
            Shell32.SHGetFileInfo(
                pszFile,
                Shell32.FileAttributeDirectory,
                ref sfi,
                (uint)Marshal.SizeOf(sfi),
                (uint)(Shgfi.SysIconIndex | Shgfi.LargeIcon | Shgfi.UseFileAttributes | Shgfi.OpenIcon));
            return sfi.iIcon;
        }

        private static IntPtr GetIcon(int iImage, IconSize size)
        {
            IImageList spiml = null;
            Guid guil = new Guid(Shell32.IidIImageList);

            Shell32.SHGetImageList((int)size, ref guil, ref spiml);
            IntPtr hIcon = IntPtr.Zero;
            spiml.GetIcon(iImage, Shell32.IldTransparent | Shell32.IldImage, ref hIcon);

            return hIcon;
        }

        public static Icon GetIconFromFile(string filePath, IconSize size)
        {
            var iconIndex = GetFileIconIndex(filePath);
            var iconHandle = GetIcon(iconIndex, size);
            return Icon.FromHandle(iconHandle);
        }

        public static Icon GetIconFromDirectory(string directoryPath, IconSize size)
        {
            var iconIndex = GetDirectoryIconIndex(directoryPath);
            var iconHandle = GetIcon(iconIndex, size);
            return Icon.FromHandle(iconHandle);
        }

        public static ShFileInfo GetFileInfo(string filePath)
        {
            ShFileInfo sfi = default(ShFileInfo);
            Shell32.SHGetFileInfo(
                filePath,
                0,
                ref sfi,
                (uint)Marshal.SizeOf(sfi),
                (uint)(Shgfi.UseFileAttributes | Shgfi.DisplayName | Shgfi.TypeName));
            return sfi;
        }

        public static IList<Process> WhoIsLocking(string path)
        {
            string key = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();

            int res = Rstrtmgr.RmStartSession(out uint handle, 0, key);

            if (res != 0)
                throw new Exception("Could not begin restart session.  Unable to determine file locker.");

            try
            {
                const int ERROR_MORE_DATA = 234;
                uint pnProcInfo = 0,
                     lpdwRebootReasons = Constants.RmRebootReasonNone;

                string[] resources = new string[] { path }; // Just checking on one resource.

                res = Rstrtmgr.RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);

                if (res != 0)
                    throw new Exception("Could not register resource.");

                // Note: there's a race condition here -- the first call to RmGetList() returns
                //       the total number of process. However, when we call RmGetList() again to get
                //       the actual processes this number may have increased.
                res = Rstrtmgr.RmGetList(handle, out uint pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                if (res == ERROR_MORE_DATA)
                {
                    // Create an array to store the process results
                    var processInfo = new RmProcessInfo[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;

                    // Get the list
                    res = Rstrtmgr.RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);

                    if (res == 0)
                    {
                        processes = new List<Process>((int)pnProcInfo);

                        // Enumerate all of the results and add them to the
                        // list to be returned
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            try
                            {
                                processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                            }

                            // catch the error -- in case the process is no longer running
                            catch (ArgumentException)
                            {
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Could not list processes locking resource.");
                    }
                }
                else if (res != 0)
                {
                    throw new Exception("Could not list processes locking resource. Failed to get size of result.");
                }
            }
            finally
            {
                Rstrtmgr.RmEndSession(handle);
            }

            return processes;
        }
    }

    public enum IconSize
    {
        /// <summary>
        /// 16x16
        /// </summary>
        Small = Shell32.ShilSmall,

        /// <summary>
        /// 32x32
        /// </summary>
        Medium = Shell32.ShilLarge,

        /// <summary>
        /// 48x48
        /// </summary>
        Large = Shell32.ShilExtralarge,

        /// <summary>
        /// 256x256
        /// </summary>
        Jumbo = Shell32.ShilJumbo,
    }
}
