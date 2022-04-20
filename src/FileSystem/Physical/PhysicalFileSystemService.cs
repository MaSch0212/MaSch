namespace MaSch.FileSystem.Physical;

/// <summary>
/// Implementation of the <see cref="IFileSystemService"/> interface that managed the physical file system.
/// </summary>
public class PhysicalFileSystemService : FileSystemServiceBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhysicalFileSystemService"/> class.
    /// </summary>
    public PhysicalFileSystemService()
    {
    }

    /// <inheritdoc />
    protected override IDirectoryService CreateDirectoryService()
    {
        return new PhysicalDirectoryService(this);
    }

    /// <inheritdoc />
    protected override IFileService CreateFileService()
    {
        return new PhysicalFileService(this);
    }
}
