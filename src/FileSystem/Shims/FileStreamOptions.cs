﻿#if !NET6_0_OR_GREATER
namespace MaSch.FileSystem;

#pragma warning disable SA1600 // Elements should be documented

public sealed class FileStreamOptions
{
    public FileMode Mode { get; set; } = FileMode.Open;
    public FileAccess Access { get; set; } = FileAccess.Read;
    public FileShare Share { get; set; } = FileShare.Read;
    public FileOptions Options { get; set; }
    public int BufferSize { get; set; } = 4096;
}
#endif