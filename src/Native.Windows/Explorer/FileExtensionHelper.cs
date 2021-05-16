﻿using Microsoft.Win32;
using System;

namespace MaSch.Native.Windows.Explorer
{
    public static class FileExtensionHelper
    {
        public static void NotifyChangeToExplorer()
        {
            Shell32.SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool SetAssociation(string extension, string keyName, string openWith, int iconId, string fileDescription)
        {
            if (keyName == null)
                throw new ArgumentNullException(nameof(keyName));

            bool hasChange = false;

            var root = Registry.CurrentUser.OpenSubKey(@"Software\Classes", true);
            var baseKey = root?.OpenSubKey(extension, true);
            if (baseKey == null)
            {
                baseKey = root?.CreateSubKey(extension);
                hasChange = true;
            }

            if (baseKey?.GetValue(string.Empty)?.ToString() != keyName)
            {
                baseKey?.SetValue(string.Empty, keyName);
                hasChange = true;
            }

            var openMethod = root?.CreateSubKey(keyName);
            if (openMethod == null)
            {
                openMethod = root?.CreateSubKey(keyName);
                hasChange = true;
            }

            if (openMethod?.GetValue(string.Empty)?.ToString() != fileDescription)
            {
                openMethod?.SetValue(string.Empty, fileDescription);
                hasChange = true;
            }

            var iconPath = $"\"{openWith}\",{iconId}";
            if (openMethod?.OpenSubKey("DefaultIcon")?.GetValue(string.Empty)?.ToString() != iconPath)
            {
                openMethod?.CreateSubKey("DefaultIcon")?.SetValue(string.Empty, $"\"{openWith}\",{iconId}");
                hasChange = true;
            }

            var shell = openMethod?.OpenSubKey("Shell");
            if (shell == null)
            {
                shell = openMethod?.CreateSubKey("Shell");
                hasChange = true;
            }

            var command = $"\"{openWith}\" \"%1\"";
            if (shell?.OpenSubKey("open")?.OpenSubKey("command")?.GetValue(string.Empty)?.ToString() != command)
            {
                shell?.CreateSubKey("open")?.CreateSubKey("command")?.SetValue(string.Empty, "\"" + openWith + "\"" + " \"%1\"");
                hasChange = true;
            }

            shell?.Close();
            openMethod?.Close();
            baseKey?.Close();
            root?.Close();

            if (hasChange)
            {
                var currentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + extension, true);
                if (currentUser != null)
                {
                    currentUser.DeleteSubKey("UserChoice", false);
                    currentUser.Close();
                }
            }

            return hasChange;
        }
    }
}
