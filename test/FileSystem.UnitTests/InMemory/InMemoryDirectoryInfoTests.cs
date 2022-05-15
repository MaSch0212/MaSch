using MaSch.Core.Extensions;
using MaSch.FileSystem.InMemory;
using MaSch.FileSystem.InMemory.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.FileSystem.UnitTests.InMemory;

[TestClass]
public class InMemoryDirectoryInfoTests : TestClassBase
{
    private static readonly Random _rng = new();
    private InMemoryFileSystemService _fileSystemService = null!;

    [TestMethod]
    public void Ctor_ExistingNode_Null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp", null!));
    }

    [TestMethod]
    public void Ctor_ExistingNode_NotNull()
    {
        var node = new DirectoryNode(_fileSystemService.Nodes["C:\\"], null, "temp");
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp", node);

        var sutPo = new PrivateObject(sut);
        Assert.AreSame(node, sutPo.GetField<ContainerNode?>("_node"));
    }

    [TestMethod]
    public void Ctor_DirectoryExists()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp");

        var sutPo = new PrivateObject(sut);
        Assert.IsNotNull(sutPo.GetField<ContainerNode?>("_node"));
    }

    [TestMethod]
    public void Ctor_DirectoryDoesNotExist()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp2");

        var sutPo = new PrivateObject(sut);
        Assert.IsNull(sutPo.GetField<ContainerNode?>("_node"));
    }

    [TestMethod]
    public void Refresh_Created()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp2");
        var sutPo = new PrivateObject(sut);

        _fileSystemService.Directory.CreateDirectory("C:\\temp2");

        sut.Refresh();

        Assert.IsNotNull(sutPo.GetField<ContainerNode?>("_node"));
    }

    [TestMethod]
    public void Refresh_Removed()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp");
        var sutPo = new PrivateObject(sut);

        _fileSystemService.Directory.Delete("C:\\temp");

        sut.Refresh();

        Assert.IsNull(sutPo.GetField<ContainerNode?>("_node"));
    }

    [TestMethod]
    public void Get_Attributes_DirectoryExists()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp");
        var expectedAttributes = _rng.NextEnum<FileAttributes>();
        _fileSystemService.Nodes["C:\\"].Directories[0].Attributes = expectedAttributes;

        var actualAttributes = sut.Attributes;

        Assert.AreEqual(expectedAttributes, actualAttributes);
    }

    [TestMethod]
    public void Get_Attributes_DirectoryDoesNotExist()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp2");

        var actualAttributes = sut.Attributes;

        Assert.AreEqual((FileAttributes)(-1), actualAttributes);
    }

    [TestMethod]
    public void Set_Attributes_DirectoryExists()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp");
        var expectedAttributes = _rng.NextEnum<FileAttributes>();

        sut.Attributes = expectedAttributes;

        Assert.AreEqual(expectedAttributes, _fileSystemService.Nodes["C:\\"].Directories[0].Attributes);
    }

    [TestMethod]
    public void Set_Attributes_DirectoryDoesNotExist()
    {
        var sut = new InMemoryDirectoryInfo(_fileSystemService, "C:\\temp2");
        var expectedAttributes = _rng.NextEnum<FileAttributes>();

        Assert.ThrowsException<FileNotFoundException>(() => sut.Attributes = expectedAttributes);
    }

    protected override void OnInitializeTest()
    {
        base.OnInitializeTest();
        _fileSystemService = new InMemoryFileSystemService();
        _fileSystemService.CreatePathRoot("C:\\");
        _fileSystemService.Directory.CreateDirectory("C:\\temp");
    }
}
