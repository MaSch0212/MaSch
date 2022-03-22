namespace MaSch.FileSystem.FileSystemBuilder;

public interface IFileBuilder : IFileSystemEntryBuilder<IFileBuilder>, IFileSystemActionBuilder
{
    IFileBuilder WithContent(byte[] bytes);
    IFileBuilder WithContent(string text);
    IFileBuilder WithContent(string text, Encoding encoding);
}
