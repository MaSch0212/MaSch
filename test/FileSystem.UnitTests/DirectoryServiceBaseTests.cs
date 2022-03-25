using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class DirectoryServiceBaseTests : TestClassBase
{
    private const string Path = "C:\\Folder\\SubFolder";
    private DirectoryServiceBase Sut => SutMock.Object;
    private Mock<DirectoryServiceBase> SutMock { get; set; } = null!;
    private Mock<IFileService> FileService { get; set; } = null!;

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        SutMock = CreateService(out var fs);
        FileService = fs;
    }

    [TestMethod]
    public void Constructor_NullCheck()
    {
        var ex = Assert.ThrowsException<TargetInvocationException>(() => Mocks.Create<DirectoryServiceBase>(new object?[] { null }).Object);
        Assert.IsInstanceOfType<ArgumentNullException>(ex.InnerException);
    }

    [TestMethod]
    public void Delete_Path()
    {
        SutMock.Setup(x => x.Delete(Path, false)).Verifiable(Verifiables, Times.Once());
        Sut.Delete(Path);
    }

    [TestMethod]
    public void EnumerateDirectories_Path()
    {
        SutMock.Setup(x => x.EnumerateDirectories(Path, "*", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.EnumerateDirectories(Path);
    }

    [TestMethod]
    public void EnumerateDirectories_Path_SearchPattern()
    {
        SutMock.Setup(x => x.EnumerateDirectories(Path, "abc", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.EnumerateDirectories(Path, "abc");
    }

    [TestMethod]
    public void EnumerateFiles_Path()
    {
        SutMock.Setup(x => x.EnumerateFiles(Path, "*", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.EnumerateFiles(Path);
    }

    [TestMethod]
    public void EnumerateFiles_Path_SearchPattern()
    {
        SutMock.Setup(x => x.EnumerateFiles(Path, "abc", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.EnumerateFiles(Path, "abc");
    }

    [TestMethod]
    public void EnumerateFileSystemEntries_Path()
    {
        SutMock.Setup(x => x.EnumerateFileSystemEntries(Path, "*", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.EnumerateFileSystemEntries(Path);
    }

    [TestMethod]
    public void EnumerateFileSystemEntries_Path_SearchPattern()
    {
        SutMock.Setup(x => x.EnumerateFileSystemEntries(Path, "abc", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.EnumerateFileSystemEntries(Path, "abc");
    }

    [TestMethod]
    public void GetDirectories_Path()
    {
        SutMock.Setup(x => x.GetDirectories(Path, "*", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetDirectories(Path);
    }

    [TestMethod]
    public void GetDirectories_Path_SearchPattern()
    {
        SutMock.Setup(x => x.GetDirectories(Path, "abc", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetDirectories(Path, "abc");
    }

    [TestMethod]
    public void GetDirectories_Path_SearchPattern_SearchOption()
    {
        SutMock.Setup(x => x.EnumerateDirectories(Path, "abc", SearchOption.AllDirectories))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetDirectories(Path, "abc", SearchOption.AllDirectories);
    }

    [TestMethod]
    public void GetFiles_Path()
    {
        SutMock.Setup(x => x.GetFiles(Path, "*", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetFiles(Path);
    }

    [TestMethod]
    public void GetFiles_Path_SearchPattern()
    {
        SutMock.Setup(x => x.GetFiles(Path, "abc", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetFiles(Path, "abc");
    }

    [TestMethod]
    public void GetFiles_Path_SearchPattern_SearchOption()
    {
        SutMock.Setup(x => x.EnumerateFiles(Path, "abc", SearchOption.AllDirectories))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetFiles(Path, "abc", SearchOption.AllDirectories);
    }

    [TestMethod]
    public void GetFileSystemEntries_Path()
    {
        SutMock.Setup(x => x.GetFileSystemEntries(Path, "*", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetFileSystemEntries(Path);
    }

    [TestMethod]
    public void GetFileSystemEntries_Path_SearchPattern()
    {
        SutMock.Setup(x => x.GetFileSystemEntries(Path, "abc", SearchOption.TopDirectoryOnly))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetFileSystemEntries(Path, "abc");
    }

    [TestMethod]
    public void GetFileSystemEntries_Path_SearchPattern_SearchOption()
    {
        SutMock.Setup(x => x.EnumerateFileSystemEntries(Path, "abc", SearchOption.AllDirectories))
               .Returns(Array.Empty<string>())
               .Verifiable(Verifiables, Times.Once());
        Sut.GetFileSystemEntries(Path, "abc", SearchOption.AllDirectories);
    }

    [TestMethod]
    public void GetDirectoryRoot()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Sut.GetDirectoryRoot(null!));
        Assert.AreEqual("C:\\", Sut.GetDirectoryRoot(Path));
    }

    [TestMethod]
    public void GetParent_Null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Sut.GetParent(null!));
    }

    [TestMethod]
    public void GetParent()
    {
        var di = Mocks.Create<IDirectoryInfo>();
        SutMock.Setup(x => x.GetInfo(System.IO.Path.GetDirectoryName(Path)!))
               .Returns(di.Object)
               .Verifiable(Verifiables, Times.Once());
        Assert.AreSame(di.Object, Sut.GetParent(Path));
    }

    [TestMethod]
    public void GetParent_NoParent()
    {
        SutMock.Setup(x => x.GetInfo(System.IO.Path.GetDirectoryName(Path)!))
               .Returns((IDirectoryInfo)null!)
               .Verifiable(Verifiables, Times.Never());
        Assert.IsNull(Sut.GetParent("C:\\"));
    }

    [TestMethod]
    public void GetCreationTime()
    {
        var dt = DateTime.UtcNow;
        SutMock.Setup(x => x.GetCreationTimeUtc(Path)).Returns(dt).Verifiable(Verifiables, Times.Once());
        Assert.AreEqual(dt.ToLocalTime(), Sut.GetCreationTime(Path));
    }

    [TestMethod]
    public void GetLastAccessTime()
    {
        var dt = DateTime.UtcNow;
        SutMock.Setup(x => x.GetLastAccessTimeUtc(Path)).Returns(dt).Verifiable(Verifiables, Times.Once());
        Assert.AreEqual(dt.ToLocalTime(), Sut.GetLastAccessTime(Path));
    }

    [TestMethod]
    public void GetLastWriteTime()
    {
        var dt = DateTime.UtcNow;
        SutMock.Setup(x => x.GetLastWriteTimeUtc(Path)).Returns(dt).Verifiable(Verifiables, Times.Once());
        Assert.AreEqual(dt.ToLocalTime(), Sut.GetLastWriteTime(Path));
    }

#if !NETFRAMEWORK && (!NETSTANDARD || NETSTANDARD2_1_OR_GREATER)

    [TestMethod]
    public void GetDirectories_SearchPattern_EnumerationOptions()
    {
        var enumOptions = new EnumerationOptions();
        var result = new[] { Guid.NewGuid().ToString() };
        SutMock.Setup(x => x.EnumerateDirectories(Path, "abc", enumOptions))
               .Returns(result)
               .Verifiable(Verifiables, Times.Once());
        Assert.AreCollectionsEqual(result, Sut.GetDirectories(Path, "abc", enumOptions));
    }

    [TestMethod]
    public void GetFiles_SearchPattern_EnumerationOptions()
    {
        var enumOptions = new EnumerationOptions();
        var result = new[] { Guid.NewGuid().ToString() };
        SutMock.Setup(x => x.EnumerateFiles(Path, "abc", enumOptions))
               .Returns(result)
               .Verifiable(Verifiables, Times.Once());
        Assert.AreCollectionsEqual(result, Sut.GetFiles(Path, "abc", enumOptions));
    }

    [TestMethod]
    public void GetFileSystemEntries_SearchPattern_EnumerationOptions()
    {
        var enumOptions = new EnumerationOptions();
        var result = new[] { Guid.NewGuid().ToString() };
        SutMock.Setup(x => x.EnumerateFileSystemEntries(Path, "abc", enumOptions))
               .Returns(result)
               .Verifiable(Verifiables, Times.Once());
        Assert.AreCollectionsEqual(result, Sut.GetFileSystemEntries(Path, "abc", enumOptions));
    }

#endif

    private Mock<DirectoryServiceBase> CreateService(out Mock<IFileService> fileService)
    {
        var localFileService = fileService = Mocks.Create<IFileService>();
        var fileSystemService = Mocks.Create<IFileSystemService>();
        var dirService = Mocks.Create<DirectoryServiceBase>(MockBehavior.Loose, fileSystemService.Object);
        dirService.CallBase = true;

        fileSystemService.Setup(x => x.Directory).Returns(dirService.Object);
        fileSystemService.Setup(x => x.File).Returns(localFileService.Object);
        fileSystemService.Setup(x => x.GetDirectoryInfo(It.IsAny<string>())).Returns<string>(x => dirService.Object.GetInfo(x));
        fileSystemService.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns<string>(x => localFileService.Object.GetInfo(x));

        return dirService;
    }
}
