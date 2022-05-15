namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class FileSystemServiceBaseTests : TestClassBase
{
    [TestMethod]
    public void Ctor()
    {
        var sut = CreateService(out var fileServiceMock, out var directoryServiceMock);

        Assert.AreSame(fileServiceMock.Object, sut.File);
        Assert.AreSame(directoryServiceMock.Object, sut.Directory);
    }

    [TestMethod]
    public void GetDirectoryInfo()
    {
        var directoryPath = "C:\\blub\\blubbi";
        var sut = CreateService(out _, out var directoryServiceMock);

        var expectedInfo = Mocks.Create<IDirectoryInfo>().Object;
        directoryServiceMock
            .Setup(x => x.GetInfo(directoryPath))
            .Returns(expectedInfo)
            .Verifiable(Verifiables, Times.Once());

        var actualInfo = sut.GetDirectoryInfo(directoryPath);

        Assert.AreEqual(expectedInfo, actualInfo);
    }

    [TestMethod]
    public void GetFileInfo()
    {
        var filePath = "C:\\blub\\file.txt";
        var sut = CreateService(out var fileServiceMock, out _);

        var expectedInfo = Mocks.Create<IFileInfo>().Object;
        fileServiceMock
            .Setup(x => x.GetInfo(filePath))
            .Returns(expectedInfo)
            .Verifiable(Verifiables, Times.Once());

        var actualInfo = sut.GetFileInfo(filePath);

        Assert.AreEqual(expectedInfo, actualInfo);
    }

    private static FileSystemServiceBase CreateService(out Mock<IFileService> fileServiceMock, out Mock<IDirectoryService> directoryServiceMock)
    {
        var fileSystemServiceMock = new FileSystemServiceBaseMock();
        fileServiceMock = fileSystemServiceMock.FileMock;
        directoryServiceMock = fileSystemServiceMock.DirectoryMock;

        return fileSystemServiceMock;
    }

    private class FileSystemServiceBaseMock : FileSystemServiceBase
    {
        private Mock<IFileService>? _fileMock;
        private Mock<IDirectoryService>? _directoryMock;

        public Mock<IFileService> FileMock => _fileMock ??= new Mock<IFileService>(MockBehavior.Strict);
        public Mock<IDirectoryService> DirectoryMock => _directoryMock ??= new Mock<IDirectoryService>(MockBehavior.Strict);

        protected override IDirectoryService CreateDirectoryService()
        {
            return DirectoryMock.Object;
        }

        protected override IFileService CreateFileService()
        {
            return FileMock.Object;
        }
    }
}
