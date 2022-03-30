using MaSch.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSch.FileSystem.UnitTests;

[TestClass]
public class FileInfoBaseTests : TestClassBase
{
    private const string Path = "C:\\Folder\\file.txt";
    private static readonly Random Random = new();

    private FileInfoBase Sut { get; set; } = null!;
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
    public void Get_DirectoryName()
    {
        Assert.AreEqual(@"C:\Folder", Sut.DirectoryName);
    }

    [TestMethod]
    public void Get_Directory()
    {
        var dirInfo = Mocks.Create<IDirectoryInfo>().Object;
        FileService.Setup(x => x.GetDirectory(Path)).Returns(dirInfo).Verifiable(Verifiables, Times.Once());
        Assert.AreSame(dirInfo, Sut.Directory);
    }

    [TestMethod]
    [DataRow(true, DisplayName = "Exists")]
    [DataRow(false, DisplayName = "Does not exists")]
    public void Get_Exists(bool exists)
    {
        FileService.Setup(x => x.Exists(Path)).Returns(exists).Verifiable(Verifiables, Times.Once());
        Assert.AreEqual(exists, Sut.Exists);
    }

    [TestMethod]
    public void Get_CreationTimeUtc()
    {
        var expected = DateTime.UtcNow;
        FileService.Setup(x => x.GetCreationTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.CreationTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Set_CreationTimeUtc()
    {
        var expected = DateTime.UtcNow;
        FileService.Setup(x => x.SetCreationTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.CreationTimeUtc = expected;
    }

    [TestMethod]
    public void Get_CreationTime()
    {
        var expected = DateTime.Now;
        FileService.Setup(x => x.GetCreationTime(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.CreationTime;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Set_CreationTime()
    {
        var expected = DateTime.Now;
        FileService.Setup(x => x.SetCreationTime(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.CreationTime = expected;
    }

    [TestMethod]
    public void Get_LastAccessTimeUtc()
    {
        var expected = DateTime.UtcNow;
        FileService.Setup(x => x.GetLastAccessTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastAccessTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Set_LastAccessTimeUtc()
    {
        var expected = DateTime.UtcNow;
        FileService.Setup(x => x.SetLastAccessTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastAccessTimeUtc = expected;
    }

    [TestMethod]
    public void Get_LastAccessTime()
    {
        var expected = DateTime.Now;
        FileService.Setup(x => x.GetLastAccessTime(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastAccessTime;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Set_LastAccessTime()
    {
        var expected = DateTime.Now;
        FileService.Setup(x => x.SetLastAccessTime(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastAccessTime = expected;
    }

    [TestMethod]
    public void Get_LastWriteTimeUtc()
    {
        var expected = DateTime.UtcNow;
        FileService.Setup(x => x.GetLastWriteTimeUtc(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastWriteTimeUtc;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Set_LastWriteTimeUtc()
    {
        var expected = DateTime.UtcNow;
        FileService.Setup(x => x.SetLastWriteTimeUtc(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastWriteTimeUtc = expected;
    }

    [TestMethod]
    public void Get_LastWriteTime()
    {
        var expected = DateTime.Now;
        FileService.Setup(x => x.GetLastWriteTime(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        var actual = Sut.LastWriteTime;

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Set_LastWriteTime()
    {
        var expected = DateTime.Now;
        FileService.Setup(x => x.SetLastWriteTime(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.LastWriteTime = expected;
    }

    [TestMethod]
    public void Get_Attributes()
    {
        var expected = Random.NextEnum<FileAttributes>();
        FileService.Setup(x => x.GetAttributes(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreEqual(expected, Sut.Attributes);
    }

    [TestMethod]
    public void Set_Attributes()
    {
        var expected = Random.NextEnum<FileAttributes>();
        FileService.Setup(x => x.SetAttributes(Path, expected)).Verifiable(Verifiables, Times.Once());

        Sut.Attributes = expected;
    }

    [TestMethod]
    public void AppendText()
    {
        using var ms = new MemoryStream();
        using var result = new StreamWriter(ms);
        FileService.Setup(x => x.AppendText(Path)).Returns(result).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(result, Sut.AppendText());
    }

    [TestMethod]
    public void CopyTo()
    {
        var expectedPath = Guid.NewGuid().ToString();
        var expectedInfo = Mocks.Create<IFileInfo>().Object;
        FileService.Setup(x => x.Copy(Path, expectedPath)).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedPath)).Returns(expectedInfo).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expectedInfo, Sut.CopyTo(expectedPath));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void CopyTo_Overwrite(bool overwrite)
    {
        var expectedPath = Guid.NewGuid().ToString();
        var expectedInfo = Mocks.Create<IFileInfo>().Object;
        FileService.Setup(x => x.Copy(Path, expectedPath, overwrite)).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedPath)).Returns(expectedInfo).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expectedInfo, Sut.CopyTo(expectedPath, overwrite));
    }

    [TestMethod]
    public void Create()
    {
        using var expected = new MemoryStream();
        FileService.Setup(x => x.Create(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.Create());
    }

    [TestMethod]
    public void CreateText()
    {
        using var ms = new MemoryStream();
        using var expected = new StreamWriter(ms);
        FileService.Setup(x => x.CreateText(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.CreateText());
    }

    [TestMethod]
    public void MoveTo()
    {
        var expectedPath = Guid.NewGuid().ToString();
        FileService.Setup(x => x.Move(Path, expectedPath)).Verifiable(Verifiables, Times.Once());

        Sut.MoveTo(expectedPath);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void MoveTo_Overwrite(bool overwrite)
    {
        var expectedPath = Guid.NewGuid().ToString();
        FileService.Setup(x => x.Move(Path, expectedPath, overwrite)).Verifiable(Verifiables, Times.Once());

        Sut.MoveTo(expectedPath, overwrite);
    }

    [TestMethod]
    public void Open_Mode()
    {
        var mode = Random.NextEnum<FileMode>();
        using var expected = new MemoryStream();
        FileService.Setup(x => x.Open(Path, mode)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.Open(mode));
    }

    [TestMethod]
    public void Open_Mode_Access()
    {
        var mode = Random.NextEnum<FileMode>();
        var access = Random.NextEnum<FileAccess>();
        using var expected = new MemoryStream();
        FileService.Setup(x => x.Open(Path, mode, access)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.Open(mode, access));
    }

    [TestMethod]
    public void Open_Mode_Access_Share()
    {
        var mode = Random.NextEnum<FileMode>();
        var access = Random.NextEnum<FileAccess>();
        var share = Random.NextEnum<FileShare>();
        using var expected = new MemoryStream();
        FileService.Setup(x => x.Open(Path, mode, access, share)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.Open(mode, access, share));
    }

    [TestMethod]
    public void Open_FileStreamOptions()
    {
        var options = new FileStreamOptions();
        using var expected = new MemoryStream();
        FileService.Setup(x => x.Open(Path, options)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.Open(options));
    }

    [TestMethod]
    public void OpenRead()
    {
        var mode = Random.NextEnum<FileMode>();
        using var expected = new MemoryStream();
        FileService.Setup(x => x.OpenRead(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.OpenRead());
    }

    [TestMethod]
    public void OpenText()
    {
        var mode = Random.NextEnum<FileMode>();
        using var ms = new MemoryStream();
        using var expected = new StreamReader(ms);
        FileService.Setup(x => x.OpenText(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.OpenText());
    }

    [TestMethod]
    public void OpenWrite()
    {
        var mode = Random.NextEnum<FileMode>();
        using var expected = new MemoryStream();
        FileService.Setup(x => x.OpenWrite(Path)).Returns(expected).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expected, Sut.OpenWrite());
    }

    [TestMethod]
    public void Replace()
    {
        var expectedDestPath = Guid.NewGuid().ToString();
        var expectedBackupPath = Guid.NewGuid().ToString();
        var expectedInfo = Mocks.Create<IFileInfo>().Object;
        FileService.Setup(x => x.Replace(Path, expectedDestPath, expectedBackupPath)).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedDestPath)).Returns(expectedInfo).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expectedInfo, Sut.Replace(expectedDestPath, expectedBackupPath));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void Replace_IgnoreMetadataErrors(bool ignoreMetadataErrors)
    {
        var expectedDestPath = Guid.NewGuid().ToString();
        var expectedBackupPath = Guid.NewGuid().ToString();
        var expectedInfo = Mocks.Create<IFileInfo>().Object;
        FileService.Setup(x => x.Replace(Path, expectedDestPath, expectedBackupPath, ignoreMetadataErrors)).Verifiable(Verifiables, Times.Once());
        FileService.Setup(x => x.GetInfo(expectedDestPath)).Returns(expectedInfo).Verifiable(Verifiables, Times.Once());

        Assert.AreSame(expectedInfo, Sut.Replace(expectedDestPath, expectedBackupPath, ignoreMetadataErrors));
    }

    [TestMethod]
    public void Delete()
    {
        FileService.Setup(x => x.Delete(Path)).Verifiable(Verifiables, Times.Once());

        Sut.Delete();
    }

    private FileInfoBase CreateInfo(string path, out Mock<IDirectoryService> directoryService, out Mock<IFileService> fileService)
    {
        var dirService = directoryService = Mocks.Create<IDirectoryService>();
        var localFileService = fileService = Mocks.Create<IFileService>();
        var fileSystemService = Mocks.Create<IFileSystemService>();

        fileSystemService.Setup(x => x.Directory).Returns(directoryService.Object);
        fileSystemService.Setup(x => x.File).Returns(localFileService.Object);
        fileSystemService.Setup(x => x.GetDirectoryInfo(It.IsAny<string>())).Returns<string>(x => dirService.Object.GetInfo(x));
        fileSystemService.Setup(x => x.GetFileInfo(It.IsAny<string>())).Returns<string>(x => localFileService.Object.GetInfo(x));

        var info = Mocks.Create<FileInfoBase>(MockBehavior.Loose, fileSystemService.Object, path);
        info.CallBase = true;
        return info.Object;
    }
}
