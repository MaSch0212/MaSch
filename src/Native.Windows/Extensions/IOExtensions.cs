using MaSch.Native.Windows.Explorer;
using System.Drawing;
using FileInfo = MaSch.Native.Windows.Explorer.FileInfo;

namespace MaSch.Native.Windows.Extensions;

public static class IOExtensions
{
    public static Icon GetIcon(this System.IO.FileInfo fileInfo, IconSize size)
    {
        return FileInfo.GetIconFromFile(fileInfo?.FullName ?? "file", size);
    }

    public static Icon GetIcon(this System.IO.DirectoryInfo directoryInfo, IconSize size)
    {
        return FileInfo.GetIconFromDirectory(directoryInfo?.FullName ?? "dir", size);
    }
}
