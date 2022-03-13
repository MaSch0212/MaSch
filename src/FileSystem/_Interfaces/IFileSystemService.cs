using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.FileSystem;

public interface IFileSystemService
{
    IFileService File { get; }
    IDirectoryService Directory { get; }

    IFileInfo GetFileInfo(string filePath);
    IDirectoryInfo GetDirectoryInfo(string directoryPath);
}
