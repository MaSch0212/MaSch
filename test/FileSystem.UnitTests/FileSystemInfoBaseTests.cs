using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class FileSystemInfoBaseTests : TestClassBase
{
    private const string Path = "C:\\Folder\\file.txt";

    private FileSystemInfoBase Sut { get; set; } = null!;
    private Mock<IDirectoryService> DirectoryService { get; set; } = null!;
    private Mock<IFileService> FileService { get; set; } = null!;

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        Sut = CreateInfo(Path, out var ds, out var fs);
        DirectoryService = ds;
        FileService = fs;
    }

    [TestMethod]
    public void Constructor_NullChecks()
    {
        var fileSystemService = Mocks.Create<IFileSystemService>().Object;

        TargetInvocationException ex;
        ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<FileSystemInfoBase>(null, Path).Object);
        Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
        ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<FileSystemInfoBase>(fileSystemService, null).Object);
        Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
        ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<FileSystemInfoBase>(fileSystemService, string.Empty).Object);
        Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);
        ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<FileSystemInfoBase>(fileSystemService, "   ").Object);
        Assert.IsInstanceOfType<ArgumentException>(ex.InnerException);
    }

    [TestMethod]
    public void Get_Extension()
    {
        Assert.AreEqual(".txt", Sut.Extension);
    }

    [TestMethod]
    public void Get_Name()
    {
        Assert.AreEqual("file.txt", Sut.Name);
    }

    [TestMethod]
    public void Get_OriginalAndFullPath()
    {
        var sut = CreateInfo("blub\\abc.txt", out _, out _);
        var sutPo = new PrivateObject(sut);

        Assert.AreEqual("blub\\abc.txt", sutPo.GetProperty<string>("OriginalPath"));
        Assert.AreEqual(System.IO.Path.GetFullPath("blub\\abc.txt"), sut.FullName);
    }

    private FileSystemInfoBase CreateInfo(string path, out Mock<IDirectoryService> directoryService, out Mock<IFileService> fileService)
    {
        var dirService = directoryService = Mocks.Create<IDirectoryService>();
        var localFileService = fileService = Mocks.Create<IFileService>();
        var fileSystemService = Mocks.Create<IFileSystemService>();

        fileSystemService.Setup(x => x.Directory).Returns(directoryService.Object);
        fileSystemService.Setup(x => x.File).Returns(localFileService.Object);
        fileSystemService.Setup(x => x.GetDirectoryInfo(It.IsAny<string>())).Returns<string>(x => dirService.Object.GetInfo(x));
        fileSystemService.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns<string>(x => localFileService.Object.GetInfo(x));

        var info = Mocks.Create<FileSystemInfoBase>(MockBehavior.Loose, fileSystemService.Object, path);
        info.CallBase = true;
        return info.Object;
    }
}
