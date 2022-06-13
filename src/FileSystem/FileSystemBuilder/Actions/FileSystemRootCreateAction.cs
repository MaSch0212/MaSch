namespace MaSch.FileSystem.FileSystemBuilder.Actions;

internal class FileSystemRootCreateAction : IFileSystemAction
{
    public FileSystemRootCreateAction(string rootName)
    {
        RootName = rootName;
        SubActionList = new List<IFileSystemAction>();
        SubActions = new ReadOnlyCollection<IFileSystemAction>(SubActionList);
    }

    public string RootName { get; }
    public IReadOnlyCollection<IFileSystemAction> SubActions { get; }

    internal List<IFileSystemAction> SubActionList { get; }

    public void Invoke(IFileSystemService service)
    {
        if (!service.Directory.Exists(RootName))
        {
            if (service is IPathRootCreator rootCreator)
                rootCreator.CreatePathRoot(RootName);
            else
                throw new DirectoryNotFoundException($"The path root \"{RootName}\" does not exist and the service \"{service.GetType().FullName}\" does not support creating path roots.");
        }
    }
}
