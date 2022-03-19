namespace MaSch.FileSystem;

public class FileSystemServiceBuilder
{
    private readonly Expression _fileSystemVariableExpression;
    private readonly LinkedList<Expression> _expressions = new();
    private Expression? _createServiceExpression;

    public FileSystemServiceBuilder()
    {
        _fileSystemVariableExpression = Expression.Variable(typeof(IFileSystemService), "fs");
    }

    public FileSystemServiceBuilder UsingFileSystem<T>()
        where T : IFileSystemService, new()
    {
        _createServiceExpression = GetCreateServiceExpression(typeof(T));
        return this;
    }

    public FileSystemServiceBuilder UsingPhysicalFileSystem() => UsingFileSystem<Physical.PhysicalFileSystemService>();
    public FileSystemServiceBuilder UsingInMemoryFileSystem() => UsingFileSystem<InMemory.InMemoryFileSystemService>();

    public FileSystemServiceBuilder WithDirectory(string path)
    {

        return this;
    }

    public IFileSystemService Build()
    {
        var returnTarget = Expression.Label(typeof(IFileSystemService));
        var expression = Expression.Block(
            _expressions
                .Prepend(_createServiceExpression ?? GetCreateServiceExpression(typeof(Physical.PhysicalFileSystemService)))
                .Prepend(_fileSystemVariableExpression)
                .Append(Expression.Return(returnTarget, _fileSystemVariableExpression)));
        var lambda = Expression.Lambda(expression);
        return (IFileSystemService)lambda.Compile().DynamicInvoke()!;
    }

    private Expression GetCreateServiceExpression(Type serviceType)
    {
        return Expression.Assign(
            _fileSystemVariableExpression,
            Expression.New(serviceType));
    }
}