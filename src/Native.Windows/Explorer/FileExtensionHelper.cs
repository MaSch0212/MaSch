using Microsoft.Win32;

namespace MaSch.Native.Windows.Explorer;

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
        var iconKey = openMethod?.OpenSubKey("DefaultIcon");
        if (iconKey?.GetValue(string.Empty)?.ToString() != iconPath)
        {
            iconKey?.Dispose();
            iconKey = openMethod?.CreateSubKey("DefaultIcon");
            iconKey?.SetValue(string.Empty, $"\"{openWith}\",{iconId}");
            hasChange = true;
        }

        var shell = openMethod?.OpenSubKey("Shell");
        if (shell == null)
        {
            shell = openMethod?.CreateSubKey("Shell");
            hasChange = true;
        }

        var command = $"\"{openWith}\" \"%1\"";

        var openShellKey = shell?.OpenSubKey("open");
        var commandKey = openShellKey?.OpenSubKey("command");
        if (commandKey?.GetValue(string.Empty)?.ToString() != command)
        {
            openShellKey?.Dispose();
            commandKey?.Dispose();
            openShellKey = shell?.CreateSubKey("open");
            commandKey = openShellKey?.CreateSubKey("command");
            commandKey?.SetValue(string.Empty, "\"" + openWith + "\"" + " \"%1\"");
            hasChange = true;
        }

        shell?.Dispose();
        openMethod?.Dispose();
        baseKey?.Dispose();
        root?.Dispose();
        iconKey?.Dispose();
        openShellKey?.Dispose();
        commandKey?.Dispose();

        if (hasChange)
        {
            using var currentUser = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\" + extension, true);
            if (currentUser != null)
            {
                currentUser.DeleteSubKey("UserChoice", false);
                currentUser.Close();
            }
        }

        return hasChange;
    }
}
