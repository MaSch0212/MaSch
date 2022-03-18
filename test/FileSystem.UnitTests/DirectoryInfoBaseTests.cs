using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class DirectoryInfoBaseTests : TestClassBase
{
    private const string Path = "C:\\Folder\\SubFolder";
    private DirectoryInfoBase Sut { get; set; } = null!;
    private Mock<IDirectoryService> DirectoryService { get; set; } = null!;

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        Sut = CreateInfo(Path, out var ds);
        DirectoryService = ds;
    }

    [TestMethod]
    public void Parent()
    {
        var expected = Mocks.Create<IDirectoryInfo>();
        DirectoryService.Setup(x => x.GetParent(Path)).Returns(expected.Object).Verifiable(Verifiables, Times.Once());

        var actual = Sut.Parent;

        Assert.AreEqual(expected.Object, actual);
    }

    [TestMethod]
    public void Root()
    {
        var expected = Mocks.Create<IDirectoryInfo>();
        DirectoryService.Setup(x => x.GetDirectoryRoot(Path)).Returns("C:\\").Verifiable(Verifiables, Times.Once());
        DirectoryService.Setup(x => x.GetInfo("C:\\")).Returns(expected.Object).Verifiable(Verifiables, Times.Once());

        var actual = Sut.Root;

        Assert.AreEqual(expected.Object, actual);
    }

    [TestMethod]
    public void Exists()
    {
        DirectoryService.Setup(x => x.Exists(Path)).Returns(true).Verifiable(Verifiables, Times.Once());

        var actual = Sut.Exists;

        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void CreationTimeUtc_Getter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.GetCreationTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.CreationTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CreationTimeUtc_Setter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.SetCreationTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.CreationTimeUtc = expected;
    }

    [TestMethod]
    public void LastAccessTimeUtc_Getter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.GetLastAccessTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastAccessTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void LastAccessTimeUtc_Setter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.SetLastAccessTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastAccessTimeUtc = expected;
    }

    [TestMethod]
    public void LastWriteTimeUtc_Getter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.GetLastWriteTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastWriteTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void LastWriteTimeUtc_Setter()
    {
        var expected = DateTime.UtcNow;
        DirectoryService.Setup(x => x.SetLastWriteTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastWriteTimeUtc = expected;
    }

    [TestMethod]
    public void Create()
    {
        DirectoryService.Setup(x => x.CreateDirectory(Path)).Returns((IDirectoryInfo)null!).Verifiable(Verifiables, Times.Once());

        Sut.Create();
    }

    [TestMethod]
    public void CreateSubdirectory_ParameterChecks()
    {
        DirectoryService.Setup(x => x.GetInfo(It.IsAny<string>())).Returns((IDirectoryInfo)null!).Verifiable(Verifiables, Times.Never());

        Assert.ThrowsException<ArgumentNullException>(() => Sut.CreateSubdirectory(null!));
        Assert.ThrowsException<ArgumentException>(() => Sut.CreateSubdirectory(string.Empty));
        Assert.ThrowsException<ArgumentException>(() => Sut.CreateSubdirectory("C:\\test"));
    }

    [TestMethod]
    public void CreateSubdirectory_Success()
    {
        var expected = Mocks.Create<IDirectoryInfo>().Object;
        DirectoryService.Setup(x => x.GetInfo(Path + "\\abc")).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.CreateSubdirectory("abc");

        Assert.AreEqual(expected, actual);
    }

    private DirectoryInfoBase CreateInfo(string path, out Mock<IDirectoryService> directoryService)
    {
        var dirService = directoryService = Mocks.Create<IDirectoryService>();
        var fileSystemService = Mocks.Create<IFileSystemService>();

        fileSystemService.Setup(x => x.Directory).Returns(directoryService.Object);
        fileSystemService.Setup(x => x.GetDirectoryInfo(It.IsAny<string>())).Returns<string>(x => dirService.Object.GetInfo(x));

        var info = Mocks.Create<DirectoryInfoBase>(MockBehavior.Loose, fileSystemService.Object, path);
        info.CallBase = true;
        return info.Object;
    }
}
